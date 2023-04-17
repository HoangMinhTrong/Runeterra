import {Outlet} from "react-router-dom";
import NavBarAdmin from "./components/Admin/NavBarAdmin";




function AdminMainApp() {
  return (
    <div className="AdminMainApp">
        <NavBarAdmin/>
        <Outlet/>
    </div>
  );
}

export default AdminMainApp
