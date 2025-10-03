export type TwoFactorInfo = {
    sharedKey: string,
    recoveryCodes: string[],
    recoveryCodesLeft: number,
    isTwoFactorEnabled: boolean,
    isMachineRemembered: boolean,
};

async function Fetch2faInfo(): Promise<TwoFactorInfo> {
    const response = await fetch("/api/manage/2fa", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: "{}",
        credentials: "include",
    });
    if (!response.ok) {
        const json = await response.json();
        throw new Error(json.message || response.statusText);
    }
    return await response.json() as TwoFactorInfo;
};

export default Fetch2faInfo;