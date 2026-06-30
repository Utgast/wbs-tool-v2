import { useDroppable } from '@dnd-kit/core'
import { WBS_NODE_TYPES } from '../utils/wbsRules'

function RootDropZone({ activeDragTemplateType }) {
  const isMainPackageDrag = activeDragTemplateType === WBS_NODE_TYPES.MAIN_PACKAGE
  const isSubPackageDrag = activeDragTemplateType === WBS_NODE_TYPES.SUB_PACKAGE
  const isTaskDrag = activeDragTemplateType === WBS_NODE_TYPES.TASK

  const { isOver, setNodeRef } = useDroppable({
    id: 'root-drop-zone',
    data: {
      kind: 'root',
    },
    disabled: !isMainPackageDrag,
  })

  const className = [
    'root-drop-zone',
    isMainPackageDrag ? 'active' : '',
    isOver ? 'over' : '',
    (isSubPackageDrag || isTaskDrag) ? 'blocked' : '',
  ]
    .filter(Boolean)
    .join(' ')

  let title = '+ Hauptpaket hier ablegen'
  let subtitle = 'oder links per Klick erstellen'

  if (isMainPackageDrag && !isOver) {
    title = '+ Hauptpaket hier ablegen'
    subtitle = 'auf oberster Ebene anlegen'
  }

  if (isMainPackageDrag && isOver) {
    title = 'Jetzt loslassen'
    subtitle = 'Hauptpaket wird auf oberster Ebene erstellt'
  }

  if (isSubPackageDrag) {
    title = 'Unterpaket hier nicht möglich'
    subtitle = 'Unterpakete können nur unter Hauptpaketen oder Unterpaketen angelegt werden'
  }

  if (isTaskDrag) {
    title = 'Aufgabe hier nicht möglich'
    subtitle = 'Aufgaben können nur unter Hauptpaketen oder Unterpaketen angelegt werden'
  }

  return (
    <div ref={setNodeRef} className={className}>
      <span className="root-drop-zone-title">{title}</span>
      <span className="root-drop-zone-subtitle">{subtitle}</span>
    </div>
  )
}

export default RootDropZone