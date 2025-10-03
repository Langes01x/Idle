import { useLocation } from "react-router";

export type RecoveryCodeState = {
    recoveryCodes: string[],
    message: string,
};

function RecoveryCodes() {
    const location = useLocation();
    const state = location.state as RecoveryCodeState;

    return (
        <>
            <h3>Recovery codes</h3>
            <p className="text-success">{state && state.message}</p>
            <div className="alert alert-warning" role="alert">
                <p>
                    <span className="glyphicon glyphicon-warning-sign"></span>
                    <strong>Put these codes in a safe place.</strong>
                </p>
                <p>
                    If you lose your device and don't have the recovery codes you will lose access to your account.
                </p>
            </div>
            <div className="row">
                {state && Object.values<string>(state.recoveryCodes).map((val: string) => { return (<code className="col-md-6 col-sm-12">{val}</code>); })}
            </div>
        </>
    );
};

export default RecoveryCodes;