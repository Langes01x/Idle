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
import TwoFactorAuthentication, { TwoFactorLoader } from "./Account/Manage/TwoFactor/TwoFactorAuthentication";
import EnableAuthenticator from "./Account/Manage/TwoFactor/EnableAuthenticator";
import ResetAuthenticator from "./Account/Manage/TwoFactor/ResetAuthenticator";
import GenerateRecoveryCodes from "./Account/Manage/TwoFactor/GenerateRecoveryCodes";
import ProfileRedirect from "./Account/Manage/ProfileRedirect";
import RecoveryCodes from "./Account/Manage/TwoFactor/RecoveryCodes";
import LoginTwoFactor from "./Account/LoginTwoFactor";
import LoginRecovery from "./Account/LoginRecovery";
import GameLayout from "./Game/GameLayout";
import Game from "./Game/Game";

const Router = createBrowserRouter([
    {
        path: "/",
        Component: Layout,
        children: [
            { index: true, Component: Login },
            { path: "register", Component: Register },
            { path: "login", Component: Login },
            { path: "loginTwoFactor", Component: LoginTwoFactor },
            { path: "loginRecovery", Component: LoginRecovery },
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
                    {
                        path: "twoFactorAuthentication",
                        children: [
                            { index: true, Component: TwoFactorAuthentication, loader: TwoFactorLoader },
                            { path: "enableAuthenticator", Component: EnableAuthenticator, loader: TwoFactorLoader },
                            { path: "resetAuthenticator", Component: ResetAuthenticator },
                            { path: "generateRecoveryCodes", Component: GenerateRecoveryCodes },
                            { path: "recoveryCodes", Component: RecoveryCodes },
                        ],
                    },
                ],
            },
            {
                path: "game",
                Component: GameLayout,
                children: [
                    { index: true, Component: Game },
                ],
            },
        ],
    },
]);

export default Router;