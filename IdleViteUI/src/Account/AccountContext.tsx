import { createContext } from "react";

export type Account = {
    id: string,
    email: string,
    lastIdleCollection: Date,
    experience: number,
    gold: number,
    diamonds: number,
    levelsCleared: number,
};

const AccountContext = createContext<{ account: Account | null, setAccount: React.Dispatch<React.SetStateAction<Account | null>> }>({ account: null, setAccount: () => null });
export default AccountContext;