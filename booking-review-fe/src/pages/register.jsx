import React, { useState } from "react";
import { useForm } from "react-hook-form";
import { register as registerUser } from "../services/authService";
import { useNavigate } from "react-router-dom";
import "../styles/Register.css";

const Register = () => {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");
  const navigate = useNavigate();

  const onSubmit = async (data) => {
    setErrorMessage("");
    setSuccessMessage("");

    try {
      const response = await registerUser(data);
      setSuccessMessage(response.message);
      
      // Chuyển hướng sang trang đăng nhập sau 3 giây
      setTimeout(() => navigate("/login"), 3000);
    } catch (error) {
      setErrorMessage(error || "Đăng ký thất bại!");
    }
  };

  return (
    <div className="register-container">
      <div className="register-card">
        <h2>Đăng ký</h2>
        <p>Miễn phí và sẽ luôn như vậy.</p>

        {errorMessage && <p className="error-message">{errorMessage}</p>}
        {successMessage && <p className="success-message">{successMessage}</p>}

        <form onSubmit={handleSubmit(onSubmit)}>
          <input type="text" placeholder="Họ và tên" {...register("fullName", { required: "Vui lòng nhập họ tên" })} />
          {errors.fullName && <span>{errors.fullName.message}</span>}

          <input type="email" placeholder="Email" {...register("email", { required: "Vui lòng nhập email" })} />
          {errors.email && <span>{errors.email.message}</span>}

          <input type="text" placeholder="Tên người dùng" {...register("username", { required: "Vui lòng nhập tên người dùng" })} />
          {errors.username && <span>{errors.username.message}</span>}

          <input type="password" placeholder="Mật khẩu" {...register("password", { required: "Vui lòng nhập mật khẩu", minLength: { value: 6, message: "Mật khẩu ít nhất 6 ký tự" } })} />
          {errors.password && <span>{errors.password.message}</span>}

          <input type="text" placeholder="Số tài khoản ngân hàng (tuỳ chọn)" {...register("bankAccount")} />

          <button type="submit">Đăng ký</button>
        </form>

        <p>
          Đã có tài khoản? <a href="/login">Đăng nhập</a>
        </p>
      </div>
    </div>
  );
};

export default Register;
