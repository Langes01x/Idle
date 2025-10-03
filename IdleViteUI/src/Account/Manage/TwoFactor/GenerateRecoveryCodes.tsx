import { useActionState, type JSX } from "react";
import type { TwoFactorInfo } from "./Fetch2faInfo";
import { Link, useNavigate } from "react-router";
import type { RecoveryCodeState } from "./RecoveryCodes";

function GenerateRecoveryCodes() {
    const navigate = useNavigate();

    const [modelError, handleReset, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            try {
                const response = await fetch("/api/manage/2fa", {
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
                const json = await response.json() as TwoFactorInfo;
                const state: RecoveryCodeState = {
                    recoveryCodes: json.recoveryCodes,
                    message: "",
                };
                navigate("/manage/twoFactorAuthentication/recoveryCodes", { state: state });
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
            <h3>Generate two-factor authentication (2FA) recovery codes</h3>
            <div className="alert alert-warning" role="alert">
                <p>
                    Generating new recovery codes does not change the keys used in authenticator apps. If you wish to change the key
                    used in an authenticator app you should <Link to="/manage/twoFactorAuthentication/resetAuthenticator">reset your authenticator keys</Link>.
                </p>
            </div>
            <div>
                <form action={handleReset}>
                    {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                    <input type="hidden" name="resetRecoveryCodes" value="true" />
                    <button type="submit" className="btn btn-danger" disabled={isPending}>Generate Recovery Codes</button>
                </form>
            </div>
        </>
    );
};

export default GenerateRecoveryCodes;