import type { CharacterInfo } from "./Characters";

async function FetchCharacters(): Promise<CharacterInfo[]> {
    const response = await fetch("/api/Characters", {
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
    return await response.json() as CharacterInfo[];
};

export default FetchCharacters;