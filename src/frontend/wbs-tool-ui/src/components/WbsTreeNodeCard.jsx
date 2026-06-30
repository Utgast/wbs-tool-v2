import { useDroppable } from '@dnd-kit/core'
import { useEffect, useMemo, useRef, useState } from 'react'
import { canCreateUnder, WBS_NODE_TYPES } from '../utils/wbsRules'

function formatHours(value) {
  if (value === null || value === undefined || value === '') {
    return null
  }

  const numberValue = Number(value)

  if (Number.isNaN(numberValue)) {
    return null
  }

  return `${numberValue.toLocaleString('de-DE', {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  })} h`
}

function getNodeVariantClass(type) {
  if (type === WBS_NODE_TYPES.MAIN_PACKAGE) {
    return 'tree-node-main-package'
  }

  if (type === WBS_NODE_TYPES.SUB_PACKAGE) {
    return 'tree-node-sub-package'
  }

  return 'tree-node-task'
}

function getNodeTypeLabel(type) {
  if (type === WBS_NODE_TYPES.MAIN_PACKAGE) {
    return 'Hauptpaket'
  }

  if (type === WBS_NODE_TYPES.SUB_PACKAGE) {
    return 'Unterpaket'
  }

  if (type === WBS_NODE_TYPES.TASK) {
    return 'Aufgabe'
  }

  return type || 'Element'
}

function getNodeLevelLabel(type) {
  if (type === WBS_NODE_TYPES.MAIN_PACKAGE) {
    return 'Container'
  }

  if (type === WBS_NODE_TYPES.SUB_PACKAGE) {
    return 'Gliederung'
  }

  if (type === WBS_NODE_TYPES.TASK) {
    return 'Arbeitspaket'
  }

  return 'WBS-Element'
}

function WbsTreeNodeCard({
  node,
  isSelected,
  level,
  onSelect,
  onEditNode,
  onDeactivateNode,
  activeDragTemplateType,
}) {
  const [isMenuOpen, setIsMenuOpen] = useState(false)
  const menuRef = useRef(null)

  const isDropAllowed = activeDragTemplateType
    ? canCreateUnder(activeDragTemplateType, node)
    : false

  const { isOver, setNodeRef } = useDroppable({
    id: `node-drop-${node.id}`,
    data: {
      kind: 'node',
      nodeId: node.id,
      nodeType: node.type,
    },
    disabled: !isDropAllowed,
  })

  useEffect(() => {
    function handleDocumentClick(event) {
      if (!menuRef.current) {
        return
      }

      if (!menuRef.current.contains(event.target)) {
        setIsMenuOpen(false)
      }
    }

    if (isMenuOpen) {
      document.addEventListener('mousedown', handleDocumentClick)
    }

    return () => {
      document.removeEventListener('mousedown', handleDocumentClick)
    }
  }, [isMenuOpen])

  const plannedHours = formatHours(node.plannedHoursTotal ?? node.plannedHours)
  const actualHours = formatHours(node.actualHoursTotal ?? node.actualHours)
  const showMetaRow = plannedHours || actualHours || node.isBlocked

  const variantClass = useMemo(() => getNodeVariantClass(node.type), [node.type])
  const typeLabel = useMemo(() => getNodeTypeLabel(node.type), [node.type])
  const levelLabel = useMemo(() => getNodeLevelLabel(node.type), [node.type])

  const className = [
    'tree-node-card',
    variantClass,
    isSelected ? 'selected' : '',
    activeDragTemplateType && isDropAllowed ? 'drop-allowed' : '',
    activeDragTemplateType && !isDropAllowed ? 'drop-blocked' : '',
    isOver && isDropAllowed ? 'drop-over' : '',
  ]
    .filter(Boolean)
    .join(' ')

  function handleMenuToggle(event) {
    event.preventDefault()
    event.stopPropagation()
    setIsMenuOpen((prev) => !prev)
  }

  function handleEditClick(event) {
    event.preventDefault()
    event.stopPropagation()
    setIsMenuOpen(false)
    onEditNode(node)
  }

  function handleDeactivateClick(event) {
    event.preventDefault()
    event.stopPropagation()
    setIsMenuOpen(false)
    onDeactivateNode(node)
  }

  return (
    <div
      ref={setNodeRef}
      className={className}
      data-level={level}
      data-node-type={node.type}
    >
      <button
        type="button"
        className="tree-node-select-button"
        onClick={() => onSelect(node)}
      >
        <span className="tree-node-indent-indicator" aria-hidden="true" />

        <span className="tree-node-content-block">
          <span className="tree-node-header-row">
            <span className="tree-node-wbs-badge">
              {node.visibleWbsId || '-'}
            </span>

            <span className="tree-node-type-badge">
              {typeLabel}
            </span>

            <span className="tree-node-level-label">
              {levelLabel}
            </span>
          </span>

          <span className="tree-node-title-text">
            {node.title || '(ohne Titel)'}
          </span>

          {showMetaRow && (
            <span className="tree-node-meta-row">
              {plannedHours && (
                <span className="tree-node-meta-pill">
                  <span>Plan</span>
                  <strong>{plannedHours}</strong>
                </span>
              )}

              {actualHours && (
                <span className="tree-node-meta-pill">
                  <span>Ist</span>
                  <strong>{actualHours}</strong>
                </span>
              )}

              {node.isBlocked && (
                <span className="tree-node-meta-pill tree-node-meta-pill-blocked">
                  Blockiert
                </span>
              )}
            </span>
          )}
        </span>
      </button>

      <div className="tree-node-actions" ref={menuRef}>
        <button
          type="button"
          className="tree-node-actions-button"
          onClick={handleMenuToggle}
          aria-label={`Aktionen für ${node.title || 'Knoten'}`}
          aria-expanded={isMenuOpen}
        >
          ⋯
        </button>

        {isMenuOpen && (
          <div className="tree-node-menu">
            <button
              type="button"
              className="tree-node-menu-item"
              onClick={handleEditClick}
            >
              Bearbeiten
            </button>

            <button
              type="button"
              className="tree-node-menu-item tree-node-menu-item-danger"
              onClick={handleDeactivateClick}
            >
              Deaktivieren
            </button>
          </div>
        )}
      </div>
    </div>
  )
}

export default WbsTreeNodeCard