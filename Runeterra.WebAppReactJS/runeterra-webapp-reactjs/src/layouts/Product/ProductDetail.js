import React, {useEffect, useState} from 'react';
import instance from "../../apis/Auth/Auth.js"
import ProductDetailView from "../../views/products/ProductDetailView";
import {Route, Routes, useParams} from "react-router-dom";
import Navbar from "../../components/Navbars/AuthNavbar";
import FooterSmall from "../../components/Footers/FooterSmall";
import { toast } from 'react-toastify';
const baseURL = `https://localhost:6001/api/product`;
const cartURL = "https://localhost:7241/gateway/cart";
const ProductDetail = () => {
    const [product, setProduct] = useState({});
    const [quantity, setQuantity] = useState(1);
    const { id } = useParams();
    useEffect(() => {
        instance.get(`${baseURL}/${id}`)
            .then((response) =>{
                setProduct(response.data);
                console.log(response.data)
            }).catch((error) => {
            console.log(error);
        });
    }, [id]);
    const handleAddToCart = async (e) => {
        e.preventDefault();
        try {
            const response = await instance.post(cartURL, {
                quantity: quantity,
                productId: id,
            });
            if (response.status === 200 || response.status === 201) {
               toast.success('Add to cart success!')
            }
        } catch (error) {
            console.error(error);
        }
    };
    return (
        <div>
            <Navbar transparent />
            {
                <Routes>
                    <Route path={"/"} element={<ProductDetailView product={product} onSubmit={handleAddToCart} setQuantity={setQuantity} quantity={quantity}/>} />
                </Routes>
            }
            <FooterSmall absolute />
        </div>
    );
};

export default ProductDetail;