import type { Character } from "./Characters";

async function FetchCharacters(): Promise<Character[]> {
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
    return await response.json() as Character[];
};

export default FetchCharacters;