import React, {useEffect, useState} from 'react';
import instance from "../../apis/Auth/Auth";
import OrderDetailView from "../../views/orders/OrderDetailView";

const baseURL = "https://localhost:7241/gateway/get-orders";
const Order = (props) => {
    const [orderDetails, setOrderDetail] = useState([]);


    useEffect(() => {
        instance.get(baseURL)
            .then((response) =>{
                setOrderDetail(response.data);
                console.log(response.data)
            }).catch((error) => {
            console.log(error);
        });

    }, []);


    if (!orderDetails) return null;
    return (
        <div>
            <div className="bg-gray-50 min-h-screen">
                <div className="max-w-7xl mx-auto pt-8 pb-16 px-4 sm:px-6 lg:px-8">
                    <h1 className="text-3xl font-bold text-gray-900 mb-8">Your Order Details</h1>
                    <div className="space-y-4">
                            {
                                orderDetails.map(orderDetail => (
                                    <OrderDetailView orderDetail={orderDetail} />
                                ))
                            }
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Order;