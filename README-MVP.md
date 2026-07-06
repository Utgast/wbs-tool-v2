# WBS Tool MVP – Developer Guide

Base URL: `http://localhost:5046`

---

## Build

**Backend**

```bash
cd src/backend/WbsTool.Api
dotnet build
```

**Frontend**

```bash
cd src/frontend/wbs-tool-ui
npm install
npm run build
```

---

## Start

**Option A – Batch (Windows)**

```bat
start-dev.bat
```

Öffnet zwei CMD-Fenster: Backend auf `http://localhost:5046`, Frontend auf `http://localhost:5173`.

**Option B – VS Code Tasks**

`Terminal → Run Task → Start Full Stack`

Startet Backend (`dotnet run`) und Frontend (`npm run dev`) parallel.

**Option C – manuell**

```bash
# Backend
cd src/backend/WbsTool.Api
dotnet run

# Frontend (separates Terminal)
cd src/frontend/wbs-tool-ui
npm run dev
```

---

## Seed

Seeded das Amprion PQ Referenzprojekt. Nur in Development verfügbar.

```http
POST http://localhost:5046/api/dev-seed/amprion-pq
```

Beispiel mit curl:

```bash
curl -X POST http://localhost:5046/api/dev-seed/amprion-pq
```

Erwartete Antwort: Seed-Ergebnis mit Anzahl erstellter Entitäten.

> Kann mehrfach aufgerufen werden – der Service ist idempotent.

---

## OpenAPI

Nur in Development verfügbar.

```
http://localhost:5046/openapi/v1.json
```

Zeigt alle registrierten Endpunkte. Kann in Tools wie Postman, Insomnia oder Bruno als Collection importiert werden.

---

## Dashboard Regression Test

Referenzprojekt: **Amprion PQ Freileitung**
ProjectId: `7f3faaa5-1245-4d43-978b-88b5bab3a23b`

```http
GET http://localhost:5046/api/projects/7f3faaa5-1245-4d43-978b-88b5bab3a23b/dashboard
```

Erwartete Werte:

| Feld              | Erwarteter Wert |
|-------------------|-----------------|
| totalPlannedHours | 681             |
| totalActualHours  | 674             |
| progressPercent   | ≈ 98.97         |
| blockedNodes      | 0               |

> Werte stammen ausschließlich aus WbsNodes. ResourceAssignments und CapacityAllocations werden für Dashboard-Summen nicht verwendet.

---

## WBS Test

### Alle WBS-Knoten eines Projekts abrufen

```http
GET http://localhost:5046/api/projects/7f3faaa5-1245-4d43-978b-88b5bab3a23b/wbs
```

Erwartete Anzahl Knoten: **49**

### WBS als Baum

```http
GET http://localhost:5046/api/projects/7f3faaa5-1245-4d43-978b-88b5bab3a23b/wbs/tree
```

### Kontrollknoten: WBS-ID 2.1.2

Titel: *Identifikation Fremdleitungen / Schutzgebiete*

Erwartete konsolidierte Werte:
- `plannedHoursTotal`: 16
- `actualHoursTotal`: 24

Erwartete sichtbare Ressourcen: Ahmad, Ibrahim, Phine, Victor

---

## ResourceDemand Test

### Alle ResourceDemands abrufen

```http
GET http://localhost:5046/api/resourcedemands
```

### Neue ResourceDemand erstellen

```http
POST http://localhost:5046/api/resourcedemands
Content-Type: application/json

{
  "projectId": "7f3faaa5-1245-4d43-978b-88b5bab3a23b",
  "title": "Test Demand",
  "createdBy": "test-user"
}
```

---

## Competencies Test

### Alle Competencies abrufen

```http
GET http://localhost:5046/api/competencies
```

### Person Competencies abrufen

```http
GET http://localhost:5046/api/persons/{personId}/competencies
```

### WBS Required Competencies abrufen

```http
GET http://localhost:5046/api/wbs/{wbsNodeId}/requiredcompetencies
```

---

## ProcessPhases Test

### Alle ProcessPhases abrufen

```http
GET http://localhost:5046/api/ProcessPhases
```

### Project ProcessPhases abrufen

```http
GET http://localhost:5046/api/Projects/7f3faaa5-1245-4d43-978b-88b5bab3a23b/ProcessPhases
```

---

## Health Check

```http
GET http://localhost:5046/api/health
```

Erwartete Antwort:

```json
{
  "status": "ok",
  "service": "WbsTool.Api",
  "timestamp": "..."
}
```
