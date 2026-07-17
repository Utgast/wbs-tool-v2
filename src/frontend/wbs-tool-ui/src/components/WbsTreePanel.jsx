import RootDropZone from './RootDropZone'
import WbsTree from './WbsTree'

function WbsTreePanel({
  selectedProject,
  loadingWorkspace,
  wbsTree,
  selectedNodeId,
  totalNodeCount,
  onSelectNode,
  onEditNode,
  onDeactivateNode,
  activeDragTemplateType,
}) {
  return (
    <section className="panel wbs-tree-panel">
      <div className="wbs-tree-panel-header">
        <div>
          <h2>WBS-Struktur</h2>
          <p>
            Hierarchische Projektstruktur mit Hauptpaketen, Unterpaketen und Aufgaben.
          </p>
        </div>

        <div className="tree-summary">
          <span>Knoten gesamt</span>
          <strong>{totalNodeCount}</strong>
        </div>
      </div>

      {selectedProject && (
        <RootDropZone activeDragTemplateType={activeDragTemplateType} />
      )}

      {!selectedProject ? (
        <div className="empty-state">
          <p>Bitte zuerst ein Projekt auswählen.</p>
        </div>
      ) : loadingWorkspace ? (
        <div className="empty-state">
          <p>Lade WBS...</p>
        </div>
      ) : wbsTree.length === 0 ? (
        <div className="empty-state">
          <p>Diese WBS ist noch leer.</p>
          <p>Lege links ein Hauptpaket an, um zu starten.</p>
        </div>
      ) : (
        <div className="wbs-tree-scroll-area">
          <WbsTree
            nodes={wbsTree}
            selectedNodeId={selectedNodeId}
            onSelectNode={onSelectNode}
            onEditNode={onEditNode}
            onDeactivateNode={onDeactivateNode}
            activeDragTemplateType={activeDragTemplateType}
          />
        </div>
      )}
    </section>
  )
}

export default WbsTreePanel