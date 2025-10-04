import { useContext } from "react";
import AccountContext from "../AccountContext";

function Profile() {
    const { account } = useContext(AccountContext);

    return (
        <>
            <h3>Profile</h3>
            <div className="row">
                <div className="col-md-6">
                    <form>
                        <div className="form-floating mb-3">
                            <input id="email" name="email" type="text" className="form-control" autoComplete="username"
                                aria-required="true" placeholder="name@example.com" value={account?.email}
                                disabled />
                            <label htmlFor="email" className="form-label">Email</label>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
};

export default Profile;