import axiosInstance from "../utils/axiosConfig";

const createBookingPost = async (bookingData) => {
    try {
        const response = await axiosInstance.post("/Booking", bookingData);
        return response.data;
    } catch (error) {
        throw error.response?.data || "Error creating booking!";
    }
}

const getNearbyBookingPosts = async (lat, lng) => {
    try {
        const response = await axiosInstance.get(`/Booking/nearby?userLat=${lat}&userLng=${lng}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching nearby posts:", error);
        throw error.response?.data || "Error fetching nearby booking posts!";
    }
};

export { createBookingPost, getNearbyBookingPosts };