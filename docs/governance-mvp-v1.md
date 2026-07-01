# Governance MVP v1.0

## Principle
The system is role-based.
No direct individual permissions.
People receive roles in clear scopes.

GitHub Copilot must not introduce single-user permission exceptions.

## MVP Roles
- Admin
- Owner
- ProjectManager
- ResourceManager
- Contributor
- Reviewer
- Reader

## MVP Scopes
- Global
- Project

No MVP scopes for:
- WbsNode
- Competency
- ProcessPhase
- Capacity domain
- ImportIssue

## Role Meaning

### Admin
Global technical administration.
Can manage technical data and role assignments.
Admin is not automatically a fachlicher Entscheider.

### Owner
Global fachliche Governance.
Owns standards, governance, role model and critical fachliche decisions.
Owner is not automatically operative project user.

### ProjectManager
Project-scoped operative project owner.
Can maintain WBS, status, LPH mappings, required competencies and create resource demands.
Cannot assign roles in MVP.
Cannot approve own resource demand.

### ResourceManager
Usually project-scoped.
Can review, approve, reject, cover and close resource demands.
Can evaluate capacity conflicts.

### Contributor
Project-scoped operative worker.
Can update own / assigned operational data, actual hours and comments.
Cannot change WBS structure or approve demands.

### Reviewer
Project-scoped reviewer / quality role.
Can review, comment and evaluate ImportIssues.

### Reader
Read-only access globally or per project.

## Role Assignment Rules
Only Admin and Owner can assign roles in MVP.
ProjectManager can request role assignments but cannot grant them.

RoleAssignment fields:
- Id
- PersonId
- Role
- ScopeType
- ProjectId nullable
- AssignedBy
- AssignedAt
- ValidFrom nullable
- ValidUntil nullable
- Comment nullable
- IsActive
- RevokedBy nullable
- RevokedAt nullable

Rules:
- ScopeType = Project requires ProjectId.
- ScopeType = Global requires ProjectId empty.
- RoleAssignment must be auditable.

## Delegation MVP
No standalone delegation engine.
Only time-limited RoleAssignment is allowed.
No chain delegation.
No self-delegation.
No delegation on individual WBS nodes.
Permanent exceptions without end date are not allowed.

## Status Rules

### WbsNodeStatus
- Draft
- InProgress
- InReview
- Delivered
- Closed
- Archived

Rules:
- Draft: ProjectManager can edit.
- InProgress: ProjectManager can edit; Contributor can update operational fields.
- InReview: Reviewer can review; free editing restricted.
- Delivered: only ProjectManager/Owner can reopen/change with reason.
- Closed: no operational changes.
- Archived: read-only.

### ResourceDemandStatus
- Draft
- Submitted
- InReview
- Approved
- Rejected
- Covered
- Closed
- Cancelled

Rules:
- Draft: ProjectManager can edit.
- Submitted/InReview: ResourceManager reviews.
- Approved/Rejected: decision by ResourceManager.
- Covered: demand covered by person/role/external support.
- Closed: no operational changes.

### ImportIssueStatus
- Open
- InReview
- Resolved
- Ignored

Rules:
- Reviewer handles data quality review in MVP.
- Ignored requires reason.

## Audit Minimum
MVP must support tracing of:
- role assignment
- role revocation
- ResourceDemand status change
- WbsNode status change
- ImportIssue resolve/ignore

Minimum fields where relevant:
- CreatedAt
- CreatedBy
- UpdatedAt
- UpdatedBy
- StatusChangedAt
- StatusChangedBy
- AssignedBy
- AssignedAt
- RevokedBy
- RevokedAt

Full event sourcing is not MVP.
