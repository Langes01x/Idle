import { createBrowserRouter } from "react-router";
import Layout from "./Layout";
import Login from "./Account/Login";
import Register from "./Account/Register";
import ForgotPassword from "./Account/ForgotPassword";
import ResendConfirmationEmail from "./Account/ResendConfirmationEmail";
import ManageLayout from "./Account/Manage/ManageLayout";
import Profile from "./Account/Manage/Profile";
import Email from "./Account/Manage/Email";
import ChangePassword from "./Account/Manage/ChangePassword";
import TwoFactorAuthentication from "./Account/Manage/TwoFactor/TwoFactorAuthentication";
import EnableAuthenticator from "./Account/Manage/TwoFactor/EnableAuthenticator";
import ResetAuthenticator from "./Account/Manage/TwoFactor/ResetAuthenticator";
import GenerateRecoveryCodes from "./Account/Manage/TwoFactor/GenerateRecoveryCodes";
import ProfileRedirect from "./Account/Manage/ProfileRedirect";
import RecoveryCodes from "./Account/Manage/TwoFactor/RecoveryCodes";
import LoginTwoFactor from "./Account/LoginTwoFactor";
import LoginRecovery from "./Account/LoginRecovery";
import GameLayout from "./Game/GameLayout";
import Game from "./Game/Game";
import FetchAccountInfo from "./Account/Manage/FetchAccountInfo";
import Fetch2faInfo from "./Account/Manage/TwoFactor/Fetch2faInfo";
import Characters, { CharactersLoader } from "./Game/Characters";
import PersonalData from "./Account/Manage/PersonalData";
import Character, { CharacterLoader } from "./Game/Character";

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
                    { path: "email", Component: Email, loader: FetchAccountInfo },
                    { path: "changePassword", Component: ChangePassword },
                    {
                        path: "twoFactorAuthentication",
                        children: [
                            { index: true, Component: TwoFactorAuthentication, loader: Fetch2faInfo },
                            { path: "enableAuthenticator", Component: EnableAuthenticator, loader: Fetch2faInfo },
                            { path: "resetAuthenticator", Component: ResetAuthenticator },
                            { path: "generateRecoveryCodes", Component: GenerateRecoveryCodes },
                            { path: "recoveryCodes", Component: RecoveryCodes },
                        ],
                    },
                    { path: "personalData", Component: PersonalData },
                ],
            },
            {
                path: "game",
                Component: GameLayout,
                children: [
                    { index: true, Component: Game },
                    { path: "characters", Component: Characters, loader: CharactersLoader },
                    { path: "characters/:id", Component: Character, loader: CharacterLoader },
                ],
            },
        ],
    },
]);

export default Router;