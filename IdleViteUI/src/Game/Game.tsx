import { useActionState, useContext, useEffect, useState, type JSX } from "react";
import AccountContext, { type Account } from "../Account/AccountContext";
import "./Game.css";
import { Link } from "react-router";

function Game() {
    const { account, setAccount } = useContext(AccountContext);
    const formatter = new Intl.NumberFormat(undefined, { notation: 'compact', maximumFractionDigits: 3 });
    const accumulationTime = Math.floor((new Date().getTime() - new Date(account!.lastIdleCollection).getTime()) / 1000);
    const [experienceRewards, setExperienceRewards] = useState(accumulationTime * account!.experienceAccumulationRate);
    const [goldRewards, setGoldRewards] = useState(accumulationTime * account!.goldAccumulationRate);
    const [diamondRewards, setDiamondRewards] = useState(accumulationTime * account!.diamondAccumulationRate);

    // Update collection rewards
    useEffect(() => {
        const intervalId = setInterval(() => {
            setExperienceRewards(e => e + account!.experienceAccumulationRate);
            setGoldRewards(g => g + account!.goldAccumulationRate);
            setDiamondRewards(d => d + account!.diamondAccumulationRate);
        }, 1000);

        return () => {
            clearInterval(intervalId);
        };
    }, [account]);

    const [modelError, handleCollect, isPending] = useActionState<JSX.Element | null | undefined, FormData>(
        async (_previousState, formData) => {
            try {
                const response = await fetch("/api/Account/Collect", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(Object.fromEntries(formData)),
                    credentials: "include",
                });
                if (!response.ok) {
                    const json = await response.json();
                    if ('errors' in json) {
                        return (<>{Object.values<string>(json.errors).map((val: string) => { return (<p>{val}</p>); })}</>);
                    }
                    return (<p>{response.statusText}</p>);
                }
                setExperienceRewards(0);
                setGoldRewards(0);
                setDiamondRewards(0);
                setAccount(await response.json() as Account);
                return null;
            } catch (error) {
                if (error instanceof Error) {
                    return (<p>{error.message}</p>);
                }
            }
        },
        null,
    );

    return (
        <div className="text-start">
            <div className="info-grid w-200px">
                <label>Experience:</label><output>{formatter.format(account!.experience)}</output>
                <label>Gold:</label><output>{formatter.format(account!.gold)}</output>
                <label>Diamonds:</label><output>{formatter.format(account!.diamonds)}</output>
            </div>
            <div className="rounded-box">
                <form className="form-floating" action={handleCollect}>
                    {modelError && <div className="text-danger" role="alert">{modelError}</div>}
                    <button type="submit" className="w-100 btn btn-primary" disabled={isPending}>Collect Rewards</button>
                </form>
                <div className="info-grid w-200px">
                    <label>Experience:</label><output>{formatter.format(experienceRewards)}</output>
                    <label>Gold:</label><output>{formatter.format(goldRewards)}</output>
                    <label>Diamonds:</label><output>{formatter.format(diamondRewards)}</output>
                </div>
            </div>
            <div>
                <Link to="/game/characters" className="btn btn-primary">Characters</Link>
            </div>
        </div>
    );
};

export default Game;