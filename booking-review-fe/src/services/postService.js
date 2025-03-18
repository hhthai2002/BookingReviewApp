import axiosInstance from "../utils/axiosConfig.js";

const HandleCreatePost = async (content, imageFiles) => {
  try {
    const token = localStorage.getItem("token");
    if (!token) throw new Error("User is not authenticated");

    const formData = new FormData();
    formData.append("Content", content);
    
    imageFiles.forEach((file) => {
      formData.append("imageFiles", file);
    });

    const response = await axiosInstance.post("/Post", formData, {
      headers: {
        "Content-Type": "multipart/form-data",
      },
    });

    return response.data;
  } catch (error) {
    throw error.response?.data || "Error creating post!";
  }
};

const getMyPosts = async () => {
  try {
    const response = await axiosInstance.get("/Post/my-posts");
    return response.data;
  } catch (error) {
    throw error.response?.data || "Error fetching user's posts!";
  }
};

//get all posts
const getAllPosts = async () => {
  try {
    const response = await axiosInstance.get("/Post");
    return response.data;
  } catch (error) {
    throw error.response?.data || "Error fetching posts!";
  }
};

export { HandleCreatePost, getMyPosts, getAllPosts };
