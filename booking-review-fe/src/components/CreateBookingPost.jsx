import React, { useState } from "react";
import { createBookingPost } from "../services/bookingService";
import MapComponent from "./MapComponent";

const CreateBookingPost = ({ onBookingCreated }) => {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [price, setPrice] = useState("");
  const [locationName, setLocationName] = useState("");
  const [suggestions, setSuggestions] = useState([]);
  const [location, setLocation] = useState({ lat: 10.7769, lng: 106.7009 });
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleLocationInput = async (input) => {
    setLocationName(input);

    if (input.length < 3) {
      setSuggestions([]);
      return;
    }

    try {
      const response = await fetch(
        `https://nominatim.openstreetmap.org/search?format=json&q=${input}&countrycodes=VN`
      );
      const data = await response.json();

      if (data.length === 0) {
        const globalResponse = await fetch(
          `https://nominatim.openstreetmap.org/search?format=json&q=${input}`
        );
        const globalData = await globalResponse.json();
        setSuggestions(globalData);
      } else {
        setSuggestions(data);
      }
    } catch (err) {
      console.error("Error fetching location suggestions:", err);
    }
  };

  const handleSelectLocation = (place) => {
    setLocationName(place.display_name);
    setLocation({ lat: parseFloat(place.lat), lng: parseFloat(place.lon) });
    setSuggestions([]);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError("");

    try {
      const newBooking = await createBookingPost({
        title,
        description,
        price: parseFloat(price),
        location: locationName,
        latitude: location.lat,
        longitude: location.lng,
      });

      setTitle("");
      setDescription("");
      setPrice("");
      setLocationName("");
      setLocation({ lat: 10.7769, lng: 106.7009 });
      if (onBookingCreated) onBookingCreated(newBooking);
    } catch (err) {
      setError(err.message || "Failed to create booking post.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="create-booking">
      <h2>Create a Booking Post</h2>
      {error && <p className="error-message">{error}</p>}
      <form onSubmit={handleSubmit} style={{ height: "95%", margin: "1rem" }}>
        <input
          type="text"
          placeholder="Title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          required
        />
        <textarea
          placeholder="Description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          required
        />
        <input
          type="number"
          placeholder="Price"
          value={price}
          onChange={(e) => setPrice(e.target.value)}
          min="0"
          required
        />

        <div className="location-input">
          <input
            type="text"
            placeholder="Search Location..."
            value={locationName}
            onChange={(e) => handleLocationInput(e.target.value)}
            required
          />
          {suggestions.length > 0 && (
            <ul className="suggestions-list">
              {suggestions.map((place, index) => (
                <li key={index} onClick={() => handleSelectLocation(place)}>
                  {place.display_name}
                </li>
              ))}
            </ul>
          )}
        </div>

        <MapComponent location={location} setLocation={setLocation} setLocationName={setLocationName} />

        <button type="submit" disabled={loading}>
          {loading ? "Creating..." : "Create Booking"}
        </button>
      </form>
    </div>
  );
};

export default CreateBookingPost;
