import { useActionState, useContext, useState, type ChangeEvent, type JSX } from "react"
import { Link, useLoaderData } from "react-router";
import { Modal } from "react-bootstrap";
import AccountContext from "../Account/AccountContext";
import FetchAccount from "../Account/FetchAccount";
import type { CharacterInfo, GetCharactersInfo } from "./FetchCharacters";
import "./Characters.css"
import FetchCharacters from "./FetchCharacters";

function DisplayCharacter(char: CharacterInfo) {
    return (
        <Link className="btn btn-dark character-button" to={"/game/characters/" + char.id}>
            <div className="rounded-box character">
                {DisplayCharacterInfo(char)}
            </div>
        </Link>
    );
};

export function DisplayCharacterInfo(char: CharacterInfo) {
    return (
        <>
            <div className="info-grid w-100">
                <label>Name:</label><output>{char.firstName} {char.lastName}</output>
                <label>Level:</label><output>{char.level}</output>
                <label>Rarity:</label><output>{char.rarity}</output>
                <label>Class:</label><output>{char.class}</output>
            </div>
            <hr />
            <div className="info-grid w-100">
                <label>Strength:</label><output>{char.strength}</output>
                <label>Intelligence:</label><output>{char.intelligence}</output>
                <label>Dexterity:</label><output>{char.dexterity}</output>
                <label>Vitality:</label><output>{char.vitality}</output>
                <label>Constitution:</label><output>{char.constitution}</output>
                <label>Wisdom:</label><output>{char.wisdom}</output>
            </div>
        </>
    );
};

function DisplayNewCharacter(char: CharacterInfo) {
    let opacity = "opacity-0";
    switch (char.rarity) {
        case "Legendary":
            opacity = "opacity-100";
            break;
        case "Epic":
            opacity = "opacity-75";
            break;
        case "Rare":
            opacity = "opacity-50";
            break;
        case "Uncommon":
            opacity = "opacity-25";
            break;
    };
    return (
        <div className="new-char-container character">
            <div className={"new-char-border " + opacity}></div>
            <div className="new-char-content">
                {DisplayCharacterInfo(char)}
            </div>
        </div>
    );
};

export async function CharactersLoader() {
    return await FetchCharacters(null);
}

function Characters() {
    const { characters: loadedCharacters, summonCost, sortOrderOptions, defaultSortOrder } = useLoaderData<GetCharactersInfo>();
    const [sortOrder, setSortOrder] = useState(defaultSortOrder);
    const [characters, setCharacters] = useState(loadedCharacters);
    const { account, setAccount } = useContext(AccountContext);
    const formatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });
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
                setCharacters((await FetchCharacters(null)).characters);
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

    const [sortError, handleSort, isSortPending] = useActionState<JSX.Element | null | undefined, ChangeEvent<HTMLSelectElement>>(
        async (_previousState, event) => {
            event.preventDefault();
            try {
                const newValue = +event.target.value;
                const charactersInfo = await FetchCharacters(newValue);
                setCharacters(charactersInfo.characters);
                setSortOrder(newValue);
                return null;
            } catch {
                return (<p>Sorting failed</p>);
            }
        },
        null,
    );

    return (
        <div>
            {
                newCharacters &&
                <Modal show={modalOpen} onHide={CloseModal}>
                    <Modal.Header closeButton>
                        <Modal.Title>New Characters</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <div className="char-grid">{newCharacters.map(DisplayNewCharacter)}</div>
                    </Modal.Body>
                </Modal>
            }
            <div className="text-start">
                {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                <Link to="/game" className="btn btn-primary">&lt;&lt; Back</Link>
                <form className="d-inline-block" action={handleSummon}>
                    <input type="hidden" name="quantity" value="1" />
                    <button type="submit" className="btn btn-primary" disabled={!canSummonOnce || isPending}>Summon 1x for {summonCost}</button>
                </form>
                <form className="d-inline-block" action={handleSummon}>
                    <input type="hidden" name="quantity" value="10" />
                    <button type="submit" className="btn btn-primary" disabled={(account?.hasUsedFreePull && !canSummonTen) || isPending}>Summon 10x for {account?.hasUsedFreePull ? summonCost * 10 : "Free"}</button>
                </form >
                <label>Diamonds:</label>
                <output>{formatter.format(account!.diamonds)}</output>
                <form className="float-end">
                    {sortError && <div className="text-danger" role="alert">{sortError}</div>}
                    <label htmlFor="sortOrder">Sort by:&nbsp;</label>
                    <select id="sortOrder" name="sortOrder" value={sortOrder} onChange={handleSort} disabled={isSortPending}>
                        {
                            Object.entries<number>(sortOrderOptions).map(([label, value]) => <option key={label} value={value}>{label}</option>)
                        }
                    </select>
                </form>
            </div>
            <div className="char-grid">
                {characters.map(DisplayCharacter)}
            </div>
        </div>
    );
};

export default Characters;