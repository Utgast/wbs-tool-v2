# API-Übersicht

Alle Endpunkte basieren auf: `http://localhost:5046`

OpenAPI/Swagger: `http://localhost:5046/openapi/v1.json`

---

## Health

### GET /api/health

**Beschreibung:** Status-Check des Services

**Response:**
```json
{
  "status": "ok",
  "service": "WbsTool.Api",
  "timestamp": "2026-07-06T..."
}
```

---

## Projects

### GET /api/projects

Alle Projekte abrufen.

**Response:** `Array<ProjectDto>`

### GET /api/projects/{projectId}

Ein Projekt abrufen.

**Parameter:** 
- `projectId` (Guid)

**Response:** `ProjectDto`

### GET /api/projects/{projectId}/dashboard

Dashboard mit konsolidierten WBS-Metriken.

**Parameter:** 
- `projectId` (Guid)

**Response:** `ProjectDashboardDto`
```json
{
  "projectId": "...",
  "totalPlannedHours": 681,
  "totalActualHours": 674,
  "progressPercent": 98.97,
  "blockedNodes": 0
}
```

### POST /api/projects

Neues Projekt erstellen.

**Request:**
```json
{
  "projectNumber": "PRJ-001",
  "name": "Projektname",
  "description": "Beschreibung",
  "plannedStart": "2026-09-01",
  "plannedEnd": "2027-01-31"
}
```

**Response:** `ProjectDto`

---

## WBS (Work Breakdown Structure)

### GET /api/projects/{projectId}/wbs

Alle WBS-Knoten eines Projekts.

**Response:** `Array<WbsNodeDto>`

### GET /api/projects/{projectId}/wbs/tree

WBS als Baumstruktur (hierarchisch).

**Response:** `Array<WbsTreeNodeDto>`

### POST /api/projects/{projectId}/wbs

Neuen WBS-Knoten erstellen.

**Request:**
```json
{
  "parentNodeId": "...",
  "wbsId": "1.1",
  "title": "Knoten-Titel",
  "plannedHours": 40,
  "actualHours": 35
}
```

**Response:** `WbsNodeDto`

---

## Resource Assignments

### GET /api/projects/{projectId}/wbs/{wbsNodeId}/assignments

Alle Ressourcen-Zuweisungen für einen WBS-Knoten.

**Response:** `Array<ResourceAssignmentDto>`

### POST /api/projects/{projectId}/wbs/{wbsNodeId}/assignments

Neue Ressourcen-Zuweisung erstellen.

**Request:**
```json
{
  "personId": "...",
  "rateCategoryId": "...",
  "plannedHours": 40,
  "actualHours": 35
}
```

**Response:** `ResourceAssignmentDto`

---

## Resource Demands

### GET /api/resourcedemands

Alle ResourceDemands.

**Response:** `Array<ResourceDemandDto>`

### GET /api/resourcedemands/{id}

Ein ResourceDemand abrufen.

**Response:** `ResourceDemandDto`

### POST /api/resourcedemands

Neue ResourceDemand erstellen.

**Request:**
```json
{
  "projectId": "...",
  "wbsNodeId": "...",
  "requiredCompetencyId": "...",
  "title": "Demand-Titel",
  "plannedHours": 80,
  "startDate": "2026-09-01",
  "endDate": "2026-10-31",
  "createdBy": "username"
}
```

**Response:** `ResourceDemandDto`

### PUT /api/resourcedemands/{id}

ResourceDemand aktualisieren (inkl. Status).

**Request:**
```json
{
  "title": "...",
  "status": "Approved",
  "updatedBy": "username"
}
```

**Response:** `ResourceDemandDto`

---

## Competencies

### GET /api/competencies

Alle aktiven Competencies.

**Response:** `Array<CompetencyDto>`

### GET /api/persons/{personId}/competencies

Competencies einer Person.

**Response:** `Array<PersonCompetencyDto>`

### POST /api/persons/{personId}/competencies

Neue Competency für Person zuweisen.

**Request:**
```json
{
  "competencyId": "...",
  "proficiencyLevel": 3,
  "comment": "Senior-Level"
}
```

**Response:** `PersonCompetencyDto`

### GET /api/wbs/{wbsNodeId}/requiredcompetencies

Erforderliche Competencies für einen WBS-Knoten.

**Response:** `Array<WbsRequiredCompetencyDto>`

### POST /api/wbs/{wbsNodeId}/requiredcompetencies

Neue erforderliche Competency für WBS-Knoten festlegen.

**Request:**
```json
{
  "projectId": "...",
  "competencyId": "...",
  "requiredLevel": 2
}
```

**Response:** `WbsRequiredCompetencyDto`

---

## ProcessPhases

### GET /api/ProcessPhases

Alle aktiven ProcessPhases.

**Response:** `Array<ProcessPhaseDto>`

### GET /api/Projects/{projectId}/ProcessPhases

ProcessPhases eines Projekts (via WbsPhaseMappings).

**Response:** `Array<ProcessPhaseDto>`

---

## Persons

### GET /api/persons

Alle aktiven Personen.

**Response:** `Array<PersonDto>`

---

## RateCategories

### GET /api/ratecategories

Alle Tarifkategorien.

**Response:** `Array<RateCategoryDto>`

---

## TaskStatuses

### GET /api/taskstatuses

Alle WBS-Status-Definitionen.

**Response:** `Array<TaskStatusDto>`

---

## Development Seed

### POST /api/dev-seed/amprion-pq

Seed das Amprion PQ Referenzprojekt. *(Development nur)*

**Response:**
```json
{
  "projectsCreated": 1,
  "wbsNodesCreated": 49,
  "resourceAssignmentsCreated": 81,
  "...": "..."
}
```
