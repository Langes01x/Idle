import { useContext, useEffect, useState } from "react";
import { Modal, ProgressBar } from "react-bootstrap";
import { Link, useLoaderData, type Params } from "react-router";
import "./Level.css"
import AccountContext from "../Account/AccountContext";
import FetchAccount from "../Account/FetchAccount";
import type { Rarity } from "./FetchCharacters";

type Position = "Front" | "Middle" | "Back";
type Side = "Player" | "Enemy";
type CombatResult = "Won" | "Lost" | "Draw";

export type LevelInfo = {
    id: number,
    number: number,
    experienceReward: number,
    goldReward: number,
    diamondReward: number,
    backEnemy: EnemyInfo | null,
    middleEnemy: EnemyInfo | null,
    frontEnemy: EnemyInfo | null,
};

export type EnemyInfo = {
    id: number,
    name: string,
    physicalDamage: number,
    aetherDamage: number,
    fireDamage: number,
    coldDamage: number,
    poisonDamage: number,
    critRating: number,
    critMultiplier: number,
    actionSpeed: number,
    health: number,
    currentHealth: number,
    armour: number,
    barrier: number,
    evasion: number,
    fireResistance: number,
    coldResistance: number,
    poisonResistance: number,
};

export type CharacterSnapshotInfo = {
    level: number,
    health: number,
    currentHealth: number,
    rarity: Rarity,
    class: string,
    firstName: string,
    lastName: string,
};

export type CombatActionInfo = {
    time: number,
    attackerSide: Side,
    attackerPosition: Position,
    defenderPosition: Position,
    isDefenderDead: boolean,
    physicalDamageDealt: number,
    aetherDamageDealt: number,
    fireDamageDealt: number,
    coldDamageDealt: number,
    poisonDamageDealt: number,
    isCrit: boolean,
    isMiss: boolean,
};

export type CombatSummaryInfo = {
    backCharacter: CharacterSnapshotInfo | null,
    middleCharacter: CharacterSnapshotInfo | null,
    frontCharacter: CharacterSnapshotInfo | null,
    level: LevelInfo,
    combatActions: CombatActionInfo[],
    result: CombatResult,
    rewardsGiven: boolean,
};

export async function LevelLoader({ params, request }: { params: Params<"areaId" | "levelId">, request: Request }) {
    const partyId = new URL(request.url).searchParams.get("partyId");
    const response = await fetch("/api/Areas/" + params.areaId + "/Levels/" + params.levelId + "/Attempt?partyId=" + partyId, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        credentials: "include",
    });
    if (!response.ok) {
        const json = await response.json();
        throw new Error(json.message || response.statusText);
    }

    return await response.json() as CombatSummaryInfo;
};

function Level() {
    const { setAccount } = useContext(AccountContext);
    const combatSummary = useLoaderData<CombatSummaryInfo>();
    const compactFormatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });
    const statFormatter = new Intl.NumberFormat(undefined, { maximumFractionDigits: 2 });
    const [time, setTime] = useState(0);
    const [, setActions] = useState(combatSummary.combatActions);
    const [showResult, setShowResult] = useState(false);

    useEffect(() => {
        function ApplyAction(action: CombatActionInfo) {
            // TODO: Damage effects
            let defender: EnemyInfo | CharacterSnapshotInfo | null;
            if (action.attackerSide == "Enemy") {
                if (action.defenderPosition == "Front") {
                    defender = combatSummary.frontCharacter;
                }
                else if (action.defenderPosition == "Middle") {
                    defender = combatSummary.middleCharacter;
                }
                else {
                    defender = combatSummary.backCharacter;
                }
            }
            else {
                if (action.defenderPosition == "Front") {
                    defender = combatSummary.level.frontEnemy;
                }
                else if (action.defenderPosition == "Middle") {
                    defender = combatSummary.level.middleEnemy;
                }
                else {
                    defender = combatSummary.level.backEnemy;
                }
            }
            if (defender == null) {
                return;
            }
            defender.currentHealth -= action.physicalDamageDealt;
            defender.currentHealth -= action.aetherDamageDealt;
            defender.currentHealth -= action.fireDamageDealt;
            defender.currentHealth -= action.coldDamageDealt;
            defender.currentHealth -= action.poisonDamageDealt;
            if (defender.currentHealth < 0) {
                defender.currentHealth = 0;
            }
        };

        const intervalId = setInterval(() => {
            setTime(prevTime => {
                const currentTime = prevTime + 5;
                setActions(prevActions => {
                    const currentActions = prevActions;
                    if (currentActions.some(Boolean)) {
                        while (true) {
                            const action = currentActions.shift();
                            if (action !== undefined) {
                                if (action.time <= currentTime) {
                                    ApplyAction(action);
                                }
                                else {
                                    currentActions.unshift(action);
                                    break;
                                }
                            }
                            else {
                                break;
                            }
                        }
                    }
                    else {
                        setShowResult(true);
                        FetchAccount().then(setAccount);
                        clearInterval(intervalId);
                    }
                    return currentActions;
                });
                return currentTime;
            });
        }, 5);

        return () => clearInterval(intervalId);
    }, [combatSummary, setAccount]);

    function DisplayEnemy(enemy: EnemyInfo) {
        return (
            <div className="rounded-box enemy bg-enemy w-30">
                {enemy.name}
                <div className="info-grid w-100">
                    <label>Health:</label><output>{statFormatter.format(enemy.currentHealth)}</output>
                </div>
                <ProgressBar variant="danger" min={0} max={enemy.health} now={enemy.currentHealth}></ProgressBar>
            </div>
        )
    };

    function DisplayCharacter(character: CharacterSnapshotInfo) {
        return (
            <div className="rounded-box character bg-character w-30">
                {character.firstName} {character.lastName}
                <div className="info-grid w-100">
                    <label>Health:</label><output>{statFormatter.format(character.currentHealth)}</output>
                </div>
                <ProgressBar variant="danger" min={0} max={character.health} now={character.currentHealth}></ProgressBar>
            </div>
        )
    };

    return (
        <>
            {
                showResult &&
                <Modal show={showResult} className="level-modal">
                    <Modal.Header>
                        <Modal.Title>{combatSummary.result}</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        {
                            combatSummary.result === "Won" &&
                            <div>
                                <h5>Rewards:</h5>
                                <ul>
                                    {!combatSummary.rewardsGiven && <li>Rewards already received.</li>}
                                    <li className={combatSummary.rewardsGiven ? "" : "received-reward"}>Experience: {compactFormatter.format(combatSummary.level.experienceReward)}</li>
                                    <li className={combatSummary.rewardsGiven ? "" : "received-reward"}>Gold: {compactFormatter.format(combatSummary.level.goldReward)}</li>
                                    <li className={combatSummary.rewardsGiven ? "" : "received-reward"}>Diamonds: {compactFormatter.format(combatSummary.level.diamondReward)}</li>
                                </ul>
                            </div>
                        }
                        <Link to="/game" className="btn btn-primary float-end">Return</Link>
                    </Modal.Body>
                </Modal>
            }
            <div className="game-header">Time: {time / 1000}</div>
            <div className="row justify-content-between">
                <div className="char-grid col-6">
                    {combatSummary.backCharacter && DisplayCharacter(combatSummary.backCharacter)}
                    {combatSummary.middleCharacter && DisplayCharacter(combatSummary.middleCharacter)}
                    {combatSummary.frontCharacter && DisplayCharacter(combatSummary.frontCharacter)}
                </div>
                <div className="enemy-grid col-6">
                    {combatSummary.level.frontEnemy && DisplayEnemy(combatSummary.level.frontEnemy)}
                    {combatSummary.level.middleEnemy && DisplayEnemy(combatSummary.level.middleEnemy)}
                    {combatSummary.level.backEnemy && DisplayEnemy(combatSummary.level.backEnemy)}
                </div>
            </div>
        </>);
};

export default Level;