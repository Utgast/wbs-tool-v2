import { DndContext } from '@dnd-kit/core'
import { useEffect, useState } from 'react'
import './App.css'

import ProjectHeader from './components/ProjectHeader'
import TemplatePalette from './components/TemplatePalette'
import WbsDetailPanel from './components/WbsDetailPanel'
import WbsTreePanel from './components/WbsTreePanel'

import { getPersons } from './services/personsApi'
import { getRateCategories } from './services/rateCategoriesApi'
import {
  createResourceAssignment,
  getResourceAssignments,
} from './services/resourceAssignmentsApi'
import { getTaskStatuses } from './services/taskStatusesApi'

import {
  createWbsNode,
  deactivateWbsNode,
  getProjectDashboard,
  getProjects,
  getWbsTree,
  updateWbsNode,
} from './services/api'

import {
  buildCreatePayload,
  canCreateUnder,
  findNodeInTree,
  flattenTree,
  getCreateValidationMessage,
  isNodeMeaningfullyEmpty,
  WBS_NODE_TYPES,
} from './utils/wbsRules'

const LAST_SELECTED_PROJECT_ID_KEY = 'wbs-tool:last-selected-project-id'

function App() {
  const [projects, setProjects] = useState([])
  const [selectedProject, setSelectedProject] = useState(null)

  const [dashboard, setDashboard] = useState(null)
  const [wbsTree, setWbsTree] = useState([])

  const [selectedNode, setSelectedNode] = useState(null)
  const [detailMode, setDetailMode] = useState('empty')
  const [draftNode, setDraftNode] = useState(null)

  const [isDirty, setIsDirty] = useState(false)
  const [isSaving, setIsSaving] = useState(false)

  const [lastCreatedNode, setLastCreatedNode] = useState(null)

  const [loadingProjects, setLoadingProjects] = useState(false)
  const [loadingWorkspace, setLoadingWorkspace] = useState(false)

  const [error, setError] = useState('')
  const [toast, setToast] = useState(null)

  const [activeDragTemplateType, setActiveDragTemplateType] = useState(null)

  const [persons, setPersons] = useState([])
  const [rateCategories, setRateCategories] = useState([])
  const [taskStatuses, setTaskStatuses] = useState([])

  const [masterDataLoading, setMasterDataLoading] = useState(false)
  const [masterDataError, setMasterDataError] = useState('')

  const [resourceAssignments, setResourceAssignments] = useState([])
  const [resourceAssignmentsLoading, setResourceAssignmentsLoading] = useState(false)
  const [resourceAssignmentsError, setResourceAssignmentsError] = useState('')

  const [isCreatingAssignment, setIsCreatingAssignment] = useState(false)
  const [assignmentActionError, setAssignmentActionError] = useState('')

  useEffect(() => {
    loadProjects()
    loadMasterData()
  }, [])

  useEffect(() => {
    if (!selectedProject?.id) {
      setDashboard(null)
      setWbsTree([])
      setSelectedNode(null)
      setDraftNode(null)
      setDetailMode('empty')
      setResourceAssignments([])
      setResourceAssignmentsError('')
      setAssignmentActionError('')
      return
    }

    loadWorkspace(selectedProject.id)
  }, [selectedProject])

  function getStoredProjectId() {
    try {
      return localStorage.getItem(LAST_SELECTED_PROJECT_ID_KEY)
    } catch {
      return null
    }
  }

  function storeProjectId(projectId) {
    try {
      if (projectId) {
        localStorage.setItem(LAST_SELECTED_PROJECT_ID_KEY, String(projectId))
      }
    } catch {
      // App soll auch funktionieren, falls localStorage blockiert ist.
    }
  }

  async function loadProjects() {
    setLoadingProjects(true)
    setError('')

    try {
      const data = await getProjects()
      const safeProjects = Array.isArray(data) ? data : []

      setProjects(safeProjects)

      if (safeProjects.length > 0) {
        const storedProjectId = getStoredProjectId()

        const rememberedProject =
          safeProjects.find((project) => String(project.id) === String(storedProjectId)) ||
          null

        const nextSelectedProject = rememberedProject || safeProjects[0]

        setSelectedProject(nextSelectedProject)
        storeProjectId(nextSelectedProject.id)
      } else {
        setSelectedProject(null)
      }
    } catch (err) {
      setError(err.message || 'Fehler beim Laden der Projekte')
    } finally {
      setLoadingProjects(false)
    }
  }

  async function loadMasterData() {
    setMasterDataLoading(true)
    setMasterDataError('')

    try {
      const [personsData, rateCategoriesData, taskStatusesData] = await Promise.all([
        getPersons(),
        getRateCategories(),
        getTaskStatuses(),
      ])

      setPersons(Array.isArray(personsData) ? personsData : [])
      setRateCategories(Array.isArray(rateCategoriesData) ? rateCategoriesData : [])
      setTaskStatuses(Array.isArray(taskStatusesData) ? taskStatusesData : [])
    } catch (err) {
      setMasterDataError(err.message || 'Stammdaten konnten nicht geladen werden')
    } finally {
      setMasterDataLoading(false)
    }
  }

  async function loadResourceAssignments(projectId, wbsNodeId) {
    if (!projectId || !wbsNodeId) {
      setResourceAssignments([])
      setResourceAssignmentsError('')
      return
    }

    setResourceAssignmentsLoading(true)
    setResourceAssignmentsError('')

    try {
      const assignments = await getResourceAssignments(projectId, wbsNodeId)
      setResourceAssignments(Array.isArray(assignments) ? assignments : [])
    } catch (err) {
      setResourceAssignments([])
      setResourceAssignmentsError(
        err.message || 'Ressourcen-Zuordnungen konnten nicht geladen werden'
      )
    } finally {
      setResourceAssignmentsLoading(false)
    }
  }

  async function loadWorkspace(projectId) {
    setLoadingWorkspace(true)
    setError('')
    setResourceAssignments([])
    setResourceAssignmentsError('')
    setAssignmentActionError('')

    try {
      const [dashboardData, treeData] = await Promise.all([
        getProjectDashboard(projectId),
        getWbsTree(projectId),
      ])

      setDashboard(dashboardData)
      setWbsTree(Array.isArray(treeData) ? treeData : [])
      setSelectedNode(null)
      setDraftNode(null)
      setDetailMode('empty')
      setLastCreatedNode(null)
      setIsDirty(false)
    } catch (err) {
      setError(err.message || 'Fehler beim Laden des Arbeitsbereichs')
      setDashboard(null)
      setWbsTree([])
      setSelectedNode(null)
      setDraftNode(null)
      setDetailMode('empty')
      setLastCreatedNode(null)
      setIsDirty(false)
      setResourceAssignments([])
      setResourceAssignmentsError('')
      setAssignmentActionError('')
    } finally {
      setLoadingWorkspace(false)
    }
  }

  function handleProjectChange(event) {
    const projectId = event.target.value
    const project = projects.find((item) => String(item.id) === String(projectId)) || null

    setSelectedProject(project)
    storeProjectId(project?.id)

    setResourceAssignments([])
    setResourceAssignmentsError('')
    setAssignmentActionError('')
  }

  function handleSelectNode(node) {
    setSelectedNode(node)
    setDraftNode({ ...node })
    setDetailMode('view-edit')
    setIsDirty(false)
    setError('')
    setAssignmentActionError('')

    if (selectedProject?.id && node?.id) {
      loadResourceAssignments(selectedProject.id, node.id)
    } else {
      setResourceAssignments([])
      setResourceAssignmentsError('')
    }
  }

  function handleEditNode(node) {
    handleSelectNode(node)
  }

  async function handleDeactivateNodeFromTree(node) {
    if (!node?.id || !selectedProject?.id) {
      return
    }

    try {
      await deactivateWbsNode(selectedProject.id, node.id)

      const refreshedTree = await getWbsTree(selectedProject.id)
      const refreshedDashboard = await getProjectDashboard(selectedProject.id)

      setWbsTree(Array.isArray(refreshedTree) ? refreshedTree : [])
      setDashboard(refreshedDashboard)

      if (selectedNode?.id === node.id) {
        setSelectedNode(null)
        setDraftNode(null)
        setDetailMode('empty')
        setIsDirty(false)
        setResourceAssignments([])
        setResourceAssignmentsError('')
        setAssignmentActionError('')
      }

      setToast({
        type: 'info',
        message: 'Der WBS-Knoten wurde deaktiviert.',
      })
    } catch (err) {
      setError(err.message || 'Der WBS-Knoten konnte nicht deaktiviert werden.')
      setToast({
        type: 'error',
        message: err.message || 'Der WBS-Knoten konnte nicht deaktiviert werden.',
      })
    }
  }

  function handleDraftChange(event) {
    const { name, value, type, checked } = event.target

    setDraftNode((prev) => {
      if (!prev) {
        return prev
      }

      return {
        ...prev,
        [name]: type === 'checkbox' ? checked : value,
      }
    })

    setIsDirty(true)
  }

  async function handleSaveDraft() {
    if (!draftNode?.id || !selectedProject?.id) {
      return
    }

    setIsSaving(true)
    setError('')

    try {
      const plannedHoursValue =
        draftNode.plannedHours !== null &&
        draftNode.plannedHours !== undefined &&
        draftNode.plannedHours !== ''
          ? draftNode.plannedHours
          : draftNode.plannedHoursTotal

      const actualHoursValue =
        draftNode.actualHours !== null &&
        draftNode.actualHours !== undefined &&
        draftNode.actualHours !== ''
          ? draftNode.actualHours
          : draftNode.actualHoursTotal

      const payload = {
        visibleWbsId: draftNode.visibleWbsId ?? '',
        title: draftNode.title ?? '',
        description: draftNode.description ?? '',
        type: draftNode.type ?? '',
        sortOrder:
          draftNode.sortOrder === '' ||
          draftNode.sortOrder === null ||
          draftNode.sortOrder === undefined
            ? 0
            : Number(draftNode.sortOrder),
        plannedStart: draftNode.plannedStart || null,
        plannedEnd: draftNode.plannedEnd || null,
        plannedHours:
          plannedHoursValue === '' ||
          plannedHoursValue === null ||
          plannedHoursValue === undefined
            ? null
            : Number(plannedHoursValue),
        actualHours:
          actualHoursValue === '' ||
          actualHoursValue === null ||
          actualHoursValue === undefined
            ? null
            : Number(actualHoursValue),
        isBlocked: Boolean(draftNode.isBlocked),
        comment: draftNode.comment ?? '',
        isActive: draftNode.isActive ?? draftNode.active ?? true,
      }

      await updateWbsNode(selectedProject.id, draftNode.id, payload)

      const refreshedTree = await getWbsTree(selectedProject.id)
      const safeTree = Array.isArray(refreshedTree) ? refreshedTree : []

      setWbsTree(safeTree)

      const refreshedNode = findNodeInTree(safeTree, draftNode.id)

      if (refreshedNode) {
        setSelectedNode(refreshedNode)
        setDraftNode({ ...refreshedNode })
        setDetailMode('view-edit')
        await loadResourceAssignments(selectedProject.id, refreshedNode.id)
      } else {
        setSelectedNode(null)
        setDraftNode(null)
        setDetailMode('empty')
        setResourceAssignments([])
        setResourceAssignmentsError('')
        setAssignmentActionError('')
      }

      const refreshedDashboard = await getProjectDashboard(selectedProject.id)
      setDashboard(refreshedDashboard)

      setIsDirty(false)
      setToast({
        type: 'success',
        message: 'Der WBS-Knoten wurde gespeichert.',
      })
    } catch (err) {
      setError(err.message || 'Der WBS-Knoten konnte nicht gespeichert werden.')
      setToast({
        type: 'error',
        message: err.message || 'Der WBS-Knoten konnte nicht gespeichert werden.',
      })
    } finally {
      setIsSaving(false)
    }
  }

  async function handleCancelDraft() {
    if (
      detailMode === 'create-edit' &&
      draftNode?.id &&
      lastCreatedNode?.id === draftNode.id &&
      selectedProject?.id &&
      isNodeMeaningfullyEmpty(draftNode)
    ) {
      try {
        await deactivateWbsNode(selectedProject.id, draftNode.id)

        const refreshedTree = await getWbsTree(selectedProject.id)
        const refreshedDashboard = await getProjectDashboard(selectedProject.id)

        setWbsTree(Array.isArray(refreshedTree) ? refreshedTree : [])
        setDashboard(refreshedDashboard)
        setSelectedNode(null)
        setDraftNode(null)
        setDetailMode('empty')
        setIsDirty(false)
        setLastCreatedNode(null)
        setResourceAssignments([])
        setResourceAssignmentsError('')
        setAssignmentActionError('')

        setToast({
          type: 'info',
          message: 'Die Neuanlage wurde verworfen.',
        })
      } catch (err) {
        setError(err.message || 'Das neue Element konnte nicht verworfen werden.')
        setToast({
          type: 'error',
          message: err.message || 'Das neue Element konnte nicht verworfen werden.',
        })
      }

      return
    }

    if (selectedNode) {
      setDraftNode({ ...selectedNode })
      setIsDirty(false)
      setDetailMode('view-edit')
    } else {
      setDraftNode(null)
      setDetailMode('empty')
      setIsDirty(false)
      setResourceAssignments([])
      setResourceAssignmentsError('')
      setAssignmentActionError('')
    }
  }

  async function createFromTemplateAtTarget(templateType, targetNode) {
    if (!selectedProject?.id) {
      setToast({
        type: 'error',
        message: 'Bitte zuerst ein Projekt auswählen.',
      })
      return
    }

    if (!canCreateUnder(templateType, targetNode)) {
      setToast({
        type: 'error',
        message: getCreateValidationMessage(templateType, targetNode),
      })
      return
    }

    try {
      setError('')

      const payload = buildCreatePayload({
        projectId: selectedProject.id,
        templateType,
        targetNode,
      })

      const createdNode = await createWbsNode(payload)

      const refreshedTree = await getWbsTree(selectedProject.id)
      const refreshedDashboard = await getProjectDashboard(selectedProject.id)
      const safeTree = Array.isArray(refreshedTree) ? refreshedTree : []

      setWbsTree(safeTree)
      setDashboard(refreshedDashboard)

      const createdInTree = createdNode?.id
        ? findNodeInTree(safeTree, createdNode.id)
        : null

      const nextSelectedNode = createdInTree || createdNode

      setSelectedNode(nextSelectedNode)
      setDraftNode({
        ...nextSelectedNode,
        title: '',
      })
      setDetailMode('create-edit')
      setIsDirty(false)
      setLastCreatedNode(nextSelectedNode)
      setResourceAssignments([])
      setResourceAssignmentsError('')
      setAssignmentActionError('')

      setToast({
        type: 'success',
        message: 'Neues Element wurde angelegt. Bitte jetzt einen Titel vergeben.',
      })
    } catch (err) {
      setError(err.message || 'Das Element konnte nicht angelegt werden.')
      setToast({
        type: 'error',
        message: err.message || 'Das Element konnte nicht angelegt werden.',
      })
    }
  }

  async function handleCreateFromTemplate(templateType) {
    const targetNode = templateType === WBS_NODE_TYPES.MAIN_PACKAGE ? null : selectedNode

    await createFromTemplateAtTarget(templateType, targetNode)
  }

  async function handleDeactivateSelectedNode() {
    if (!selectedNode?.id || !selectedProject?.id) {
      return
    }

    try {
      await deactivateWbsNode(selectedProject.id, selectedNode.id)

      const refreshedTree = await getWbsTree(selectedProject.id)
      const refreshedDashboard = await getProjectDashboard(selectedProject.id)

      setWbsTree(Array.isArray(refreshedTree) ? refreshedTree : [])
      setDashboard(refreshedDashboard)
      setSelectedNode(null)
      setDraftNode(null)
      setDetailMode('empty')
      setIsDirty(false)
      setResourceAssignments([])
      setResourceAssignmentsError('')
      setAssignmentActionError('')

      setToast({
        type: 'info',
        message: 'Der WBS-Knoten wurde deaktiviert.',
      })
    } catch (err) {
      setError(err.message || 'Der WBS-Knoten konnte nicht deaktiviert werden.')
      setToast({
        type: 'error',
        message: err.message || 'Der WBS-Knoten konnte nicht deaktiviert werden.',
      })
    }
  }

  async function handleUndoLastCreate() {
    if (!lastCreatedNode?.id || !selectedProject?.id) {
      return
    }

    try {
      await deactivateWbsNode(selectedProject.id, lastCreatedNode.id)

      const refreshedTree = await getWbsTree(selectedProject.id)
      const refreshedDashboard = await getProjectDashboard(selectedProject.id)

      setWbsTree(Array.isArray(refreshedTree) ? refreshedTree : [])
      setDashboard(refreshedDashboard)
      setSelectedNode(null)
      setDraftNode(null)
      setDetailMode('empty')
      setIsDirty(false)
      setResourceAssignments([])
      setResourceAssignmentsError('')
      setAssignmentActionError('')

      setToast({
        type: 'info',
        message: 'Die letzte Neuanlage wurde rückgängig gemacht.',
      })
      setLastCreatedNode(null)
    } catch (err) {
      setError(err.message || 'Rückgängig konnte nicht ausgeführt werden.')
      setToast({
        type: 'error',
        message: err.message || 'Rückgängig konnte nicht ausgeführt werden.',
      })
    }
  }

  async function handleCreateAssignment(payload) {
    if (!selectedProject?.id || !selectedNode?.id) {
      return
    }

    setIsCreatingAssignment(true)
    setAssignmentActionError('')
    setResourceAssignmentsError('')

    try {
      await createResourceAssignment(selectedProject.id, selectedNode.id, payload)
      await loadResourceAssignments(selectedProject.id, selectedNode.id)

      const refreshedDashboard = await getProjectDashboard(selectedProject.id)
      setDashboard(refreshedDashboard)

      setToast({
        type: 'success',
        message: 'Ressourcen-Zuordnung wurde angelegt.',
      })
    } catch (err) {
      const message = err.message || 'Ressourcen-Zuordnung konnte nicht angelegt werden.'
      setAssignmentActionError(message)
      setToast({
        type: 'error',
        message,
      })
    } finally {
      setIsCreatingAssignment(false)
    }
  }

  function handleDragStart(event) {
    const templateType = event.active?.data?.current?.templateType ?? null
    setActiveDragTemplateType(templateType)

    if (templateType === WBS_NODE_TYPES.MAIN_PACKAGE) {
      setToast({
        type: 'info',
        message: 'Ziehe das Hauptpaket auf die Root-Zone, um es auf oberster Ebene anzulegen.',
      })
    }

    if (templateType === WBS_NODE_TYPES.SUB_PACKAGE) {
      setToast({
        type: 'info',
        message: 'Ziehe das Unterpaket auf ein Hauptpaket oder Unterpaket.',
      })
    }

    if (templateType === WBS_NODE_TYPES.TASK) {
      setToast({
        type: 'info',
        message: 'Ziehe die Aufgabe auf ein Hauptpaket oder Unterpaket.',
      })
    }
  }

  async function handleDragEnd(event) {
    const templateType = event.active?.data?.current?.templateType ?? null
    const overData = event.over?.data?.current ?? null
    const overId = event.over?.id ?? null

    setActiveDragTemplateType(null)

    if (!templateType) {
      return
    }

    if (!overId) {
      setToast({
        type: 'error',
        message: 'Das Element wurde nicht auf einem gültigen Ziel abgelegt.',
      })
      return
    }

    if (templateType === WBS_NODE_TYPES.MAIN_PACKAGE && overId === 'root-drop-zone') {
      await createFromTemplateAtTarget(WBS_NODE_TYPES.MAIN_PACKAGE, null)
      return
    }

    if (overData?.kind === 'node' && overData?.nodeId) {
      const targetNode = findNodeInTree(wbsTree, overData.nodeId)

      if (!targetNode) {
        setToast({
          type: 'error',
          message: 'Der Zielknoten wurde nicht gefunden.',
        })
        return
      }

      if (!canCreateUnder(templateType, targetNode)) {
        setToast({
          type: 'error',
          message: getCreateValidationMessage(templateType, targetNode),
        })
        return
      }

      await createFromTemplateAtTarget(templateType, targetNode)
      return
    }

    setToast({
      type: 'error',
      message: 'Dieses Element kann hier nicht erstellt werden.',
    })
  }

  function handleDragCancel() {
    setActiveDragTemplateType(null)
    setToast({
      type: 'info',
      message: 'Drag & Drop wurde abgebrochen.',
    })
  }

  const flatNodes = flattenTree(wbsTree)

  return (
    <DndContext
      onDragStart={handleDragStart}
      onDragEnd={handleDragEnd}
      onDragCancel={handleDragCancel}
    >
      <div className="app-shell">
        <ProjectHeader
          projects={projects}
          selectedProject={selectedProject}
          dashboard={dashboard}
          loadingProjects={loadingProjects}
          loadingWorkspace={loadingWorkspace}
          onProjectChange={handleProjectChange}
        />

        {error && <div className="error-banner">{error}</div>}

        {masterDataError && <div className="error-banner">{masterDataError}</div>}

        {activeDragTemplateType && (
          <div className="drag-status-banner">
            Ziehen aktiv: {activeDragTemplateType}
          </div>
        )}

        <main className="workspace-grid">
          <TemplatePalette
            selectedProject={selectedProject}
            selectedNode={selectedNode}
            onCreateFromTemplate={handleCreateFromTemplate}
          />

          <WbsTreePanel
            selectedProject={selectedProject}
            loadingWorkspace={loadingWorkspace}
            wbsTree={wbsTree}
            selectedNodeId={selectedNode?.id}
            totalNodeCount={flatNodes.length}
            onSelectNode={handleSelectNode}
            onEditNode={handleEditNode}
            onDeactivateNode={handleDeactivateNodeFromTree}
            activeDragTemplateType={activeDragTemplateType}
          />

          <WbsDetailPanel
            detailMode={detailMode}
            draftNode={draftNode}
            isDirty={isDirty}
            isSaving={isSaving}
            selectedNode={selectedNode}
            onDraftChange={handleDraftChange}
            onSaveDraft={handleSaveDraft}
            onCancelDraft={handleCancelDraft}
            onDeactivateSelectedNode={handleDeactivateSelectedNode}
            persons={persons}
            rateCategories={rateCategories}
            taskStatuses={taskStatuses}
            masterDataLoading={masterDataLoading}
            masterDataError={masterDataError}
            resourceAssignments={resourceAssignments}
            resourceAssignmentsLoading={resourceAssignmentsLoading}
            resourceAssignmentsError={resourceAssignmentsError}
            isCreatingAssignment={isCreatingAssignment}
            assignmentActionError={assignmentActionError}
            onCreateAssignment={handleCreateAssignment}
          />
        </main>

        {toast && (
          <div className={`toast ${toast.type || 'info'}`}>
            <span>{toast.message}</span>

            {lastCreatedNode && toast.type === 'success' && (
              <button type="button" onClick={handleUndoLastCreate}>
                Rückgängig
              </button>
            )}

            <button type="button" onClick={() => setToast(null)}>
              Schließen
            </button>
          </div>
        )}
      </div>
    </DndContext>
  )
}

export default App