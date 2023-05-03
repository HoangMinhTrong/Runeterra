import React from "react";
import {Link, useNavigate} from "react-router-dom";
import {createPopper} from "@popperjs/core";
import {useJwt} from 'react-jwt';

const IndexDropdown = () => {
    const token = localStorage.getItem('token');
    const {decodedToken, isExpired} = useJwt(token);
    const role = decodedToken?.['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
    // dropdown props

    const [dropdownPopoverShow, setDropdownPopoverShow] = React.useState(false);
    const btnDropdownRef = React.createRef();
    const popoverDropdownRef = React.createRef();
    const navigate = useNavigate()
    const openDropdownPopover = () => {


        createPopper(btnDropdownRef.current, popoverDropdownRef.current, {
            placement: "bottom-start",
        });
        setDropdownPopoverShow(true);
    };
    const closeDropdownPopover = () => {
        setDropdownPopoverShow(false);
    };
    return (<>
        <a
            className="hover:text-blueGray-500 text-blueGray-700 px-3 py-4 lg:py-2 flex items-center text-xs uppercase font-bold"
            href="#pablo"
            ref={btnDropdownRef}
            onClick={(e) => {
                e.preventDefault();
                dropdownPopoverShow ? closeDropdownPopover() : openDropdownPopover();
            }}
        >
            Shortcut keys
        </a>
        <div
            ref={popoverDropdownRef}
            className={(dropdownPopoverShow ? "block " : "hidden ") + "bg-white text-base z-50 float-left py-2 list-none text-left rounded shadow-lg min-w-48"}
        >
            {role === "Admin" && (<>
        <span
            className={"text-sm pt-2 pb-0 px-4 font-bold block w-full whitespace-nowrap bg-transparent text-blueGray-400"}
        >
          Admin Layout
        </span>

                <Link
                    to="/admin/dashboard"
                    className="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700"
                >
                    Dashboard
                </Link>
                <Link
                    to="/admin/settings"
                    className="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700"
                >
                    Settings
                </Link>
                <Link
                    to="/admin/tables"
                    className="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700"
                >
                    Tables
                </Link>
                <Link
                    to="/admin/maps"
                    className="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700"
                >
                    Maps
                </Link>
            </>)}

            <div className="h-0 mx-4 my-2 border border-solid border-blueGray-100"/>
            <span
                className={"text-sm pt-2 pb-0 px-4 font-bold block w-full whitespace-nowrap bg-transparent text-blueGray-400"}
            >
          Layout
        </span>
            <div
                onClick={() => navigate("/order-detail")}
                className="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700"
            >
                My Order
            </div>
            <Link
                to="/auth/register"
                className="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700"
            >
                Register
            </Link>
            <div className="h-0 mx-4 my-2 border border-solid border-blueGray-100"/>
            <span
                className={"text-sm pt-2 pb-0 px-4 font-bold block w-full whitespace-nowrap bg-transparent text-blueGray-400"}
            >
          No Layout
        </span>
            <Link
                to="/landing"
                className="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700"
            >
                Landing
            </Link>
            <Link
                to="/profile"
                className="text-sm py-2 px-4 font-normal block w-full whitespace-nowrap bg-transparent text-blueGray-700"
            >
                Profile
            </Link>
        </div>
    </>);
};

export default IndexDropdown;
