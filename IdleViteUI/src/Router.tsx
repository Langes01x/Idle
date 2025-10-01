import { createBrowserRouter } from "react-router";
import Layout from "./Layout";
import Login from "./Account/Login";
import Register from "./Account/Register";
import ForgotPassword from "./Account/ForgotPassword";
import ResendConfirmationEmail from "./Account/ResendConfirmationEmail";
import ManageLayout from "./Account/Manage/ManageLayout";
import Profile from "./Account/Manage/Profile";

const Router = createBrowserRouter([
    {
        path: "/",
        Component: Layout,
        children: [
            { index: true, Component: Login },
            { path: "register", Component: Register },
            { path: "login", Component: Login },
            { path: "forgotPassword", Component: ForgotPassword },
            { path: "resendConfirmationEmail", Component: ResendConfirmationEmail },
            {
                path: "manage",
                Component: ManageLayout,
                children: [
                    { index: true, Component: Profile },
                ],
            },
        ],
    },
]);

export default Router;