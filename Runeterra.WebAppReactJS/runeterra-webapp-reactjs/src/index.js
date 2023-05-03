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
import Cart from "./layouts/Cart/Cart";
import Order from "./layouts/Order/Order";
import OrderProductDetail from "./layouts/Order/OrderProductDetail";
import StoreOwnerPage from "./layouts/StoreOwner/StoreOwnerPage";
import CreateProduct from "./layouts/StoreOwner/CreateProduct";
import ProductList from "./layouts/StoreOwner/ProductList";


const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(

  <React.StrictMode>
      <BrowserRouter>
          <Routes>
              <Route path="/" element={<HomePage/>} />
              <Route path="/product/:id" element={<ProductDetail/>} />
              <Route path="/auth/*" element={<Auth />} />
              <Route path="/cart" element={<Cart />} />
              <Route path="/order-detail" element={<Order />} />
              <Route path="/order/:orderId/products" element={<OrderProductDetail />} />
          </Routes>
          <Routes>
              <Route path="/storeowner/" element={<StoreOwnerPage/>} />
              <Route path="/storeowner/createproduct" element={<CreateProduct/>} />
              <Route path="/storeowner/productlist" element={<ProductList/>} />
          </Routes>
      </BrowserRouter>
  </React.StrictMode>,
);


