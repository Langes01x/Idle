import { useContext, useState } from "react";
import { Carousel } from "react-bootstrap";
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
    enemies: EnemyInfo[] | null,
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
    const [areaIndex, setAreaIndex] = useState<number>(nextArea - 1);
    const [, setSelectedArea] = useState<AreaInfo>(areas[nextArea - 1]);

    function handleSelectArea(index: number) {
        setAreaIndex(index);
        setSelectedArea(areas[index]);
    };

    function DisplayArea(area: AreaInfo) {
        return (
            <Carousel.Item>
                <Carousel.Caption>
                    <h2>Area {area.number}: {area.name}</h2>
                </Carousel.Caption>
                <div className="area">
                    {area.levels?.map(level => DisplayLevel(level, area))}
                </div>
            </Carousel.Item>
        );
    };

    function DisplayLevel(level: LevelInfo, area: AreaInfo) {
        return (
            <div className="level-container">
                <div className="level">
                    <img className={"level-node" + ((level.number - 1) % 10 === 9 ? " boss" : "")} src={node} />
                    <p className="level-text">{area.number} - {level.number}</p>
                    {level.number === nextLevel && area.number === nextArea && <img className="level-pin" src={pin} />}
                </div>
            </div>
        );
    };

    return (
        <Carousel className="map" interval={null} activeIndex={areaIndex} onSelect={handleSelectArea}
            wrap={false}>
            {areas.map(DisplayArea)}
        </Carousel>
    );
};

export default Map;