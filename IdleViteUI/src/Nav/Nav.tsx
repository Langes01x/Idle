import './Nav.css'
import { useContext } from 'react';
import AccountContext from '../Account/AccountContext';
import { NavLink, useNavigate } from 'react-router';

function Nav() {
    const { account, setAccount } = useContext(AccountContext);
    const navigate = useNavigate();

    async function handleLogout() {
        try {
            const response = await fetch("/api/logout", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                credentials: "include",
            });
            if (response.ok) {
                setAccount(null);
                navigate("/login");
            }
        } catch {
            // Do nothing on logout failure
        }
    };

    return (
        <nav className="navbar navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3">
            <div className="container-fluid">
                <a className="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">Idle</a>
                <button className="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target=".navbar-collapse"
                    aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
                    <span className="navbar-toggler-icon"></span>
                </button>
                <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
                    <ul className="navbar-nav flex-grow-1">
                        {
                            account &&
                            <li className="nav-item">
                                <a className="nav-link" asp-area="" asp-controller="Game" asp-action="Index">Game</a>
                            </li>
                        }
                    </ul>
                    <ul className="navbar-nav">
                        {
                            account ?
                                <>
                                    <li className="nav-item">
                                        <NavLink className="nav-link" to="/manage">{account.email}</NavLink>
                                    </li>
                                    <li className="nav-item">
                                        <form className="form-inline" action={handleLogout}>
                                            <button type="submit" className="nav-link btn btn-link">Logout</button>
                                        </form>
                                    </li>
                                </> :
                                <>
                                    <li className="nav-item">
                                        <NavLink className="nav-link" to="/register">Register</NavLink>
                                    </li>
                                    <li className="nav-item">
                                        <NavLink className="nav-link" to="/login">Login</NavLink>
                                    </li>
                                </>
                        }
                    </ul>
                </div>
            </div>
        </nav>
    );
};

export default Nav;