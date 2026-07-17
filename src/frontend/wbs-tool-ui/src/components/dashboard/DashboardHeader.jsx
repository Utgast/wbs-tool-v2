export default function DashboardHeader({
  projectName,
  progressPercent = 0,
  riskCount = 0,
  competencyCoverage = 0,
  utilizationPercent = 0,
}) {
  return (
    <section className="dashboard-header">
      <div className="dashboard-header-title">
        <h1>{projectName || 'Projekt ohne Namen'}</h1>
        <p>Projektleitstelle</p>
      </div>

      <div className="dashboard-kpi-row">
        <div className="dashboard-kpi-card">
          <span className="dashboard-kpi-value">
            {Number(progressPercent).toFixed(0)} %
          </span>
          <span className="dashboard-kpi-label">Fortschritt</span>
        </div>

        <div className="dashboard-kpi-card">
          <span className="dashboard-kpi-value">
            {Number(riskCount).toFixed(0)}
          </span>
          <span className="dashboard-kpi-label">Risiken</span>
        </div>

        <div className="dashboard-kpi-card">
          <span className="dashboard-kpi-value">
            {Number(competencyCoverage).toFixed(0)} %
          </span>
          <span className="dashboard-kpi-label">Skills</span>
        </div>

        <div className="dashboard-kpi-card">
          <span className="dashboard-kpi-value">
            {Number(utilizationPercent).toFixed(0)} %
          </span>
          <span className="dashboard-kpi-label">Kapazität</span>
        </div>
      </div>
    </section>
  );
}