import { createBrowserRouter } from "react-router";
import Layout from "./Layout";
import Login from "./Account/Login";
import Register from "./Account/Register";
import ForgotPassword from "./Account/ForgotPassword";
import ResendConfirmationEmail from "./Account/ResendConfirmationEmail";
import ManageLayout from "./Account/Manage/ManageLayout";
import Profile from "./Account/Manage/Profile";
import Email, { EmailLoader } from "./Account/Manage/Email";
import ChangePassword from "./Account/Manage/ChangePassword";
import ProfileRedirect from "./Account/Manage/ProfileRedirect";

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
                    { index: true, loader: ProfileRedirect },
                    { path: "profile", Component: Profile },
                    { path: "email", Component: Email, loader: EmailLoader },
                    { path: "changePassword", Component: ChangePassword },
                ],
            },
        ],
    },
]);

export default Router;