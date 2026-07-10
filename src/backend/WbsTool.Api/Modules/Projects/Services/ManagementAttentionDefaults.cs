namespace WbsTool.Api.Modules.Projects.Services;

/// <summary>
/// Zentrale Konfiguration fuer die Handlungsbedarf-Vorschlagslogik.
/// Vorbereitung fuer spaetere Administration. Noch keine Persistierung.
/// </summary>
internal static class ManagementAttentionDefaults
{
    // --- Priority Scores ---

    public const decimal CriticalRiskPriorityScore = 100m;
    public const decimal OverdueDeliverablePriorityScore = 90m;
    public const decimal OpenRiskPriorityScore = 70m;

    // --- Reaktionsfristen (Tage ab heute) ---

    public const int CriticalRiskReactionDays = 3;
    public const int OverdueDeliverableReactionDays = 2;
    public const int OpenRiskReactionDays = 7;

    // --- Standardmassnahmen ---

    public const string CriticalRiskSuggestedAction = "Risikobewertung und Massnahme pruefen";
    public const string OpenRiskSuggestedAction = "Risiko ueberwachen und Status pruefen";
    public const string OverdueDeliverableSuggestedAction = "Deliverable Status klaeren";
}
