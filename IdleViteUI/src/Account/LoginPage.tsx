import { useActionState, useContext, useState } from "react"
import AccountContext from "./AccountContext";
import FetchAccount from "./FetchAccount";

function LoginPage() {
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const [emailError, setEmailError] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const { setAccount } = useContext(AccountContext);

    const [modelError, handleLogin, isPending] = useActionState<string | null | undefined, FormData>(
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
                    return json.message || response.statusText;
                }
                setAccount(await FetchAccount());
                return null;
            } catch (error) {
                if (error instanceof Error) {
                    return error.message;
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
                                    <a id="forgot-password" asp-page="./ForgotPassword">Forgot your password?</a>
                                </p>
                                <p>
                                    <a asp-page="./Register" asp-route-returnUrl="@Model.ReturnUrl">Register as a new user</a>
                                </p>
                                <p>
                                    <a id="resend-confirmation" asp-page="./ResendEmailConfirmation">Resend email confirmation</a>
                                </p>
                            </div>
                        </form>
                    </section>
                </div>
            </div>
        </>
    );
};

export default LoginPage;