import { useEffect, useMemo, useState } from 'react'

const ASSIGNMENT_ROLES = [
  { value: 'Responsible', label: 'Verantwortlich' },
  { value: 'Consulted', label: 'Beratung' },
  { value: 'Contributor', label: 'Bearbeitung' },
]

function formatHours(value) {
  if (value === null || value === undefined || value === '') {
    return '–'
  }

  const numericValue = Number(value)

  if (Number.isNaN(numericValue)) {
    return '–'
  }

  return `${numericValue.toLocaleString('de-DE', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  })} h`
}

function formatCurrency(value, currency = 'EUR') {
  if (value === null || value === undefined || value === '') {
    return '–'
  }

  const numericValue = Number(value)

  if (Number.isNaN(numericValue)) {
    return '–'
  }

  try {
    return new Intl.NumberFormat('de-DE', {
      style: 'currency',
      currency,
    }).format(numericValue)
  } catch {
    return `${numericValue.toLocaleString('de-DE')} ${currency}`
  }
}

function getRoleLabel(role) {
  const match = ASSIGNMENT_ROLES.find((item) => item.value === role)
  return match?.label || role || '–'
}

function getPersonLabel(assignment, persons) {
  if (assignment?.personDisplayName) {
    return assignment.personDisplayName
  }

  if (assignment?.personName) {
    return assignment.personName
  }

  if (assignment?.displayName) {
    return assignment.displayName
  }

  const personId = assignment?.personId

  if (personId) {
    const person = persons.find((item) => String(item.id) === String(personId))

    if (person) {
      return person.displayName
    }
  }

  return 'Nicht zugeordnet'
}

function getRateCategoryLabel(rateCategories, rateCategoryId) {
  if (!rateCategoryId) {
    return '–'
  }

  const category = rateCategories.find(
    (item) => String(item.id) === String(rateCategoryId)
  )

  if (!category) {
    return '–'
  }

  return `${category.code} – ${category.name}`
}

function getRateCategoryRate(rateCategories, rateCategoryId) {
  if (!rateCategoryId) {
    return null
  }

  return (
    rateCategories.find((item) => String(item.id) === String(rateCategoryId)) ||
    null
  )
}

function createEmptyAssignmentDraft() {
  return {
    personId: '',
    assignmentRole: '',
    plannedRateCategoryId: '',
    actualRateCategoryId: '',
    plannedHours: '',
    actualHours: '',
    comment: '',
  }
}

export default function WbsResourceAssignmentsPanel({
  selectedNode,
  resourceAssignments,
  resourceAssignmentsLoading,
  resourceAssignmentsError,
  masterDataLoading,
  masterDataError,
  persons,
  rateCategories,
  isCreatingAssignment,
  assignmentActionError,
  onCreateAssignment,
}) {
  const [createMode, setCreateMode] = useState(false)
  const [draft, setDraft] = useState(createEmptyAssignmentDraft())

  const safeAssignments = Array.isArray(resourceAssignments)
    ? resourceAssignments
    : []

  const safePersons = Array.isArray(persons) ? persons : []
  const safeRateCategories = Array.isArray(rateCategories)
    ? rateCategories
    : []

  useEffect(() => {
    setCreateMode(false)
    setDraft(createEmptyAssignmentDraft())
  }, [selectedNode?.id])

  const plannedRateCategory = useMemo(() => {
    return getRateCategoryRate(safeRateCategories, draft.plannedRateCategoryId)
  }, [safeRateCategories, draft.plannedRateCategoryId])

  const actualRateCategory = useMemo(() => {
    return getRateCategoryRate(safeRateCategories, draft.actualRateCategoryId)
  }, [safeRateCategories, draft.actualRateCategoryId])

  const isCreateDisabled =
    isCreatingAssignment ||
    masterDataLoading ||
    !selectedNode ||
    !draft.personId ||
    !draft.assignmentRole

  function handleDraftChange(event) {
    const { name, value } = event.target

    setDraft((prev) => ({
      ...prev,
      [name]: value,
    }))
  }

  async function handleCreateSubmit(event) {
    event.preventDefault()

    if (isCreateDisabled) {
      return
    }

    const payload = {
      personId: draft.personId,
      assignmentRole: draft.assignmentRole,
      plannedRateCategoryId: draft.plannedRateCategoryId || null,
      actualRateCategoryId: draft.actualRateCategoryId || null,
      plannedHours:
        draft.plannedHours === '' || draft.plannedHours === null
          ? null
          : Number(draft.plannedHours),
      actualHours:
        draft.actualHours === '' || draft.actualHours === null
          ? null
          : Number(draft.actualHours),
      comment: draft.comment?.trim() ?? '',
    }

    await onCreateAssignment(payload)

    setDraft(createEmptyAssignmentDraft())
    setCreateMode(false)
  }

  function handleCancelCreate() {
    setDraft(createEmptyAssignmentDraft())
    setCreateMode(false)
  }

  if (!selectedNode) {
    return (
      <section className="resource-panel">
        <div className="resource-panel-header">
          <div>
            <h3>Ressourcen</h3>
            <p>Bitte zuerst einen WBS-Knoten auswählen.</p>
          </div>
        </div>
      </section>
    )
  }

  return (
    <section className="resource-panel">
      <div className="resource-panel-header">
        <div>
          <h3>Ressourcen</h3>
          <p>Direkte Personen-, Rollen- und Stunden-Zuordnungen für diesen WBS-Knoten.</p>
        </div>

        {!createMode && (
          <button
            type="button"
            className="secondary-button"
            onClick={() => setCreateMode(true)}
            disabled={masterDataLoading}
          >
            + Zuordnung
          </button>
        )}
      </div>

      {masterDataLoading && (
        <div className="resource-info">Stammdaten werden geladen …</div>
      )}

      {masterDataError && (
        <div className="resource-error">
          Stammdatenfehler: {masterDataError}
        </div>
      )}

      <div className="resource-masterdata-summary">
        <div className="resource-masterdata-card">
          <span className="resource-masterdata-label">Personen</span>
          <strong>{safePersons.length}</strong>
        </div>

        <div className="resource-masterdata-card">
          <span className="resource-masterdata-label">Kategorien</span>
          <strong>{safeRateCategories.length}</strong>
        </div>
      </div>

      {assignmentActionError && (
        <div className="resource-error">{assignmentActionError}</div>
      )}

      {createMode && (
        <form className="resource-create-form" onSubmit={handleCreateSubmit}>
          <div className="form-row">
            <label htmlFor="assignment-personId">Person *</label>
            <select
              id="assignment-personId"
              name="personId"
              value={draft.personId}
              onChange={handleDraftChange}
            >
              <option value="">Bitte auswählen</option>

              {safePersons.map((person) => (
                <option key={person.id} value={person.id}>
                  {person.displayName}
                  {person.isPlaceholder ? ' (Platzhalter)' : ''}
                </option>
              ))}
            </select>
          </div>

          <div className="form-row">
            <label htmlFor="assignment-assignmentRole">Rolle *</label>
            <select
              id="assignment-assignmentRole"
              name="assignmentRole"
              value={draft.assignmentRole}
              onChange={handleDraftChange}
            >
              <option value="">Bitte auswählen</option>

              {ASSIGNMENT_ROLES.map((role) => (
                <option key={role.value} value={role.value}>
                  {role.label}
                </option>
              ))}
            </select>
          </div>

          <div className="form-row">
            <label htmlFor="assignment-plannedRateCategoryId">
              Plan-Stundensatzkategorie
            </label>
            <select
              id="assignment-plannedRateCategoryId"
              name="plannedRateCategoryId"
              value={draft.plannedRateCategoryId}
              onChange={handleDraftChange}
            >
              <option value="">Keine Kategorie</option>

              {safeRateCategories.map((rateCategory) => (
                <option key={rateCategory.id} value={rateCategory.id}>
                  {rateCategory.code} – {rateCategory.name}
                </option>
              ))}
            </select>
          </div>

          <div className="form-row">
            <label htmlFor="assignment-actualRateCategoryId">
              Ist-Stundensatzkategorie
            </label>
            <select
              id="assignment-actualRateCategoryId"
              name="actualRateCategoryId"
              value={draft.actualRateCategoryId}
              onChange={handleDraftChange}
            >
              <option value="">Keine Kategorie</option>

              {safeRateCategories.map((rateCategory) => (
                <option key={rateCategory.id} value={rateCategory.id}>
                  {rateCategory.code} – {rateCategory.name}
                </option>
              ))}
            </select>
          </div>

          <div className="form-row">
            <label htmlFor="assignment-plannedHours">Geplante Stunden</label>
            <input
              id="assignment-plannedHours"
              name="plannedHours"
              type="number"
              min="0"
              step="0.25"
              value={draft.plannedHours}
              onChange={handleDraftChange}
            />
          </div>

          <div className="form-row">
            <label htmlFor="assignment-actualHours">Ist-Stunden</label>
            <input
              id="assignment-actualHours"
              name="actualHours"
              type="number"
              min="0"
              step="0.25"
              value={draft.actualHours}
              onChange={handleDraftChange}
            />
          </div>

          <div className="form-row">
            <label htmlFor="assignment-comment">Kommentar</label>
            <textarea
              id="assignment-comment"
              name="comment"
              rows="3"
              value={draft.comment}
              onChange={handleDraftChange}
            />
          </div>

          {(plannedRateCategory || actualRateCategory) && (
            <div className="resource-rate-preview">
              {plannedRateCategory && (
                <div>
                  <span className="resource-assignment-label">Plan-Satz</span>
                  <strong>
                    {formatCurrency(
                      plannedRateCategory.hourlyRate,
                      plannedRateCategory.currency
                    )}
                  </strong>
                </div>
              )}

              {actualRateCategory && (
                <div>
                  <span className="resource-assignment-label">Ist-Satz</span>
                  <strong>
                    {formatCurrency(
                      actualRateCategory.hourlyRate,
                      actualRateCategory.currency
                    )}
                  </strong>
                </div>
              )}
            </div>
          )}

          <div className="form-actions">
            <button type="submit" disabled={isCreateDisabled}>
              {isCreatingAssignment
                ? 'Speichern...'
                : 'Zuordnung speichern'}
            </button>

            <button
              type="button"
              className="secondary-button"
              onClick={handleCancelCreate}
            >
              Abbrechen
            </button>
          </div>
        </form>
      )}

      {resourceAssignmentsLoading && (
        <div className="resource-info">
          Ressourcen-Zuordnungen werden geladen …
        </div>
      )}

      {resourceAssignmentsError && (
        <div className="resource-error">{resourceAssignmentsError}</div>
      )}

      {!resourceAssignmentsLoading &&
        !resourceAssignmentsError &&
        safeAssignments.length === 0 && (
          <div className="resource-empty-state">
            <strong>Noch keine Ressourcen zugeordnet.</strong>
            <span>
              Lege eine Zuordnung an, um Person, Rolle sowie Plan- und
              Ist-Stunden zu pflegen.
            </span>
          </div>
        )}

      {!resourceAssignmentsLoading &&
        !resourceAssignmentsError &&
        safeAssignments.length > 0 && (
          <div className="resource-list">
            {safeAssignments.map((assignment, index) => {
              const assignmentId =
                assignment.id || assignment.assignmentId || `assignment-${index}`

              const personLabel = getPersonLabel(assignment, safePersons)
              const roleLabel = getRoleLabel(assignment.assignmentRole)

              return (
                <article key={assignmentId} className="resource-card">
                  <div className="resource-card-main">
                    <div className="resource-person-summary">
                      <strong className="resource-person-name">
                        {personLabel}
                      </strong>
                      <span className="resource-person-role">
                        {roleLabel}
                      </span>
                    </div>

                    <span className="tree-node-type">
                      {assignment.isActive === false ? 'Inaktiv' : 'Aktiv'}
                    </span>
                  </div>

                  <div className="resource-card-grid">
                    <div>
                      <span className="resource-label">Planstunden</span>
                      <strong>{formatHours(assignment.plannedHours)}</strong>
                    </div>

                    <div>
                      <span className="resource-label">Ist-Stunden</span>
                      <strong>{formatHours(assignment.actualHours)}</strong>
                    </div>

                    <div>
                      <span className="resource-label">Plan-Kat.</span>
                      <strong>
                        {assignment.plannedRateCategoryCode ||
                          getRateCategoryLabel(
                            safeRateCategories,
                            assignment.plannedRateCategoryId
                          )}
                      </strong>
                    </div>

                    <div>
                      <span className="resource-label">Ist-Kat.</span>
                      <strong>
                        {assignment.actualRateCategoryCode ||
                          getRateCategoryLabel(
                            safeRateCategories,
                            assignment.actualRateCategoryId
                          )}
                      </strong>
                    </div>
                  </div>

                  {assignment.comment && (
                    <p className="resource-comment">{assignment.comment}</p>
                  )}
                </article>
              )
            })}
          </div>
        )}
    </section>
  )   
}