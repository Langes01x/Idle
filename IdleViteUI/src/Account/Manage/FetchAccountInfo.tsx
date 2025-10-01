export type AccountInfo = {
    email: string,
    isEmailConfirmed: boolean,
};

async function FetchAccountInfo(): Promise<AccountInfo> {
    const response = await fetch("/api/manage/info", {
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
    return await response.json() as AccountInfo;
};

export default FetchAccountInfo;