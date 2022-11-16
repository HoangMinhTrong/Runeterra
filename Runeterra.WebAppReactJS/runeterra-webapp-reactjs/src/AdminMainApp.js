import {Outlet} from "react-router-dom";
import NavBarAdmin from "./components/Admin/NavBarAdmin";
import FooterAdmin from "./components/Admin/FooterAdmin";
import HomePageAdmin from "./components/Admin/HomePageAdmin";




function AdminMainApp() {
  return (
    <div className="AdminMainApp">
        <NavBarAdmin/>
        <Outlet/>
    </div>
  );
}

export default AdminMainApp
