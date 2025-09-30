import { createBrowserRouter } from "react-router";
import Layout from "./Layout";
import LoginPage from "./Account/LoginPage";

const Router = createBrowserRouter([
    {
        path: "/",
        Component: Layout,
        children: [
            { index: true, Component: LoginPage }
        ]
    }
]);

export default Router;