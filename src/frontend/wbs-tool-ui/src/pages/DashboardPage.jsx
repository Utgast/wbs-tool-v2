import { useState } from 'react'

export default function DashboardPage({
  dashboard,
  wbsNodes = [],
  onOpenWbsNode,
}) {
  const [activeTile, setActiveTile] = useState('overview')

  if (!dashboard) {
    return (
      <section className="page-placeholder">
        <h2>Projektcockpit</h2>
        <p>Keine Dashboard-Daten vorhanden.</p>
      </section>
    )
  }

  const progressPercent = dashboard.progressPercent ?? 0
  const totalPlannedHours = dashboard.totalPlannedHours ?? 0
  const totalActualHours = dashboard.totalActualHours ?? 0
  const blockedNodes = dashboard.blockedNodes ?? 0
  const overdueNodes = dashboard.overdueNodes ?? 0

  const plannedDemandHours = dashboard.plannedDemandHours ?? 0
  const assignedHours = dashboard.assignedHours ?? 0
  const rawOpenHours = dashboard.openHours ?? 0
  const capacityHours = dashboard.capacityHours ?? 0
  const utilizationPercent = dashboard.utilizationPercent ?? 0

  const openDisplay =
    rawOpenHours < 0
      ? `Überdeckt: ${formatNumber(Math.abs(rawOpenHours))} h`
      : `${formatNumber(rawOpenHours)} h`

  const openStatus =
    rawOpenHours < 0
      ? 'warning'
      : rawOpenHours === 0
      ? 'success'
      : 'danger'

  const utilizationStatus =
    utilizationPercent > 110
      ? 'danger'
      : utilizationPercent > 90
      ? 'warning'
      : 'success'

  const overdueStatus =
    overdueNodes > 0 ? 'danger' : 'success'

  const blockedStatus =
    blockedNodes > 0 ? 'warning' : 'success'

  return (
    <section
      className="dashboard-page"
      style={{
        display: 'flex',
        flexDirection: 'column',
        gap: '20px',
      }}
    >
     
<DashboardHeader
  projectName={dashboard.projectName}
  activeTile={activeTile}
  onReset={() => setActiveTile('overview')}
/>


      <DashboardSection title="Handlungsbedarf">
        <InteractiveKpiTile
          id="overdue"
          title="Überfällig"
          value={formatNumber(overdueNodes)}
          unit="Vorgänge"
          status={overdueStatus}
          description="Terminlich kritische WBS-Elemente"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="blocked"
          title="Blockiert"
          value={formatNumber(blockedNodes)}
          unit="Vorgänge"
          status={blockedStatus}
          description="Elemente ohne aktuellen Fortschritt"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="open"
          title="Offen"
          value={openDisplay}
          unit=""
          status={openStatus}
          description="Nicht gedeckter oder überdeckter Bedarf"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="utilization"
          title="Auslastung"
          value={`${formatNumber(utilizationPercent)}%`}
          unit=""
          status={utilizationStatus}
          description="Kapazitätsauslastung im Projekt"
          activeTile={activeTile}
          onClick={setActiveTile}
        />
      </DashboardSection>

      <DashboardSection title="Projektstatus">
        <InteractiveKpiTile
          id="progress"
          title="Fortschritt"
          value={`${formatNumber(progressPercent)}%`}
          unit=""
          status="info"
          description="Aktueller Projektfortschritt"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="planned"
          title="Planstunden"
          value={formatNumber(totalPlannedHours)}
          unit="h"
          status="neutral"
          description="Geplante Gesamtstunden"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="actual"
          title="Ist-Stunden"
          value={formatNumber(totalActualHours)}
          unit="h"
          status="success"
          description="Bereits verbuchte Stunden"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="capacity"
          title="Kapazität"
          value={formatNumber(capacityHours)}
          unit="h"
          status="info"
          description="Verfügbare Kapazität"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="demand"
          title="Bedarf"
          value={formatNumber(plannedDemandHours)}
          unit="h"
          status="purple"
          description="Geplanter Ressourcenbedarf"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="assigned"
          title="Zugeordnet"
          value={formatNumber(assignedHours)}
          unit="h"
          status="success"
          description="Bereits zugeordnete Ressourcenstunden"
          activeTile={activeTile}
          onClick={setActiveTile}
        />
      </DashboardSection>
      
      
<DashboardSection title="WBS-Hauptpakete">
  {wbsNodes.map((node) => (
    <button
      key={node.id}
      type="button"
      onClick={() => onOpenWbsNode?.(node)}
      style={{
        textAlign: 'left',
        border: '1px solid #e5e7eb',
        borderRadius: '14px',
        padding: '16px',
        backgroundColor: '#ffffff',
        cursor: 'pointer',
        boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
      }}
    >
      <div
        style={{
          fontSize: '12px',
          color: '#6b7280',
          marginBottom: '6px',
        }}
      >
        Hauptpaket {node.visibleWbsId}
      </div>

      <div
        style={{
          fontWeight: '700',
          fontSize: '18px',
          color: '#111827',
        }}
      >
        {node.title}
      </div>
    </button>
  ))}
</DashboardSection>

      <DashboardDetailPanel
        activeTile={activeTile}
        dashboard={{
          progressPercent,
          totalPlannedHours,
          totalActualHours,
          blockedNodes,
          overdueNodes,
          plannedDemandHours,
          assignedHours,
          rawOpenHours,
          capacityHours,
          utilizationPercent,
        }}
      />
    </section>
  )
}

function DashboardHeader({ projectName, activeTile, onReset }) {
  return (
    <div
      style={{
        backgroundColor: '#ffffff',
        border: '1px solid #e5e7eb',
        borderRadius: '14px',
        padding: '18px 20px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        gap: '16px',
        flexWrap: 'wrap',
      }}
    >
      <div>
        <h2
          style={{
            margin: 0,
            color: '#111827',
          }}
        >
          Projektcockpit
        </h2>

        <p
          style={{
            margin: '6px 0 0 0',
            color: '#6b7280',
            fontSize: '14px',
          }}
        >
          {projectName
            ? `Aktives Projekt: ${projectName}`
            : 'Interaktives Steuerungsdashboard'}
        </p>
      </div>

      {activeTile !== 'overview' && (
        <button
          type="button"
          onClick={onReset}
          style={{
            border: '1px solid #d1d5db',
            backgroundColor: '#ffffff',
            borderRadius: '10px',
            padding: '10px 14px',
            cursor: 'pointer',
            fontWeight: '700',
            color: '#374151',
          }}
        >
          Übersicht anzeigen
        </button>
      )}
    </div>
  )
}

function DashboardSection({ title, children }) {
  return (
    <section>
      <h3
        style={{
          margin: '0 0 12px 0',
          color: '#111827',
        }}
      >
        {title}
      </h3>

      <div
        style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(180px, 1fr))',
          gap: '14px',
        }}
      >
        {children}
      </div>
    </section>
  )
}

function InteractiveKpiTile({
  id,
  title,
  value,
  unit,
  status,
  description,
  activeTile,
  onClick,
}) {
  const selected = activeTile === id
  const config = getStatusConfig(status)

  return (
    <button
      type="button"
      onClick={() => onClick(id)}
      style={{
        textAlign: 'left',
        border: selected
          ? `2px solid ${config.borderColor}`
          : '1px solid #e5e7eb',
        borderLeft: `6px solid ${config.accentColor}`,
        borderRadius: '14px',
        padding: '16px',
        backgroundColor: selected ? config.selectedBackground : '#ffffff',
        boxShadow: selected
          ? `0 4px 14px ${config.shadowColor}`
          : '0 2px 8px rgba(0,0,0,0.08)',
        cursor: 'pointer',
        minHeight: '132px',
        transition: 'all 0.15s ease',
      }}
    >
      <div
        style={{
          display: 'flex',
          justifyContent: 'space-between',
          gap: '10px',
          alignItems: 'flex-start',
          marginBottom: '10px',
        }}
      >
        <div
          style={{
            fontSize: '12px',
            color: '#6b7280',
            textTransform: 'uppercase',
            letterSpacing: '0.5px',
            fontWeight: '700',
          }}
        >
          {title}
        </div>

        <span
          style={{
            fontSize: '18px',
          }}
        >
          {config.icon}
        </span>
      </div>

      <div
        style={{
          color: config.textColor,
          fontWeight: '800',
          fontSize: value.length > 12 ? '20px' : '28px',
          lineHeight: '1.1',
          marginBottom: '8px',
        }}
      >
        {value}
        {unit && (
          <span
            style={{
              fontSize: '14px',
              marginLeft: '4px',
              color: '#6b7280',
            }}
          >
            {unit}
          </span>
        )}
      </div>

      <div
        style={{
          color: '#6b7280',
          fontSize: '13px',
          lineHeight: '1.35',
        }}
      >
        {description}
      </div>
    </button>
  )
}

function DashboardDetailPanel({ activeTile, dashboard }) {
  if (activeTile === 'overview') {
    return (
      <DetailPanel
        title="Steuerungsübersicht"
        status="info"
        text="Wähle eine Kachel aus, um Details, Bewertung und nächste Handlungsmöglichkeiten zu sehen."
      >
        <div
          style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(220px, 1fr))',
            gap: '12px',
          }}
        >
          <DetailFact
            label="Fortschritt"
            value={`${formatNumber(dashboard.progressPercent)}%`}
          />
          <DetailFact
            label="Plan / Ist"
            value={`${formatNumber(dashboard.totalPlannedHours)} h / ${formatNumber(
              dashboard.totalActualHours
            )} h`}
          />
          <DetailFact
            label="Bedarf / Zugeordnet"
            value={`${formatNumber(
              dashboard.plannedDemandHours
            )} h / ${formatNumber(dashboard.assignedHours)} h`}
          />
          <DetailFact
            label="Kapazität / Auslastung"
            value={`${formatNumber(
              dashboard.capacityHours
            )} h / ${formatNumber(dashboard.utilizationPercent)}%`}
          />
        </div>
      </DetailPanel>
    )
  }

  if (activeTile === 'overdue') {
    return (
      <DetailPanel
        title="Überfällige Vorgänge"
        status={dashboard.overdueNodes > 0 ? 'danger' : 'success'}
        text={
          dashboard.overdueNodes > 0
            ? `Es gibt aktuell ${formatNumber(
                dashboard.overdueNodes
              )} überfällige Vorgänge im Dashboard-Aggregat.`
            : 'Aktuell sind keine überfälligen Vorgänge im Dashboard-Aggregat vorhanden.'
        }
      >
        <DetailHint>
          Die Anzahl kommt bereits aus dem Backend. Für eine echte Liste der
          betroffenen WBS-Elemente brauchen wir als nächsten Schritt einen
          Detail-Endpunkt oder die Übergabe der WBS-Knoten an das Dashboard.
        </DetailHint>
      </DetailPanel>
    )
  }

  if (activeTile === 'blocked') {
    return (
      <DetailPanel
        title="Blockierte Elemente"
        status={dashboard.blockedNodes > 0 ? 'warning' : 'success'}
        text={
          dashboard.blockedNodes > 0
            ? `Es gibt aktuell ${formatNumber(
                dashboard.blockedNodes
              )} blockierte Elemente im Dashboard-Aggregat.`
            : 'Aktuell sind keine blockierten Elemente im Dashboard-Aggregat vorhanden.'
        }
      >
        <DetailHint>
          Für den operativen Drilldown sollten später die blockierten
          WBS-Elemente mit Name, Verantwortlichem, Termin und Status geladen
          werden.
        </DetailHint>
      </DetailPanel>
    )
  }

  if (activeTile === 'open') {
    const isOvercovered = dashboard.rawOpenHours < 0
    const value = Math.abs(dashboard.rawOpenHours)

    return (
      <DetailPanel
        title="Offene oder überdeckte Ressourcenstunden"
        status={isOvercovered ? 'warning' : dashboard.rawOpenHours > 0 ? 'danger' : 'success'}
        text={
          isOvercovered
            ? `Der Ressourcenbedarf ist aktuell um ${formatNumber(
                value
              )} h überdeckt.`
            : dashboard.rawOpenHours > 0
            ? `Es sind aktuell ${formatNumber(
                dashboard.rawOpenHours
              )} h noch nicht zugeordnet.`
            : 'Der geplante Bedarf ist aktuell vollständig gedeckt.'
        }
      >
        <DetailFact
          label="Geplanter Bedarf"
          value={`${formatNumber(dashboard.plannedDemandHours)} h`}
        />
        <DetailFact
          label="Zugeordnet"
          value={`${formatNumber(dashboard.assignedHours)} h`}
        />
      </DetailPanel>
    )
  }

  if (activeTile === 'utilization') {
    return (
      <DetailPanel
        title="Kapazitätsauslastung"
        status={
          dashboard.utilizationPercent > 110
            ? 'danger'
            : dashboard.utilizationPercent > 90
            ? 'warning'
            : 'success'
        }
        text={`Die aktuelle Auslastung liegt bei ${formatNumber(
          dashboard.utilizationPercent
        )}%.`}
      >
        <DetailFact
          label="Kapazität"
          value={`${formatNumber(dashboard.capacityHours)} h`}
        />
        <DetailFact
          label="Zugeordnet"
          value={`${formatNumber(dashboard.assignedHours)} h`}
        />
        <DetailHint>
          Werte über 100% sollten fachlich als Überlastung dargestellt werden,
          nicht nur als Prozentzahl.
        </DetailHint>
      </DetailPanel>
    )
  }

  if (activeTile === 'progress') {
    return (
      <DetailPanel
        title="Projektfortschritt"
        status="info"
        text={`Der aktuelle Fortschritt liegt bei ${formatNumber(
          dashboard.progressPercent
        )}%.`}
      >
        <DetailHint>
          Für Sprint 3.1 sollte dieser Wert gegen den Sollfortschritt aus
          PlannedStart, PlannedEnd und heutigem Datum verglichen werden.
        </DetailHint>
      </DetailPanel>
    )
  }

  if (activeTile === 'planned') {
    return (
      <DetailPanel
        title="Planstunden"
        status="neutral"
        text={`Aktuell sind ${formatNumber(
          dashboard.totalPlannedHours
        )} h geplant.`}
      >
        <DetailFact
          label="Planstunden"
          value={`${formatNumber(dashboard.totalPlannedHours)} h`}
        />
      </DetailPanel>
    )
  }

  if (activeTile === 'actual') {
    return (
      <DetailPanel
        title="Ist-Stunden"
        status="success"
        text={`Aktuell sind ${formatNumber(
          dashboard.totalActualHours
        )} h als Ist-Stunden erfasst.`}
      >
        <DetailFact
          label="Ist-Stunden"
          value={`${formatNumber(dashboard.totalActualHours)} h`}
        />
      </DetailPanel>
    )
  }

  if (activeTile === 'capacity') {
    return (
      <DetailPanel
        title="Kapazität"
        status="info"
        text={`Aktuell stehen ${formatNumber(
          dashboard.capacityHours
        )} h Kapazität zur Verfügung.`}
      >
        <DetailFact
          label="Kapazität"
          value={`${formatNumber(dashboard.capacityHours)} h`}
        />
      </DetailPanel>
    )
  }

  if (activeTile === 'demand') {
    return (
      <DetailPanel
        title="Ressourcenbedarf"
        status="purple"
        text={`Aktuell sind ${formatNumber(
          dashboard.plannedDemandHours
        )} h Ressourcenbedarf geplant.`}
      >
        <DetailFact
          label="Bedarf"
          value={`${formatNumber(dashboard.plannedDemandHours)} h`}
        />
      </DetailPanel>
    )
  }

  if (activeTile === 'assigned') {
    return (
      <DetailPanel
        title="Zugeordnete Ressourcen"
        status="success"
        text={`Aktuell sind ${formatNumber(
          dashboard.assignedHours
        )} h Ressourcen zugeordnet.`}
      >
        <DetailFact
          label="Zugeordnet"
          value={`${formatNumber(dashboard.assignedHours)} h`}
        />
      </DetailPanel>
    )
  }

  return null
}

function DetailPanel({ title, status, text, children }) {
  const config = getStatusConfig(status)

  return (
    <section
      style={{
        backgroundColor: '#ffffff',
        border: '1px solid #e5e7eb',
        borderTop: `5px solid ${config.accentColor}`,
        borderRadius: '14px',
        padding: '20px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
      }}
    >
      <div
        style={{
          display: 'flex',
          gap: '12px',
          alignItems: 'flex-start',
          marginBottom: '14px',
        }}
      >
        <div
          style={{
            fontSize: '24px',
          }}
        >
          {config.icon}
        </div>

        <div>
          <h3
            style={{
              margin: 0,
              color: '#111827',
            }}
          >
            {title}
          </h3>

          <p
            style={{
              margin: '6px 0 0 0',
              color: '#4b5563',
              lineHeight: '1.45',
            }}
          >
            {text}
          </p>
        </div>
      </div>

      <div
        style={{
          marginTop: '16px',
        }}
      >
        {children}
      </div>
    </section>
  )
}

function DetailFact({ label, value }) {
  return (
    <div
      style={{
        backgroundColor: '#f9fafb',
        border: '1px solid #e5e7eb',
        borderRadius: '12px',
        padding: '14px',
      }}
    >
      <div
        style={{
          color: '#6b7280',
          fontSize: '12px',
          textTransform: 'uppercase',
          letterSpacing: '0.5px',
          marginBottom: '6px',
          fontWeight: '700',
        }}
      >
        {label}
      </div>

      <div
        style={{
          color: '#111827',
          fontSize: '20px',
          fontWeight: '800',
        }}
      >
        {value}
      </div>
    </div>
  )
}

function DetailHint({ children }) {
  return (
    <div
      style={{
        padding: '14px',
        backgroundColor: '#f9fafb',
        border: '1px dashed #d1d5db',
        borderRadius: '12px',
        color: '#4b5563',
        lineHeight: '1.45',
      }}
    >
      {children}
    </div>
  )
}

function getStatusConfig(status) {
  const configs = {
    danger: {
      icon: '🔴',
      accentColor: '#dc2626',
      borderColor: '#dc2626',
      textColor: '#dc2626',
      selectedBackground: '#fef2f2',
      shadowColor: 'rgba(220,38,38,0.20)',
    },
    warning: {
      icon: '🟠',
      accentColor: '#ea580c',
      borderColor: '#ea580c',
      textColor: '#ea580c',
      selectedBackground: '#fff7ed',
      shadowColor: 'rgba(234,88,12,0.20)',
    },
    success: {
      icon: '🟢',
      accentColor: '#16a34a',
      borderColor: '#16a34a',
      textColor: '#16a34a',
      selectedBackground: '#f0fdf4',
      shadowColor: 'rgba(22,163,74,0.20)',
    },
    info: {
      icon: '🔵',
      accentColor: '#2563eb',
      borderColor: '#2563eb',
      textColor: '#2563eb',
      selectedBackground: '#eff6ff',
      shadowColor: 'rgba(37,99,235,0.20)',
    },
    purple: {
      icon: '🟣',
      accentColor: '#7c3aed',
      borderColor: '#7c3aed',
      textColor: '#7c3aed',
      selectedBackground: '#f5f3ff',
      shadowColor: 'rgba(124,58,237,0.20)',
    },
    neutral: {
      icon: '⚪',
      accentColor: '#6b7280',
      borderColor: '#6b7280',
      textColor: '#374151',
      selectedBackground: '#f9fafb',
      shadowColor: 'rgba(107,114,128,0.18)',
    },
  }

  return configs[status] ?? configs.neutral
}

function formatNumber(value) {
  if (value === null || value === undefined || Number.isNaN(Number(value))) {
    return '0'
  }

  return new Intl.NumberFormat('de-DE', {
    maximumFractionDigits: 2,
  }).format(Number(value))
}