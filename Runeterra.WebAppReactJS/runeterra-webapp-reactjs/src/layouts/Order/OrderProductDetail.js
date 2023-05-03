import React, {useEffect, useState} from 'react';
import instance from "../../apis/Auth/Auth";
import {useParams} from "react-router-dom";

import OrderProduct from "../../views/orders/OrderProduct";

const OrderProductDetail = () => {
    const { orderId } = useParams();
    const [products, setProducts] = useState([]);

    useEffect(() => {
        instance.get(`https://localhost:6001/api/Order/${orderId}/products`)
            .then((response) =>{
                setProducts(response.data);
                console.log(response.data)
            }).catch((error) => {
            console.log(error);
        });

    }, []);
    return (
        <div>
                        <OrderProduct products={products}/>
        </div>
    );
};

export default OrderProductDetail;