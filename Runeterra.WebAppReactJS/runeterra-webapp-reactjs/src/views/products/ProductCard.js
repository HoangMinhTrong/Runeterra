import React, {useState} from 'react';
import {useNavigate} from "react-router-dom";
function ProductCard({product}) {
    const navigate = useNavigate();
    const onHandleClickDetail = () => {
        navigate(`/product/${product.id}`);
    }
    return (
        <div onClick={onHandleClickDetail}>
            <div className="h-120 w-72 rounded shadow-lg mx-auto border border-palette-lighter ">
                <div className="h-72 border-b-2 border-palette-lighter relative">
                    <img className="transform duration-500 ease-in-out hover:scale-110"
                         alt="" src={"https://doggystickers.vercel.app/_next/image?url=https%3A%2F%2Fcdn.shopify.com%2Fs%2Ffiles%2F1%2F2800%2F2014%2Fproducts%2Fmockup-fc750eaa.jpg%3Fv%3D1616988549&w=1920&q=75"}
                         />
                </div>
                <div className="h-48 relative">
                    <div className="font-primary text-palette-primary text-2xl pt-4 px-4 font-semibold">
                        <div key={product.id} type="hide"></div>
                         {product.name}
                    </div>
                    <div className="text-lg text-gray-600 p-4 font-primary font-light">
                        Description
                    </div>
                    <div
                        className="text-palette-dark font-primary font-medium text-base absolute bottom-0 right-0 mb-4 pl-8 pr-4 pb-1 pt-2 bg-palette-lighter
            rounded-tl-sm triangle"
                    >
                        {product.price} $
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ProductCard;