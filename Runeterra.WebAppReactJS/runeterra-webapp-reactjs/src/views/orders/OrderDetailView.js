import React from 'react';
import {useNavigate} from "react-router-dom";

const OrderDetailView = (props) => {
    const {orderDetail} = props;
    const navigate = useNavigate();
    const onHandleClickDetail = () => {
        navigate(`/order/${orderDetail.id}/products`);
    }
    return (
        <div>
            <div onClick={onHandleClickDetail} className="bg-white shadow overflow-hidden rounded-lg">
                            <dl className="sm:divide-y sm:divide-gray-200">
                                <div className="py-3 sm:py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                                    <dt className="text-sm font-medium text-gray-500">Order ID</dt>
                                    <dd className="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">{orderDetail.id}</dd>
                                </div>
                                <div className="py-3 sm:py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                                    <dt className="text-sm font-medium text-gray-500">Total Amount</dt>
                                    <dd className="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">${orderDetail.total}</dd>
                                </div>
                                <div className="py-3 sm:py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                                    <dt className="text-sm font-medium text-gray-500">Status</dt>
                                    <dd className="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">Completed</dd>
                                </div>
                            </dl>
            </div>
        </div>
    );
};

export default OrderDetailView;