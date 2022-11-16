import React from 'react';
import ReactDOM from 'react-dom/client';
import {BrowserRouter} from "react-router-dom";
import './index.css';
import App from './App';
import AdminMainApp from "./AdminMainApp";
import {Routes, Route} from "react-router-dom";
import About from "./components/User/About";
import HomePage from "./components/User/HomePage";
import CreateProduct from "./components/Admin/CreateProduct";
import HomePageAdmin from "./components/Admin/HomePageAdmin";



const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
      <BrowserRouter>
          <Routes>
              <Route element={<App/>}>
                  <Route path="/" element={<HomePage/>} />
                  <Route path="/about" element={<About/>} />
              </Route>
              <Route element={<AdminMainApp/>}>
                  <Route path="/admin" element={<HomePageAdmin/>} />
                  <Route path="/admin/createproduct" element={<CreateProduct/>} />
              </Route>
          </Routes>
      </BrowserRouter>
  </React.StrictMode>
);


