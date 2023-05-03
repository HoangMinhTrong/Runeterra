import React, {useState} from 'react';
import Sidebar from "../../components/Sidebar/Sidebar";
import AdminNavbar from "../../components/Navbars/AdminNavbar";
import instance from "../../apis/Auth/Auth";
import CreateProductView from "../../views/products/CreateProductView";
import {toast} from "react-toastify";
import ProductListView from "../../views/products/ProductListView";
const baseURL = "https://localhost:7241/gateway/product";
const CreateProduct = () => {
    const [name, setName] = useState("")
    const [description, setDescription] = useState("")
    const [price, setPrice] = useState(0)
    const [status, setStatus] = useState(true)
    const [quantity, setQuantity] = useState(0)
    const [imageUrl, setImageUrl] = useState("")
    const [productTypeId, setProductTypeId] = useState(0)
    const submitHandle = async (e) => {
        e.preventDefault();
        try {
            const resp = await instance.post(baseURL,{
                name: name,
                description: description,
                price: price,
                status: status,
                quantity: quantity,
                imageUrl: imageUrl,
                productTypeId: productTypeId,
             })
            if (resp.status === 200) {
                console.log(resp.data)
                setName('')
                setDescription('')
                setPrice('')
                setStatus('')
                setQuantity('')
                setImageUrl('')
                setProductTypeId(0)
                toast.success("Add success");
            }
        } catch (error){
            console.log(error.response)
        }
    }

    return (
        <div>
            <Sidebar/>
            <div className="relative md:ml-64 bg-blueGray-100">
                <AdminNavbar/>
                <div className="flex items-center justify-center h-screen">
                    <div className="px-4 md:px-10 w-full">
                            <CreateProductView
                                submitHandle={submitHandle}
                                name={name}
                                setName={setName}
                                description={description}
                                setDescription={setDescription}
                                price={price}
                                setPrice={setPrice}
                                status={status}
                                setStatus={setStatus}
                                quantity={quantity}
                                setQuantity={setQuantity}
                                imageUrl={imageUrl}
                                setImageUrl={setImageUrl}
                                productTypeId={productTypeId}
                                setProductTypeId={setProductTypeId}
                            />
                    </div>
                </div>
            </div>
        </div>
    );
};

export default CreateProduct;