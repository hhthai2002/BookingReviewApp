import React, { useEffect, useState } from "react";
import { getAllPosts } from "../services/postService";
import "../styles/PostList.css";

const PostList = () => {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchPosts = async () => {
      try {
        const data = await getAllPosts();
        setPosts(data);
      } catch (err) {
        setError(err || "Error fetching posts!");
      } finally {
        setLoading(false);
      }
    };

    fetchPosts();
  }, []);

  if (loading) return <p className="loading-message">Loading posts...</p>;
  if (error) return <p className="error-message">{error}</p>;

  return (
    <div className="post-list">
      {posts.length === 0 ? (
        <p>No posts available.</p>
      ) : (
        posts.map((post) => (
          <div key={post.id} className="post">
            <div className="post-header">
              <h3>{post.userName || "Anonymous"}</h3>
              <p>{new Date(post.createdAt).toLocaleString()}</p>
            </div>
            <p>{post.content}</p>

            {post.imageUrls && post.imageUrls.length > 0 && (
              <div className="post-images">
                {post.imageUrls.map((url, index) => (
                  <img
                    key={index}
                    src={`http://localhost:5085${url}`}
                    alt={`Post ${index}`}
                  />
                ))}
              </div>
            )}
          </div>
        ))
      )}
    </div>
  );
};

export default PostList;
