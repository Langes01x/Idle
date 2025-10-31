import type { CharacterInfo } from "./FetchCharacters";

export type PartyInfo = {
    id: number,
    name: string | undefined,
    backCharacter: CharacterInfo | null,
    middleCharacter: CharacterInfo | null,
    frontCharacter: CharacterInfo | null,
};

async function FetchParties() {
    const response = await fetch("/api/Parties/", {
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
    return await response.json() as PartyInfo[];
};

export default FetchParties;