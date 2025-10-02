import { useActionState, useState, type JSX } from "react";

function ChangePassword() {
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmPassword, setConfirmPassword] = useState('');
    const [oldPasswordError, setOldPasswordError] = useState('');
    const [newPasswordError, setNewPasswordError] = useState('');
    const [confirmPasswordError, setConfirmPasswordError] = useState('');

    const [modelError, handleChangePassword, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            if (!oldPassword) {
                setOldPasswordError("Old password is required");
            } else {
                setOldPasswordError("");
            }
            if (!newPassword) {
                setNewPasswordError("New password is required");
            } else {
                setNewPasswordError("");
            }
            if (newPassword !== confirmPassword) {
                setConfirmPasswordError("Password and confirmation don't match");
            } else {
                setConfirmPasswordError("");
            }
            if (!oldPassword || !newPassword || newPassword !== confirmPassword) {
                return null;
            }

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
                return (<p className="text-success">Password changed</p>);
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
            <h3>Change Password</h3>
            <div className="row">
                <div className="col-md-6">
                    <form action={handleChangePassword}>
                        {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                        <div className="form-floating mb-3">
                            <input id="oldPassword" name="oldPassword" type="password" className="form-control" autoComplete="current-password"
                                aria-required="true" placeholder="Please enter your old password." value={oldPassword}
                                onChange={(e) => setOldPassword(e.target.value)} />
                            <label htmlFor="oldPassword" className="form-label">Old Password</label>
                            {oldPasswordError && <span className="text-danger">{oldPasswordError}</span>}
                        </div>
                        <div className="form-floating mb-3">
                            <input id="newPassword" name="newPassword" type="password" className="form-control" autoComplete="new-password"
                                aria-required="true" placeholder="Please enter your new password." value={newPassword}
                                onChange={(e) => setNewPassword(e.target.value)} />
                            <label htmlFor="newPassword" className="form-label">New Password</label>
                            {newPasswordError && <span className="text-danger">{newPasswordError}</span>}
                        </div>
                        <div className="form-floating mb-3">
                            <input id="confirmPassword" name="confirmPassword" type="password" className="form-control" autoComplete="new-password"
                                aria-required="true" placeholder="Please confirm your new password." value={confirmPassword}
                                onChange={(e) => setConfirmPassword(e.target.value)} />
                            <label htmlFor="confirmPassword" className="form-label">Confirm Password</label>
                            {confirmPasswordError && <span className="text-danger">{confirmPasswordError}</span>}
                        </div>
                        <button type="submit" className="w-100 btn btn-lg btn-primary" disabled={isPending}>Update password</button>
                    </form>
                </div>
            </div>
        </>
    );
};

export default ChangePassword;