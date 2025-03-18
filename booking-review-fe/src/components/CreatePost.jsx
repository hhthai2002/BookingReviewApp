import React, { useState } from "react";
import { HandleCreatePost } from "../services/postService";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faImages, faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import "../styles/CreatePost.css";

const CreatePost = ({ onPostCreated }) => {
  const [content, setContent] = useState("");
  const [images, setImages] = useState([]);
  const [previews, setPreviews] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const handleImageChange = (e) => {
    const files = Array.from(e.target.files);
    setImages(files);
    
    const previewUrls = files.map((file) => URL.createObjectURL(file));
    setPreviews(previewUrls);
  };

  const handleSubmit = async () => {
    if (!content.trim() && images.length === 0) {
      setError("The post must not leave blank!");
      return;
    }

    setLoading(true);
    setError("");

    try {
      const newPost = await HandleCreatePost(content, images);

      setContent("");
      setImages([]);
      setPreviews([]);

      if (onPostCreated) onPostCreated(newPost);
    } catch (err) {
      setError(err || "Error occurs when creating a post!");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="create-post">
      <textarea
        placeholder="How is your day 😊?"
        value={content}
        onChange={(e) => setContent(e.target.value)}
      ></textarea>

      <div className="preview-container">
        {previews.map((src, index) => (
          <img key={index} src={src} alt={`Preview ${index}`} className="preview-image" />
        ))}
      </div>

      <div className="post-actions">
        <label className="upload-btn">
          <FontAwesomeIcon icon={faImages} /> Upload Images
          <input
            type="file"
            accept="image/*"
            multiple
            onChange={handleImageChange}
            hidden
          />
        </label>

        <button className="post-btn" onClick={handleSubmit} disabled={loading}>
          {loading ? (
            "Uploading..."
          ) : (
            <>
              <FontAwesomeIcon icon={faPaperPlane} /> Create Post
            </>
          )}
        </button>
      </div>

      {error && <p className="error-message">{error}</p>}
    </div>
  );
};

export default CreatePost;
