import { useActionState, useContext, useState, type JSX } from "react";
import AccountContext from "./AccountContext";
import FetchAccount from "./FetchAccount";
import { Link, useLocation } from "react-router";

export type LoginTwoFactorState = {
    email: string,
    password: string,
};

function LoginTwoFactor() {
    const location = useLocation();
    const state = location.state as LoginTwoFactorState;
    const [twoFactorCode, setTwoFactorCode] = useState('');
    const [twoFactorCodeError, setTwoFactorCodeError] = useState('');
    const { setAccount } = useContext(AccountContext);

    const [modelError, handleLogin, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            if (!twoFactorCode) {
                setTwoFactorCodeError("Verification code is required");
                return null;
            }
            setTwoFactorCodeError("");

            try {
                const response = await fetch("/api/login?useCookies=true", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(Object.fromEntries(formData)),
                    credentials: "include",
                });
                if (!response.ok) {
                    const json = await response.json();
                    if ('errors' in json) {
                        return (<>{Object.values<string>(json.errors).map((val: string) => { return (<p>{val}</p>); })}</>);
                    }
                    return (<p>{response.statusText}</p>);
                }
                setAccount(await FetchAccount());
                // TODO: Redirect to game page
                return null;
            } catch (error) {
                if (error instanceof Error) {
                    return (<p>{error.message}</p>);
                }
            }
        },
        null,
    );

    return (
        <>
            <h1>Two-factor authentication</h1>
            <hr />
            <p>Your login is protected with an authenticator app. Enter your authenticator code below.</p>
            <div className="row">
                <div className="col-md-4">
                    <form action={handleLogin}>
                        {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                        <input id="email" name="email" type="hidden" value={state.email} />
                        <input id="password" name="password" type="hidden" value={state.password} />
                        <div className="form-floating mb-3">
                            <input id="twoFactorCode" name="twoFactorCode" type="text" className="form-control" autoComplete="off"
                                aria-required="true" placeholder="Please enter the code." value={twoFactorCode}
                                onChange={(e) => setTwoFactorCode(e.target.value)} />
                            <label htmlFor="twoFactorCode" className="form-label">Verification Code</label>
                            {twoFactorCodeError && <span className="text-danger">{twoFactorCodeError}</span>}
                        </div>
                        <div>
                            <button type="submit" className="w-100 btn btn-lg btn-primary" disabled={isPending}>Log in</button>
                        </div>
                    </form>
                </div>
            </div>
            <p>
                Don't have access to your authenticator device? You can&nbsp;
                <Link to="/loginRecovery" state={{ email: state.email, password: state.password }}>log in with a recovery code</Link>.
            </p>
        </>
    );
};

export default LoginTwoFactor;