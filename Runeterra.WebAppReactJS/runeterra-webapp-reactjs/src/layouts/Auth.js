import React, {useState} from "react";
import { Routes, Route, useNavigate } from "react-router-dom";

// components

import Navbar from "../components/Navbars/AuthNavbar.js";
import FooterSmall from "../components/Footers/FooterSmall.js";

// views

import Login from "../views/auth/Login.js";
import axios from "axios";

// base
const baseUrl = "https://localhost:7241/gateway/user/auth";
export default function Auth() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post(baseUrl, {
                userName: username,
                password: password,
            });
            if (response.data && response.data.token) {
                localStorage.setItem('token', response.data.token); // Lưu token vào localStorage
                setIsAuthenticated(true); // Đánh dấu đã xác thực thành công
            }
            navigate("/")
        } catch (error) {
            console.error(error);
        }
    };


    return (
        <>
            <Navbar transparent />
            {isAuthenticated ? (
                    <div>
                        <button onClick={() => setIsAuthenticated(false)}>Logout</button>
                    </div>
                ) :
                <main>
                <section className="relative w-full h-full py-40 min-h-screen">
                    <div
                        className="absolute top-0 w-full h-full bg-blueGray-800 bg-no-repeat bg-full bg-[url('../assets/img/register_bg_2.png')]"
                        // style={{
                        //     backgroundImage:
                        //         "url(" + require("../assets/img/register_bg_2.png").default + ")",
                        // }}
                    ></div>
                    <Routes>
                        <Route path="/login" element={<Login onSubmit={handleSubmit} username={username} setUsername={setUsername} password={password} setPassword={setPassword} />} />
                    </Routes>
                    <FooterSmall absolute />
                </section>
            </main>}
        </>
    );
}