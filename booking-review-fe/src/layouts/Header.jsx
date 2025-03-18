import React, { useContext } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import "../styles/Header.css";

import {logout} from "../services/authService"

import logo_dark from "../assets/logo-dark.png";
import logo_light from "../assets/logo-light.png";
import search_icon_light from "../assets/search-w.png";
import search_icon_dark from "../assets/search-b.png";
import toogle_light from "../assets/night.png";
import toogle_dark from "../assets/day.png";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUser } from "@fortawesome/free-solid-svg-icons";
import { faHouse } from "@fortawesome/free-solid-svg-icons";
import { faEarthAsia } from "@fortawesome/free-solid-svg-icons";
import { faBell } from "@fortawesome/free-solid-svg-icons";
import { faRightFromBracket } from "@fortawesome/free-solid-svg-icons";

import { ThemeContext } from "../hooks/ThemeContext";

const Header = () => {
  const navigate = useNavigate();
  const location = useLocation();

  const { theme, setTheme } = useContext(ThemeContext);

  const toggleTheme = () => {
    setTheme(theme === "light" ? "dark" : "light");
  };

  // check if the current path is active
  const isActive = (path) => location.pathname === path ? "active" : "";

  return (
    <div className="header">
      <img
        src={theme == "light" ? logo_light : logo_dark}
        alt=""
        className="logo"
        onClick={() => navigate("/home")}
      />
       <ul>
        <li className={isActive("/home")} onClick={() => navigate("/home")}>
          <FontAwesomeIcon icon={faHouse} />
        </li>
        <li className={isActive("/booking")} onClick={() => navigate("/booking")}>
          <FontAwesomeIcon icon={faEarthAsia} />
        </li>
        <li className={isActive("/notifications")}>
          <FontAwesomeIcon icon={faBell} />
        </li>
        <li className={isActive("/profile")} onClick={() => navigate("/profile")}>
          <FontAwesomeIcon icon={faUser} />
        </li>
        <li>
          <FontAwesomeIcon icon={faRightFromBracket} onClick={logout} />
        </li>
      </ul>

      <div className="search-box">
        <input type="text" placeholder="Search" />
        <img
          src={theme == "light" ? search_icon_light : search_icon_dark}
          alt=""
        />
      </div>

      <img
        onClick={() => {
          toggleTheme();
        }}
        src={theme == "light" ? toogle_light : toogle_dark}
        alt=""
        className="toogle-icon"
      />
    </div>
  );
};

export default Header;
