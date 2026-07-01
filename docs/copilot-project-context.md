# Copilot Project Context — WBS Tool

## Purpose
We are developing a WBS / project control / resource management system that replaces Excel as the leading working basis step by step.

The system combines:
- WBS / project structure
- process phases / Leistungsphasen (LPH)
- resources and role assignments
- competencies
- cross-project capacity
- resource demand workflow
- import/data-quality issues

## Target Architecture
- Backend: .NET 9 Web API
- Domain/business logic: API only
- ORM: EF Core
- Local DB now: SQLite
- Target DB later: Azure SQL
- Current frontend: React + Vite
- Later UI: SharePoint/SPFx React Webpart
- SharePoint is only UI / entry point, NOT the core database

## Non-Negotiable Rule
Dashboard and WBS tree values MUST use consolidated values from WbsNode:
- PlannedHours
- ActualHours

ResourceAssignments are detail data and MUST NOT be used for dashboard or tree totals.

Reason:
The original Excel separates:
1. WBS_Knoten = consolidated values per WBS ID
2. Ressourcen_Zuordnung = multiple person/role rows per WBS ID

If ResourceAssignments are summed for dashboard/tree, double counting occurs.

## Current Regression Reference
Project: Amprion PQ Freileitung
ProjectId: 7f3faaa5-1245-4d43-978b-88b5bab3a23b

Expected dashboard:
- PlannedHours total: 681
- ActualHours total: 674
- Progress: approx. 98.97 %
- Blocker: 0

Expected counts:
- WBS nodes: 49
- ResourceAssignments: 81
- Persons: 16
- RateCategories: 5
- Status values: 6

Control node:
- WBS-ID: 2.1.2
- Title: Identifikation Fremdleitungen / Schutzgebiete
- plannedHoursTotal: 16
- actualHoursTotal: 24
- visible resources: Ahmad, Ibrahim, Phine, Victor

## Development Mode
Before technical implementation:
1. Business analysis
2. Risks
3. Alternatives
4. Data model impact
5. UX impact
6. Governance impact
7. Azure SQL / SharePoint-SPFx impact
8. Handover impact for Arcadis Digital Engineering
9. Recommendation
10. Only then implementation

GitHub Copilot is the implementer, not the architect.
