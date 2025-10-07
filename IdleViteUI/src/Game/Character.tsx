import { Link, useLoaderData, type Params } from "react-router";
import type { CharacterInfo } from "./Characters";

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
    const char = useLoaderData<CharacterInfo>();
    const formatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });

    return (
        <>
            <div className="text-start">
                <Link to="/game/characters" className="btn btn-primary">&lt;&lt; Back</Link>
            </div>
            <div className="row">
                <div className="rounded-box wide-character col-md-6">
                    <div className="info-grid w-100">
                        <label>Name:</label><output>{char.firstName} {char.lastName}</output>
                        <label>Level:</label><output>{char.level}</output>
                        <label>Rarity:</label><output>{char.rarity}</output>
                        <label>Class:</label><output>{char.class}</output>
                    </div>
                    <hr />
                    <div className="info-grid w-100">
                        <label>Total Experience:</label><output>{formatter.format(char.experience)}</output>
                        <label>To Next Level:</label><output>{formatter.format(char.experienceToNextLevel)}</output>
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
                </div>
            </div>
        </>
    );
};

export default Character;