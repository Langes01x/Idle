import { useActionState, useContext, useState, type JSX } from "react";
import { Carousel, Modal } from "react-bootstrap";
import { Link, useLoaderData } from "react-router";
import AccountContext from "../Account/AccountContext";
import pin from "../../public/pin.svg"
import node from "../../public/node.svg"
import "./Map.css"
import FetchParties, { type PartyInfo } from "./FetchParties";
import { DisplayCharacterInfo } from "./Characters";
import type { EnemyInfo, LevelInfo } from "./Level";

export type AreaInfo = {
    id: number,
    number: number,
    name: string,
    levels: LevelInfo[] | null,
};

type AreaLevel = {
    level: LevelInfo,
    area: AreaInfo,
};

function DisplayNoCharacter() {
    return (
        <div className="no-character-button">
            x
        </div>
    );
};

export async function AreasLoader() {
    const response = await fetch("/api/Areas/", {
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

    return await response.json() as AreaInfo[];
};

function Map() {
    const { account, } = useContext(AccountContext);
    const areas = useLoaderData<AreaInfo[]>();
    const nextLevel = ((account?.levelsCleared ?? 0) % 20) + 1;
    const nextArea = Math.floor((account?.levelsCleared ?? 0) / 20) + 1;
    const [modalOpen, setModalOpen] = useState(true);
    const [selectedAreaLevel, setSelectedAreaLevel] = useState<AreaLevel | undefined>(undefined);
    const [parties, setParties] = useState<PartyInfo[] | undefined>(undefined);
    const [selectedParty, setSelectedParty] = useState<PartyInfo | undefined>(parties !== undefined && parties.some(Boolean) ? parties[0] : undefined);
    const compactFormatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });
    const percentFormatter = new Intl.NumberFormat(undefined, { style: 'percent' });
    const speedFormatter = new Intl.NumberFormat(undefined, { style: 'unit', unit: 'second', maximumFractionDigits: 2 });
    const statFormatter = new Intl.NumberFormat(undefined, { maximumFractionDigits: 2 });

    function CloseModal() {
        setSelectedAreaLevel(undefined);
        setModalOpen(false);
    };

    function handleSelectParty(index: number | undefined) {
        setSelectedParty(index === undefined || parties === undefined ? undefined : parties[index]);
    };

    function DisplayParty(party: PartyInfo) {
        return (
            <Carousel.Item>
                <div className="col-md-6 offset-md-3 info-grid">
                    <label>Party Name:</label><output>{party.name}</output>
                </div>
                <div className="char-grid party-grid">
                    <div className="rounded-box character bg-character size-character">
                        {party.backCharacter === null ? DisplayNoCharacter() : DisplayCharacterInfo(party.backCharacter)}
                    </div>
                    <div className="rounded-box character bg-character size-character">
                        {party.middleCharacter === null ? DisplayNoCharacter() : DisplayCharacterInfo(party.middleCharacter)}
                    </div>
                    <div className="rounded-box character bg-character size-character">
                        {party.frontCharacter === null ? DisplayNoCharacter() : DisplayCharacterInfo(party.frontCharacter)}
                    </div>
                </div>
            </Carousel.Item>
        );
    };

    const [levelSelectError, handleLevelSelect, isLevelSelectPending] = useActionState<JSX.Element | null | undefined, AreaLevel>(
        async (_previousState, areaLevel) => {
            try {
                if (parties === undefined) {
                    const parties = await FetchParties();
                    setParties(parties);
                    if (parties.some(Boolean)) {
                        setSelectedParty(parties[0]);
                    }
                }

                const response = await fetch("/api/Areas/" + areaLevel.area.id + "/Levels/" + areaLevel.level.id, {
                    method: "GET",
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
                const level = await response.json() as LevelInfo;
                setSelectedAreaLevel({ level: level, area: areaLevel.area });
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

    function DisplayArea(area: AreaInfo) {
        return (
            <Carousel.Item>
                <Carousel.Caption>
                    <h2>Area {area.number}: {area.name}</h2>
                </Carousel.Caption>
                <div className={"area background-" + area.name}>
                    {area.levels?.map(level => DisplayLevel({ level, area }))}
                </div>
            </Carousel.Item>
        );
    };

    function DisplayLevel(areaLevel: AreaLevel) {
        return (
            <div className="level-container">
                <button className="level" onClick={() => handleLevelSelect(areaLevel)} aria-disabled={isLevelSelectPending}>
                    <img className={"level-node" + ((areaLevel.level.number - 1) % 10 === 9 ? " boss" : "")} src={node} />
                    <p className="level-text">{areaLevel.area.number} - {areaLevel.level.number}</p>
                    {areaLevel.level.number === nextLevel && areaLevel.area.number === nextArea && <img className="level-pin" src={pin} />}
                </button>
            </div>
        );
    };

    function DisplayEnemy(enemy: EnemyInfo) {
        return (
            <div className="rounded-box enemy bg-enemy size-100 d-flex justify-content-center">
                {enemy.name}
                <div className="enemy-tooltip">
                    <div className="info-grid w-100">
                        <label>Name:</label><output>{enemy.name}</output>
                    </div>
                    <hr />
                    <div className="info-grid w-100">
                        {enemy.physicalDamage !== 0 && <><label>Physical Damage:</label><output>{enemy.physicalDamage}</output></>}
                        {enemy.aetherDamage !== 0 && <><label>Aether Damage:</label><output>{enemy.aetherDamage}</output></>}
                        {enemy.fireDamage !== 0 && <><label>Fire Damage:</label><output>{enemy.fireDamage}</output></>}
                        {enemy.coldDamage !== 0 && <><label>Cold Damage:</label><output>{enemy.coldDamage}</output></>}
                        {enemy.poisonDamage !== 0 && <><label>Poison Damage:</label><output>{enemy.poisonDamage}</output></>}
                        <label>Crit Rating:</label><output>{statFormatter.format(enemy.critRating)}</output>
                        <label>Crit Multiplier:</label><output>{percentFormatter.format(enemy.critMultiplier)}</output>
                        <label>Action Speed:</label><output>{speedFormatter.format(enemy.actionSpeed)}</output>
                    </div>
                    <hr />
                    <div className="info-grid w-100">
                        <label>Health:</label><output>{statFormatter.format(enemy.health)}</output>
                        <label>Armour:</label><output>{statFormatter.format(enemy.armour)}</output>
                        <label>Barrier:</label><output>{statFormatter.format(enemy.barrier)}</output>
                        <label>Evasion:</label><output>{percentFormatter.format(enemy.evasion)}</output>
                    </div>
                    <hr />
                    <div className="info-grid w-100">
                        <label>Fire Resistance:</label><output>{percentFormatter.format(enemy.fireResistance)}</output>
                        <label>Cold Resistance:</label><output>{percentFormatter.format(enemy.coldResistance)}</output>
                        <label>Poison Resistance:</label><output>{percentFormatter.format(enemy.poisonResistance)}</output>
                    </div>
                </div>
            </div>
        )
    };

    function DisplayLevelDetails(areaLevel: AreaLevel | undefined) {
        if (areaLevel === undefined) {
            return (<></>);
        }
        const rewardsAlreadyReceived = areaLevel.area.number < nextArea ||
            (areaLevel.area.number == nextArea && areaLevel.level.number < nextLevel);
        const levelHasEnemies = areaLevel.level.frontEnemy !== null || areaLevel.level.middleEnemy !== null || areaLevel.level.backEnemy !== null;
        const canAttemptLevel = (areaLevel.area.number < nextArea ||
            (areaLevel.area.number == nextArea && areaLevel.level.number <= nextLevel));
        const partyHasCharacters = selectedParty !== undefined &&
            (selectedParty.frontCharacter !== null || selectedParty.middleCharacter !== null || selectedParty.backCharacter !== null);
        const disableAttemptButton = !levelHasEnemies || !partyHasCharacters;
        return (
            <Modal.Body>
                <h5>Enemies:</h5>
                <div className="enemy-grid">
                    {!levelHasEnemies && <div className="text-danger" role="alert">Level has no enemies.</div>}
                    {areaLevel.level.frontEnemy && DisplayEnemy(areaLevel.level.frontEnemy)}
                    {areaLevel.level.middleEnemy && DisplayEnemy(areaLevel.level.middleEnemy)}
                    {areaLevel.level.backEnemy && DisplayEnemy(areaLevel.level.backEnemy)}
                </div>
                <h5>Rewards:</h5>
                <ul>
                    {rewardsAlreadyReceived && <li>Rewards already received.</li>}
                    <li className={rewardsAlreadyReceived ? "received-reward" : ""}>Experience: {compactFormatter.format(areaLevel.level.experienceReward)}</li>
                    <li className={rewardsAlreadyReceived ? "received-reward" : ""}>Gold: {compactFormatter.format(areaLevel.level.goldReward)}</li>
                    <li className={rewardsAlreadyReceived ? "received-reward" : ""}>Diamonds: {compactFormatter.format(areaLevel.level.diamondReward)}</li>
                </ul>
                {
                    canAttemptLevel &&
                    <>
                        <h5>Party:</h5>
                        <Carousel className="select-party-carousel" interval={null} onSelect={handleSelectParty}>
                            {parties !== undefined ? parties.map(DisplayParty) : <></>}
                        </Carousel>
                        <Link to={{ pathname: "/game/areas/" + areaLevel.area.id + "/levels/" + areaLevel.level.id, search: "?partyId=" + selectedParty?.id }} className={"btn btn-primary float-end" + (disableAttemptButton ? " disabled" : "")} aria-disabled={disableAttemptButton}>Attempt</Link>
                    </>
                }
            </Modal.Body>
        )
    }

    return (
        <>
            {
                selectedAreaLevel &&
                <Modal show={modalOpen} onHide={CloseModal} className="level-modal">
                    <Modal.Header closeButton>
                        <Modal.Title>Level: {selectedAreaLevel.area.number} - {selectedAreaLevel.level.number}</Modal.Title>
                    </Modal.Header>
                    {DisplayLevelDetails(selectedAreaLevel)}
                </Modal>
            }
            {levelSelectError && <div className="text-danger" role="alert">{levelSelectError}</div>}
            <Carousel className="map" interval={null} wrap={false}>
                {areas.map(DisplayArea)}
            </Carousel>
        </>
    );
};

export default Map;