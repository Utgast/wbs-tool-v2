import { useEffect, useRef } from 'react'
import WbsResourceAssignmentsPanel from './WbsResourceAssignmentsPanel'

function getPlannedHoursValue(draftNode) {
  if (!draftNode) {
    return ''
  }

  if (draftNode.plannedHours !== null && draftNode.plannedHours !== undefined) {
    return draftNode.plannedHours
  }

  if (draftNode.plannedHoursTotal !== null && draftNode.plannedHoursTotal !== undefined) {
    return draftNode.plannedHoursTotal
  }

  return ''
}

function getActualHoursValue(draftNode) {
  if (!draftNode) {
    return ''
  }

  if (draftNode.actualHours !== null && draftNode.actualHours !== undefined) {
    return draftNode.actualHours
  }

  if (draftNode.actualHoursTotal !== null && draftNode.actualHoursTotal !== undefined) {
    return draftNode.actualHoursTotal
  }

  return ''
}

function WbsDetailPanel({
  detailMode,
  draftNode,
  isDirty,
  isSaving,
  selectedNode,
  onDraftChange,
  onSaveDraft,
  onCancelDraft,
  onDeactivateSelectedNode,
  persons,
  rateCategories,
  taskStatuses,
  masterDataLoading,
  masterDataError,
  resourceAssignments,
  resourceAssignmentsLoading,
  resourceAssignmentsError,
  isCreatingAssignment,
  assignmentActionError,
  onCreateAssignment,
}) {
  const titleInputRef = useRef(null)

  useEffect(() => {
    if (detailMode === 'create-edit' && draftNode) {
      titleInputRef.current?.focus()
      titleInputRef.current?.select()
    }
  }, [detailMode, draftNode?.id])

  const panelTitle =
    detailMode === 'create-edit'
      ? 'Neues Element anlegen'
      : detailMode === 'view-edit'
        ? 'Element bearbeiten'
        : 'Details'

  const titleValue = typeof draftNode?.title === 'string' ? draftNode.title : ''
  const isTitleValid = titleValue.trim().length > 0

  const isSaveDisabled =
    isSaving || !isTitleValid || (detailMode !== 'create-edit' && !isDirty)

  const plannedHoursValue = getPlannedHoursValue(draftNode)
  const actualHoursValue = getActualHoursValue(draftNode)

  return (
    <section className="panel">
      <h2>{panelTitle}</h2>

      {detailMode === 'empty' || !draftNode ? (
        <div className="empty-state">
          <p>Wähle einen WBS-Knoten aus oder lege links ein neues Element an.</p>

          <WbsResourceAssignmentsPanel
            selectedNode={selectedNode}
            resourceAssignments={resourceAssignments}
            resourceAssignmentsLoading={resourceAssignmentsLoading}
            resourceAssignmentsError={resourceAssignmentsError}
            masterDataLoading={masterDataLoading}
            masterDataError={masterDataError}
            persons={persons}
            rateCategories={rateCategories}
            taskStatuses={taskStatuses}
            isCreatingAssignment={isCreatingAssignment}
            assignmentActionError={assignmentActionError}
            onCreateAssignment={onCreateAssignment}
          />
        </div>
      ) : (
        <>
          <form
            className="detail-form"
            onSubmit={(event) => {
              event.preventDefault()

              if (!isTitleValid) {
                titleInputRef.current?.focus()
                return
              }

              onSaveDraft()
            }}
          >
            <div className="form-row">
              <label htmlFor="title">
                Titel <span className="required-mark">*</span>
              </label>
              <input
                ref={titleInputRef}
                id="title"
                name="title"
                value={draftNode.title ?? ''}
                onChange={onDraftChange}
                aria-invalid={!isTitleValid}
              />
              {!isTitleValid && (
                <div className="field-error">Bitte einen Titel eingeben.</div>
              )}
            </div>

            <div className="form-row">
              <label htmlFor="type">Typ</label>
              <input id="type" name="type" value={draftNode.type ?? ''} readOnly />
            </div>

            <div className="form-row">
              <label htmlFor="description">Beschreibung</label>
              <textarea
                id="description"
                name="description"
                value={draftNode.description ?? ''}
                onChange={onDraftChange}
                rows="4"
              />
            </div>

            <div className="form-row">
              <label htmlFor="plannedStart">Planstart</label>
              <input
                id="plannedStart"
                name="plannedStart"
                type="date"
                value={draftNode.plannedStart ?? ''}
                onChange={onDraftChange}
              />
            </div>

            <div className="form-row">
              <label htmlFor="plannedEnd">Planende</label>
              <input
                id="plannedEnd"
                name="plannedEnd"
                type="date"
                value={draftNode.plannedEnd ?? ''}
                onChange={onDraftChange}
              />
            </div>

            <div className="form-row">
              <label htmlFor="plannedHours">Konsolidierte Planstunden</label>
              <input
                id="plannedHours"
                name="plannedHours"
                type="number"
                min="0"
                step="0.25"
                value={plannedHoursValue}
                onChange={onDraftChange}
              />
            </div>

            <div className="form-row">
              <label htmlFor="actualHours">Konsolidierte Ist-Stunden</label>
              <input
                id="actualHours"
                name="actualHours"
                type="number"
                min="0"
                step="0.25"
                value={actualHoursValue}
                onChange={onDraftChange}
              />
            </div>

            <div className="selection-hint">
              <strong>Hinweis zu Stundenwerten</strong>
              <div className="selection-hint-description">
                Diese Werte sind die konsolidierten WBS-Werte aus der Aufgabenstruktur.
                Die einzelnen Personen- und Rollenstunden werden unten im Bereich Ressourcen
                angezeigt.
              </div>
            </div>

            <div className="form-row checkbox-row">
              <label htmlFor="isBlocked">Blockiert</label>
              <input
                id="isBlocked"
                name="isBlocked"
                type="checkbox"
                checked={Boolean(draftNode.isBlocked)}
                onChange={onDraftChange}
              />
            </div>

            <div className="form-row">
              <label htmlFor="comment">Kommentar</label>
              <textarea
                id="comment"
                name="comment"
                value={draftNode.comment ?? ''}
                onChange={onDraftChange}
                rows="3"
              />
            </div>

            <div className="meta-box">
              <div>
                <strong>WBS-ID:</strong> {draftNode.visibleWbsId ?? '-'}
              </div>
              <div>
                <strong>Ebene:</strong> {draftNode.level ?? '-'}
              </div>
              <div>
                <strong>Sortierung:</strong> {draftNode.sortOrder ?? '-'}
              </div>
            </div>

            <div className="form-actions">
              <button type="submit" disabled={isSaveDisabled}>
                {isSaving ? 'Speichern...' : 'Speichern'}
              </button>

              <button type="button" onClick={onCancelDraft} disabled={isSaving}>
                Abbrechen
              </button>

              <button
                type="button"
                className="danger"
                onClick={onDeactivateSelectedNode}
                disabled={isSaving || !selectedNode}
              >
                Deaktivieren
              </button>
            </div>
          </form>

          <WbsResourceAssignmentsPanel
            selectedNode={selectedNode}
            resourceAssignments={resourceAssignments}
            resourceAssignmentsLoading={resourceAssignmentsLoading}
            resourceAssignmentsError={resourceAssignmentsError}
            masterDataLoading={masterDataLoading}
            masterDataError={masterDataError}
            persons={persons}
            rateCategories={rateCategories}
            taskStatuses={taskStatuses}
            isCreatingAssignment={isCreatingAssignment}
            assignmentActionError={assignmentActionError}
            onCreateAssignment={onCreateAssignment}
          />
        </>
      )}
    </section>
  )
}

export default WbsDetailPanel