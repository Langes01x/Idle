import { Link, useLoaderData, useLocation, useNavigate } from "react-router";
import Fetch2faInfo, { type TwoFactorInfo } from "./Fetch2faInfo";
import { useActionState, type JSX } from "react";

export type TwoFactorAuthenticationState = {
    message: string,
};

export async function TwoFactorLoader() {
    return {
        twoFactorInfo: await Fetch2faInfo()
    };
};

function RecoveryCodes(twoFactorInfo: TwoFactorInfo) {
    if (twoFactorInfo.recoveryCodesLeft === 0) {
        return (
            <div className="alert alert-danger">
                <strong>You have no recovery codes left.</strong>
                <p>You must <Link to="/manage/twoFactorAuthentication/generateRecoveryCodes">generate a new set of recovery codes</Link> before you can log in with a recovery code.</p>
            </div>
        );
    } else if (twoFactorInfo.recoveryCodesLeft === 1) {
        return (
            <div className="alert alert-danger">
                <strong>You have 1 recovery code left.</strong>
                <p>You should <Link to="/manage/twoFactorAuthentication/generateRecoveryCodes">generate a new set of recovery codes</Link>.</p>
            </div>
        );
    } else if (twoFactorInfo.recoveryCodesLeft <= 3) {
        return (
            <div className="alert alert-warning">
                <strong>You have {twoFactorInfo.recoveryCodesLeft} recovery codes left.</strong>
                <p>You can <Link to="/manage/twoFactorAuthentication/generateRecoveryCodes">generate a new set of recovery codes</Link>.</p>
            </div>
        );
    }
    return <></>;
};

function TwoFactorAuthentication() {
    const { twoFactorInfo } = useLoaderData();
    const navigate = useNavigate();
    const location = useLocation();
    const state = location.state as TwoFactorAuthenticationState | undefined;

    const [forgetModelError, handleForgetBrowser, isForgetPending] = useActionState<JSX.Element | null | undefined, FormData>(
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
                const state: TwoFactorAuthenticationState = {
                    message: "Browser forgotten",
                };
                navigate("/manage/twoFactorAuthentication", { state: state });
            } catch (error) {
                if (error instanceof Error) {
                    return (<p>{error.message}</p>);
                }
            }
        },
        null,
    );

    const [disableModelError, handleDisable, isDisablePending] = useActionState<JSX.Element | null | undefined, FormData>(
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
                const state: TwoFactorAuthenticationState = {
                    message: "Two factor disabled",
                };
                navigate("/manage/twoFactorAuthentication", { state: state });
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
            <h3>Two-factor Authentication (2FA)</h3>
            <p className="text-success">{state && state.message}</p>
            {twoFactorInfo.isTwoFactorEnabled && RecoveryCodes(twoFactorInfo)}
            {
                twoFactorInfo.isTwoFactorEnabled && twoFactorInfo.isMachineRemembered &&
                <form className="d-inline-block" action={handleForgetBrowser}>
                    {forgetModelError && <div className="text-danger" role="alert">{forgetModelError}</div>}
                    <input type="hidden" name="forgetMachine" value="true" />
                    <button type="submit" className="btn btn-primary" disabled={isForgetPending}>Forget this browser</button>
                </form>
            }
            {
                twoFactorInfo.isTwoFactorEnabled &&
                <>
                    <form className="d-inline-block" action={handleDisable}>
                        {disableModelError && <div className="text-danger" role="alert">{disableModelError}</div>}
                        <input type="hidden" name="enable" value="false" />
                        <button type="submit" className="btn btn-primary" disabled={isDisablePending}>Disable 2FA</button>
                    </form>
                    <Link to="/manage/twoFactorAuthentication/generateRecoveryCodes" className="btn btn-primary">Reset recovery codes</Link>
                </>
            }

            <h4>Authenticator App</h4>
            {
                twoFactorInfo.isTwoFactorEnabled ?
                    <>
                        <Link to="/manage/twoFactorAuthentication/enableAuthenticator" className="btn btn-primary">Set up authenticator app</Link>
                        <Link to="/manage/twoFactorAuthentication/resetAuthenticator" className="btn btn-primary">Reset authenticator app</Link>
                    </> :
                    <Link to="/manage/twoFactorAuthentication/enableAuthenticator" className="btn btn-primary">Add authenticator app</Link>
            }
        </>
    );
};

export default TwoFactorAuthentication;