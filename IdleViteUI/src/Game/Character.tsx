import { Link, useLoaderData, type Params } from "react-router";
import type { CharacterInfo } from "./Characters";
import { useActionState, useContext, useState, type JSX } from "react";
import AccountContext from "../Account/AccountContext";
import FetchAccount from "../Account/FetchAccount";

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
    return await response.json() as CharacterInfo;
};

function Character() {
    const loadedCharacter = useLoaderData<CharacterInfo>();
    const [character, setCharacter] = useState(loadedCharacter);
    const { account, setAccount } = useContext(AccountContext);
    const canLevelUp = account!.experience >= character.experienceToNextLevel;
    const formatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });

    const [modelError, handleLevelUp, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
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
                const json = await response.json() as CharacterInfo;
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

    return (
        <>
            <div className="text-start">
                {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                <Link to="/game/characters" className="btn btn-primary">&lt;&lt; Back</Link>
                <form className="d-inline-block" action={handleLevelUp}>
                    <button type="submit" className="btn btn-primary" disabled={!canLevelUp || isPending}>Level Up</button>
                </form>
                <label>Experience:</label>
                <output>{formatter.format(account!.experience)}</output>
            </div>
            <div className="row">
                <div className="rounded-box wide-character col-md-6">
                    <div className="info-grid w-100">
                        <label>Name:</label><output>{character.firstName} {character.lastName}</output>
                        <label>Level:</label><output>{character.level}</output>
                        <label>Rarity:</label><output>{character.rarity}</output>
                        <label>Class:</label><output>{character.class}</output>
                    </div>
                    <hr />
                    <div className="info-grid w-100">
                        <label>Total Experience:</label><output>{formatter.format(character.experience)}</output>
                        <label>To Next Level:</label><output>{formatter.format(character.experienceToNextLevel)}</output>
                    </div>
                    <hr />
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
        </>
    );
};

export default Character;