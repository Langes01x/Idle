import { Outlet, useNavigate } from "react-router";
import { useContext } from "react";
import AccountContext from "../Account/AccountContext";

function GameLayout() {
    const { account } = useContext(AccountContext);
    const navigate = useNavigate();

    if (account === null) {
        navigate("/login");
    }

    return (
        <Outlet />
    )
};

export default GameLayout;