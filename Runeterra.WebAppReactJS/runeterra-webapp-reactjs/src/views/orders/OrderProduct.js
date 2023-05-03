import React from 'react';

const OrderProduct = ({products}) => {
    return (
        <div>
            <div className="bg-gray-50 min-h-screen">
                <div className="max-w-7xl mx-auto pt-8 pb-16 px-4 sm:px-6 lg:px-8">
                    <h1 className="text-3xl font-bold text-gray-900 mb-8">Your Order Product Details</h1>
                 <div className="border-t border-gray-200">
                    <div className="space-y-4">
                    {
                        products.map(prod => (
                            <div className="bg-white shadow overflow-hidden rounded-lg">
                            <dl className="sm:divide-y sm:divide-gray-200">
                                <div className="py-3 sm:py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                                    <dt className="text-sm font-medium text-gray-500">Product Name</dt>
                                    <dd className="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2"> {prod.product.name}</dd>
                                </div>
                                <div className="py-3 sm:py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                                    <dt className="text-sm font-medium text-gray-500">Price</dt>
                                    <dd className="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2"> {prod.product.price}</dd>
                                </div>

                                <div className="py-3 sm:py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                                    <dt className="text-sm font-medium text-gray-500">Quantity</dt>
                                    <dd className="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2"> {prod.quantity}</dd>
                                </div>

                                <div className="py-3 sm:py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                                    <dt className="text-sm font-medium text-gray-500">Sub Total</dt>
                                    <dd className="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2"> ${prod.product.price * prod.quantity}</dd>
                                </div>
                                <div className="py-3 sm:py-4 sm:grid sm:grid-cols-3 sm:gap-4 sm:px-6">
                                    <dt className="text-sm font-medium text-gray-500">Image</dt>
                                    <dd className="mt-1 text-sm text-gray-900 sm:mt-0 sm:col-span-2">
                                        <img className="h-10 w-10 rounded-full"
                                             alt="" src={"https://doggystickers.vercel.app/_next/image?url=https%3A%2F%2Fcdn.shopify.com%2Fs%2Ffiles%2F1%2F2800%2F2014%2Fproducts%2Fmockup-fc750eaa.jpg%3Fv%3D1616988549&w=1920&q=75"}
                                        />
                                    </dd>
                                </div>
                            </dl>
                            </div>
                        ))
                    }
                    </div>
                </div>
            </div>
            </div>
        </div>
    );
};

export default OrderProduct;