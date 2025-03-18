import React from "react";
import { convertToVietnamTime } from "../utils/convertToVietnamTime";
import "../styles/PostList.css";

const MyPosts = ({ posts }) => {
  return (
    <div className="post-list">
      {posts.length === 0 ? (
        <p>No posts available.</p>
      ) : (
        posts.map((post) => (
          <div key={post.id} className="post">
            <div className="post-header">
              <h3>You</h3>
              <p>{convertToVietnamTime(post.createdAt)}</p>
            </div>
            <p>{post.content}</p>

            {post.imageUrls && post.imageUrls.length > 0 && (
              <div className="post-images">
                {post.imageUrls.map((url, index) => (
                  <img key={index} src={`http://localhost:5085${url}`} alt={`Post ${index}`} />
                ))}
              </div>
            )}
          </div>
        ))
      )}
    </div>
  );
};

export default MyPosts;
