# System-Übersicht

## Ziel des Systems

Das WBS Tool ist eine webbasierte Anwendung zur Verwaltung von Projektstrukturen (WBS), Ressourcenkapazitäten und Kompetenzen. Es beantwortet zentrale Fragen im Projektmanagement:

- **Was** muss getan werden? (WBS)
- **In welchem Kontext** ist eine Aufgabe einzuordnen? (ProcessPhases)
- **Welche Kompetenz** ist erforderlich? (Competencies, WbsRequiredCompetency)
- **Wer** hat diese Kompetenz? (Person, PersonCompetency)
- **Ist diese Person verfügbar**? (CapacityAllocation)
- **Falls nicht, welcher Bedarf besteht**? (ResourceDemand)

## Kernmodule

| Modul | Zweck | Scope |
|-------|-------|-------|
| **Projects** | Verwaltung von Projekten | Metadaten, Dashboard |
| **WBS** | Projektstruktur als Baum | Nodes, konsolidierte Metriken |
| **ResourceAssignments** | Zuweisung von Ressourcen zu WBS-Knoten | Personen, LPH-Stunden |
| **ResourceDemands** | Explizite Anforderungen an Ressourcen | Status, Bewertung |
| **Competencies** | Kompetenzkatalog und Zuordnungen | Katalog, Person-Kompetenz, WBS-Anforderungen |
| **ProcessPhases** | Phasenkatalog eines Projekts | Phasen, Mappings zu WBS |
| **CapacityAllocations** | Kapazitätsplanung pro Person | Verfügbarkeit, Auslastung |
| **Persons** | Ressourcen-Stammdaten | Aktive Personen, Platzhalter |
| **RateCategories** | Tarifkategorien | Kostenverwaltung |
| **TaskStatuses** | WBS-Knoten Status | Vordefinierte Status |

## Architekturübersicht

```
Frontend (React/Vite)
    ↓
API Layer (ASP.NET Core 9, OpenAPI)
    ↓
Business Logic (Service Pattern)
    ↓
Data Access Layer (EF Core 9)
    ↓
SQLite Database
```

### Architektur-Regeln

1. **API ist Single Source of Business Logic**
   - Alle Berechnungen und Validierungen in Services
   - Kein Business Logic im Frontend

2. **Dashboard verwendet ausschließlich WbsNode-Werte**
   - ResourceAssignments und CapacityAllocations werden für Dashboard-Summen nicht verwendet
   - Konsolidierte Werte stammen aus WbsNode: `plannedHoursTotal`, `actualHoursTotal`

3. **Keine Authentifizierung im MVP**
   - Basis-Implementierung ohne Rollensystem
   - Vorbereitung für zukünftige Governance

4. **Idempotente Seed-Operationen**
   - Mehrfaches Ausführen hat keine negativen Effekte
   - Entwicklungs- und Test-Support

## Aktueller MVP-Umfang

### Implementiert

- ✅ Project CRUD
- ✅ WBS Tree (Lesen + Schreiben + konsolidierte Metriken)
- ✅ ResourceAssignments (Zuweisung + LPH-Tracking)
- ✅ ResourceDemands (CRUD + Status-Tracking)
- ✅ Competencies (Katalog + Person-Zuordnungen + WBS-Anforderungen)
- ✅ ProcessPhases (Lesen + Projekt-Mapping)
- ✅ CapacityAllocations (Planung)
- ✅ Persons (Stammdaten)
- ✅ Dashboard (WBS-basiert)
- ✅ OpenAPI/Swagger
- ✅ Referenzdaten-Seed (Amprion PQ)

### Nicht implementiert (zukünftig)

- Authentifizierung
- Rollenbasierte Zugriffskontrolle
- Audit-Log
- Versionierung
- Import/Export (Excel, CSV)
- Matching-Engine
- Notifications
- Reporting Engine

## Nicht-Ziele (MVP)

- Single Sign-On (SSO)
- Integration mit externen Systemen
- Real-time Collaboration
- KI-basierte Ressourcenoptimierung
- Finanzmodul
- Multilingual Support (Phase 2)
