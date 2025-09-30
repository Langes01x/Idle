import type { Account } from "./AccountContext";

async function FetchAccount(): Promise<Account> {
    const response = await fetch("/api/Account", {
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
    return await response.json() as Account;
};

export default FetchAccount;