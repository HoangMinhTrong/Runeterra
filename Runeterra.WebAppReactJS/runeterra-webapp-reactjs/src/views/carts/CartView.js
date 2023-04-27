import React from 'react';
import {useNavigate} from "react-router-dom";
const CartView = ({carts, checkout}) => {
    const navigate = useNavigate()
    const onHandleClickBack = () => {
        navigate("/");
    }
    const onHandleClickBackProduct = () => {
        navigate(`/product/${carts.id}`);
    }
    const total = carts.reduce((acc, cart) => acc + cart.quantity * cart.price, 0);

    return (
        <div>
            <div className="min-h-80 max-w-2xl my-4 sm:my-8 mx-auto w-full">
                <table className="mx-auto">
                    <thead>
                    <tr className="uppercase text-xs sm:text-sm text-palette-primary border-b border-palette-light">
                        <th className="font-primary font-normal px-6 py-4">Product</th>
                        <th className="font-primary font-normal px-6 py-4">Quantity</th>
                        <th className="font-primary font-normal px-6 py-4 hidden sm:table-cell">Price</th>
                        <th className="font-primary font-normal px-6 py-4">Remove</th>
                    </tr>
                    </thead>
                    <tbody className="divide-y divide-palette-lighter">
                    {
                        carts.map(cart => (
                        <tr className="text-sm sm:text-base text-gray-600 text-center">
                            <td className="font-primary font-medium px-4 sm:px-6 py-4 flex items-center">
                                <img className="transform duration-500 ease-in-out hover:scale-110"
                                     height="64" width="64"
                                     alt="" src={"https://doggystickers.vercel.app/_next/image?url=https%3A%2F%2Fcdn.shopify.com%2Fs%2Ffiles%2F1%2F2800%2F2014%2Fproducts%2Fmockup-fc750eaa.jpg%3Fv%3D1616988549&w=1920&q=75"}
                                />
                                <div className="pt-1 hover:text-palette-dark" onClick={onHandleClickBackProduct}>
                                    <div key={cart.id} type="hide"></div>
                                     {cart.name}, {cart.description}
                                </div>
                            </td>
                            <td className="font-primary font-medium px-4 sm:px-6 py-4">
                                <input
                                    type="number"
                                    inputMode="numeric"
                                    id="variant-quantity"
                                    name="variant-quantity"
                                    min="1"
                                    step="1"
                                    value= {cart.quantity}
                                    className="text-gray-900 form-input border border-gray-300 w-16 rounded-sm focus:border-palette-light focus:ring-palette-light"
                                />
                            </td>
                            <td className="font-primary text-base font-light px-4 sm:px-6 py-4 hidden sm:table-cell">
                                ${cart.price}
                            </td>
                            <td className="font-primary font-medium px-4 sm:px-6 py-4">
                                <button
                                    aria-label="delete-item"
                                    className=""
                                >
                                    X
                                </button>
                            </td>
                        </tr>
                        ))}
                            <tr className="text-center">
                                <td></td>
                                <td className="font-primary text-base text-gray-600 font-semibold uppercase px-4 sm:px-6 py-4">Subtotal</td>
                                <td className="font-primary text-lg text-palette-primary font-medium px-4 sm:px-6 py-4">
                                    {total}
                                </td>
                                <td></td>
                            </tr>
                    </tbody>
                </table>
            </div>
            <div className="max-w-sm mx-auto space-y-4 px-2">
                <div
                    aria-label="checkout-products"
                    className="bg-palette-primary text-white text-lg font-primary font-semibold pt-2 pb-1 leading-relaxed flex
      justify-center items-center focus:ring-1 focus:ring-palette-light focus:outline-none w-full hover:bg-palette-dark rounded-sm"
                    onClick={checkout}
                >
                    Check Out
                </div>
                <div className="flex flex-col justify-between h-full w-full md:w-1/2 max-w-xs mx-auto space-y-4 min-h-128">
                    <div
                        aria-label="back-to-products"
                        className="border border-palette-primary text-palette-primary text-lg font-primary font-semibold pt-2 pb-1 leading-relaxed flex justify-center items-center focus:ring-1 focus:ring-palette-light focus:outline-none w-full hover:bg-palette-lighter rounded-sm"
                        onClick= {onHandleClickBack}
                    >
                        Back To All Products
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CartView;