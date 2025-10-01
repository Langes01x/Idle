import { Outlet } from "react-router";
import ManageNav from "./ManageNav";

function ManageLayout() {
    return (
        <>
            <h1>Manage account</h1>

            <div>
                <h2>Change your account settings</h2>
                <hr />
                <div className="row">
                    <div className="col-md-3">
                        <ManageNav />
                    </div>
                    <div className="col-md-9">
                        <Outlet />
                    </div>
                </div>
            </div>
        </>
    )
};

export default ManageLayout;