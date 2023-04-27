import React, {useEffect, useState} from 'react';
import instance from "../../apis/Auth/Auth";
import CartView from "../../views/carts/CartView";
import Navbar from "../../components/Navbars/IndexNavbar";
const baseURL = "https://localhost:7241/gateway/cart";
const checkoutURL = "https://localhost:7241/gateway/checkout-paypal";
const Cart = () => {
    const [carts, setCarts] = useState([]);

    useEffect(() => {
        instance.get(baseURL)
            .then((response) =>{
                setCarts(response.data);
                console.log(response.data)
            }).catch((error) => {
            console.log(error);
        });

    }, []);



const checkOut = async (e) => {
        e.preventDefault();
        try {
            const response = await instance.post(checkoutURL);
            window.location.href = response.data.approvalUrl;
        } catch (error) {
            console.error(error);
        }
    };
    
    if (!carts) return null;
    return (
        <div>
                <Navbar transparent carts={carts}/>
                <CartView carts={carts} checkout={checkOut}/>
        </div>
    );
};

export default Cart;