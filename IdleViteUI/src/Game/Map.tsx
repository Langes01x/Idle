import { useActionState, useContext, useState, type JSX } from "react";
import { Carousel, Modal } from "react-bootstrap";
import { useLoaderData } from "react-router";
import AccountContext from "../Account/AccountContext";
import pin from "../../public/pin.svg"
import node from "../../public/node.svg"
import "./Map.css"

type AreaInfo = {
    id: number,
    number: number,
    name: string,
    levels: LevelInfo[] | null,
};

type LevelInfo = {
    id: number,
    number: number,
    experienceReward: number,
    goldReward: number,
    diamondReward: number,
    backEnemy: EnemyInfo | null,
    middleEnemy: EnemyInfo | null,
    frontEnemy: EnemyInfo | null,
};

type EnemyInfo = {
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
    armour: number,
    barrier: number,
    evasion: number,
    fireResistance: number,
    coldResistance: number,
    poisonResistance: number,
};

type AreaLevel = {
    level: LevelInfo,
    area: AreaInfo,
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
    const compactFormatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });
    const percentFormatter = new Intl.NumberFormat(undefined, { style: 'percent' });
    const speedFormatter = new Intl.NumberFormat(undefined, { style: 'unit', unit: 'second', maximumFractionDigits: 2 });
    const statFormatter = new Intl.NumberFormat(undefined, { maximumFractionDigits: 2 });

    function CloseModal() {
        setSelectedAreaLevel(undefined);
        setModalOpen(false);
    };

    const [levelSelectError, handleLevelSelect, isLevelSelectPending] = useActionState<JSX.Element | null | undefined, AreaLevel>(
        async (_previousState, areaLevel) => {
            try {
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
                <div className="level" onClick={() => handleLevelSelect(areaLevel)} aria-disabled={isLevelSelectPending}>
                    <img className={"level-node" + ((areaLevel.level.number - 1) % 10 === 9 ? " boss" : "")} src={node} />
                    <p className="level-text">{areaLevel.area.number} - {areaLevel.level.number}</p>
                    {areaLevel.level.number === nextLevel && areaLevel.area.number === nextArea && <img className="level-pin" src={pin} />}
                </div>
            </div>
        );
    };

    function DisplayEnemy(enemy: EnemyInfo) {
        return (
            <div className="rounded-box enemy">
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

    return (
        <>
            {
                selectedAreaLevel &&
                <Modal show={modalOpen} onHide={CloseModal}>
                    <Modal.Header closeButton>
                        <Modal.Title>Level: {selectedAreaLevel.area.number} - {selectedAreaLevel.level.number}</Modal.Title>
                    </Modal.Header>
                    <Modal.Body>
                        <h5>Enemies:</h5>
                        <div className="enemy-grid">
                            {selectedAreaLevel.level.frontEnemy && DisplayEnemy(selectedAreaLevel.level.frontEnemy)}
                            {selectedAreaLevel.level.middleEnemy && DisplayEnemy(selectedAreaLevel.level.middleEnemy)}
                            {selectedAreaLevel.level.backEnemy && DisplayEnemy(selectedAreaLevel.level.backEnemy)}
                        </div>
                        <h5>Rewards:</h5>
                        <ul>
                            <li>Experience: {compactFormatter.format(selectedAreaLevel.level.experienceReward)}</li>
                            <li>Gold: {compactFormatter.format(selectedAreaLevel.level.goldReward)}</li>
                            <li>Diamonds: {compactFormatter.format(selectedAreaLevel.level.diamondReward)}</li>
                        </ul>
                    </Modal.Body>
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