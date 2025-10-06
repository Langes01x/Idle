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
                <NavLink className="nav-link" to="/manage/twoFactorAuthentication">Two-factor authentication</NavLink>
            </li>
            <li className="nav-item">
                <NavLink className="nav-link" to="/manage/personalData">Personal data</NavLink>
            </li>
        </ul>
    );
};

export default ManageNav;