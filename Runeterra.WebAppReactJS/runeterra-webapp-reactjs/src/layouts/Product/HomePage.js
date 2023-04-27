
import React from "react";
import instance from "../../apis/Auth/Auth.js"
import "../../assets/styles/homepage.css"
import ProductCard from "../../views/products/ProductCard";
import Navbar from "../../components/Navbars/IndexNavbar";
import FooterSmall from "../../components/Footers/FooterSmall";
const baseURL = "https://localhost:7241/gateway/product";

export default function HomePage({prod}){
    const [product, setProduct] = React.useState([]);

    React.useEffect(() => {
        instance.get(baseURL).then((response) => {
            setProduct(response.data);
            console.log(response.data)
        });
    }, []);

    if (!product) return null;

    return (
        <>
            <Navbar transparent />
            <h1 className="leading-relaxed font-primary font-extrabold text-4xl text-center text-palette-primary mt-4 py-2 sm:py-4 mt-12">Welcome to Runeterra!</h1>
            <section className="flex justify-center items-center">
                <div className="container px-4 px-lg-5 mt-5 shadow-xl">
                    <div className="py-12 max-w-6xl mx-auto grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-x-4 gap-y-8">
                        {
                            product.map(prod => (
                                <ProductCard key={prod.id} product={prod} />
                            ))
                        }
                    </div>
                </div>
            </section>
            <FooterSmall absolute />
        </>

    );
}