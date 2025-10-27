import { Carousel } from "react-bootstrap";
import type { CharacterInfo, GetCharactersInfo } from "./FetchCharacters";
import { Link, useLoaderData } from "react-router";
import { useActionState, useState, type ChangeEvent, type JSX } from "react";
import { DisplayCharacterInfo } from "./Characters";
import FetchCharacters from "./FetchCharacters";
import "./Parties.css";

type PartyInfo = {
    id: number,
    name: string | undefined,
    backCharacter: CharacterInfo | null,
    middleCharacter: CharacterInfo | null,
    frontCharacter: CharacterInfo | null,
};

type PartiesLoaderInfo = {
    parties: PartyInfo[],
    getCharactersInfo: GetCharactersInfo,
};

type Position = "Back" | "Middle" | "Front";

function DisplayAddToParty() {
    return (
        <div className="add-button">
            +
        </div>
    );
};

export async function PartiesLoader() {
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

    const loaderInfo: PartiesLoaderInfo = {
        parties: await response.json() as PartyInfo[],
        getCharactersInfo: await FetchCharacters(null)
    };
    return loaderInfo;
};

function Parties() {
    const { parties, getCharactersInfo: { characters: loadedCharacters, sortOrderOptions, defaultSortOrder } } = useLoaderData<PartiesLoaderInfo>();
    const [sortOrder, setSortOrder] = useState(defaultSortOrder);
    const [characters, setCharacters] = useState(loadedCharacters);
    const [partyIndex, setPartyIndex] = useState<number | undefined>(parties.length > 0 ? 0 : undefined);
    const [selectedParty, setSelectedParty] = useState<PartyInfo | undefined>(parties.length > 0 ? parties[0] : undefined);
    const [selectedPosition, setSelectedPosition] = useState<Position | undefined>(undefined);

    function handleSelectParty(index: number | undefined) {
        setPartyIndex(index);
        setSelectedParty(index === undefined ? undefined : parties[index]);
    };

    const [updateError, handleUpdateName, isUpdatePending] = useActionState<JSX.Element | null | undefined>(
        async () => {
            try {
                const response = await fetch("/api/Parties", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(selectedParty),
                    credentials: "include",
                });
                if (!response.ok) {
                    const json = await response.json();
                    if ('errors' in json) {
                        return (<>{Object.values<string>(json.errors).map((val: string) => { return (<p>{val}</p>); })}</>);
                    }
                    return (<p>{response.statusText}</p>);
                }
                return null;
            } catch {
                return (<p>Party save failed</p>);
            }
        },
        null,
    );

    function DisplayParty(party: PartyInfo) {
        function handlePartyNameChange(event: ChangeEvent<HTMLInputElement>) {
            party.name = event.target.value;
        };

        return (
            <Carousel.Item>
                <div className="col-md-6 offset-md-3">
                    <div className="input-group">
                        {updateError && <div className="text-danger" role="alert">{updateError}</div>}
                        <input type="text" className="form-control" placeholder="Party Name" value={party.name} onChange={handlePartyNameChange}></input>
                        <button className="btn btn-primary" onClick={handleUpdateName} disabled={isUpdatePending}>Update</button>
                    </div>
                </div>
                <div className="char-grid">
                    <button className="btn btn-dark character-button" onClick={() => setSelectedPosition("Back")}>
                        <div className="new-char-container character">
                            {selectedPosition == "Back" ? <div className="new-char-border"></div> : <></>}
                            <div className="new-char-content">
                                {party.backCharacter === null ? DisplayAddToParty() : DisplayCharacterInfo(party.backCharacter)}
                            </div>
                        </div>
                    </button>
                    <button className="btn btn-dark character-button" onClick={() => setSelectedPosition("Middle")}>
                        <div className="new-char-container character">
                            {selectedPosition == "Middle" ? <div className="new-char-border"></div> : <></>}
                            <div className="new-char-content">
                                {party.middleCharacter === null ? DisplayAddToParty() : DisplayCharacterInfo(party.middleCharacter)}
                            </div>
                        </div>
                    </button>
                    <button className="btn btn-dark character-button" onClick={() => setSelectedPosition("Front")}>
                        <div className="new-char-container character">
                            {selectedPosition == "Front" ? <div className="new-char-border"></div> : <></>}
                            <div className="new-char-content">
                                {party.frontCharacter === null ? DisplayAddToParty() : DisplayCharacterInfo(party.frontCharacter)}
                            </div>
                        </div>
                    </button>
                </div>
            </Carousel.Item>
        );
    };

    function DisplayCharacter(char: CharacterInfo) {
        if (selectedParty === undefined ||
            selectedPosition === undefined ||
            selectedParty.backCharacter?.id === char.id ||
            selectedParty.middleCharacter?.id === char.id ||
            selectedParty.frontCharacter?.id === char.id) {
            return (<></>);
        }
        return (
            <button className="btn btn-dark character-button" onClick={() => handleSetCharacter(char)}>
                <div className="rounded-box character">
                    {DisplayCharacterInfo(char)}
                </div>
            </button>
        );
    };

    function DisplayRemove() {
        if (selectedParty === undefined ||
            selectedPosition === undefined) {
            return (<></>);
        }
        if (selectedPosition === "Back" &&
            selectedParty.backCharacter === null) {
            return (<></>);
        }
        if (selectedPosition === "Middle" &&
            selectedParty.middleCharacter === null) {
            return (<></>);
        }
        if (selectedPosition === "Front" &&
            selectedParty.frontCharacter === null) {
            return (<></>);
        }
        return (
            <button className="btn btn-dark character-button" onClick={() => handleSetCharacter(null)}>
                <div className="rounded-box character">
                    <div className="add-button">
                        –
                    </div>
                </div>
            </button>
        );
    };

    const [addError, handleAddParty, isAddPending] = useActionState<JSX.Element | null | undefined>(
        async () => {
            try {
                const party: PartyInfo = {
                    id: 0,
                    name: undefined,
                    backCharacter: null,
                    middleCharacter: null,
                    frontCharacter: null,
                };
                const response = await fetch("/api/Parties", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(party),
                    credentials: "include",
                });
                if (!response.ok) {
                    const json = await response.json();
                    if ('errors' in json) {
                        return (<>{Object.values<string>(json.errors).map((val: string) => { return (<p>{val}</p>); })}</>);
                    }
                    return (<p>{response.statusText}</p>);
                }
                const createdParty = await response.json() as PartyInfo;
                parties.push(createdParty);
                setPartyIndex(parties.length - 1);
                setSelectedParty(createdParty);
                return null;
            } catch {
                return (<p>Party creation failed</p>);
            }
        },
        null,
    );

    const [removeError, handleRemoveParty, isRemovePending] = useActionState<JSX.Element | null | undefined>(
        async () => {
            try {
                if (partyIndex === undefined) {
                    return null;
                }
                const response = await fetch("/api/Parties/" + selectedParty?.id + "/Delete", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    credentials: "include",
                });
                if (!response.ok) {
                    const json = await response.json();
                    if ('errors' in json) {
                        return (<>{Object.values<string>(json.errors).map((val: string) => { return (<p>{val}</p>); })}</>);
                    }
                    return (<p>{response.statusText}</p>);
                }
                parties.splice(partyIndex, 1);
                const newPartyIndex = parties.length === 0 ? undefined : Math.min(partyIndex, parties.length - 1);
                setPartyIndex(newPartyIndex);
                setSelectedParty(newPartyIndex === undefined ? undefined : parties[newPartyIndex]);
                return null;
            } catch {
                return (<p>Party removal failed</p>);
            }
        },
        null,
    );

    const [setError, handleSetCharacter,] = useActionState<JSX.Element | null | undefined, CharacterInfo | null>(
        async (_previousState, char) => {
            function ResetOriginalCharacter(originalCharacter: CharacterInfo | null) {
                if (selectedParty !== undefined) {
                    if (selectedPosition === "Back") {
                        selectedParty.backCharacter = originalCharacter;
                    }
                    if (selectedPosition === "Middle") {
                        selectedParty.middleCharacter = originalCharacter;
                    }
                    if (selectedPosition === "Front") {
                        selectedParty.frontCharacter = originalCharacter;
                    }
                }
            };

            if (selectedParty === undefined) {
                return null;
            }
            let originalCharacter: CharacterInfo | null = null;
            if (selectedPosition === "Back") {
                originalCharacter = selectedParty.backCharacter;
                selectedParty.backCharacter = char;
            }
            if (selectedPosition === "Middle") {
                originalCharacter = selectedParty.middleCharacter;
                selectedParty.middleCharacter = char;
            }
            if (selectedPosition === "Front") {
                originalCharacter = selectedParty.frontCharacter;
                selectedParty.frontCharacter = char;
            }
            try {
                const response = await fetch("/api/Parties", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(selectedParty),
                    credentials: "include",
                });
                if (!response.ok) {
                    ResetOriginalCharacter(originalCharacter);
                    const json = await response.json();
                    if ('errors' in json) {
                        return (<>{Object.values<string>(json.errors).map((val: string) => { return (<p>{val}</p>); })}</>);
                    }
                    return (<p>{response.statusText}</p>);
                }
                return null;
            } catch {
                ResetOriginalCharacter(originalCharacter);
                return (<p>Character set failed</p>);
            }
        },
        null,
    );

    const [sortError, handleSort, isSortPending] = useActionState<JSX.Element | null | undefined, ChangeEvent<HTMLSelectElement>>(
        async (_previousState, event) => {
            event.preventDefault();
            try {
                const newValue = +event.target.value;
                const charactersInfo = await FetchCharacters(newValue);
                setCharacters(charactersInfo.characters);
                setSortOrder(newValue);
                return null;
            } catch {
                return (<p>Sorting failed</p>);
            }
        },
        null,
    );

    return (
        <div>
            <div className="text-start">
                <Link to="/game" className="btn btn-primary">&lt;&lt; Back</Link>
            </div>
            <Carousel className="party-carousel" interval={null} activeIndex={partyIndex} onSelect={handleSelectParty}>
                {parties.map(DisplayParty)}
            </Carousel>
            <div className="text-start">
                <button className="btn btn-primary" onClick={handleAddParty} disabled={isAddPending}>Add Party</button>
                {addError && <div className="text-danger d-inline-block" role="alert">{addError}</div>}
                <button className="btn btn-primary" onClick={handleRemoveParty} disabled={selectedParty === undefined || isRemovePending}>Remove Party</button>
                {removeError && <div className="text-danger d-inline-block" role="alert">{removeError}</div>}
                {setError && <div className="text-danger d-inline-block" role="alert">{setError}</div>}
                <form className="float-end">
                    {sortError && <div className="text-danger" role="alert">{sortError}</div>}
                    <label htmlFor="sortOrder">Sort by:&nbsp;</label>
                    <select id="sortOrder" name="sortOrder" value={sortOrder} onChange={handleSort} disabled={isSortPending}>
                        {
                            Object.entries<number>(sortOrderOptions).map(([label, value]) => <option key={label} value={value}>{label}</option>)
                        }
                    </select>
                </form>
            </div>
            <div className="char-grid">
                {DisplayRemove()}
                {characters.map(DisplayCharacter)}
            </div>
        </div>
    );
};

export default Parties;