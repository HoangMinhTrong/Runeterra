import {Outlet} from "react-router-dom";
import NavBar from "./components/User/NavBar";
import Footer from "./components/User/Footer";
import Header from "./components/User/Header";


function App() {
  return (
    <div className="App">
        <NavBar/>
        <Header/>
        <Outlet />
        <Footer/>
    </div>
  );
}

export default App;
