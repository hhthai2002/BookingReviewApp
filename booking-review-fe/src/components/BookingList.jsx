import React, { useEffect, useState, useContext } from "react";
import { getNearbyBookingPosts } from "../services/bookingService";
import { ThemeContext } from "../hooks/ThemeContext";
import "../styles/BookingList.css";

const BookingList = ({ userLocation }) => {
  const [bookings, setBookings] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const { theme } = useContext(ThemeContext);

  useEffect(() => {
    if (!userLocation) return;

    const fetchBookings = async () => {
      try {
        const data = await getNearbyBookingPosts(userLocation.lat, userLocation.lng);
        setBookings(data);
      } catch (err) {
        setError(err.message || "Failed to fetch booking posts.");
      } finally {
        setLoading(false);
      }
    };

    fetchBookings();
  }, [userLocation]);

  if (loading) return <p className="loading-message">Loading bookings...</p>;
  if (error) return <p className="error-message">{error}</p>;

  return (
    <div className={`booking-list ${theme}`}>
      <h2>Nearby Booking Posts</h2>
      {bookings.length === 0 ? (
        <p>No bookings available.</p>
      ) : (
        bookings.map((booking, index) => (
          <div key={booking.id || index} className="booking-item">
            <h3>{booking.title}</h3>
            <p>{booking.description}</p>
            <p className="price">Price: ${booking.price}</p>
            <p className="location">📍 {booking.location}</p>
          </div>
        ))
      )}
    </div>
  );
};

export default BookingList;
