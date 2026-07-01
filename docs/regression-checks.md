# Regression Checks

## Fixed Project Reference
Project: Amprion PQ Freileitung
ProjectId: 7f3faaa5-1245-4d43-978b-88b5bab3a23b

## Dashboard Expected Values
- totalPlannedHours: 681
- totalActualHours: 674
- progressPercent: approx. 98.97
- blockedNodes: 0

Important:
These values must come from WbsNodes, not ResourceAssignments.

## Count Checks
- WBS nodes: 49
- ResourceAssignments: 81
- Persons: 16
- RateCategories: 5
- Status values: 6

## Control Node
WBS-ID: 2.1.2
Title: Identifikation Fremdleitungen / Schutzgebiete
Expected consolidated values:
- plannedHoursTotal: 16
- actualHoursTotal: 24

Expected visible resources:
- Ahmad
- Ibrahim
- Phine
- Victor

## Build Checks
Backend:
Run from src/backend/WbsTool.Api:
- dotnet build

Frontend:
Run from src/frontend/wbs-tool-ui:
- npm run build

## Git Check
Run from repository root:
- git status --short

Expected:
No DB, seed, vscode or backup files staged.
