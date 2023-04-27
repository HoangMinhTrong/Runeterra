import React from 'react';
import ReactDOM from 'react-dom/client';
import {BrowserRouter} from "react-router-dom";
import "@fortawesome/fontawesome-free/css/all.min.css";
import "./assets/styles/tailwind.css";
import "./assets/styles/index.css"
import {Routes, Route} from "react-router-dom";
import HomePage from "./layouts/Product/HomePage";
import Auth from "./layouts/Auth.js"
import ProductDetail from "./layouts/Product/ProductDetail";
import CartView from "./views/carts/CartView";
import Cart from "./layouts/Cart/Cart";

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
      <BrowserRouter>
          <Routes>
              <Route path="/" element={<HomePage/>} />
              <Route path="/product/:id" element={<ProductDetail/>} />
              <Route path="/auth/*" element={<Auth />} />
              <Route path="/cart" element={<Cart />} />
          </Routes>
      </BrowserRouter>
  </React.StrictMode>,
);


