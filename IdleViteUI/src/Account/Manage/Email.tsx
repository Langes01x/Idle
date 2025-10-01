import { useLoaderData } from "react-router";
import FetchAccountInfo from "./FetchAccountInfo";
import { useActionState, useState, type JSX } from "react";

export async function EmailLoader() {
    return {
        accountInfo: await FetchAccountInfo()
    };
};

function Email() {
    const { accountInfo } = useLoaderData();
    const [email, setEmail] = useState('');
    const [emailError, setEmailError] = useState('');

    const [emailModelError, handleChangeEmail, isEmailPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            if (!email) {
                setEmailError("Email is required");
                return null;
            }
            setEmailError("");

            try {
                const response = await fetch("/api/manage/info", {
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
                return (<p className="text-success">Check email to complete verification</p>);
            } catch (error) {
                if (error instanceof Error) {
                    return (<p>{error.message}</p>);
                }
            }
        },
        null,
    );

    const [confirmModelError, handleConfirmEmail, isConfirmPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            try {
                const response = await fetch("/api/resendConfirmationEmail", {
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
                return (<p className="text-success">Check email to complete verification</p>);
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
            <h3>Manage Email</h3>
            <div className="row">
                <div className="col-md-6">
                    <form action={handleConfirmEmail}>
                        {confirmModelError && <div className="text-danger" role="alert">{confirmModelError}</div>}
                        <div className="form-floating mb-3 input-group">
                            <input id="email" name="email" type="text" className="form-control" autoComplete="username"
                                aria-required="true" placeholder="name@example.com" value={accountInfo.email}
                                readOnly />
                            <label htmlFor="email" className="form-label">Email</label>
                            <div className="input-group-append d-flex">
                                {
                                    accountInfo.isEmailConfirmed ?
                                        <span className="h-100 input-group-text text-success font-weight-bold">✓</span> :
                                        <button type="submit" className="w-100 btn btn-lg btn-primary" disabled={isConfirmPending}>Verify</button>
                                }
                            </div>
                        </div>
                    </form>
                    <form action={handleChangeEmail}>
                        {emailModelError && <div className="text-danger" role="alert">{emailModelError}</div>}
                        <div className="form-floating mb-3">
                            <input id="newEmail" name="newEmail" type="text" className="form-control" autoComplete="username"
                                aria-required="true" placeholder="name@example.com" value={email}
                                onChange={(e) => setEmail(e.target.value)} />
                            <label htmlFor="newEmail" className="form-label">New Email</label>
                            {emailError && <span className="text-danger">{emailError}</span>}
                        </div>
                        <button type="submit" className="w-100 btn btn-lg btn-primary" disabled={isEmailPending}>Change email</button>
                    </form>
                </div>
            </div>
        </>
    );
};

export default Email;