# 17 - Prozesse: LPH-Übersicht

## Ziel

Sprint-042.1 macht den Prozesse-Reiter erstmals nutzbar.

Prozessphasen / LPH werden tabellarisch angezeigt mit Code, Name, Reihenfolge und Status.

---

## Betroffene Dateien

| Datei | Änderung |
|---|---|
| `src/frontend/wbs-tool-ui/src/services/api.js` | Neue Funktion `getProcessPhases()` |
| `src/frontend/wbs-tool-ui/src/pages/ProcessesPage.tsx` | Komplett rewritten – Tabelle mit LPH |

**Backend:**
Keine Änderung – existierender Endpoint wird verwendet.

---

## Verwendete API

### Backend-Endpoint

```
GET /api/ProcessPhases
```

**Controller:** `ProcessPhasesController.cs`  
**Service:** `IProcessPhaseService.GetAll()`

Liefert alle Prozessphasen (ProcessPhaseDto):
- Id
- Code
- Name
- Goal
- Description
- DefaultResponsibility
- SortOrder
- IsActive

---

## Angezeigte Felder

Tabelle mit 4 Spalten:

| Spalte | Quelle | Beschreibung |
|---|---|---|
| Code | ProcessPhaseDto.Code | Eindeutiger Phasencode |
| Name | ProcessPhaseDto.Name | Bezeichnung der Phase |
| Reihenfolge | ProcessPhaseDto.SortOrder | Sequenznummer für die Abfolge |
| Status | ProcessPhaseDto.IsActive | Badge: "Aktiv" (grün) oder "Inaktiv" (rot) |

Tabelle wird nach SortOrder sortiert angezeigt.

---

## MVP-Grenzen

Bewusst nicht Teil von Sprint-042.1:

- Keine Deliverables
- Keine Workflows
- Keine Bearbeitung
- Keine Zuweisung von Deliverables zu Phasen
- Keine Lieferfähigkeit pro Phase
- Keine Handlungsbedarf-Logik

---

## Build-Ergebnis

```
dotnet build → Erfolgreich
npm run build → Erfolgreich
```

---

## Reifegradverbesserung

**Prozesse-Reiter vor Sprint-042.1:** 20 %  
**Prozesse-Reiter nach Sprint-042.1:** 40 %

Grund: LPH-Struktur ist jetzt sichtbar. Basis für Deliverable-Anzeige (Sprint-042.2) gelegt.

---

## Nächster Mikro-Sprint

Sprint-042.2 – Prozesse-Reiter: Deliverables sichtbar machen

Ziel: Deliverables im Prozesse-Reiter anzeigen (Name, Type, Status, DueDate, Owner).
