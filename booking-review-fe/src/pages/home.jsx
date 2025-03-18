import React, { useContext } from "react";
import Header from "../layouts/Header";
import { ThemeContext } from "../hooks/ThemeContext";
import PostList from "../components/PostList";

const Home = () => {
  const { theme } = useContext(ThemeContext);

  return (
    <div className={`container ${theme}`}>
      <Header />
      <div className="home-content">
        <PostList />
      </div>
    </div>
  );
};

export default Home;
