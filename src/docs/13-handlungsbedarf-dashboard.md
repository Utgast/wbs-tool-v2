# 13 - Handlungsbedarf im Dashboard

## Ziel

Sprint-039 macht den berechneten Handlungsbedarf erstmals im Projektleiter-Cockpit sichtbar.

Die bestehende Logik aus `ManagementAttentionService` wird ohne Aenderung der Fachlogik in das Dashboard integriert. Der Projektleiter kann die vorgeschlagenen Massnahmen mit echten Projektdaten bewerten.

---

## Betroffene Dateien

| Datei | Aenderung |
|---|---|
| `Modules/Projects/Contracts/ProjectDashboardDto.cs` | Neues Feld `TopManagementAttentionItems` |
| `Modules/Projects/Services/ProjectDashboardService.cs` | `IManagementAttentionService` injiziert, Top 10 berechnet |
| `src/pages/DashboardPage.jsx` | Neuer Bereich "TOP HANDELN" mit `AttentionItemCard`-Komponente |

---

## Backend-Anpassungen

### ProjectDashboardDto

Neues Feld:

```csharp
public List<ManagementAttentionViewDto> TopManagementAttentionItems { get; set; } = [];
```

### ProjectDashboardService

`IManagementAttentionService` wird per Constructor Injection hinzugefuegt.

Berechnung:

```csharp
var topManagementAttentionItems = _managementAttentionService
    .GetAttentionItems(projectId)
    .Take(10)
    .ToList();
```

Die Sortierung (PriorityScore absteigend) kommt bereits aus `ManagementAttentionService.GetAttentionItems`.

---

## Dashboard-Anpassungen

Neuer Bereich "TOP HANDELN" wird nach dem `HandlungsbedarfPanel` angezeigt.

Maximal 10 Eintraege (Backend-seitig bereits begrenzt).

Neue Komponente `AttentionItemCard` rendert jeden Eintrag.

---

## Angezeigte Felder

| Feld | Anzeige |
|---|---|
| PriorityScore | Badge (farblich) |
| Type | Label oben links |
| Title | Hauptueberschrift der Karte |
| Explanation | Erklaerungstext |
| SuggestedAction | Blau hervorgehobene Handlungsempfehlung |
| ReactionDate | Datum mit Label "Reaktion bis:" |
| SuggestedOwnerPersonId | GUID mit Label "Vorgeschl. Owner:" |

---

## Bewertungslogik (Farbe)

| PriorityScore | Farbe |
|---|---|
| >= 90 | Rot |
| 70 - 89 | Gelb |
| < 70 | Gruen |

Reine Darstellung. Keine neue Fachlogik.

---

## MVP-Grenzen

Bewusst nicht Teil von Sprint-039:

- keine Persistierung
- keine Admin-Konfiguration
- keine Bearbeitungsfunktion (kein Uebernehmen / Ablehnen / Zuweisen)
- keine Kompetenz-Gaps
- keine Ressourcen-Gaps

---

## Build-Ergebnis

```
dotnet build -o bin/_sprint039_build  →  Erfolgreich
npm run build                          →  Erfolgreich (749ms)
```

---

## Erste Beobachtungen

Mit echten Projektdaten kann der Projektleiter jetzt:
- sehen, welche Risiken und Deliverables den hoechsten Handlungsbedarf erzeugen
- die Erklaerungen nachvollziehen (Explanation-Feld)
- die vorgeschlagenen Reaktionsdaten und Massnahmen bewerten
- pruefen, ob PriorityScore-Gewichtung (100 / 90 / 70) fachlich korrekt ist

---

## Naechster Sprint

Sprint-040 – Review Handlungsbedarf mit echten Projektdaten

Ziel: Bewertung der Vorschlagslogik anhand realer Daten. Anpassungsbedarf identifizieren.
