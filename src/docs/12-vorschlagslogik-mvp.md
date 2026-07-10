# 12 - Vorschlagslogik MVP

## Ziel

Sprint-038 implementiert die erste Version der Vorschlagslogik fuer Handlungsbedarf.

Der ManagementAttentionService erzeugt jetzt pro Handlungsbedarfstyp konkrete, erklaerbare Vorschlaege:
- SuggestedAction (was zu tun ist)
- Explanation (warum dieser Handlungsbedarf entstand)
- ReactionDate (bis wann reagiert werden soll)

Leitprinzip: System schlaegt vor – Mensch entscheidet.

---

## Betroffene Dateien

| Datei | Aenderung |
|---|---|
| `Modules/Projects/Services/ManagementAttentionDefaults.cs` | Neu angelegt – zentrale Konfigurationsklasse |
| `Modules/Projects/Services/ManagementAttentionService.cs` | BuildRiskAttentionItems und BuildOverdueDeliverableAttentionItems erweitert |

---

## Neue Vorschlagsregeln

### Critical Risk (Severity = High, Status offen)

| Feld | Wert |
|---|---|
| SuggestedAction | "Risikobewertung und Massnahme pruefen" |
| Explanation | "Risiko '{Title}' ist mit Schweregrad High eingestuft und noch offen. Unmittelbarer Handlungsbedarf." |
| ReactionDate | heute + 3 Tage |
| PriorityScore | 100 |

### Open Risk (Severity < High, Status offen)

| Feld | Wert |
|---|---|
| SuggestedAction | "Risiko ueberwachen und Status pruefen" |
| Explanation | "Risiko '{Title}' ist offen und wurde noch nicht abgeschlossen. Regelmaessige Ueberwachung erforderlich." |
| ReactionDate | heute + 7 Tage |
| PriorityScore | 70 |

### Overdue Deliverable (DueDate < heute, Status != Delivered)

| Feld | Wert |
|---|---|
| SuggestedAction | "Deliverable Status klaeren" |
| Explanation | "Deliverable '{Name}' war faellig am {DueDate} und ist noch nicht abgeliefert. Faelligkeitsdatum ueberschritten." |
| ReactionDate | heute + 2 Tage |
| PriorityScore | 90 |

---

## Explanation-Logik

Jede Explanation ist kontextspezifisch und enthaelt:
- den konkreten Titel oder Namen des betroffenen Objekts
- den Grund fuer den Handlungsbedarf (Schweregrad, Faelligkeit)
- eine handlungsorientierte Aussage

Principle: Vorschlaege muessen erklärbar sein (vgl. Architekturprinzipien).

Technisch: Die Explanation wird in-memory generiert (nach DB-Abfrage), da String-Interpolation nicht in EF Core SQL uebersetzbar ist.

---

## ReactionDate-Logik

Das ReactionDate wird nicht mehr aus den Quelldaten uebernommen (z. B. Risk.DueDate), sondern immer relativ zu `heute` berechnet:

| Typ | Formel |
|---|---|
| Critical Risk | today.AddDays(3) |
| Open Risk | today.AddDays(7) |
| Overdue Deliverable | today.AddDays(2) |

Die Offsetwerte sind in `ManagementAttentionDefaults` zentralisiert.

---

## Zentrale Konfiguration

Neue Datei: `ManagementAttentionDefaults.cs` (internal static class)

Dort gebuendelt:
- PriorityScore je Typ (CriticalRiskPriorityScore, OpenRiskPriorityScore, OverdueDeliverablePriorityScore)
- Reaktionsfristen in Tagen (CriticalRiskReactionDays, OpenRiskReactionDays, OverdueDeliverableReactionDays)
- Standardmassnahmen-Texte (CriticalRiskSuggestedAction, OpenRiskSuggestedAction, OverdueDeliverableSuggestedAction)

Noch keine Persistierung. Noch keine Admin-Oberflaeche.
Technische Vorbereitung fuer spaetere Konfigurierbarkeit.

---

## MVP-Grenzen

Bewusst nicht Teil von Sprint-038:

- keine API
- kein Dashboard
- keine Persistierung
- keine Admin-Oberflaeche
- keine Kompetenz-Gaps
- keine Ressourcen-Gaps
- keine neue Entity
- keine Datenbankmigrationen

---

## Build-Ergebnis

```
dotnet build -o bin/_sprint038_build
Build: Erfolgreich
WbsTool.Api.dll
```

---

## Naechster Sprint

Sprint-039 – Handlungsbedarf-Konfiguration (Admin-Zielbild)

Ziel: Konzept und technisches Zielbild fuer eine spaetere Administration der Konfigurationswerte aus ManagementAttentionDefaults.
