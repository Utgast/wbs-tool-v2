# MVP Scope v1.0

## MVP Goal
The MVP must answer:
- What needs to be done?
- Which LPH / process context applies?
- Which competency is required?
- Who has the competency?
- Is that person available across projects?
- If not, which resource demand must be raised?

## MUST
Entities / domains:
- Project
- WbsNode
- ResourceAssignment
- ImportIssue
- ProcessPhase
- WbsPhaseMapping
- Person
- Competency
- PersonCompetency
- WbsRequiredCompetency
- CapacityAllocation
- ResourceDemand
- RoleAssignment

Functional:
- Dashboard from WbsNodes only
- WBS tree
- WBS detail panel
- Resource assignments panel
- ImportIssues view
- simple LPH assignment
- simple competency assignment
- simple capacity display
- manual ResourceDemand creation
- role-based governance foundation

## SHOULD
- competency gap display
- capacity warning on overload
- ResourceDemand status transitions
- filters by LPH / status / competency / demand
- server-side role checks

## COULD LATER
- productive SPFx UI
- productive Azure SQL
- Entra ID / OIDC
- Power BI
- full audit trail
- document management logic
- automatic skill matching
- automatic capacity optimization
- multi-stage approval workflows

## Explicit Non-MVP
- no object-level rights on individual WBS nodes
- no complex delegation engine
- no generic permission matrix UI
- no full process library
- no raw SQL optimization
- no recalculation of imported costs unless approved
- no HR/holiday/absence planning

## MVP Capacity Rule
CapacityAllocation contains:
- PersonId
- ProjectId nullable
- WbsNodeId nullable
- StartDate
- EndDate
- PlannedHours nullable
- ActualHours nullable
- AllocationPercent nullable
- Source
- Comment

If PlannedHours exists, hours are the leading value.
AllocationPercent is display/warning support in MVP.
No automatic daily or weekly distribution in MVP.

Person may get optional WeeklyCapacityHours for later capacity checks.

## MVP ResourceDemand Rule
ProjectManager can create/submit demand.
ResourceManager reviews/approves/rejects/covers/closes demand.
Owner handles escalations.

ResourceDemand exists when:
- required competency is missing, or
- competency holders are overloaded, or
- PM/lead identifies additional resource need.
