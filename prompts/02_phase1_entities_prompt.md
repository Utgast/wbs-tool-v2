# Prompt: Phase 1A EF Core Entities Only

```text
Read docs/*.md listed in docs/copilot-project-context.md.
Implement only EF Core model additions for MVP entities/enums.
Do not modify services/controllers/frontend.
Do not create migration yet.
Do not touch seed or DB files.

Target additions:
- AppRole enum
- ScopeType enum
- WbsNodeStatus enum if not existing
- ResourceDemandStatus enum
- ImportIssueStatus enum
- ProcessPhase entity
- WbsPhaseMapping entity
- Competency entity
- PersonCompetency entity
- WbsRequiredCompetency entity
- CapacityAllocation entity
- ResourceDemand entity
- RoleAssignment entity

Keep Azure SQL compatible.
Use Guid Ids.
Use ProjectId where required.
Before code, inspect existing models and AppDbContext and list exact files to change.
```
