# Prozesse-Reiter: Deliverables sichtbar machen

**Sprint:** 042.2  
**Datum:** 2026-07-10  
**Status:** Abgeschlossen ✓

---

## 1. Ziel

Deliverables (Liefergegenstände) im Prozesse-Reiter sichtbar machen und zusammen mit den Leistungsphasen (LPH) darstellen, um projektspezifische Lieferergebnisse nachvollziehbar zu machen.

### Leitsatz

> "Sichtbarkeit vollständiger Prozessstrukturen – erst Read-Only, dann Bearbeitung."

---

## 2. Betroffene Dateien

| Datei | Änderung | Status |
|---|---|---|
| [ProcessesPage.tsx](../src/frontend/wbs-tool-ui/src/pages/ProcessesPage.tsx) | Erweitert um Deliverables-Tabelle, projektspezifisches Laden | ✅ |
| [App.jsx](../src/frontend/wbs-tool-ui/src/App.jsx) | ProcessesPage importiert, aktiviert statt Placeholder | ✅ |
| api.js | Keine Änderung (getProjectDeliverables existiert bereits) | ✓ |

**Gesamtumfang:** 2 Frontend-Dateien geändert, 0 Backend-Änderungen

---

## 3. Verwendete Datenquellen

### API-Endpunkte

| Endpunkt | Methode | Zweck |
|---|---|---|
| `/api/ProcessPhases` | GET | Alle Leistungsphasen (global) |
| `/api/projects/{projectId}/deliverables` | GET | Projekt-spezifische Deliverables |
| `/api/persons` | GET | Personen-Stammdaten für Owner-Lookup |

### Backend-Controller

- **DeliverablesController.cs** – `GetByProjectId(projectId)` bereits vorhanden
- **ProcessPhasesController.cs** – `GetAll()` bereits vorhanden
- **PersonsController.cs** – `GetAll()` bereits vorhanden

---

## 4. Angezeigte Felder

### Leistungsphasen-Tabelle (unverändert)

| Spalte | Quelle | Typ | Format |
|---|---|---|---|
| **Code** | code | string | Monospace (z.B. "LPH-01") |
| **Name** | name | string | Normal |
| **Reihenfolge** | sortOrder | int | Sortiert, zentriert |
| **Status** | isActive | boolean | Badge (Grün=Aktiv, Rot=Inaktiv) |

### Deliverables-Tabelle (neu)

| Spalte | Quelle | Typ | Format | Lookup |
|---|---|---|---|---|
| **Name** | name | string | Normal (Fettdruck) | – |
| **Typ** | type | enum | Raw-String | – |
| **Status** | status | enum | Raw-String | – |
| **Due Date** | dueDate | DateOnly | DD.MM.YYYY | – |
| **Phase** | processPhaseId | GUID | Phase-Name | ✓ Lookup in Phases |
| **Owner** | ownerPersonId | GUID | Person-Name | ✓ Lookup in Persons |

---

## 5. Fehlende Felder (bewusst nicht angezeigt)

| Feld | Grund |
|---|---|
| **description** | Zu viel Platz in Tabelle, wird nicht angezeigt |
| **wbsNodeId** | Würde Navigation erfordern (nicht MVP-Scope) |
| **projectId** | Redundant (wird via Projekt-Kontext geladen) |
| **createdAt** | Nicht relevant für Deliverables-Übersicht |

---

## 6. MVP-Grenzen (bewusst nicht implementiert)

### Funktional ausgeschlossen

- ❌ **Bearbeitung** von Deliverables
- ❌ **Erstellung** neuer Deliverables aus Prozess-View
- ❌ **Status-Übergänge** (z.B. Draft → InProgress → Delivered)
- ❌ **Freigaben** oder Approval-Workflows
- ❌ **Eskalationen** oder Handlungsbedarf-Integration
- ❌ **Vorschlagslogik** für überfällige Deliverables

### UI ausgeschlossen

- ❌ **Filter** nach Phase, Typ, Status
- ❌ **Suche** in Deliverables
- ❌ **Sortierung** interaktiv (nur Standard-Sortierung)
- ❌ **Details-View** oder Pop-Up für einzelne Deliverables
- ❌ **Navigation** zu Deliverable-Details oder WBS-Knoten

---

## 7. Technische Implementierungsdetails

### Props in ProcessesPage

```typescript
interface ProcessesPageProps {
  projectId?: string
}
```

- **projectId** wird von App.jsx übergeben: `selectedProject?.id`
- Ermöglicht projektspezifisches Laden der Deliverables
- Falls keine projectId vorhanden: nur LPH-Tabelle angezeigt, Deliverables-Section ausgeblendet

### Parallel-Laden in useEffect

```typescript
const [phasesData, deliverablesData, personsData] = await Promise.all([
  getProcessPhases(),
  projectId ? getProjectDeliverables(projectId) : Promise.resolve([]),
  getPersons(),
])
```

- **getProcessPhases()** – global, immer laden
- **getProjectDeliverables(projectId)** – nur wenn projectId vorhanden
- **getPersons()** – global, für Owner-Lookup

### Lookup-Funktionen

```typescript
// Phase-Name aus processPhaseId
const phaseName = deliverable.processPhaseId
  ? phases.find((p) => p.id === deliverable.processPhaseId)?.name || 'Nicht zugeordnet'
  : 'Nicht zugeordnet'

// Owner-Name aus ownerPersonId
const ownerName = deliverable.ownerPersonId
  ? persons.find((p) => p.id === deliverable.ownerPersonId)?.displayName || 'Unbekannt'
  : 'Unbekannt'

// Datum formatieren DD.MM.YYYY
const formattedDate = deliverable.dueDate
  ? new Date(deliverable.dueDate).toLocaleDateString('de-DE', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
    })
  : 'Nicht definiert'
```

---

## 8. Build-Ergebnis

### Backend Build

```
Status: ✅ Erfolgreich
Dauer: 1,0s
Output: bin\Debug\net9.0\WbsTool.Api.dll
```

**Hinweis:** Keine Backend-Änderungen erforderlich (Endpunkt bereits vorhanden).

### Frontend Build

```
Status: ✅ Erfolgreich
Dauer: 708ms
Module: 57 transformiert
Artefakte:
  - index.html        0.46 kB (gzip: 0.30 kB)
  - index-*.css      12.58 kB (gzip: 2.77 kB)
  - index-*.js      280.56 kB (gzip: 81.32 kB)
```

---

## 9. Reifegradverbesserung

### Prozesse-Reiter Reifegrad

| Aspekt | Vorher | Nachher | Verbesserung |
|---|---|---|---|
| **LPH-Struktur** | 40% | 40% | ✓ Unverändert |
| **Deliverables-Sichtbarkeit** | – | 60% | ✓ Neu implementiert |
| **Gesamt Prozesse-Reiter** | **40%** | **60%** | ✓ **+20%** |

### Bewertung

Nach Sprint-042.2 können Benutzer:
- ✅ Alle Leistungsphasen überblicken
- ✅ Projekt-spezifische Deliverables sehen
- ✅ Deliverables zu Phasen zuordnen (via Phase-Name)
- ✅ Owner pro Deliverable identifizieren
- ✅ Fälligkeitsdaten einsehen

Noch nicht möglich:
- ❌ Status-Übergänge
- ❌ Bearbeitung oder Erstellung
- ❌ Automatische Eskalation überfälliger Deliverables

---

## 10. Nächster Mikro-Sprint

**Sprint-043.1: Kompetenzen-Reiter – Kompetenzkatalog sichtbar machen**

### Scope

- **Ziel:** Kompetenzkatalog anzeigen (Code, Name, Kategorie, Status)
- **Dateien:** 2–3 (api.js, CompetenciesPage.tsx, ggf. App.jsx)
- **API:** 1 Endpunkt (getCompetencies)
- **Reifegrad:** Kompetenzen 40% → 60%

### Vorbereitung

- Prüfen: Existiert CompetenciesController?
- Prüfen: Welche Felder in CompetencyDto?
- Prüfen: Ist CompetenciesPage bereits in App.jsx?

---

## 11. Versionierung

- **Sprint:** 042.2
- **Titel:** Prozesse-Reiter – Deliverables sichtbar machen
- **Datum:** 2026-07-10
- **Codeänderungen:** 2 Frontend-Dateien
- **Backend-Änderungen:** 0
- **Dokumentationen:** 1 (diese Datei)

---

