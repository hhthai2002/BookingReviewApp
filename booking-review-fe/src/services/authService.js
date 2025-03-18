import axiosInstance from "../utils/axiosConfig.js";

//register
const register = async (userData) => {
  try {
    const response = await axiosInstance.post("/auth/register", userData);
    return response.data;
  } catch (error) {
    throw error.response?.data || "Register Failed!";
  }
};

const login = async (email, password) => {
  try {
    const response = await axiosInstance.post("/auth/login", {
      email,
      password,
    });
    return response.data;
  } catch (error) {
    throw error.response?.data || "Login Failed!";
  }
};

// logout with deleting token and move token to blacklisted on redis server
// const logout = async () => {
//   try {
//     const token = localStorage.getItem("token"); // get token from local storage
//     if (!token) {
//       console.error("No token found! User is not logged in.");
//       return;
//     }

//     await axiosInstance.post(
//       "/auth/logout",
//       {},
//       {
//         headers: {
//           Authorization: `Bearer ${token}`, // send token to header
//         },
//       }
//     );

//     localStorage.removeItem("token");
//     window.location.href = "/login"; // redirect to login page
//   } catch (error) {
//     console.error("Logout failed:", error.response?.data || "Unknown error");
//   }
// };

const logout = () => {
  localStorage.removeItem("token");
  window.location.href = "/login";
};

// get profile
const getProfile = async () => {
  try {
    const token = localStorage.getItem("token");
    if (!token) {
      throw new Error("User is not authenticated!");
    }

    const response = await axiosInstance.get("/auth/profile", {
      headers: {
        Authorization: `Bearer ${token}`, // send token to header
      },
    });

    return response.data;
  } catch (error) {
    console.error(
      "Failed to fetch profile:",
      error.response?.data || error.message
    );
    throw error.response?.data || "Failed to fetch profile!";
  }
};

//forgot password
const forgotPassword = async (email) => {
  try {
    const response = await axiosInstance.post("/auth/forgot-password", {
      email,
    });
    return response.data;
  } catch (error) {
    throw error.response?.data || "Failed to send reset email!";
  }
};

//reset password
const resetPassword = async (token, newPassword, confirmPassword) => {
  try {
    const response = await axiosInstance.post("/auth/reset-password", {
      token,
      newPassword,
      confirmPassword
    });
    return response.data;
  } catch (error) {
    throw error.response?.data || "Failed to reset password!";
  }
};

export { register, login, logout, getProfile, forgotPassword, resetPassword };
