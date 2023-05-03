import React from 'react';
import AdminNavbar from "../../components/Navbars/AdminNavbar";
import Sidebar from "../../components/Sidebar/Sidebar";

const StoreOwnerPage = () => {
    return (
        <div>
            <Sidebar/>
            <div className="relative md:ml-64 bg-blueGray-100">
                <AdminNavbar/>
                <div className="relative bg-lightBlue-600 md:pt-32 pb-32 pt-12">
                    <div className="px-4 md:px-10 mx-auto w-full">
                        <h1 className="leading-relaxed font-primary font-extrabold text-4xl text-center text-palette-primary mt-4 py-2 sm:py-4 mt-12">Welcome to Runeterra!</h1>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default StoreOwnerPage;