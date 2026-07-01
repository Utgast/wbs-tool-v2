# Implementation Rules

## General
- No generic answers.
- No blind refactoring.
- One implementation slice at a time.
- Business logic stays in .NET API.
- Frontend must not become source of truth.
- Excel must not remain source of truth.

## EF Core / Database
- Use Guid Ids.
- Use EF Core relationships.
- Do not add SQLite-specific logic.
- Do not rely on raw SQL unless explicitly approved.
- Keep Azure SQL migration possible.
- Use migrations consciously.
- Project-related entities need ProjectId.
- WbsNode unique key: ProjectId + WbsCode.
- ResourceAssignment: multiple per WbsNode allowed.
- CapacityAllocation.ProjectId nullable for external/internal work.
- RoleAssignment.ProjectId nullable depending on ScopeType.

## Dashboard / WBS Rule
Dashboard and WBS tree totals must use WbsNode.PlannedHours and WbsNode.ActualHours.
Never calculate these totals from ResourceAssignments.

## Costs
MVP imports and displays planned/actual costs.
Do not automatically recalculate costs unless explicitly approved.

## Placeholders
Raw values like "alle" or generic "Ingenieur" must not automatically become real Persons.
Keep raw display fields or role placeholders until data cleanup is approved.

## Code Output Rule
If code is changed, output complete affected files.
Before code, state:
1. affected files
2. reason
3. risks
4. alternatives
5. recommendation

## Git Rules
Do not use git add .
Do not touch or commit:
- src/backend/WbsTool.Api/wbstool.db
- src/backend/WbsTool.Api/wbstool.db-shm
- src/backend/WbsTool.Api/wbstool.db-wal
- src/backend/WbsTool.Api/wbstool_backup_before_phase1.db
- src/backend/WbsTool.Api/Data/Seed/
- .vscode/
- src/backend/WbsTool.Api/check-duplicates.csx
- src/frontend/wbs-tool-ui/src/App.css.backup-before-clean

## Build Checks
After backend changes:
- dotnet build

After frontend changes:
- npm run build

After any change:
- git status --short
- regression checks from docs/regression-checks.md
