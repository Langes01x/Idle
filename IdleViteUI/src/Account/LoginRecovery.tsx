import { useActionState, useContext, useState, type JSX } from "react";
import AccountContext from "./AccountContext";
import FetchAccount from "./FetchAccount";
import { useLocation } from "react-router";
import type { LoginTwoFactorState } from "./LoginTwoFactor";

function LoginRecovery() {
    const location = useLocation();
    const state = location.state as LoginTwoFactorState;
    const [recoveryCode, setRecoveryCode] = useState('');
    const [recoveryCodeError, setRecoveryCodeError] = useState('');
    const { setAccount } = useContext(AccountContext);

    const [modelError, handleLogin, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            if (!recoveryCode) {
                setRecoveryCodeError("Verification code is required");
                return null;
            }
            setRecoveryCodeError("");

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
            <h1>Two-factor recovery</h1>
            <hr />
            <p>
                You have requested to log in with a recovery code. This login will not be remembered until you provide
                an authenticator app code at log in or disable 2FA and log in again.
            </p>
            <div className="row">
                <div className="col-md-4">
                    <form action={handleLogin}>
                        {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                        <input id="email" name="email" type="hidden" value={state.email} />
                        <input id="password" name="password" type="hidden" value={state.password} />
                        <div className="form-floating mb-3">
                            <input id="twoFactorRecoveryCode" name="twoFactorRecoveryCode" type="text" className="form-control" autoComplete="off"
                                aria-required="true" placeholder="Please enter the code." value={recoveryCode}
                                onChange={(e) => setRecoveryCode(e.target.value)} />
                            <label htmlFor="twoFactorCode" className="form-label">Recovery Code</label>
                            {recoveryCodeError && <span className="text-danger">{recoveryCodeError}</span>}
                        </div>
                        <div>
                            <button type="submit" className="w-100 btn btn-lg btn-primary" disabled={isPending}>Log in</button>
                        </div>
                    </form>
                </div>
            </div>
        </>
    );
};

export default LoginRecovery;