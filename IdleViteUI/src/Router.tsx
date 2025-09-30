import { createBrowserRouter } from "react-router";
import Layout from "./Layout";
import Login from "./Account/Login";
import Register from "./Account/Register";

const Router = createBrowserRouter([
    {
        path: "/",
        Component: Layout,
        children: [
            { index: true, Component: Login },
            { path: "register", Component: Register },
            { path: "login", Component: Login },
        ],
    },
]);

export default Router;