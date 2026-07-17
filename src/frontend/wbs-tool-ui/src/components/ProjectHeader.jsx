function formatNumber(value) {
  if (value === null || value === undefined || value === '') {
    return '-'
  }

  const numberValue = Number(value)

  if (Number.isNaN(numberValue)) {
    return '-'
  }

  return numberValue.toLocaleString('de-DE', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  })
}

function formatPercent(value) {
  if (value === null || value === undefined || value === '') {
    return '-'
  }

  const numberValue = Number(value)

  if (Number.isNaN(numberValue)) {
    return '-'
  }

  return `${numberValue.toLocaleString('de-DE', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  })}%`
}

function ProjectHeader({
  projects,
  selectedProject,
  dashboard,
  loadingProjects,
  loadingWorkspace,
  onProjectChange,
}) {
  return (
    <>
      <header className="topbar">
        <h1>WBS Workspace</h1>

        <div className="project-select">
          <label htmlFor="projectSelect">Projekt</label>
          <select
            id="projectSelect"
            value={selectedProject?.id ?? ''}
            onChange={onProjectChange}
            disabled={loadingProjects || projects.length === 0}
          >
            {projects.length === 0 && (
              <option value="">Keine Projekte verfügbar</option>
            )}

            {projects.map((project) => (
              <option key={project.id} value={project.id}>
                {project.name || project.title || `Projekt ${project.id}`}
              </option>
            ))}
          </select>
        </div>
      </header>

      <section className="dashboard-panel">
        <h2>Projektübersicht</h2>

        {loadingWorkspace ? (
          <p>Lade Projektübersicht...</p>
        ) : selectedProject ? (
          <div className="dashboard-grid">
            <div className="dashboard-card">
              <span className="label">Geplante Stunden</span>
              <strong>{formatNumber(dashboard?.totalPlannedHours)}</strong>
            </div>

            <div className="dashboard-card">
              <span className="label">Ist-Stunden</span>
              <strong>{formatNumber(dashboard?.totalActualHours)}</strong>
            </div>

            <div className="dashboard-card">
              <span className="label">Fortschritt</span>
              <strong>{formatPercent(dashboard?.progressPercent)}</strong>
            </div>

            <div className="dashboard-card">
              <span className="label">Blocker</span>
              <strong>{formatNumber(dashboard?.blockedNodes)}</strong>
            </div>
          </div>
        ) : (
          <p>Bitte zuerst ein Projekt auswählen.</p>
        )}
      </section>
    </>
  )
}

export default ProjectHeader