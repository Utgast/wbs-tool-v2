# Data Source Mapping

## Repository
C:\Projekte\wbs-tool

## Excel Source 1 — WBS
Path:
C:\Users\reinhard2074\OneDrive - ARCADIS\Desktop\Tabellen automatisieren\WBS_AmprionPQ-bearbeitet.xlsx

### Sheet: WBS_Knoten
Leading source for consolidated WBS nodes.

Mapping:
- WBS_ID -> WbsNode.WbsCode
- Parent_WBS_ID -> WbsNode.ParentWbsCode
- WBS_Ebene -> WbsNode.Level
- Typ -> WbsNode.NodeType
- Titel -> WbsNode.Title
- Status -> WbsNode.Status / TaskStatus
- Geplanter_Start -> WbsNode.PlannedStart
- Geplantes_Ende -> WbsNode.PlannedEnd
- Geplante_Tage -> WbsNode.PlannedDays
- Geplante_Stunden -> WbsNode.PlannedHours
- Ist_Start -> WbsNode.ActualStart
- Ist_Ende -> WbsNode.ActualEnd
- Ist_Tage -> WbsNode.ActualDays
- Ist_Stunden -> WbsNode.ActualHours
- Kommentar -> WbsNode.Comment

Rule:
WbsNode is the source for consolidated dashboard and tree values.

### Sheet: Ressourcen_Zuordnung
Detail source for resources per WBS node. Do NOT use for dashboard totals.

Mapping:
- WBS_ID -> ResourceAssignment.WbsCode / WbsNode relation
- Person -> ResourceAssignment.PersonNameRaw / later PersonId
- Rolle -> ResourceAssignment.RoleName
- Verantwortlich -> ResourceAssignment.IsResponsible
- Beratung_durch -> ResourceAssignment.IsConsulted
- Bearbeitung_durch -> ResourceAssignment.IsExecuting
- Stundensatz_Kategorie -> ResourceAssignment.RateCategoryCode
- Geplante_Stunden -> ResourceAssignment.PlannedHours
- Ist_Stundensatz_Kategorie -> ResourceAssignment.ActualRateCategoryCode
- Ist_Stunden -> ResourceAssignment.ActualHours
- Geplante_Kosten -> ResourceAssignment.PlannedCost
- Ist_Kosten -> ResourceAssignment.ActualCost
- Kommentar -> ResourceAssignment.Comment

Rules:
- Multiple ResourceAssignments per WBS node are allowed.
- ResourceAssignment is detail data only.
- Values like "alle" or generic "Ingenieur" should not automatically become real Persons. Treat as raw placeholder / role name until cleaned.

### Sheet: Ressourcen
Master data for persons/roles/rates.

Mapping:
- Name -> Person.DisplayName
- Rolle -> Person.RoleTitle
- Stundensatz-Kat. -> Person.DefaultRateCategoryCode
- Stundensatz (€) -> optional reference / RateCategory.HourlyRate

### Sheet: Stundensätze
Master data for hourly rate categories.

Mapping:
- Kategorie -> RateCategory.Code
- Stundensatz (€) -> RateCategory.HourlyRate

MVP rule:
Costs are imported and displayed. Do not automatically recalculate costs until rate logic is explicitly approved.

### Sheet: Status
Master data for task status values.

Mapping:
- value -> TaskStatus.Name

Open decision:
Define which statuses mean blocked, completed, in progress.

### Sheet: Import_Pruefung
Source for data quality issues.

Mapping:
- Zeilennummer -> ImportIssue.SourceRowReference
- WBS_ID -> ImportIssue.WbsCode / later WbsNodeId
- Problemtyp -> ImportIssue.IssueType
- Beschreibung -> ImportIssue.Description
- Kritikalität -> ImportIssue.Severity
- Empfehlung -> ImportIssue.Recommendation

ImportIssueStatus:
- Open
- InReview
- Resolved
- Ignored

## Excel Source 2 — Ablaufplan / Leistungsphasen
Path:
C:\Users\reinhard2074\OneDrive - ARCADIS\Desktop\Tabellen automatisieren\Ablaufplan, Freileitung PQ Amprion.xlsx

Purpose:
Source for process phase / Leistungsphase logic.

Mapping for MVP:
- Phase -> ProcessPhase.Code
- Ziele -> ProcessPhase.Goal / Description
- Dokumente -> later Deliverable / ProcessPhase.DocumentationHint
- Verantwortlichkeit -> ProcessPhase.DefaultResponsibility
- Task -> later ProcessTask.Title
- arcadis / Fachbereich -> later ProcessTask.Discipline
- Verantwortliche Person -> later ProcessTask.DefaultResponsiblePersonName
- Tools / Potenzial / Spezifika -> later metadata/comment

MVP rule:
Do not build full process library yet. Only ProcessPhase and WbsPhaseMapping.

## Excel Source 3 — Auslastung OHL
Path:
C:\Users\reinhard2074\OneDrive - ARCADIS\Desktop\Tabellen automatisieren\Auslastung_OHL.xlsx

Purpose:
Source for persons, competencies, cross-project capacity and workload logic.

### Sheet: Auslastung
Mapping:
- Vorname + Name -> Person.DisplayName
- AG -> CapacityAllocation.ProjectName or ExternalProjectName
- Aufgabe -> CapacityAllocation.TaskDescription
- Auslastung -> CapacityAllocation.AllocationPercent
- Fortschritt -> CapacityAllocation.ProgressPercent
- Start -> CapacityAllocation.StartDate
- Ende -> CapacityAllocation.EndDate
- Notizen -> CapacityAllocation.Comment

### Sheet: Team
Mapping:
- Mitarbeiter/Name -> Person.DisplayName
- Standortzugehörigkeit -> Person.HomeOfficeLocation
- Kompetenzen -> Competency + PersonCompetency
- Notiz -> PersonCompetency.Comment or Person.Comment
