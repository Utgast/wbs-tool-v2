import { useState } from 'react'
import WbsTreeNodeCard from './WbsTreeNodeCard'

function WbsTree({
  nodes,
  selectedNodeId,
  onSelectNode,
  onEditNode,
  onDeactivateNode,
  activeDragTemplateType,
  level = 0,
}) {
  const [expandedNodes, setExpandedNodes] = useState({})
  const [selectedRootNodeId, setSelectedRootNodeId] = useState(null)

  if (!Array.isArray(nodes) || nodes.length === 0) {
    return null
  }

  function toggleNode(nodeId) {
  setExpandedNodes((prev) => ({
  ...prev,
  [nodeId]: !(prev[nodeId] ?? true),
}))
}
  

  function isNodeExpanded(nodeId) {
    return expandedNodes[nodeId] ?? true
  }

  function countChildren(node) {
    if (!Array.isArray(node.children) || node.children.length === 0) {
      return 0
    }

    return node.children.reduce(
      (sum, child) => sum + 1 + countChildren(child),
      0
    )
  }

  
function getSelectedRootNode() {
  if (level !== 0) {
    return null
  }

  const selectedFromDashboard = nodes.find(
    (node) => node.id === selectedNodeId
  )

  if (selectedFromDashboard) {
    return selectedFromDashboard
  }

  if (selectedRootNodeId) {
    const selectedNode = nodes.find(
      (node) => node.id === selectedRootNodeId
    )

    if (selectedNode) {
      return selectedNode
    }
  }

  return nodes[0]
}


  if (level === 0) {
    const selectedRootNode = getSelectedRootNode()
    const selectedRootExpanded = selectedRootNode
      ? isNodeExpanded(selectedRootNode.id)
      : false

    return (
      <div
        className="wbs-tree-master-detail"
        style={{
          display: 'grid',
          gridTemplateColumns: 'minmax(280px, 360px) 1fr',
          gap: '20px',
          alignItems: 'start',
        }}
      >
        <aside
          style={{
            backgroundColor: '#ffffff',
            border: '1px solid #e5e7eb',
            borderRadius: '14px',
            padding: '16px',
            boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
          }}
        >
          <div
            style={{
              marginBottom: '14px',
            }}
          >
            <h3
              style={{
                margin: 0,
                color: '#111827',
              }}
            >
              Hauptpakete
            </h3>

            <p
              style={{
                margin: '6px 0 0 0',
                color: '#6b7280',
                fontSize: '13px',
              }}
            >
              Großes Paket auswählen und rechts bearbeiten.
            </p>
          </div>

          <div
            className="wbs-root-card-list"
            role="tree"
            style={{
              display: 'flex',
              flexDirection: 'column',
              gap: '12px',
            }}
          >
            {nodes.map((node) => {
              const isSelectedRoot =
                selectedRootNode && selectedRootNode.id === node.id

              const hasChildren =
                Array.isArray(node.children) &&
                node.children.length > 0

              const isExpanded = isNodeExpanded(node.id)
              const childCount = countChildren(node)

              return (
                <div
                  key={node.id}
                  role="treeitem"
                  aria-selected={isSelectedRoot}
                  aria-expanded={hasChildren ? isExpanded : undefined}
                  style={{
                    borderRadius: '14px',
                    border: isSelectedRoot
                      ? '2px solid #F58220'
                      : '1px solid #d1d5db',
                    backgroundColor: isSelectedRoot
                      ? '#fff7ed'
                      : '#ffffff',
                    boxShadow: isSelectedRoot
                      ? '0 4px 12px rgba(245,130,32,0.22)'
                      : '0 1px 3px rgba(0,0,0,0.06)',
                    overflow: 'hidden',
                  }}
                >
                  <button
                    type="button"
                    onClick={() => setSelectedRootNodeId(node.id)}
                    style={{
                      width: '100%',
                      border: 'none',
                      background: 'transparent',
                      cursor: 'pointer',
                      textAlign: 'left',
                      padding: '16px',
                    }}
                  >
                    <div
                      style={{
                        display: 'flex',
                        justifyContent: 'space-between',
                        alignItems: 'flex-start',
                        gap: '12px',
                      }}
                    >
                      <div style={{ minWidth: 0 }}>
                        <div
                          style={{
                            fontWeight: '800',
                            color: '#111827',
                            marginBottom: '6px',
                            overflow: 'hidden',
                            textOverflow: 'ellipsis',
                            whiteSpace: 'nowrap',
                          }}
                        >
                          {node.name ?? node.title ?? 'Unbenanntes Hauptpaket'}
                        </div>

                        <div
                          style={{
                            display: 'flex',
                            flexWrap: 'wrap',
                            gap: '6px',
                          }}
                        >
                          <Badge label={`Ebene ${level + 1}`} />

                          <Badge
                            label={`${childCount} Elemente`}
                            variant="orange"
                          />

                          {hasChildren && (
                            <Badge
                              label={isExpanded ? 'geöffnet' : 'eingeklappt'}
                              variant={isExpanded ? 'green' : 'gray'}
                            />
                          )}
                        </div>
                      </div>

                      {hasChildren && (
                        <span
                          style={{
                            fontSize: '18px',
                            color: isSelectedRoot ? '#F58220' : '#6b7280',
                            fontWeight: '800',
                          }}
                        >
                          {isExpanded ? '▼' : '▶'}
                        </span>
                      )}
                    </div>
                  </button>

                  {hasChildren && (
                    <button
                      type="button"
                      onClick={(event) => {
                        event.stopPropagation()
                        toggleNode(node.id)
                      }}
                      style={{
                        width: '100%',
                        border: 'none',
                        borderTop: '1px solid #e5e7eb',
                        backgroundColor: isExpanded ? '#f9fafb' : '#fff7ed',
                        padding: '10px 16px',
                        cursor: 'pointer',
                        color: isExpanded ? '#374151' : '#c2410c',
                        fontWeight: '700',
                        textAlign: 'left',
                      }}
                    >
                      {isExpanded ? 'Hauptpaket einklappen' : 'Hauptpaket ausklappen'}
                    </button>
                  )}
                </div>
              )
            })}
          </div>
        </aside>

        <section
          style={{
            backgroundColor: '#ffffff',
            border: '1px solid #e5e7eb',
            borderRadius: '14px',
            padding: '18px',
            boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
            minHeight: '280px',
          }}
        >
          {!selectedRootNode && (
            <div
              style={{
                color: '#6b7280',
              }}
            >
              Kein Hauptpaket vorhanden.
            </div>
          )}

          {selectedRootNode && (
            <>
              <div
                style={{
                  marginBottom: '16px',
                  paddingBottom: '14px',
                  borderBottom: '1px solid #e5e7eb',
                }}
              >
                <div
                  style={{
                    display: 'flex',
                    justifyContent: 'space-between',
                    gap: '12px',
                    alignItems: 'center',
                    flexWrap: 'wrap',
                    marginBottom: '12px',
                  }}
                >
                  <div>
                    <h3
                      style={{
                        margin: 0,
                        color: '#111827',
                      }}
                    >
                      Ausgewähltes Hauptpaket
                    </h3>

                    <p
                      style={{
                        margin: '6px 0 0 0',
                        color: '#6b7280',
                        fontSize: '13px',
                      }}
                    >
                      Detailansicht mit Unterpaketen und Aufgaben.
                    </p>
                  </div>

                  {Array.isArray(selectedRootNode.children) &&
                    selectedRootNode.children.length > 0 && (
                      <button
                        type="button"
                        onClick={() => toggleNode(selectedRootNode.id)}
                        style={{
                          border: 'none',
                          borderRadius: '10px',
                          padding: '10px 14px',
                          backgroundColor: selectedRootExpanded
                            ? '#f3f4f6'
                            : '#F58220',
                          color: selectedRootExpanded
                            ? '#374151'
                            : '#ffffff',
                          cursor: 'pointer',
                          fontWeight: '700',
                        }}
                      >
                        {selectedRootExpanded
                          ? 'Inhalt ausblenden'
                          : 'Inhalt anzeigen'}
                      </button>
                    )}
                </div>

                <WbsTreeNodeCard
                  node={selectedRootNode}
                  isSelected={selectedNodeId === selectedRootNode.id}
                  level={0}
                  onSelect={onSelectNode}
                  onEditNode={onEditNode}
                  onDeactivateNode={onDeactivateNode}
                  activeDragTemplateType={activeDragTemplateType}
                />
              </div>

              {!selectedRootExpanded && (
                <div
                  style={{
                    padding: '28px',
                    borderRadius: '14px',
                    border: '1px dashed #d1d5db',
                    backgroundColor: '#f9fafb',
                    textAlign: 'center',
                    color: '#6b7280',
                  }}
                >
                  Dieses Hauptpaket ist eingeklappt.
                  <br />
                  Klicke auf „Inhalt anzeigen“, um Unterpakete und Aufgaben zu
                  sehen.
                </div>
              )}

              {selectedRootExpanded &&
                Array.isArray(selectedRootNode.children) &&
                selectedRootNode.children.length > 0 && (
                  <div className="wbs-tree-children">
                    <WbsTree
                      nodes={selectedRootNode.children}
                      selectedNodeId={selectedNodeId}
                      onSelectNode={onSelectNode}
                      onEditNode={onEditNode}
                      onDeactivateNode={onDeactivateNode}
                      activeDragTemplateType={activeDragTemplateType}
                      level={1}
                    />
                  </div>
                )}

              {selectedRootExpanded &&
                (!Array.isArray(selectedRootNode.children) ||
                  selectedRootNode.children.length === 0) && (
                  <div
                    style={{
                      padding: '24px',
                      borderRadius: '14px',
                      border: '1px dashed #d1d5db',
                      backgroundColor: '#f9fafb',
                      color: '#6b7280',
                    }}
                  >
                    Dieses Hauptpaket enthält aktuell keine Unterpakete oder
                    Aufgaben.
                  </div>
                )}
            </>
          )}
        </section>
      </div>
    )
  }

  return (
    <div
      className="wbs-tree-list"
      role="group"
      style={{
        display: 'flex',
        flexDirection: 'column',
        gap: '10px',
      }}
    >
      {nodes.map((node) => {
        const isSelected = selectedNodeId === node.id

        const hasChildren =
          Array.isArray(node.children) &&
          node.children.length > 0

        const isExpanded = isNodeExpanded(node.id)

        return (
          <div
            key={node.id}
            className={`wbs-tree-item level-${level}`}
            role="treeitem"
            aria-selected={isSelected}
            aria-expanded={hasChildren ? isExpanded : undefined}
            style={{
              marginLeft: level > 1 ? '18px' : 0,
            }}
          >
            <div
              style={{
                display: 'flex',
                alignItems: 'stretch',
                gap: '8px',
              }}
            >
              {hasChildren ? (
                <button
                  type="button"
                  onClick={() => toggleNode(node.id)}
                  style={{
                    width: '30px',
                    minWidth: '30px',
                    border: '1px solid #d1d5db',
                    borderRadius: '8px',
                    backgroundColor: '#ffffff',
                    cursor: 'pointer',
                    fontSize: '12px',
                    color: '#374151',
                  }}
                  title={isExpanded ? 'Einklappen' : 'Ausklappen'}
                >
                  {isExpanded ? '▼' : '▶'}
                </button>
              ) : (
                <div style={{ width: '30px', minWidth: '30px' }} />
              )}

              <div style={{ flex: 1 }}>
                <WbsTreeNodeCard
                  node={node}
                  isSelected={isSelected}
                  level={level}
                  onSelect={onSelectNode}
                  onEditNode={onEditNode}
                  onDeactivateNode={onDeactivateNode}
                  activeDragTemplateType={activeDragTemplateType}
                />
              </div>
            </div>

            {hasChildren && isExpanded && (
              <div
                className="wbs-tree-children"
                style={{
                  marginTop: '8px',
                  marginLeft: '30px',
                  paddingLeft: '12px',
                  borderLeft: '2px solid #e5e7eb',
                }}
              >
                <WbsTree
                  nodes={node.children}
                  selectedNodeId={selectedNodeId}
                  onSelectNode={onSelectNode}
                  onEditNode={onEditNode}
                  onDeactivateNode={onDeactivateNode}
                  activeDragTemplateType={activeDragTemplateType}
                  level={level + 1}
                />
              </div>
            )}
          </div>
        )
      })}
    </div>
  )
}

function Badge({ label, variant = 'gray' }) {
  const styles = {
    gray: {
      backgroundColor: '#f3f4f6',
      color: '#374151',
      borderColor: '#e5e7eb',
    },
    orange: {
      backgroundColor: '#fff7ed',
      color: '#c2410c',
      borderColor: '#fed7aa',
    },
    green: {
      backgroundColor: '#dcfce7',
      color: '#166534',
      borderColor: '#86efac',
    },
  }

  const style = styles[variant] ?? styles.gray

  return (
    <span
      style={{
        display: 'inline-flex',
        alignItems: 'center',
        borderRadius: '999px',
        padding: '3px 8px',
        fontSize: '12px',
        fontWeight: '700',
        backgroundColor: style.backgroundColor,
        color: style.color,
        border: `1px solid ${style.borderColor}`,
      }}
    >
      {label}
    </span>
  )
}

export default WbsTree