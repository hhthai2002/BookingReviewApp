import React, { useState } from "react";
import { login } from "../services/authService";
import { useNavigate } from "react-router-dom";
import "../styles/login_form.css";

const LoginForm = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleRegister = () => {
    navigate("/register");
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const data = await login(email, password);
      localStorage.setItem("token", data.token); // set token to local storage
      alert("Login Successfully!");
      navigate("/home"); // redirect to home
    } catch (err) {
      setError(err.message || "An error occurred!");
    }
  };

  return (
    <div className="content">
      <div className="flex-div">
        <div className="name-content">
          <h1 className="logo">BORE</h1>
          <p>Connect with friends and visit around the world on BORA.</p>
        </div>
        <form onSubmit={handleLogin}>
          {error && <p className="error">{error}</p>}
          <input
            type="text"
            placeholder="Email or Phone Number"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
          <input
            type="password"
            placeholder="Password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <button type="submit" className="login">
            Log In
          </button>
          <a href="/forgot-password">Forgot Password ?</a>
          <hr />
          <button type="button" onClick={handleRegister} className="create-account">Create New Account</button>
        </form>
      </div>
    </div>
  );
};

export default LoginForm;
