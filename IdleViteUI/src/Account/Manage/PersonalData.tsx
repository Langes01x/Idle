import { useActionState, useContext, useState, type JSX } from "react";
import { Modal } from "react-bootstrap";
import { useNavigate } from "react-router";
import AccountContext from "../AccountContext";

function PersonalData() {
    const navigate = useNavigate();
    const { setAccount } = useContext(AccountContext);
    const [password, setPassword] = useState('');
    const [passwordError, setPasswordError] = useState('');
    const [modalOpen, setModalOpen] = useState(false);

    function CloseModal() {
        setModalOpen(false);
    };

    function OpenModal() {
        setModalOpen(true);
    }

    const [modelError, handleDelete, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            if (!password) {
                setPasswordError("Password is required");
                return null;
            }
            setPasswordError("");

            try {
                const response = await fetch("/api/delete", {
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
                setAccount(null);
                navigate("/login");
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
            <Modal show={modalOpen} onHide={CloseModal}>
                <Modal.Header closeButton>
                    <Modal.Title>Confirm Deletion</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <form action={handleDelete}>
                        {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                        <div className="form-floating mb-3">
                            <input id="password" name="password" type="password" className="form-control" autoComplete="current-password"
                                aria-required="true" placeholder="password" value={password}
                                onChange={(e) => setPassword(e.target.value)} />
                            <label htmlFor="password" className="form-label">Password</label>
                            {passwordError && <span className="text-danger">{passwordError}</span>}
                        </div>
                        <button className="w-100 btn btn-lg btn-danger" type="submit" disabled={isPending}>Delete data and close my account</button>
                    </form>
                </Modal.Body>
            </Modal>
            <h3>Personal Data</h3>
            <div className="row">
                <div className="col-md-12">
                    <p>Your account contains personal data like your email address. This page allows you to delete that data.</p>
                    <p>
                        <strong>Deleting this data will permanently remove your account, and this cannot be recovered.</strong>
                    </p>
                    <p>
                        <button className="btn btn-danger" onClick={OpenModal}>Delete</button>
                    </p>
                </div>
            </div>
        </>
    );
};

export default PersonalData;