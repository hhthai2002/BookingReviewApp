import React, { useEffect, useState, useContext } from "react";
import Header from "../layouts/Header";
import { ThemeContext } from "../hooks/ThemeContext";
import { getProfile } from "../services/authService";
import { getMyPosts } from "../services/postService";
import "../styles/Profile.css";
import CreatePost from "../components/CreatePost";
import MyPosts from "../components/MyPost";

const Profile = () => {
  const { theme } = useContext(ThemeContext);
  const [user, setUser] = useState(null);
  const [error, setError] = useState("");
  const [posts, setPosts] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const userData = await getProfile();
        setUser(userData);

        const postData = await getMyPosts();
        setPosts(postData);
      } catch (err) {
        setError(err || "Lỗi khi tải dữ liệu.");
      }
    };
    fetchData();
  }, []);

  const handlePostCreated = (newPost) => {
    setPosts((prevPosts) => [newPost, ...prevPosts]);
  };

  if (error) return <p className="error-message">{error}</p>;
  if (!user) return <p className="loading-message">Profile is loading...</p>;

  return (
    <div className={`container ${theme}`}>
      <Header />

      <div className="profile-container">
        <div className="left-container">
          <div className="cover-photo">
            <img src="https://vudigital.co/wp-content/uploads/2023/10/logo-mu-3-giai-doan-hinh-thanh-bieu-tuong-cua-quy-do.webp" alt="Cover" />
          </div>
          <div className="profile-info">
            <img src="https://randomuser.me/api/portraits/men/1.jpg" alt="Avatar" className="profile-avatar" />
            <h2>{user.fullName}</h2>
            <p>Wallet Balance: ${user.walletBalance}</p>
            <button className="edit-btn">Edit your profile</button>
          </div>
        </div>

        <div className="right-container">
          <div className="create-post-container">
            <CreatePost onPostCreated={handlePostCreated} />
            <MyPosts posts={posts} />
          </div>
        </div>
      </div>
    </div>
  );
};

export default Profile;
