export type Rarity = "Common" | "Uncommon" | "Rare" | "Epic" | "Legendary";

export type CharacterInfo = {
    id: number,

    experience: number,
    experienceToNextLevel: number,
    level: number,
    rarity: Rarity,
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

export type CharacterSortOrderDictionary = {
    [key: string]: number
};

export type GetCharactersInfo = {
    characters: CharacterInfo[],
    summonCost: number,
    sortOrderOptions: CharacterSortOrderDictionary,
    defaultSortOrder: number,
}

async function FetchCharacters(sortOrder: number | null): Promise<GetCharactersInfo> {
    const sortOrderString = sortOrder === null ? "" : "?sortOrder=" + sortOrder;
    const response = await fetch("/api/Characters" + sortOrderString, {
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
    return await response.json() as GetCharactersInfo;
};

export default FetchCharacters;