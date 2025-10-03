import { NavLink } from "react-router";

function ManageNav() {
    return (
        <ul className="nav nav-pills flex-column">
            <li className="nav-item">
                <NavLink className="nav-link" to="/manage/profile">Profile</NavLink>
            </li>
            <li className="nav-item">
                <NavLink className="nav-link" to="/manage/email">Email</NavLink>
            </li>
            <li className="nav-item">
                <NavLink className="nav-link" to="/manage/changePassword">Password</NavLink>
            </li>
            <li className="nav-item">
                <a className="nav-link @ManageNavPages.TwoFactorAuthenticationNavClass(ViewContext)"
                    id="two-factor" asp-page="./TwoFactorAuthentication">Two-factor authentication</a>
            </li>
            <li className="nav-item">
                <a className="nav-link @ManageNavPages.PersonalDataNavClass(ViewContext)" id="personal-data"
                    asp-page="./PersonalData">Personal data</a>
            </li>
        </ul>
    );
};

export default ManageNav;