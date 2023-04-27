import React from 'react';
import "../../assets/styles/homepage.css"
import {useNavigate} from "react-router-dom";
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
function ProductDetailView(props) {
    const {product , setQuantity, quantity, onSubmit} = props;
    const navigate = useNavigate();
    const handleSubmit = (e) => {
        e.preventDefault();
        onSubmit(e);
    };
    const onHandleClickBack = () => {
        navigate("/");
    }

    return (
        <div className="min-h-screen py-12 sm:pt-20">
            <div className="flex flex-col justify-center items-center md:flex-row md:items-start space-y-8 md:space-y-0 md:space-x-4 lg:space-x-8 max-w-6xl w-11/12 mx-auto mt-24">
                <div className="w-full md:w-1/2 max-w-md border border-palette-lighter bg-white rounded shadow-lg">
                    <div className="relative h-96">
                        <img className="transform duration-500 ease-in-out hover:scale-110"
                             alt="" src={"https://doggystickers.vercel.app/_next/image?url=https%3A%2F%2Fcdn.shopify.com%2Fs%2Ffiles%2F1%2F2800%2F2014%2Fproducts%2Fmockup-fc750eaa.jpg%3Fv%3D1616988549&w=1920&q=75"}
                        />
                    </div>
                </div>
                <div className="flex flex-col justify-between h-full w-full md:w-1/2 max-w-xs mx-auto space-y-4 min-h-128">
                    <div
                    aria-label="back-to-products"
                    className="border border-palette-primary text-palette-primary text-lg font-primary font-semibold pt-2 pb-1 leading-relaxed flex justify-center items-center focus:ring-1 focus:ring-palette-light focus:outline-none w-full hover:bg-palette-lighter rounded-sm"
                    onClick= {onHandleClickBack}
                    >
                        Back To All Products
                </div>
                    <div className=" font-primary">
                        <h1 className="leading-relaxed font-extrabold text-3xl text-palette-primary py-2 sm:py-4">
                            {product.name}
                        </h1>
                        <p className="font-medium text-lg">
                            Description
                        </p>
                        <div className="text-xl text-palette-primary font-medium py-4 px-1">
                            ${product.price}
                        </div>
                    </div>
                    <form onSubmit={handleSubmit}>
                    <div className="w-full">
                        <div className="flex justify-start space-x-2 w-full">
                            <div className="flex flex-col items-start space-y-1 flex-grow-0">
                                <label className="text-gray-500 text-base">Qty.</label>
                                <input
                                    value={quantity}
                                    onChange={setQuantity}
                                    className="text-gray-900 form-input border border-gray-300 w-16 rounded-sm focus:border-palette-light focus:ring-palette-light"
                                />
                            </div>
                        </div>
                        <button
                            aria-label="cart-button"
                            className="pt-3 pb-2 bg-palette-primary text-white w-full mt-2 rounded-sm font-primary font-semibold text-xl flex
                      justify-center items-baseline  hover:bg-palette-dark cursor-none"
                            type="submit"
                        >
                            Add To Cart
                        </button>
                        <ToastContainer />
                    </div>
                    </form>
                </div>

            </div>
            </div>
    );
}

export default ProductDetailView;