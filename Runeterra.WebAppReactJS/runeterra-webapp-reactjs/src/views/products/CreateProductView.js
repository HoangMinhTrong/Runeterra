import React from 'react';
import Button from "react-bootstrap/Button";
import {ToastContainer} from "react-toastify";

const CreateProductView = ({
                               submitHandle,
                               name,
                               setName,
                               description,
                               setDescription,
                               price,
                               setPrice,
                               status,
                               setStatus,
                               quantity,
                               setQuantity,
                               imageUrl,
                               setImageUrl,
                               productTypeId,
                               setProductTypeId,
                           }) => {
    const options = [
        { id: 1, name: "Manga" },
        { id: 2, name: "Comic" },
        { id: 3, name: "Novel" },
    ];
    const optionsTrue = [
        { value: true, name: "Avai" },
        { value: false, name: "Out" },
    ];
    return (
        <div>
            <form className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4" onSubmit={submitHandle}>

                <div className="mb-4">
                    <label className="block text-gray-700 font-bold mb-2" htmlFor="name">
                        Product Name
                    </label>
                    <input
                        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="name"
                        type="text"
                        placeholder="Enter product name"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 font-bold mb-2" htmlFor="description">
                        Description
                    </label>
                    <textarea
                        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="description"
                        placeholder="Enter product description"
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 font-bold mb-2" htmlFor="price">
                        Price
                    </label>
                    <input
                        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="price"
                        type="number"
                        placeholder="Enter product price"
                        value={price}
                        onChange={(e) => setPrice(e.target.value)}
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 font-bold mb-2" htmlFor="status">
                        Status
                    </label>
                    <select
                        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="status"
                        value={status}
                        onChange={(e) => setStatus(e.target.value)}
                    >
                        {optionsTrue.map((option) => (
                            <option key={option.name} value={option.value}>
                                {option.name}
                            </option>
                        ))}
                    </select>
                    <p>Selected option: {status}</p>
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 font-bold mb-2" htmlFor="price">
                        Quantity
                    </label>
                    <input
                        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="quantity"
                        type="number"
                        placeholder="Enter product quantity"
                        value={quantity}
                        onChange={(e) => setQuantity(e.target.value)}
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 font-bold mb-2" htmlFor="name">
                        Image
                    </label>
                    <input
                        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="image"
                        type="text"
                        placeholder="Enter product name"
                        value={imageUrl}
                        onChange={(e) => setImageUrl(e.target.value)}
                    />
                </div>
                <div className="mb-4">
                    <label className="block text-gray-700 font-bold mb-2" htmlFor="type">
                        Product Type
                    </label>
                    <select
                        className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                        id="type"
                        value={productTypeId}
                        onChange={(e) => setProductTypeId(e.target.value)}
                    >
                        {options.map((option) => (
                            <option key={option.name} value={option.id}>
                                {option.name}
                            </option>
                        ))}
                    </select>
                    <p>Selected option: {productTypeId}</p>
                </div>
                <div className="flex justify-center mt-5">
                    <button
                        type='submit'
                        className='bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 border border-blue-700 rounded'
                    >
                        Add Product
                    </button>
                    <ToastContainer />
                </div>
            </form>
        </div>
    );
};

export default CreateProductView;