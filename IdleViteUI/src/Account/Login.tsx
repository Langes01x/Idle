import { useActionState, useContext, useState, type JSX } from "react"
import AccountContext from "./AccountContext";
import FetchAccount from "./FetchAccount";
import { Link, useNavigate } from "react-router";
import type { LoginTwoFactorState } from "./LoginTwoFactor";

function Login() {
    const navigate = useNavigate();
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [emailError, setEmailError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const { setAccount } = useContext(AccountContext);

    const [modelError, handleLogin, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            if (!email) {
                setEmailError("Email is required");
            } else {
                setEmailError("");
            }
            if (!password) {
                setPasswordError("Password is required");
            } else {
                setPasswordError("");
            }
            if (!email || !password) {
                return null;
            }

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
                    if ('detail' in json && json.detail == "RequiresTwoFactor") {
                        const state: LoginTwoFactorState = {
                            email: formData.get("email")?.toString() || "",
                            password: formData.get("password")?.toString() || "",
                        };
                        navigate("/LoginTwoFactor", { state: state });
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
            <h1>Log in</h1>
            <hr />
            <div className="row">
                <div className="col-md-6">
                    <section>
                        <form action={handleLogin}>
                            {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                            <div className="form-floating mb-3">
                                <input id="email" name="email" type="text" className="form-control" autoComplete="username"
                                    aria-required="true" placeholder="name@example.com" value={email}
                                    onChange={(e) => setEmail(e.target.value)} />
                                <label htmlFor="email" className="form-label">Email</label>
                                {emailError && <span className="text-danger">{emailError}</span>}
                            </div>
                            <div className="form-floating mb-3">
                                <input id="password" name="password" type="password" className="form-control" autoComplete="current-password"
                                    aria-required="true" placeholder="password" value={password}
                                    onChange={(e) => setPassword(e.target.value)} />
                                <label htmlFor="password" className="form-label">Password</label>
                                {passwordError && <span className="text-danger">{passwordError}</span>}
                            </div>
                            <div>
                                <button type="submit" className="w-100 btn btn-lg btn-primary" disabled={isPending}>Log in</button>
                            </div>
                            <div>
                                <p>
                                    <Link to="/forgotPassword">Forgot your password?</Link>
                                </p>
                                <p>
                                    <Link to="/register">Register as a new user</Link>
                                </p>
                                <p>
                                    <Link to="/resendConfirmationEmail">Resend confirmation email</Link>
                                </p>
                            </div>
                        </form>
                    </section>
                </div>
            </div>
        </>
    );
};

export default Login;