export default function HandlungsbedarfPanel({
  overdueNodes = 0,
  blockedNodes = 0,
  openHours = 0,
  utilizationPercent = 0,
  missingCompetencies = 0,
}) {

  return (
    <section className="action-panel">
      <div className="action-panel-header">
        🔴 Handlungsbedarf
      </div>

      <div className="action-panel-grid">
        <div className="action-panel-item">
          <strong>{overdueNodes}</strong>
          <span>Überfällige Vorgänge</span>
        </div>

        <div className="action-panel-item">
          <strong>{blockedNodes}</strong>
          <span>Blockierte Vorgänge</span>
        </div>

        <div className="action-panel-item">
          <strong>{Math.abs(openHours)}</strong>
          <span>
            {openHours >= 0
              ? 'Offene Stunden'
              : 'Überdeckte Stunden'}
          </span>
        </div>

        <div className="action-panel-item">
          <strong>
            {Number(utilizationPercent).toFixed(0)} %
          </strong>
          <span>Auslastung</span>
        </div>

        <div className="action-panel-item">
        <strong>{missingCompetencies}</strong>
        <span>Kompetenzlücken</span>
      </div>

      </div>
    </section>
  );
}