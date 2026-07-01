# Prompt: Phase 1B DbContext Configuration Only

```text
Read docs/*.md.
Configure AppDbContext only for the already created MVP entities.
No services/controllers/frontend.
No migration yet.

Required:
- DbSet entries
- relationships
- delete behavior
- indexes
- unique ProjectId + WbsCode for WbsNode
- indexes for ProjectId, WbsNodeId, PersonId, CompetencyId, Status
- enforce RoleAssignment Project scope rules as far as EF model supports it

Keep Azure SQL compatible.
Do not change dashboard logic.
Output complete AppDbContext.cs.
```
