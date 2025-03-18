 
/* eslint-disable no-unused-vars */
import React, { useEffect, useState } from "react";
import { MapContainer, TileLayer, Marker, Popup, useMap, useMapEvents } from "react-leaflet";
import "leaflet/dist/leaflet.css";

const DEFAULT_LOCATION = [10.7769, 106.7009];

const MapComponent = ({ location, setLocation, setLocationName }) => {
  const [userLocation, setUserLocation] = useState(null);
  const [error, setError] = useState("");

  useEffect(() => {
    if ("geolocation" in navigator) {
      navigator.geolocation.getCurrentPosition(
        (position) => {
          const newLocation = [position.coords.latitude, position.coords.longitude];
          setUserLocation(newLocation);
          setLocation({ lat: newLocation[0], lng: newLocation[1] });
          reverseGeocode(newLocation[0], newLocation[1]);
        },
        (err) => {
          setError("Cannot retrieve your location! Please enable location access.");
        }
      );
    } else {
      setError("Geolocation is not supported by your browser!");
    }
  }, [setLocation]);

  const reverseGeocode = async (lat, lng) => {
    try {
      const response = await fetch(
        `https://nominatim.openstreetmap.org/reverse?format=json&lat=${lat}&lon=${lng}&accept-language=vi`,
        {
          headers: {
            "User-Agent": "MyBookingApp/1.0 (hhthai2002@email.com)",
          },
        }
      );
  
      if (!response.ok) throw new Error("Failed to fetch location data");
  
      const data = await response.json();
      return data.display_name || "Unknown Location";
    } catch (error) {
      console.error("Error fetching location name:", error);
      return "Failed to get location";
    }
  };
  
  const MapEvents = () => {
    useMapEvents({
      click(e) {
        const { lat, lng } = e.latlng;
        setLocation({ lat, lng });
        reverseGeocode(lat, lng);
      },
    });
    return null;
  };

  return (
    <div style={{ position: "relative" }}>
      {userLocation && (
        <button className="btn-my-location" onClick={() => setLocation({ lat: userLocation[0], lng: userLocation[1] })}>
          🔍 Go to My Location
        </button>
      )}

      <MapContainer center={userLocation || DEFAULT_LOCATION} zoom={13} style={{ height: "400px", width: "100%" }}>
        <TileLayer
          url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
        />
        <MapEvents />
        {location && (
          <Marker position={[location.lat, location.lng]}>
            <Popup>📍 Selected Location</Popup>
          </Marker>
        )}
        {error && <Popup position={DEFAULT_LOCATION}>{error}</Popup>}
        <UpdateMapCenter userLocation={userLocation} />
      </MapContainer>
    </div>
  );
};

const UpdateMapCenter = ({ userLocation }) => {
  const map = useMap();
  useEffect(() => {
    if (userLocation) {
      map.setView(userLocation, 13);
    }
  }, [userLocation, map]);
  return null;
};

export default MapComponent;
