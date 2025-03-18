import React, { useEffect, useState, useContext } from "react";
import Header from "../layouts/Header";
import CreateBookingPost from "../components/CreateBookingPost";
import BookingList from "../components/BookingList";
import { ThemeContext } from "../hooks/ThemeContext";
import "../styles/Booking.css"; // Thêm CSS

const Booking = () => {
  const [userLocation, setUserLocation] = useState(null);
  const [bookings, setBookings] = useState([]);
  const { theme } = useContext(ThemeContext);

  useEffect(() => {
    navigator.geolocation.getCurrentPosition(
      (position) => {
        setUserLocation({
          lat: position.coords.latitude,
          lng: position.coords.longitude,
        });
      },
      () => {
        setUserLocation({ lat: 10.7769, lng: 106.7009 }); // default location: HCM City if user denies location access
      }
    );
  }, []);

  return (
    <div className={`container ${theme}`}>
      <Header />
      <div className="booking-layout">
        <div className="left-column">
          <CreateBookingPost onBookingCreated={(newBooking) => setBookings([newBooking, ...bookings])} />
        </div>

        <div className="right-column">
          <BookingList userLocation={userLocation} />
        </div>
      </div>
    </div>
  );
};

export default Booking;
