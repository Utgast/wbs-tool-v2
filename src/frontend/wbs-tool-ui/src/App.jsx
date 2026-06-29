import { useEffect, useState } from 'react'
import './App.css'

function WbsTreeNode({ node }) {
  return (
    <li className="wbs-node">
      <div className="wbs-node-card">
        <strong>{node.visibleWbsId}</strong> - {node.title}
        <div className="wbs-node-meta">
          <span>Typ: {node.type}</span>
          <span>Level: {node.level}</span>
          <span>Aktiv: {node.isActive ? 'Ja' : 'Nein'}</span>
        </div>
      </div>

      {node.children && node.children.length > 0 && (
        <ul className="wbs-children">
          {node.children.map((child) => (
            <WbsTreeNode key={child.id} node={child} />
          ))}
        </ul>
      )}
    </li>
  )
}

function App() {
  const [projects, setProjects] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')

  const [selectedProject, setSelectedProject] = useState(null)
  const [wbsTree, setWbsTree] = useState([])
  const [wbsLoading, setWbsLoading] = useState(false)
  const [wbsError, setWbsError] = useState('')

  const [form, setForm] = useState({
    parentId: '',
    visibleWbsId: '',
    title: '',
    description: '',
    type: 'MainPackage',
    sortOrder: 1,
    plannedStart: '',
    plannedEnd: '',
    plannedHours: '',
    actualHours: '',
    isBlocked: false,
    comment: '',
  })

  const [createError, setCreateError] = useState('')
  const [createSuccess, setCreateSuccess] = useState('')

  useEffect(() => {
    fetch('http://localhost:5046/api/projects')
      .then((response) => {
        if (!response.ok) {
          throw new Error('Fehler beim Laden der Projekte')
        }
        return response.json()
      })
      .then((data) => {
        setProjects(data)
        setLoading(false)
      })
      .catch((err) => {
        setError(err.message)
        setLoading(false)
      })
  }, [])

  const loadWbsTree = (project) => {
    setSelectedProject(project)
    setWbsLoading(true)
    setWbsError('')
    setWbsTree([])
    setCreateError('')
    setCreateSuccess('')

    fetch(`http://localhost:5046/api/projects/${project.id}/wbs/tree`)
      .then((response) => {
        if (!response.ok) {
          throw new Error('Fehler beim Laden der WBS')
        }
        return response.json()
      })
      .then((data) => {
        setWbsTree(data)
        setWbsLoading(false)
      })
      .catch((err) => {
        setWbsError(err.message)
        setWbsLoading(false)
      })
  }

  const loadWbsTreeByProjectId = (projectId) => {
    setWbsLoading(true)
    setWbsError('')

    fetch(`http://localhost:5046/api/projects/${projectId}/wbs/tree`)
      .then((response) => {
        if (!response.ok) {
          throw new Error('Fehler beim Laden der WBS')
        }
        return response.json()
      })
      .then((data) => {
        setWbsTree(data)
        setWbsLoading(false)
      })
      .catch((err) => {
        setWbsError(err.message)
        setWbsLoading(false)
      })
  }

  const flattenTree = (nodes) => {
    const result = []

    const walk = (node) => {
      result.push(node)
      if (node.children && node.children.length > 0) {
        node.children.forEach(walk)
      }
    }

    nodes.forEach(walk)
    return result
  }

  const allWbsNodes = flattenTree(wbsTree)

  const handleInputChange = (event) => {
    const { name, value, type, checked } = event.target
    setForm((prev) => ({
      ...prev,
      [name]: type === 'checkbox' ? checked : value,
    }))
  }

  const resetForm = () => {
    setForm({
      parentId: '',
      visibleWbsId: '',
      title: '',
      description: '',
      type: 'MainPackage',
      sortOrder: 1,
      plannedStart: '',
      plannedEnd: '',
      plannedHours: '',
      actualHours: '',
      isBlocked: false,
      comment: '',
    })
  }

  const handleCreateWbsNode = async (event) => {
    event.preventDefault()

    if (!selectedProject) {
      setCreateError('Bitte zuerst ein Projekt auswählen.')
      return
    }

    setCreateError('')
    setCreateSuccess('')

    const payload = {
      parentId: form.parentId || null,
      visibleWbsId: form.visibleWbsId,
      title: form.title,
      description: form.description,
      type: form.type,
      level: 1,
      sortOrder: Number(form.sortOrder) || 1,
      plannedStart: form.plannedStart || null,
      plannedEnd: form.plannedEnd || null,
      plannedHours: form.plannedHours === '' ? null : Number(form.plannedHours),
      actualHours: form.actualHours === '' ? null : Number(form.actualHours),
      isBlocked: form.isBlocked,
      comment: form.comment,
    }

    try {
      const response = await fetch(`http://localhost:5046/api/projects/${selectedProject.id}/wbs`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(payload),
      })

      const responseData = await response.json()

      if (!response.ok) {
        throw new Error(responseData.message || 'Fehler beim Anlegen des WBS-Knotens')
      }

      setCreateSuccess('WBS-Knoten erfolgreich angelegt.')
      resetForm()
      loadWbsTreeByProjectId(selectedProject.id)
    } catch (err) {
      setCreateError(err.message)
    }
  }

  return (
    <div className="app">
      <header className="app-header">
        <h1>WBS Tool</h1>
        <p>Projektübersicht</p>
      </header>

      <main className="app-content">
        <section className="projects-section">
          <h2>Projekte</h2>

          {loading && <p>Lade Projekte...</p>}
          {error && <p className="error">{error}</p>}

          {!loading && !error && (
            <div className="project-list">
              {projects.length === 0 ? (
                <p>Keine Projekte gefunden.</p>
              ) : (
                projects.map((project) => (
                  <button
                    type="button"
                    className={`project-card ${selectedProject?.id === project.id ? 'selected' : ''}`}
                    key={project.id}
                    onClick={() => loadWbsTree(project)}
                  >
                    <h3>{project.name}</h3>
                    <p><strong>Projektnummer:</strong> {project.projectNumber}</p>
                    <p><strong>Beschreibung:</strong> {project.description}</p>
                    <p><strong>Start:</strong> {project.plannedStart ?? '-'}</p>
                    <p><strong>Ende:</strong> {project.plannedEnd ?? '-'}</p>
                    <p><strong>Aktiv:</strong> {project.isActive ? 'Ja' : 'Nein'}</p>
                  </button>
                ))
              )}
            </div>
          )}
        </section>

        <section className="wbs-section">
          <h2>WBS</h2>

          {!selectedProject && <p>Bitte ein Projekt auswählen.</p>}

          {selectedProject && (
            <>
              <p className="selected-project">
                Ausgewähltes Projekt: <strong>{selectedProject.name}</strong>
              </p>

              <form className="wbs-form" onSubmit={handleCreateWbsNode}>
                <h3>WBS-Knoten anlegen</h3>

                <label>
                  Parent-Knoten
                  <select name="parentId" value={form.parentId} onChange={handleInputChange}>
                    <option value="">Kein Parent (Root-Knoten)</option>
                    {allWbsNodes.map((node) => (
                      <option key={node.id} value={node.id}>
                        {node.visibleWbsId} - {node.title}
                      </option>
                    ))}
                  </select>
                </label>

                <label>
                  Visible WBS ID
                  <input
                    type="text"
                    name="visibleWbsId"
                    value={form.visibleWbsId}
                    onChange={handleInputChange}
                    required
                  />
                </label>

                <label>
                  Titel
                  <input
                    type="text"
                    name="title"
                    value={form.title}
                    onChange={handleInputChange}
                    required
                  />
                </label>

                <label>
                  Beschreibung
                  <textarea
                    name="description"
                    value={form.description}
                    onChange={handleInputChange}
                  />
                </label>

                <label>
                  Typ
                  <select name="type" value={form.type} onChange={handleInputChange}>
                    <option value="MainPackage">MainPackage</option>
                    <option value="SubPackage">SubPackage</option>
                    <option value="Task">Task</option>
                  </select>
                </label>

                <label>
                  Sortierung
                  <input
                    type="number"
                    name="sortOrder"
                    value={form.sortOrder}
                    onChange={handleInputChange}
                  />
                </label>

                <label>
                  Planstart
                  <input
                    type="date"
                    name="plannedStart"
                    value={form.plannedStart}
                    onChange={handleInputChange}
                  />
                </label>

                <label>
                  Planende
                  <input
                    type="date"
                    name="plannedEnd"
                    value={form.plannedEnd}
                    onChange={handleInputChange}
                  />
                </label>

                <label>
                  Planstunden
                  <input
                    type="number"
                    step="0.1"
                    name="plannedHours"
                    value={form.plannedHours}
                    onChange={handleInputChange}
                  />
                </label>

                <label>
                  Ist-Stunden
                  <input
                    type="number"
                    step="0.1"
                    name="actualHours"
                    value={form.actualHours}
                    onChange={handleInputChange}
                  />
                </label>

                <label className="checkbox-label">
                  <input
                    type="checkbox"
                    name="isBlocked"
                    checked={form.isBlocked}
                    onChange={handleInputChange}
                  />
                  Blockiert
                </label>

                <label>
                  Kommentar
                  <textarea
                    name="comment"
                    value={form.comment}
                    onChange={handleInputChange}
                  />
                </label>

                <button type="submit" className="submit-button">
                  WBS-Knoten anlegen
                </button>

                {createError && <p className="error">{createError}</p>}
                {createSuccess && <p className="success">{createSuccess}</p>}
              </form>

              {wbsLoading && <p>Lade WBS...</p>}
              {wbsError && <p className="error">{wbsError}</p>}

              {!wbsLoading && !wbsError && (
                <>
                  {wbsTree.length === 0 ? (
                    <p>Keine WBS-Knoten vorhanden.</p>
                  ) : (
                    <ul className="wbs-tree">
                      {wbsTree.map((node) => (
                        <WbsTreeNode key={node.id} node={node} />
                      ))}
                    </ul>
                  )}
                </>
              )}
            </>
          )}
        </section>
      </main>
    </div>
  )
}

export default App