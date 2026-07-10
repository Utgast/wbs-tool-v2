# Sprint-042.2: Prozesse-Reiter – Deliverables sichtbar machen

**Datum:** 2026-07-10  
**Typ:** Micro-Sprint (MVP-Fertigstellungsmodus)  
**Status:** Abgeschlossen ✓

---

## 1. Ziel

Den Prozesse-Reiter funktional vervollständigen, indem projekt-spezifische Deliverables sichtbar gemacht werden – zusammen mit den Leistungsphasen. Read-Only, keine Bearbeitung.

### Leitsatz

> **"Sichtbarkeit vor Bearbeitung. Erst Nutzbarkeit, dann Intelligenz."**

---

## 2. Umsetzung

### Schritt 1: ProcessesPage erweitern

**Änderungen:**
- Props: `projectId?: string` hinzugefügt
- Types: `Deliverable` und `Person` hinzugefügt
- State: `deliverables`, `persons` hinzugefügt (zusätzlich zu `phases`)
- useEffect: Parallel-Laden von 3 Datenquellen
  - `getProcessPhases()` – global
  - `getProjectDeliverables(projectId)` – projekt-spezifisch
  - `getPersons()` – für Owner-Lookup
- UI: Zweite Tabelle mit 6 Spalten (Name, Typ, Status, Due Date, Phase, Owner)
- Lookups: Phase-Name und Owner-Name aus Arrays ermittelt

**Code:**
- Type-safe mit TypeScript
- Fehlerbehandlung via `handleResponse()`
- Fallback-Werte bei fehlenden Lookups
- Responsive Tabellenformatierung

### Schritt 2: App.jsx anpassen

**Änderungen:**
- Import: `import ProcessesPage from './pages/ProcessesPage'`
- Rendering: `{currentTab === 'processes' && <ProcessesPage projectId={selectedProject?.id} />}`
- Vorher: Placeholder-Section
- Nachher: Aktive ProcessesPage-Komponente

---

## 3. Geänderte Dateien

| Datei | Zeilen | Änderung |
|---|---|---|
| [src/frontend/wbs-tool-ui/src/pages/ProcessesPage.tsx](../src/frontend/wbs-tool-ui/src/pages/ProcessesPage.tsx) | ~300 | Komplett rewritten: Props, Types, State, Deliverables-Tabelle |
| [src/frontend/wbs-tool-ui/src/App.jsx](../src/frontend/wbs-tool-ui/src/App.jsx) | ~5 | Import + Conditional Rendering angepasst |
| api.js | 0 | Keine Änderung (getProjectDeliverables existiert) |

**Gesamtumfang:** 2 Dateien, ~305 Zeilen Code

---

## 4. Verwendete Datenquellen

### Backend-Endpunkte

```
GET /api/ProcessPhases
  → [{id, code, name, goal, description, defaultResponsibility, sortOrder, isActive}]

GET /api/projects/{projectId}/deliverables
  → [{id, name, type, status, dueDate, ownerPersonId, processPhaseId, ...}]

GET /api/persons
  → [{id, displayName, shortName, email, isPlaceholder, placeholderType, isActive}]
```

### Lookups

- **Phase-Name:** `phases.find(p => p.id === processPhaseId)?.name`
- **Owner-Name:** `persons.find(p => p.id === ownerPersonId)?.displayName`
- **Datum:** `new Date(dueDate).toLocaleDateString('de-DE', {...})`

---

## 5. Build-Ergebnisse

### Backend Build ✅

```
Status:        Erfolgreich
Dauer:         1,0s
Wiederherst.:  0,3s
Output:        bin\Debug\net9.0\WbsTool.Api.dll
Warnings:      0
Errors:        0
```

**Hinweis:** Keine Code-Änderungen am Backend erforderlich.

### Frontend Build ✅

```
Status:        Erfolgreich
Dauer:         708ms
Tool:          vite 5.4.21
Modules:       57 transformed
CSS:           12.58 kB (gzip: 2.77 kB)
JS:            280.56 kB (gzip: 81.32 kB)
Output:        dist/
Warnings:      0
Errors:        0
```

---

## 6. MVP-Grenzen (bewusst NICHT implementiert)

### Funktional ausgeschlossen

| Funktion | Grund | Status |
|---|---|---|
| **Bearbeitung** | Später in Bearbeitungs-Sprint | ❌ |
| **Erstellung** | Requires UI Wizards | ❌ |
| **Status-Übergänge** | Requires Workflow-Engine | ❌ |
| **Freigaben** | Requires Approval-Logic | ❌ |
| **Eskalationen** | Requires Intelligence-Layer | ❌ |
| **Handlungsbedarf-Integration** | Requires Scoring | ❌ |
| **Vorschläge** | Requires Management-Attention | ❌ |

### UI ausgeschlossen

| UI-Element | Grund | Status |
|---|---|---|
| **Filter** | Später in Filter-Sprint | ❌ |
| **Suche** | Später in Search-Sprint | ❌ |
| **Sortierung** (interaktiv) | Standard-Sortierung ausreichend | ❌ |
| **Details-View** | Requires Navigation-System | ❌ |
| **Navigation** | Out of Scope für Read-Only | ❌ |

---

## 7. Reifegrad-Verbesserung

### Prozesse-Reiter

| Metrik | Vorher | Nachher | Δ |
|---|---|---|---|
| **Leistungsphasen sichtbar** | ✅ 40% | ✅ 40% | – |
| **Deliverables sichtbar** | – | ✅ 60% | +20% |
| **Gesamt Prozesse** | **40%** | **60%** | **+20%** |

### Interpretation

**Vorher (40%):**
- Nur LPH-Struktur vorhanden
- Keine Deliverables-Zuordnung sichtbar
- Unvollständiges Bild des Prozess-Management

**Nachher (60%):**
- LPH-Struktur + Deliverables parallel
- Phase-zu-Deliverable-Zuordnung nachvollziehbar
- Owner und Fälligkeitsdaten sichtbar
- Basis für Prozess-Verständnis gegeben

**Nächster Schritt (→80%):**
- Status-Übergänge sichtbar machen
- Abhängigkeiten zwischen Deliverables anzeigen
- Zeitliche Sequenzierung visualisieren

---

## 8. Nächster Mikro-Sprint

### Sprint-043.1: Kompetenzen-Reiter – Kompetenzkatalog sichtbar machen

**Ziel:** Kompetenzkatalog-Tabelle mit Code, Name, Kategorie, Status

**Scope:**
- 2–3 Frontend-Dateien (api.js, CompetenciesPage.tsx, ggf. App.jsx)
- 1 API-Endpunkt (getCompetencies)
- 1 Dokumentation (docs/19-kompetenzen-katalog.md)

**Reifegrad-Ziel:** Kompetenzen 40% → 60%

**Abhängigkeiten:** Keine (unabhängig von Sprint-042.2)

---

## 9. Qualitätsprüfung

### Code Quality

- ✅ **TypeScript**: Alle Types korrekt definiert
- ✅ **Props**: Korrekt typisiert und optional
- ✅ **Error Handling**: try-catch mit aussagekräftigen Meldungen
- ✅ **Null-Checks**: Fallback-Werte bei fehlenden Lookups
- ✅ **Performance**: Parallel-Laden optimiert, keine N+1-Queries

### UI Quality

- ✅ **Konsistenz**: Tabellen-Layout wie ResourcesPage und Phase-Tabelle
- ✅ **Responsive**: Flex-Layout, Overflow handling
- ✅ **Accessibility**: Column Header korrekt formatiert
- ✅ **User Feedback**: Error-Messages und Loading-States

### Testing

- ✅ **Backend Build**: Erfolgreich, 0 Fehler
- ✅ **Frontend Build**: Erfolgreich, 0 Fehler
- ✅ **Runtime**: Komponente wird korrekt gerendert (via App.jsx)
- ✅ **Data Loading**: Paralleles Laden funktioniert, Fallbacks greifen

---

## 10. Dokumentation

### Generigte Dateien

1. **docs/18-prozesse-deliverables.md** – Detaillierte technische Dokumentation
2. **Sprints/Sprint-042.2-Prozesse-Deliverables.md** – Diese Datei (Sprint-Summary)

### Verweise

- Vorheriger Sprint: [Sprint-042.1-Prozesse-LPH.md](Sprint-042.1-Prozesse-LPH.md)
- Analyse: [../docs/14-mvp-reifegradanalyse.md](../docs/14-mvp-reifegradanalyse.md)
- Roadmap: [Sprint-040-MVP-Reifegradanalyse.md](Sprint-040-MVP-Reifegradanalyse.md)

---

## 11. Fazit

Sprint-042.2 erfolgreich abgeschlossen. Der Prozesse-Reiter ist jetzt auf **60% Reifegrad** und bietet Benutzer vollständige Sichtbarkeit von Leistungsphasen und ihren zugeordneten Deliverables. Read-Only-Funktionalität stabil, kein Refactoring notwendig, keine neuen Abhängigkeiten hinzugefügt.

**Nächster Sprint:** 043.1 (Kompetenzkatalog)

---

**Versionierung:**
- Version: 1.0
- Status: Final
- Review: Done
- Deploy-Ready: Yes ✓

