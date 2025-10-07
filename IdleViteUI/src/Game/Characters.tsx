import { useActionState, useContext, useState, type JSX } from "react"
import { Link, useLoaderData } from "react-router";
import { Modal } from "react-bootstrap";
import AccountContext from "../Account/AccountContext";
import FetchAccount from "../Account/FetchAccount";
import "./Characters.css"

export type CharacterInfo = {
    id: number,

    experience: number,
    experienceToNextLevel: number,
    level: number,
    rarity: string,
    class: string,
    firstName: string,
    lastName: string,

    strength: number,
    intelligence: number,
    dexterity: number,
    vitality: number,
    constitution: number,
    wisdom: number,
};

function DisplayCharacter(char: CharacterInfo) {
    return (
        <Link to={"/game/characters/" + char.id}>
            <div className="rounded-box character w-250px">
                <div className="info-grid w-250px">
                    <label>Name:</label><output>{char.firstName} {char.lastName}</output>
                    <label>Level:</label><output>{char.level}</output>
                    <label>Rarity:</label><output>{char.rarity}</output>
                    <label>Class:</label><output>{char.class}</output>
                </div>
                <hr />
                <div className="info-grid w-250px">
                    <label>Strength:</label><output>{char.strength}</output>
                    <label>Intelligence:</label><output>{char.intelligence}</output>
                    <label>Dexterity:</label><output>{char.dexterity}</output>
                    <label>Vitality:</label><output>{char.vitality}</output>
                    <label>Constitution:</label><output>{char.constitution}</output>
                    <label>Wisdom:</label><output>{char.wisdom}</output>
                </div>
            </div>
        </Link>
    );
};

function Characters() {
    const loadedCharacters = useLoaderData<CharacterInfo[]>();
    const [characters, setCharacters] = useState(loadedCharacters);
    const { account, setAccount } = useContext(AccountContext);
    const formatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });
    const summonCost = 3600;
    const canSummonOnce = account!.diamonds >= summonCost;
    const canSummonTen = account!.diamonds >= summonCost * 10;
    const [newCharacters, setNewCharacters] = useState<CharacterInfo[] | null>(null);
    const [modalOpen, setModalOpen] = useState(true);

    function CloseModal() {
        setModalOpen(false);
    };

    const [modelError, handleSummon, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            try {
                const response = await fetch("/api/Characters/Summon?quantity=" + formData.get("quantity")?.toString(), {
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
                const json = await response.json() as CharacterInfo[];
                setNewCharacters(json);
                setCharacters(characters.concat(json));
                setAccount(await FetchAccount());
                setModalOpen(true);
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
        <div className="text-start">
            {
                newCharacters &&
                <Modal show={modalOpen} onHide={CloseModal}>
                    <Modal.Header closeButton>
                        <Modal.Title>New Characters</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div className="char-grid">{newCharacters.map(DisplayCharacter)}</div>
                    </Modal.Body>
                </Modal>
            }
            <div>
                {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                <Link to="/game" className="btn btn-primary">&lt;&lt; Back</Link>
                <form className="d-inline-block" action={handleSummon}>
                    <input type="hidden" name="quantity" value="1" />
                    <button type="submit" className="btn btn-primary" disabled={!canSummonOnce || isPending}>Summon 1x for {summonCost}</button>
                </form>
                <form className="d-inline-block" action={handleSummon}>
                    <input type="hidden" name="quantity" value="10" />
                    <button type="submit" className="btn btn-primary" disabled={!canSummonTen || isPending}>Summon 10x for {summonCost * 10}</button>
                </form >
                <label>Diamonds:</label>
                <output>{formatter.format(account!.diamonds)}</output>
            </div >
            <div className="char-grid">
                {characters.map(DisplayCharacter)}
            </div>
        </div>
    );
};

export default Characters;