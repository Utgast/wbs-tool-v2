import { useDraggable } from '@dnd-kit/core'
import { WBS_NODE_TYPES } from '../utils/wbsRules'

function DraggableTemplateButton({
  templateType,
  label,
  disabled,
  onClick,
}) {
  const { attributes, listeners, setNodeRef, transform, isDragging } = useDraggable({
    id: `template-${templateType}`,
    data: {
      kind: 'template',
      templateType,
    },
    disabled,
  })

  const style = transform
    ? {
        transform: `translate3d(${transform.x}px, ${transform.y}px, 0)`,
        zIndex: 1000,
      }
    : undefined

  return (
    <button
      ref={setNodeRef}
      type="button"
      className={`template-button ${isDragging ? 'dragging' : ''}`}
      onClick={onClick}
      disabled={disabled}
      style={style}
      {...listeners}
      {...attributes}
    >
      {label}
    </button>
  )
}

function getTargetText(selectedNode) {
  if (!selectedNode) {
    return {
      title: 'Kein Zielknoten ausgewählt.',
      description: 'Hauptpakete werden auf oberster Ebene angelegt.',
    }
  }

  return {
    title: `Neues Element wird angelegt unter: ${selectedNode.visibleWbsId || ''} ${selectedNode.title}`.trim(),
    description: 'Unterpakete und Aufgaben werden unter dem ausgewählten Knoten erstellt.',
  }
}

function TemplatePalette({ selectedProject, selectedNode, onCreateFromTemplate }) {
  const targetText = getTargetText(selectedNode)

  return (
    <section className="panel">
      <h2>Neues Element</h2>

      <div className="palette-group">
        <div className="palette-group-header">
          <h3>Direkt anlegen</h3>
          <p>Klicke auf ein Element, um es in der WBS anzulegen.</p>
        </div>

        <div className="template-list">
          <DraggableTemplateButton
            templateType={WBS_NODE_TYPES.MAIN_PACKAGE}
            label="+ Hauptpaket"
            disabled={!selectedProject}
            onClick={() => onCreateFromTemplate(WBS_NODE_TYPES.MAIN_PACKAGE)}
          />

          <DraggableTemplateButton
            templateType={WBS_NODE_TYPES.SUB_PACKAGE}
            label="+ Unterpaket"
            disabled={!selectedProject}
            onClick={() => onCreateFromTemplate(WBS_NODE_TYPES.SUB_PACKAGE)}
          />

          <DraggableTemplateButton
            templateType={WBS_NODE_TYPES.TASK}
            label="+ Aufgabe"
            disabled={!selectedProject}
            onClick={() => onCreateFromTemplate(WBS_NODE_TYPES.TASK)}
          />
        </div>
      </div>

      <div className="palette-divider" />

      <div className="palette-group palette-group-secondary">
        <div className="palette-group-header">
          <h3>Per Drag & Drop</h3>
          <p>Ziehe ein Element in die Struktur, wenn du schneller arbeiten möchtest.</p>
        </div>
      </div>

      <div className="selection-hint">
        <strong>{targetText.title}</strong>
        <div className="selection-hint-description">{targetText.description}</div>
      </div>
    </section>
  )
}

export default TemplatePalette