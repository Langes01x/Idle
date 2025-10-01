import { useActionState, useState, type JSX } from "react";

function ResendConfirmationEmail() {
    const [email, setEmail] = useState('');
    const [emailError, setEmailError] = useState('');

    const [modelError, handleReset, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            if (!email) {
                setEmailError("Email is required");
                return null;
            }
            setEmailError("");

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
            <h1>Resend confirmation email</h1>
            <hr />
            <div className="row">
                <div className="col-md-6">
                    <section>
                        <form action={handleReset}>
                            {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                            <div className="form-floating mb-3">
                                <input id="email" name="email" type="text" className="form-control" autoComplete="username"
                                    aria-required="true" placeholder="name@example.com" value={email}
                                    onChange={(e) => setEmail(e.target.value)} />
                                <label htmlFor="email" className="form-label">Email</label>
                                {emailError && <span className="text-danger">{emailError}</span>}
                            </div>
                            <div>
                                <button type="submit" className="w-100 btn btn-lg btn-primary" disabled={isPending}>Resend confirmation email</button>
                            </div>
                        </form>
                    </section>
                </div>
            </div>
        </>
    );
};

export default ResendConfirmationEmail;