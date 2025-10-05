import { useLoaderData, useNavigate } from "react-router";
import AccountContext from "../../AccountContext";
import { useActionState, useContext, useState, type JSX } from "react";
import type { TwoFactorInfo } from "./Fetch2faInfo";
import { QRCodeSVG } from "qrcode.react";
import type { RecoveryCodeState } from "./RecoveryCodes";
import type { TwoFactorAuthenticationState } from "./TwoFactorAuthentication";

function GetAuthenticatorUri(email: string, key: string) {
    const encodedIssuer = encodeURIComponent("IdleAPI");
    const encodedEmail = encodeURIComponent(email);
    return "otpauth://totp/" + encodedIssuer + ":" + encodedEmail + "?secret=" + key + "&issuer=" + encodedIssuer + "&digits=6";
};

function FormatKey(key: string) {
    return key.match(/.{1,4}/g)?.join(' ').toLowerCase();
};

function EnableAuthenticator() {
    const twoFactorInfo = useLoaderData<TwoFactorInfo>();
    const navigate = useNavigate();
    const { account } = useContext(AccountContext);
    const [twoFactorCode, setTwoFactorCode] = useState('');
    const [twoFactorCodeError, setTwoFactorCodeError] = useState('');

    const [modelError, handleEnableAuth, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            if (!twoFactorCode) {
                setTwoFactorCodeError("Verification code is required");
                return null;
            }
            setTwoFactorCodeError("");

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
                if (json.recoveryCodes) {
                    const state: RecoveryCodeState = {
                        recoveryCodes: json.recoveryCodes,
                        message: "Two factor enabled",
                    };
                    navigate("/manage/twoFactorAuthentication/recoveryCodes", { state: state });
                }
                else {
                    const state: TwoFactorAuthenticationState = {
                        message: "Two factor enabled",
                    };
                    navigate("/manage/twoFactorAuthentication", { state: state });
                }
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
            <h3>Configure authenticator app</h3>
            <div>
                <p>To use an authenticator app go through the following steps:</p>
                <ol className="list">
                    <li>
                        <p>
                            Download a two-factor authenticator app like Microsoft Authenticator for&nbsp;
                            <a href="https://go.microsoft.com/fwlink/?Linkid=825072">Android</a> and&nbsp;
                            <a href="https://go.microsoft.com/fwlink/?Linkid=825073">iOS</a> or
                            Google Authenticator for&nbsp;
                            <a href="https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&amp;hl=en">Android</a> and&nbsp;
                            <a href="https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8">iOS</a>.
                        </p>
                    </li>
                    <li>
                        <p>Scan the QR Code or enter this key <kbd>{FormatKey(twoFactorInfo.sharedKey)}</kbd> into your two factor authenticator app. Spaces and casing do not matter.</p>
                        <QRCodeSVG value={GetAuthenticatorUri(account?.email || "", twoFactorInfo.sharedKey)} marginSize={4} />
                    </li>
                    <li>
                        <p>
                            Once you have scanned the QR code or input the key above, your two factor authentication app will provide you
                            with a unique code. Enter the code in the confirmation box below.
                        </p>
                        <div className="row">
                            <div className="col-md-6">
                                <form action={handleEnableAuth}>
                                    {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                                    <div className="form-floating mb-3">
                                        <input type="hidden" name="enable" value="true" />
                                        <input id="twoFactorCode" name="twoFactorCode" type="text" className="form-control" autoComplete="off"
                                            aria-required="true" placeholder="Please enter the code." value={twoFactorCode}
                                            onChange={(e) => setTwoFactorCode(e.target.value)} />
                                        <label htmlFor="twoFactorCode" className="form-label">Verification Code</label>
                                        {twoFactorCodeError && <span className="text-danger">{twoFactorCodeError}</span>}
                                    </div>
                                    <button type="submit" className="w-100 btn btn-lg btn-primary" disabled={isPending}>Verify</button>
                                </form>
                            </div>
                        </div>
                    </li>
                </ol>
            </div>
        </>
    );
};

export default EnableAuthenticator;