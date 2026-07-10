import { useEffect, useState } from 'react';
import DashboardHeader from '../components/dashboard/DashboardHeader';
import HandlungsbedarfPanel from '../components/dashboard/HandlungsbedarfPanel';
import {
    closeRisk,
    getProjectDeliverables,
    getProjectRisks,
    markDeliverableDelivered,
    updateDeliverable,
    updateRisk,
} from '../services/api';

const RISK_STATUS = {
  1: 'New',
  2: 'InAssessment',
  3: 'MitigationDefined',
  4: 'InProgress',
  5: 'Accepted',
  6: 'Closed',
};

const RISK_SEVERITY = {
  1: 'Low',
  2: 'Medium',
  3: 'High',
};

const DELIVERABLE_STATUS = {
  1: 'Draft',
  2: 'InProgress',
  3: 'Review',
  4: 'Approved',
  5: 'Delivered',
};

const DELIVERABLE_TYPE = {
  1: 'Planning',
  2: 'Calculation',
  3: 'Drawing',
  4: 'ApprovalDocument',
  5: 'EnvironmentalDocument',
  6: 'Release',
  7: 'Acceptance',
  8: 'Report',
  9: 'Documentation',
};

export default function DashboardPage({
  dashboard,
  wbsNodes = [],
  onOpenWbsNode,
}) {
  const [activeTile, setActiveTile] = useState('overview');
  const [riskItems, setRiskItems] = useState([]);
  const [deliverableItems, setDeliverableItems] = useState([]);
  const [loadingRiskItems, setLoadingRiskItems] = useState(false);
  const [loadingDeliverableItems, setLoadingDeliverableItems] = useState(false);
  const [actionError, setActionError] = useState('');
  const [actionBusyId, setActionBusyId] = useState('');

  useEffect(() => {
    setRiskItems([]);
    setDeliverableItems([]);
    setActionError('');
    setActionBusyId('');
  }, [dashboard?.projectId]);

  useEffect(() => {
    if (!dashboard?.projectId) {
      return;
    }

    if (
      (activeTile === 'open-risks' || activeTile === 'critical-risks') &&
      riskItems.length === 0 &&
      !loadingRiskItems
    ) {
      void loadRiskItems(dashboard.projectId, setRiskItems, setLoadingRiskItems, setActionError);
    }

    if (
      (activeTile === 'open-deliverables' || activeTile === 'overdue-deliverables') &&
      deliverableItems.length === 0 &&
      !loadingDeliverableItems
    ) {
      void loadDeliverableItems(
        dashboard.projectId,
        setDeliverableItems,
        setLoadingDeliverableItems,
        setActionError
      );
    }
  }, [
    activeTile,
    dashboard?.projectId,
    riskItems.length,
    deliverableItems.length,
    loadingRiskItems,
    loadingDeliverableItems,
  ]);

  if (!dashboard) {
    return (
      <section className="page-placeholder">
        <h2>Projektcockpit</h2>
        <p>Keine Dashboard-Daten vorhanden.</p>
      </section>
    );
  }

  const progressPercent = dashboard.progressPercent ?? 0;
  const totalPlannedHours = dashboard.totalPlannedHours ?? 0;
  const totalActualHours = dashboard.totalActualHours ?? 0;
  const blockedNodes = dashboard.blockedNodes ?? 0;
  const overdueNodes = dashboard.overdueNodes ?? 0;
  const openRisks = dashboard.openRisks ?? 0;
  const criticalRisks = dashboard.criticalRisks ?? 0;
  const openDeliverables = dashboard.openDeliverables ?? 0;
  const overdueDeliverables = dashboard.overdueDeliverables ?? 0;
  const deliveryStatus = dashboard.deliveryStatus ?? 'Green';
  const deliveryStatusReason =
    dashboard.deliveryStatusReason ?? 'Keine offenen Risiken oder Deliverables';
  const deliveryStatusTrigger =
    dashboard.deliveryStatusTrigger ?? 'Kein Ausloeser';
  const openRiskItems = Array.isArray(dashboard.openRiskItems)
    ? dashboard.openRiskItems
    : [];
  const criticalRiskItems = Array.isArray(dashboard.criticalRiskItems)
    ? dashboard.criticalRiskItems
    : [];
  const openDeliverableItems = Array.isArray(dashboard.openDeliverableItems)
    ? dashboard.openDeliverableItems
    : [];
  const overdueDeliverableItems = Array.isArray(dashboard.overdueDeliverableItems)
    ? dashboard.overdueDeliverableItems
    : [];

  const plannedDemandHours = dashboard.plannedDemandHours ?? 0;
  const assignedHours = dashboard.assignedHours ?? 0;
  const coveredHours = dashboard.coveredHours ?? assignedHours;
  const rawOpenHours = dashboard.openHours ?? 0;
  const capacityHours = dashboard.capacityHours ?? 0;
  const utilizationPercent = dashboard.utilizationPercent ?? 0;
  const missingCompetencies = dashboard.missingCompetencies ?? 0;
  const competencyCoveragePercent = dashboard.competencyCoveragePercent ?? 0;
  const topRiskItems = Array.isArray(dashboard.topRiskItems)
    ? dashboard.topRiskItems
    : [];
  const criticalDeliverableItems = Array.isArray(dashboard.criticalDeliverableItems)
    ? dashboard.criticalDeliverableItems
    : [];

  const topManagementAttentionItems = Array.isArray(dashboard.topManagementAttentionItems)
    ? dashboard.topManagementAttentionItems
    : [];

  const openDisplay =
    rawOpenHours < 0
      ? `Überdeckt: ${formatNumber(Math.abs(rawOpenHours))} h`
      : `${formatNumber(rawOpenHours)} h`;

  const openStatus =
    rawOpenHours < 0
      ? 'warning'
      : rawOpenHours === 0
      ? 'success'
      : 'danger';

  const utilizationStatus =
    utilizationPercent > 110
      ? 'danger'
      : utilizationPercent > 90
      ? 'warning'
      : 'success';

  const overdueStatus = overdueNodes > 0 ? 'danger' : 'success';
  const blockedStatus = blockedNodes > 0 ? 'warning' : 'success';
  const openRisksStatus = openRisks > 0 ? 'warning' : 'success';
  const criticalRisksStatus = criticalRisks > 0 ? 'danger' : 'success';
  const openDeliverablesStatus = openDeliverables > 0 ? 'warning' : 'success';
  const overdueDeliverablesStatus =
    overdueDeliverables > 0 ? 'danger' : 'success';
  const deliveryStatusUi =
    deliveryStatus === 'Red'
      ? 'danger'
      : deliveryStatus === 'Yellow'
      ? 'warning'
      : 'success';

  const competencyGapStatus =
    competencyCoveragePercent < 50
      ? 'danger'
      : competencyCoveragePercent < 80
      ? 'warning'
      : 'success';

  const hasMissingHours = rawOpenHours > 0;
  const hasOverload = utilizationPercent > 100;

  const openRiskItemsFiltered = riskItems.filter(
    (item) => Number(item.status) !== 5 && Number(item.status) !== 6
  );
  const criticalRiskItemsFiltered = riskItems.filter(
    (item) =>
      Number(item.severity) === 3 &&
      Number(item.status) !== 5 &&
      Number(item.status) !== 6
  );
  const openDeliverableItemsFiltered = deliverableItems.filter(
    (item) => Number(item.status) !== 5
  );
  const overdueDeliverableItemsFiltered = deliverableItems.filter(
    (item) => Number(item.status) !== 5 && isDateBeforeToday(item.dueDate)
  );

  async function handleRiskStatusChange(riskId, nextStatus) {
    const risk = riskItems.find((item) => item.id === riskId);

    if (!risk || !nextStatus) {
      return;
    }

    setActionError('');
    setActionBusyId(`risk-status-${riskId}`);

    try {
      await updateRisk(riskId, {
        title: risk.title,
        description: risk.description,
        category: risk.category,
        severity: risk.severity,
        status: Number(nextStatus),
        ownerPersonId: risk.ownerPersonId,
        dueDate: risk.dueDate,
        wbsNodeId: risk.wbsNodeId,
      });

      await loadRiskItems(dashboard.projectId, setRiskItems, setLoadingRiskItems, setActionError);
    } catch (error) {
      setActionError(error?.message || 'Risiko-Status konnte nicht geaendert werden.');
    } finally {
      setActionBusyId('');
    }
  }

  async function handleRiskClose(riskId) {
    if (!riskId) {
      return;
    }

    setActionError('');
    setActionBusyId(`risk-close-${riskId}`);

    try {
      await closeRisk(riskId);
      await loadRiskItems(dashboard.projectId, setRiskItems, setLoadingRiskItems, setActionError);
    } catch (error) {
      setActionError(error?.message || 'Risiko konnte nicht geschlossen werden.');
    } finally {
      setActionBusyId('');
    }
  }

  async function handleDeliverableStatusChange(deliverableId, nextStatus) {
    const deliverable = deliverableItems.find((item) => item.id === deliverableId);

    if (!deliverable || !nextStatus) {
      return;
    }

    setActionError('');
    setActionBusyId(`deliverable-status-${deliverableId}`);

    try {
      await updateDeliverable(deliverableId, {
        name: deliverable.name,
        description: deliverable.description,
        type: deliverable.type,
        status: Number(nextStatus),
        ownerPersonId: deliverable.ownerPersonId,
        dueDate: deliverable.dueDate,
        processPhaseId: deliverable.processPhaseId,
        wbsNodeId: deliverable.wbsNodeId,
      });

      await loadDeliverableItems(
        dashboard.projectId,
        setDeliverableItems,
        setLoadingDeliverableItems,
        setActionError
      );
    } catch (error) {
      setActionError(error?.message || 'Deliverable-Status konnte nicht geaendert werden.');
    } finally {
      setActionBusyId('');
    }
  }

  async function handleDeliverableMarkDelivered(deliverableId) {
    if (!deliverableId) {
      return;
    }

    setActionError('');
    setActionBusyId(`deliverable-deliver-${deliverableId}`);

    try {
      await markDeliverableDelivered(deliverableId);
      await loadDeliverableItems(
        dashboard.projectId,
        setDeliverableItems,
        setLoadingDeliverableItems,
        setActionError
      );
    } catch (error) {
      setActionError(error?.message || 'Deliverable konnte nicht als Delivered markiert werden.');
    } finally {
      setActionBusyId('');
    }
  }

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
      progressPercent={progressPercent}
      riskCount={overdueNodes + blockedNodes}
      competencyCoverage={dashboard.competencyCoveragePercent ?? 0}
      utilizationPercent={utilizationPercent}
    />

      
      <HandlungsbedarfPanel
        overdueNodes={overdueNodes}
        blockedNodes={blockedNodes}
        openHours={rawOpenHours}
        utilizationPercent={utilizationPercent}
        missingCompetencies={dashboard?.missingCompetencies ?? 0}
    />

      <DashboardSection title="TOP HANDELN">
        {topManagementAttentionItems.length === 0 ? (
          <p style={{ color: '#6b7280', fontSize: '14px', gridColumn: '1 / -1' }}>
            Kein Handlungsbedarf berechnet.
          </p>
        ) : (
          topManagementAttentionItems.map((item, idx) => (
            <AttentionItemCard key={idx} item={item} />
          ))
        )}
      </DashboardSection>

      <DashboardSection title="Handlungsbedarf">
        <CockpitCard
          title="Kompetenz-Gaps"
          status={competencyGapStatus}
          subtitle="Deckung und fehlende Kompetenzen"
        >
          <CockpitFact
            label="MissingCompetencies"
            value={formatNumber(missingCompetencies)}
          />
          <CockpitFact
            label="CompetencyCoveragePercent"
            value={`${formatNumber(competencyCoveragePercent)}%`}
          />
        </CockpitCard>

        <CockpitCard
          title="Kapazitaets-Gaps"
          status={hasMissingHours || hasOverload ? 'warning' : 'success'}
          subtitle="Fehlende Stunden und Auslastung"
        >
          <CockpitFact label="OpenHours" value={`${formatNumber(rawOpenHours)} h`} />
          <CockpitFact label="CoveredHours" value={`${formatNumber(coveredHours)} h`} />
          <CockpitFact
            label="UtilizationPercent"
            value={`${formatNumber(utilizationPercent)}%`}
          />
          {hasMissingHours ? <DetailHint>Fehlende Stunden erkannt.</DetailHint> : null}
          {hasOverload ? <DetailHint>Ueberlastung erkannt (Auslastung ueber 100%).</DetailHint> : null}
        </CockpitCard>

        <CockpitCard
          title="Top Risiken"
          status={topRiskItems.length > 0 ? 'warning' : 'success'}
          subtitle="Priorisiert nach Severity (High, Medium, Low)"
        >
          <CompactList
            columns={['Title', 'Severity', 'Status']}
            rows={topRiskItems.map((item) => [
              item.title ?? '-',
              item.severity ?? '-',
              item.status ?? '-',
            ])}
            emptyText="Keine offenen Risiken vorhanden."
          />
        </CockpitCard>

        <CockpitCard
          title="Kritische Deliverables"
          status={criticalDeliverableItems.length > 0 ? 'warning' : 'success'}
          subtitle="Priorisiert nach Ueberfaelligkeit, Review, Offen"
        >
          <CompactList
            columns={['Name', 'Status', 'DueDate']}
            rows={criticalDeliverableItems.map((item) => [
              item.name ?? '-',
              item.status ?? '-',
              formatDate(item.dueDate),
            ])}
            emptyText="Keine kritischen Deliverables vorhanden."
          />
        </CockpitCard>
      </DashboardSection>


      <DashboardSection title="Details Handlungsbedarf">
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

        <InteractiveKpiTile
          id="open-risks"
          title="Offene Risiken"
          value={formatNumber(openRisks)}
          unit="Risiken"
          status={openRisksStatus}
          description="Noch nicht akzeptierte oder geschlossene Risiken"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="critical-risks"
          title="Kritische Risiken"
          value={formatNumber(criticalRisks)}
          unit="Risiken"
          status={criticalRisksStatus}
          description="Offene Risiken mit hoher Kritikalitaet"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="open-deliverables"
          title="Offene Deliverables"
          value={formatNumber(openDeliverables)}
          unit="Deliverables"
          status={openDeliverablesStatus}
          description="Deliverables ohne Status Delivered"
          activeTile={activeTile}
          onClick={setActiveTile}
        />

        <InteractiveKpiTile
          id="overdue-deliverables"
          title="Ueberfaellige Deliverables"
          value={formatNumber(overdueDeliverables)}
          unit="Deliverables"
          status={overdueDeliverablesStatus}
          description="Faellige Deliverables mit Termin in der Vergangenheit"
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
          id="delivery-status"
          title="Lieferfaehigkeit"
          value={deliveryStatus}
          unit=""
          status={deliveryStatusUi}
          description={`${deliveryStatusReason} | Ausloeser: ${deliveryStatusTrigger}`}
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
  borderRadius: '16px',
  padding: '20px',
  backgroundColor: '#ffffff',
  cursor: 'pointer',
  boxShadow: '0 4px 12px rgba(0,0,0,0.08)',

  minHeight: '140px',

  width: '320px',
  alignSelf: 'start',

  display: 'flex',
  flexDirection: 'column',
  justifyContent: 'space-between',

  transition: 'all 0.15s ease',
}}
          >
            <div
  style={{
    fontSize: '12px',
    fontWeight: '700',
    color: '#6b7280',
    textTransform: 'uppercase',
    letterSpacing: '0.5px',
  }}
>
  Hauptpaket {node.visibleWbsId}
</div>

           <div>
  <div
    style={{
      fontWeight: '800',
      fontSize: '20px',
      color: '#111827',
      marginTop: '10px',
    }}
  >
    {node.title}
  </div>

  <div
    style={{
      marginTop: '12px',
      color: '#6b7280',
      fontSize: '13px',
    }}
  >
    Klick zum Öffnen der WBS
  </div>
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
          openRisks,
          criticalRisks,
          openDeliverables,
          overdueDeliverables,
          deliveryStatus,
          deliveryStatusReason,
          deliveryStatusTrigger,
          openRiskItems,
          criticalRiskItems,
          openDeliverableItems,
          overdueDeliverableItems,
          plannedDemandHours,
          assignedHours,
          rawOpenHours,
          capacityHours,
          utilizationPercent,
        }}
        drilldown={{
          openRiskItems: openRiskItemsFiltered,
          criticalRiskItems: criticalRiskItemsFiltered,
          openDeliverableItems: openDeliverableItemsFiltered,
          overdueDeliverableItems: overdueDeliverableItemsFiltered,
          loadingRiskItems,
          loadingDeliverableItems,
          actionBusyId,
          actionError,
        }}
        actions={{
          onRiskStatusChange: handleRiskStatusChange,
          onRiskClose: handleRiskClose,
          onDeliverableStatusChange: handleDeliverableStatusChange,
          onDeliverableMarkDelivered: handleDeliverableMarkDelivered,
        }}
      />
    </section>
  );
}

function DashboardSection({ title, children }) {
  const isMainPackages =
    title === 'WBS-Hauptpakete';

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

          gridTemplateColumns: isMainPackages
            ? 'repeat(auto-fit, minmax(220 1fr))'
            : 'repeat(auto-fit, minmax(180px, 1fr))',

          gap: isMainPackages ? '20px' : '14px',
        }}
      >
        {children}
      </div>
    </section>
  );
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
  const selected = activeTile === id;
  const config = getStatusConfig(status);

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
          fontSize: String(value).length > 12 ? '20px' : '28px',
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
  );
}

function DashboardDetailPanel({ activeTile, dashboard, drilldown, actions }) {
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
    );
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
    );
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
    );
  }

  if (activeTile === 'open') {
    const isOvercovered = dashboard.rawOpenHours < 0;
    const value = Math.abs(dashboard.rawOpenHours);

    return (
      <DetailPanel
        title="Offene oder überdeckte Ressourcenstunden"
        status={
          isOvercovered
            ? 'warning'
            : dashboard.rawOpenHours > 0
            ? 'danger'
            : 'success'
        }
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
    );
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
    );
  }

  if (activeTile === 'open-risks') {
    return (
      <DetailPanel
        title="Offene Risiken"
        status={dashboard.openRisks > 0 ? 'warning' : 'success'}
        text={
          dashboard.openRisks > 0
            ? `Aktuell sind ${formatNumber(
                dashboard.openRisks
              )} offene Risiken im Projekt vorhanden.`
            : 'Aktuell sind keine offenen Risiken im Projekt vorhanden.'
        }
      >
        <DetailRiskTable
          items={drilldown.openRiskItems}
          loading={drilldown.loadingRiskItems}
          busyId={drilldown.actionBusyId}
          onStatusChange={actions.onRiskStatusChange}
          onClose={actions.onRiskClose}
          emptyText="Keine offenen Risiken vorhanden."
        />
        {drilldown.actionError ? <DetailHint>{drilldown.actionError}</DetailHint> : null}
      </DetailPanel>
    );
  }

  if (activeTile === 'critical-risks') {
    return (
      <DetailPanel
        title="Kritische Risiken"
        status={dashboard.criticalRisks > 0 ? 'danger' : 'success'}
        text={
          dashboard.criticalRisks > 0
            ? `Aktuell sind ${formatNumber(
                dashboard.criticalRisks
              )} kritische offene Risiken im Projekt vorhanden.`
            : 'Aktuell sind keine kritischen offenen Risiken im Projekt vorhanden.'
        }
      >
        <DetailRiskTable
          items={drilldown.criticalRiskItems}
          loading={drilldown.loadingRiskItems}
          busyId={drilldown.actionBusyId}
          onStatusChange={actions.onRiskStatusChange}
          onClose={actions.onRiskClose}
          emptyText="Keine kritischen offenen Risiken vorhanden."
        />
        {drilldown.actionError ? <DetailHint>{drilldown.actionError}</DetailHint> : null}
      </DetailPanel>
    );
  }

  if (activeTile === 'open-deliverables') {
    return (
      <DetailPanel
        title="Offene Deliverables"
        status={dashboard.openDeliverables > 0 ? 'warning' : 'success'}
        text={
          dashboard.openDeliverables > 0
            ? `Aktuell sind ${formatNumber(
                dashboard.openDeliverables
              )} offene Deliverables im Projekt vorhanden.`
            : 'Aktuell sind keine offenen Deliverables im Projekt vorhanden.'
        }
      >
        <DetailDeliverableTable
          items={drilldown.openDeliverableItems}
          loading={drilldown.loadingDeliverableItems}
          busyId={drilldown.actionBusyId}
          onStatusChange={actions.onDeliverableStatusChange}
          onDeliver={actions.onDeliverableMarkDelivered}
          emptyText="Keine offenen Deliverables vorhanden."
        />
        {drilldown.actionError ? <DetailHint>{drilldown.actionError}</DetailHint> : null}
      </DetailPanel>
    );
  }

  if (activeTile === 'overdue-deliverables') {
    return (
      <DetailPanel
        title="Ueberfaellige Deliverables"
        status={dashboard.overdueDeliverables > 0 ? 'danger' : 'success'}
        text={
          dashboard.overdueDeliverables > 0
            ? `Aktuell sind ${formatNumber(
                dashboard.overdueDeliverables
              )} ueberfaellige Deliverables im Projekt vorhanden.`
            : 'Aktuell sind keine ueberfaelligen Deliverables im Projekt vorhanden.'
        }
      >
        <DetailDeliverableTable
          items={drilldown.overdueDeliverableItems}
          loading={drilldown.loadingDeliverableItems}
          busyId={drilldown.actionBusyId}
          onStatusChange={actions.onDeliverableStatusChange}
          onDeliver={actions.onDeliverableMarkDelivered}
          emptyText="Keine ueberfaelligen Deliverables vorhanden."
        />
        {drilldown.actionError ? <DetailHint>{drilldown.actionError}</DetailHint> : null}
      </DetailPanel>
    );
  }

  if (activeTile === 'delivery-status') {
    const status =
      dashboard.deliveryStatus === 'Red'
        ? 'danger'
        : dashboard.deliveryStatus === 'Yellow'
        ? 'warning'
        : 'success';

    return (
      <DetailPanel
        title="Lieferfaehigkeitsstatus"
        status={status}
        text={`Status: ${dashboard.deliveryStatus}. ${dashboard.deliveryStatusReason}`}
      >
        <DetailFact
          label="Wesentlicher Ausloeser"
          value={dashboard.deliveryStatusTrigger}
        />
      </DetailPanel>
    );
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
    );
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
    );
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
    );
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
    );
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
    );
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
    );
  }

  return null;
}

function DetailPanel({ title, status, text, children }) {
  const config = getStatusConfig(status);

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
  );
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
  );
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
  );
}

function CockpitCard({ title, status, subtitle, children }) {
  const config = getStatusConfig(status);

  return (
    <section
      style={{
        backgroundColor: '#ffffff',
        border: '1px solid #e5e7eb',
        borderTop: `5px solid ${config.accentColor}`,
        borderRadius: '14px',
        padding: '16px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
        display: 'flex',
        flexDirection: 'column',
        gap: '12px',
      }}
    >
      <div style={{ display: 'flex', justifyContent: 'space-between', gap: '10px' }}>
        <div>
          <h4 style={{ margin: 0, color: '#111827' }}>{title}</h4>
          <div style={{ marginTop: '4px', color: '#6b7280', fontSize: '13px' }}>{subtitle}</div>
        </div>
        <div style={{ fontSize: '20px' }}>{config.icon}</div>
      </div>
      {children}
    </section>
  );
}

function CockpitFact({ label, value }) {
  return (
    <div
      style={{
        display: 'flex',
        justifyContent: 'space-between',
        gap: '10px',
        border: '1px solid #e5e7eb',
        borderRadius: '10px',
        padding: '10px 12px',
        backgroundColor: '#f9fafb',
      }}
    >
      <span style={{ color: '#6b7280', fontSize: '13px' }}>{label}</span>
      <strong style={{ color: '#111827' }}>{value}</strong>
    </div>
  );
}

function CompactList({ columns, rows, emptyText }) {
  if (!rows || rows.length === 0) {
    return <DetailHint>{emptyText}</DetailHint>;
  }

  return (
    <div style={{ overflowX: 'auto', border: '1px solid #e5e7eb', borderRadius: '10px' }}>
      <table style={{ width: '100%', borderCollapse: 'collapse', fontSize: '13px' }}>
        <thead>
          <tr style={{ backgroundColor: '#f9fafb' }}>
            {columns.map((column) => (
              <th
                key={column}
                style={{
                  textAlign: 'left',
                  padding: '8px 10px',
                  color: '#374151',
                  borderBottom: '1px solid #e5e7eb',
                }}
              >
                {column}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {rows.map((row, rowIndex) => (
            <tr key={`compact-row-${rowIndex}`}>
              {row.map((cell, cellIndex) => (
                <td
                  key={`compact-row-${rowIndex}-cell-${cellIndex}`}
                  style={{
                    padding: '8px 10px',
                    color: '#111827',
                    borderBottom:
                      rowIndex === rows.length - 1 ? 'none' : '1px solid #f3f4f6',
                  }}
                >
                  {cell}
                </td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function DetailRiskTable({ items, loading, busyId, onStatusChange, onClose, emptyText }) {
  if (loading) {
    return <DetailHint>Risiken werden geladen...</DetailHint>;
  }

  if (!items || items.length === 0) {
    return <DetailHint>{emptyText}</DetailHint>;
  }

  return (
    <div
      style={{
        overflowX: 'auto',
        border: '1px solid #e5e7eb',
        borderRadius: '12px',
      }}
    >
      <table
        style={{
          width: '100%',
          borderCollapse: 'collapse',
          fontSize: '14px',
        }}
      >
        <thead>
          <tr style={{ backgroundColor: '#f9fafb' }}>
            {['Title', 'Severity', 'Status', 'OwnerPersonId', 'Aktionen'].map((column) => (
              <th
                key={column}
                style={{
                  textAlign: 'left',
                  padding: '10px 12px',
                  color: '#374151',
                  borderBottom: '1px solid #e5e7eb',
                }}
              >
                {column}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {items.map((item, rowIndex) => (
            <tr key={item.id ?? `risk-row-${rowIndex}`}>
              <td style={tableCellStyle(rowIndex, items.length)}>{item.title ?? '-'}</td>
              <td style={tableCellStyle(rowIndex, items.length)}>
                {RISK_SEVERITY[Number(item.severity)] ?? item.severity ?? '-'}
              </td>
              <td style={tableCellStyle(rowIndex, items.length)}>
                {RISK_STATUS[Number(item.status)] ?? item.status ?? '-'}
              </td>
              <td style={tableCellStyle(rowIndex, items.length)}>{item.ownerPersonId ?? '-'}</td>
              <td style={tableCellStyle(rowIndex, items.length)}>
                <div style={{ display: 'flex', gap: '8px', alignItems: 'center', flexWrap: 'wrap' }}>
                  <select
                    value={String(item.status ?? '')}
                    onChange={(event) => onStatusChange(item.id, event.target.value)}
                    disabled={busyId === `risk-status-${item.id}`}
                    style={actionSelectStyle}
                  >
                    {Object.entries(RISK_STATUS).map(([value, label]) => (
                      <option key={value} value={value}>
                        {label}
                      </option>
                    ))}
                  </select>

                  <button
                    type="button"
                    onClick={() => onClose(item.id)}
                    disabled={busyId === `risk-close-${item.id}` || Number(item.status) === 6}
                    style={actionButtonStyle}
                  >
                    {busyId === `risk-close-${item.id}` ? '...' : 'Close'}
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function DetailDeliverableTable({ items, loading, busyId, onStatusChange, onDeliver, emptyText }) {
  if (loading) {
    return <DetailHint>Deliverables werden geladen...</DetailHint>;
  }

  if (!items || items.length === 0) {
    return <DetailHint>{emptyText}</DetailHint>;
  }

  return (
    <div
      style={{
        overflowX: 'auto',
        border: '1px solid #e5e7eb',
        borderRadius: '12px',
      }}
    >
      <table
        style={{
          width: '100%',
          borderCollapse: 'collapse',
          fontSize: '14px',
        }}
      >
        <thead>
          <tr style={{ backgroundColor: '#f9fafb' }}>
            {['Name', 'Type', 'Status', 'DueDate', 'Aktionen'].map((column) => (
              <th
                key={column}
                style={{
                  textAlign: 'left',
                  padding: '10px 12px',
                  color: '#374151',
                  borderBottom: '1px solid #e5e7eb',
                }}
              >
                {column}
              </th>
            ))}
          </tr>
        </thead>
        <tbody>
          {items.map((item, rowIndex) => (
            <tr key={item.id ?? `deliverable-row-${rowIndex}`}>
              <td style={tableCellStyle(rowIndex, items.length)}>{item.name ?? '-'}</td>
              <td style={tableCellStyle(rowIndex, items.length)}>
                {DELIVERABLE_TYPE[Number(item.type)] ?? item.type ?? '-'}
              </td>
              <td style={tableCellStyle(rowIndex, items.length)}>
                {DELIVERABLE_STATUS[Number(item.status)] ?? item.status ?? '-'}
              </td>
              <td style={tableCellStyle(rowIndex, items.length)}>{formatDate(item.dueDate)}</td>
              <td style={tableCellStyle(rowIndex, items.length)}>
                <div style={{ display: 'flex', gap: '8px', alignItems: 'center', flexWrap: 'wrap' }}>
                  <select
                    value={String(item.status ?? '')}
                    onChange={(event) => onStatusChange(item.id, event.target.value)}
                    disabled={busyId === `deliverable-status-${item.id}`}
                    style={actionSelectStyle}
                  >
                    {Object.entries(DELIVERABLE_STATUS).map(([value, label]) => (
                      <option key={value} value={value}>
                        {label}
                      </option>
                    ))}
                  </select>

                  <button
                    type="button"
                    onClick={() => onDeliver(item.id)}
                    disabled={
                      busyId === `deliverable-deliver-${item.id}` || Number(item.status) === 5
                    }
                    style={actionButtonStyle}
                  >
                    {busyId === `deliverable-deliver-${item.id}` ? '...' : 'Deliver'}
                  </button>
                </div>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}

function tableCellStyle(rowIndex, totalRows) {
  return {
    padding: '10px 12px',
    borderBottom: rowIndex === totalRows - 1 ? 'none' : '1px solid #f3f4f6',
    color: '#111827',
    verticalAlign: 'top',
  };
}

const actionSelectStyle = {
  border: '1px solid #d1d5db',
  borderRadius: '8px',
  padding: '6px 8px',
  backgroundColor: '#ffffff',
};

const actionButtonStyle = {
  border: '1px solid #d1d5db',
  borderRadius: '8px',
  padding: '6px 10px',
  backgroundColor: '#f9fafb',
  cursor: 'pointer',
};

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
  };

  return configs[status] ?? configs.neutral;
}

function formatNumber(value) {
  if (value === null || value === undefined || Number.isNaN(Number(value))) {
    return '0';
  }

  return new Intl.NumberFormat('de-DE', {
    maximumFractionDigits: 2,
  }).format(Number(value));
}

function formatDate(value) {
  if (!value) {
    return '-';
  }

  const raw = String(value);

  if (/^\d{4}-\d{2}-\d{2}$/.test(raw)) {
    const [year, month, day] = raw.split('-');
    return `${day}.${month}.${year}`;
  }

  const parsed = new Date(raw);

  if (Number.isNaN(parsed.getTime())) {
    return raw;
  }

  return new Intl.DateTimeFormat('de-DE').format(parsed);
}

function isDateBeforeToday(value) {
  if (!value) {
    return false;
  }

  const today = new Date();
  const todayKey = `${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}-${String(
    today.getDate()
  ).padStart(2, '0')}`;
  const dateKey = String(value).slice(0, 10);

  return dateKey < todayKey;
}

async function loadRiskItems(projectId, setRiskItems, setLoadingRiskItems, setActionError) {
  if (!projectId) {
    setRiskItems([]);
    return;
  }

  setLoadingRiskItems(true);

  try {
    const data = await getProjectRisks(projectId);
    const normalized = Array.isArray(data)
      ? data.map((item) => ({
          ...item,
          status: parseEnumValue(item.status, RISK_STATUS),
          severity: parseEnumValue(item.severity, RISK_SEVERITY),
        }))
      : [];

    setRiskItems(normalized);
  } catch (error) {
    setActionError(error?.message || 'Risiken konnten nicht geladen werden.');
  } finally {
    setLoadingRiskItems(false);
  }
}

async function loadDeliverableItems(
  projectId,
  setDeliverableItems,
  setLoadingDeliverableItems,
  setActionError
) {
  if (!projectId) {
    setDeliverableItems([]);
    return;
  }

  setLoadingDeliverableItems(true);

  try {
    const data = await getProjectDeliverables(projectId);
    const normalized = Array.isArray(data)
      ? data.map((item) => ({
          ...item,
          status: parseEnumValue(item.status, DELIVERABLE_STATUS),
          type: parseEnumValue(item.type, DELIVERABLE_TYPE),
        }))
      : [];

    setDeliverableItems(normalized);
  } catch (error) {
    setActionError(error?.message || 'Deliverables konnten nicht geladen werden.');
  } finally {
    setLoadingDeliverableItems(false);
  }
}

function parseEnumValue(value, map) {
  if (typeof value === 'number') {
    return value;
  }

  const entry = Object.entries(map).find(([, label]) => label === value);

  return entry ? Number(entry[0]) : value;
}

function attentionItemColor(priorityScore) {
  if (priorityScore >= 90) {
    return { border: '#ef4444', background: '#fef2f2', badge: '#ef4444', badgeText: '#fff' };
  }
  if (priorityScore >= 70) {
    return { border: '#f59e0b', background: '#fffbeb', badge: '#f59e0b', badgeText: '#fff' };
  }
  return { border: '#22c55e', background: '#f0fdf4', badge: '#22c55e', badgeText: '#fff' };
}

function AttentionItemCard({ item }) {
  const score = item.priorityScore ?? 0;
  const colors = attentionItemColor(score);

  return (
    <div
      style={{
        border: `1px solid ${colors.border}`,
        borderLeft: `6px solid ${colors.border}`,
        borderRadius: '12px',
        padding: '16px',
        backgroundColor: colors.background,
        display: 'flex',
        flexDirection: 'column',
        gap: '8px',
        minWidth: '0',
      }}
    >
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', gap: '8px' }}>
        <span
          style={{
            fontSize: '11px',
            fontWeight: '700',
            textTransform: 'uppercase',
            color: '#6b7280',
            letterSpacing: '0.5px',
          }}
        >
          {item.type ?? '-'}
        </span>
        <span
          style={{
            backgroundColor: colors.badge,
            color: colors.badgeText,
            fontSize: '12px',
            fontWeight: '700',
            borderRadius: '6px',
            padding: '2px 8px',
            whiteSpace: 'nowrap',
          }}
        >
          {score}
        </span>
      </div>

      <div style={{ fontWeight: '700', fontSize: '15px', color: '#111827', lineHeight: '1.3' }}>
        {item.title ?? '-'}
      </div>

      <div style={{ fontSize: '13px', color: '#374151', lineHeight: '1.45' }}>
        {item.explanation ?? '-'}
      </div>

      <div
        style={{
          fontSize: '13px',
          color: '#1d4ed8',
          fontWeight: '600',
        }}
      >
        &#8594; {item.suggestedAction ?? '-'}
      </div>

      <div style={{ display: 'flex', flexWrap: 'wrap', gap: '12px', marginTop: '4px' }}>
        {item.reactionDate ? (
          <span style={{ fontSize: '12px', color: '#6b7280' }}>
            Reaktion bis: <strong>{formatDate(item.reactionDate)}</strong>
          </span>
        ) : null}
        {item.suggestedOwnerPersonId ? (
          <span style={{ fontSize: '12px', color: '#6b7280' }}>
            Vorgeschl. Owner: <strong>{item.suggestedOwnerPersonId}</strong>
          </span>
        ) : null}
      </div>
    </div>
  );
}