import { Link, useLoaderData, useNavigate, type Params } from "react-router";
import type { CharacterInfo } from "./FetchCharacters";
import { useActionState, useContext, useState, type JSX } from "react";
import AccountContext from "../Account/AccountContext";
import FetchAccount from "../Account/FetchAccount";
import { Modal } from "react-bootstrap";

interface DetailedCharacterInfo extends CharacterInfo {
    physicalDamage: number,
    aetherDamage: number,
    critRating: number,
    critMultiplier: number,
    actionSpeed: number,
    health: number,
    armour: number,
    barrier: number,
    evasion: number,
    fireResistance: number,
    coldResistance: number,
    poisonResistance: number,
};

export async function CharacterLoader({ params }: { params: Params<"id"> }) {
    const response = await fetch("/api/Characters/" + params.id, {
        method: "GET",
        headers: {
            "Content-Type": "application/json",
        },
        credentials: "include",
    });
    if (!response.ok) {
        const json = await response.json();
        throw new Error(json.message || response.statusText);
    }
    return await response.json() as DetailedCharacterInfo;
};

function Character() {
    const loadedCharacter = useLoaderData<DetailedCharacterInfo>();
    const navigate = useNavigate();
    const [character, setCharacter] = useState(loadedCharacter);
    const { account, setAccount } = useContext(AccountContext);
    const [modalOpen, setModalOpen] = useState(false);
    const canLevelUp = account!.experience >= character.experienceToNextLevel;
    const compactFormatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });
    const percentFormatter = new Intl.NumberFormat(undefined, { style: 'percent' });
    const speedFormatter = new Intl.NumberFormat(undefined, { style: 'unit', unit: 'second', maximumFractionDigits: 2 });
    const statFormatter = new Intl.NumberFormat(undefined, { maximumFractionDigits: 2 });

    function OpenModal() {
        setModalOpen(true);
    };

    function CloseModal() {
        setModalOpen(false);
    };

    const [levelUpError, handleLevelUp, isLevelUpPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async () => {
            try {
                const response = await fetch("/api/Characters/" + character.id + "/LevelUp", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    credentials: "include",
                });
                if (!response.ok) {
                    const json = await response.json();
                    if ('errors' in json) {
                        return (<>{Object.values<string>(json.errors).map((val: string) => { return (<p>{val}</p>); })}</>);
                    }
                    return (<p>{response.statusText}</p>);
                }
                const json = await response.json() as DetailedCharacterInfo;
                setCharacter(json);
                setAccount(await FetchAccount());
                return null;
            } catch (error) {
                if (error instanceof Error) {
                    return (<p>{error.message}</p>);
                }
            }
        },
        null,
    );

    const [dismissError, handleDismiss, isDismissPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async () => {
            try {
                const response = await fetch("/api/Characters/" + character.id + "/Dismiss", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    credentials: "include",
                });
                if (!response.ok) {
                    const json = await response.json();
                    if ('errors' in json) {
                        return (<>{Object.values<string>(json.errors).map((val: string) => { return (<p>{val}</p>); })}</>);
                    }
                    return (<p>{response.statusText}</p>);
                }
                setAccount(await FetchAccount());
                navigate("/game/characters");
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
            <div className="text-start">
                <Modal show={modalOpen} onHide={CloseModal}>
                    <Modal.Header closeButton>
                        <Modal.Title>Dismiss Character?</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div className="alert alert-warning" role="alert">
                            <p>
                                Dismissing the character will return all experience and give 100 diamonds.
                                Dismissing a character can not be undone so ensure you are dismissing the correct one.
                            </p>
                        </div>
                        <form action={handleDismiss}>
                            {dismissError && <div className="text-danger" role="alert">{dismissError}</div>}
                            <button className="w-100 btn btn-lg btn-danger" type="submit" disabled={isDismissPending}>Dismiss</button>
                        </form>
                    </Modal.Body>
                </Modal>
                <Link to="/game/characters" className="btn btn-primary">&lt;&lt; Back</Link>
                <button className="btn btn-primary" onClick={OpenModal}>Dismiss</button>
                {levelUpError && <div className="text-danger" role="alert">{levelUpError}</div>}
                <form className="d-inline-block" action={handleLevelUp}>
                    <button type="submit" className="btn btn-primary" disabled={!canLevelUp || isLevelUpPending}>Level Up</button>
                </form>
                <label>Experience:</label>
                <output>{compactFormatter.format(account!.experience)}</output>
                <div className="row">
                    <div className="col-md-6 col-sm-12">
                        <div className="rounded-box col-sm-12">
                            <div className="info-grid w-100">
                                <label>Name:</label><output>{character.firstName} {character.lastName}</output>
                                <label>Level:</label><output>{character.level}</output>
                                <label>Rarity:</label><output>{character.rarity}</output>
                                <label>Class:</label><output>{character.class}</output>
                            </div>
                        </div>
                        <hr />
                        <div className="rounded-box col-sm-12">
                            <div className="info-grid w-100">
                                <label>Total Experience:</label><output>{compactFormatter.format(character.experience)}</output>
                                <label>To Next Level:</label><output>{compactFormatter.format(character.experienceToNextLevel)}</output>
                            </div>
                        </div>
                        <hr />
                        <div className="rounded-box col-sm-12">
                            <div className="info-grid w-100">
                                <label>Strength:</label><output>{character.strength}</output>
                                <label>Intelligence:</label><output>{character.intelligence}</output>
                                <label>Dexterity:</label><output>{character.dexterity}</output>
                                <label>Vitality:</label><output>{character.vitality}</output>
                                <label>Constitution:</label><output>{character.constitution}</output>
                                <label>Wisdom:</label><output>{character.wisdom}</output>
                            </div>
                        </div>
                    </div>
                    <div className="col-sm-12 d-sm-block d-md-none">
                        <hr />
                    </div>
                    <div className="col-md-6">
                        <div className="rounded-box col-sm-12">
                            <div className="info-grid w-100">
                                <label>Physical Damage:</label><output>{statFormatter.format(character.physicalDamage)}</output>
                                <label>Aether Damage:</label><output>{statFormatter.format(character.aetherDamage)}</output>
                                <label>Crit Rating:</label><output>{statFormatter.format(character.critRating)}</output>
                                <label>Crit Multiplier:</label><output>{percentFormatter.format(character.critMultiplier)}</output>
                                <label>Action Speed:</label><output>{speedFormatter.format(character.actionSpeed)}</output>
                            </div>
                        </div>
                        <hr />
                        <div className="rounded-box col-sm-12">
                            <div className="info-grid w-100">
                                <label>Health:</label><output>{statFormatter.format(character.health)}</output>
                                <label>Armour:</label><output>{statFormatter.format(character.armour)}</output>
                                <label>Barrier:</label><output>{statFormatter.format(character.barrier)}</output>
                                <label>Evasion:</label><output>{percentFormatter.format(character.evasion)}</output>
                            </div>
                        </div>
                        <hr />
                        <div className="rounded-box col-sm-12">
                            <div className="info-grid col-md-4 col-sm-12">
                                <label>Fire Resistance:</label><output>{percentFormatter.format(character.fireResistance)}</output>
                            </div>
                            <div className="info-grid col-md-4 col-sm-12">
                                <label>Cold Resistance:</label><output>{percentFormatter.format(character.coldResistance)}</output>
                            </div>
                            <div className="info-grid col-md-4 col-sm-12">
                                <label>Poison Resistance:</label><output>{percentFormatter.format(character.poisonResistance)}</output>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
};

export default Character;