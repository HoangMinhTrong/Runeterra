import axios from "axios";
import React from "react";
import 'bootstrap/dist/css/bootstrap.min.css';

const baseURL = "https://localhost:7241/gateway/product";

export default function HomePage(){
    const [product, setProduct] = React.useState([]);

    React.useEffect(() => {
        axios.get(baseURL).then((response) => {
            setProduct(response.data);
            console.log(response.data)
        });
    }, []);

    if (!product) return null;

    return (
        <>
                    <section className="py-5">
                        <div className="container px-4 px-lg-5 mt-5">
                            <div className="row gx-4 gx-lg-5 row-cols-2 row-cols-md-3 row-cols-xl-4 justify-content-center">
                                {
                                    product.map(prod => (
                                        <div className="col mb-5">
                                            <div className="card h-100">
                                                <img className="card-img-top" src={prod.image.url}
                                                     alt="..."/>
                                                <div className="card-body p-4">
                                                    <div className="text-center">
                                                        <h5 className="fw-bolder" key={prod.id}>{prod.name}</h5>
                                                        <h6 className="fw-light">({prod.productType.name})</h6>
                                                        {prod.price} $
                                                    </div>
                                                </div>
                                                <div className="card-footer p-4 pt-0 border-top-0 bg-transparent">
                                                    <div className="text-center"><a className="btn btn-outline-dark mt-auto" href="src/components/User/HomePage#">View
                                                        options</a></div>
                                                </div>
                                            </div>
                                        </div>
                                    ))
                                }
                            </div>
                        </div>
                    </section>

        </>
    );
}