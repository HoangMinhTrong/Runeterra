import React from 'react';
import Sidebar from "../../components/Sidebar/Sidebar";
import AdminNavbar from "../../components/Navbars/AdminNavbar";
import instance from "../../apis/Auth/Auth";
import ProductListView from "../../views/products/ProductListView";
const baseURL = "https://localhost:7241/gateway/get-by-store";
const ProductList = () => {
    const [product, setProduct] = React.useState([]);

    React.useEffect(() => {
        instance.get(baseURL).then((response) => {
            setProduct(response.data);
            console.log(response.data)
        });
    }, []);

    if (!product) return null;
    return (
        <div>
            <Sidebar/>
            <div className="relative md:ml-64 bg-blueGray-100">
                <AdminNavbar/>
                <div className="flex items-center justify-center h-screen">
                    <div className="px-4 md:px-10 w-full">
                        <ProductListView product={product}/>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ProductList;