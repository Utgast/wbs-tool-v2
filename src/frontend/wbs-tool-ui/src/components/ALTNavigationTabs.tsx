type Tab =
    | "dashboard"
    | "wbs"
    | "resources"
    | "competencies"
    | "processes"
    | "administration";

interface Props {
    currentTab: Tab;
    onChange: (tab: Tab) => void;
}

export default function NavigationTabs({
    currentTab,
    onChange
}: Props) {
    return (
        <div style={{
            display: "flex",
            gap: "10px",
            marginBottom: "20px"
        }}>
            <button onClick={() => onChange("dashboard")}>
                Dashboard
            </button>

            <button onClick={() => onChange("wbs")}>
                WBS
            </button>

            <button onClick={() => onChange("resources")}>
                Ressourcen
            </button>

            <button onClick={() => onChange("competencies")}>
                Kompetenzen
            </button>

            <button onClick={() => onChange("processes")}>
                Prozesse
            </button>

            <button onClick={() => onChange("administration")}>
                Administration
            </button>
        </div>
    );
}