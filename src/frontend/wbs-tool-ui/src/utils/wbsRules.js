export const WBS_NODE_TYPES = {
  MAIN_PACKAGE: 'MainPackage',
  SUB_PACKAGE: 'SubPackage',
  TASK: 'Task',
}

export function isRootTarget(targetNode) {
  return targetNode === null || targetNode === undefined
}

export function isTaskNode(targetNode) {
  return targetNode?.type === WBS_NODE_TYPES.TASK
}

export function isMainPackageNode(targetNode) {
  return targetNode?.type === WBS_NODE_TYPES.MAIN_PACKAGE
}

export function isSubPackageNode(targetNode) {
  return targetNode?.type === WBS_NODE_TYPES.SUB_PACKAGE
}

export function isContainerNode(targetNode) {
  return isMainPackageNode(targetNode) || isSubPackageNode(targetNode)
}

export function canCreateUnder(templateType, targetNode) {
  if (templateType === WBS_NODE_TYPES.MAIN_PACKAGE) {
    return isRootTarget(targetNode)
  }

  if (!targetNode) {
    return false
  }

  if (isTaskNode(targetNode)) {
    return false
  }

  if (templateType === WBS_NODE_TYPES.SUB_PACKAGE) {
    return isContainerNode(targetNode)
  }

  if (templateType === WBS_NODE_TYPES.TASK) {
    return isContainerNode(targetNode)
  }

  return false
}

export function getCreateValidationMessage(templateType, targetNode) {
  if (templateType === WBS_NODE_TYPES.MAIN_PACKAGE) {
    return canCreateUnder(templateType, targetNode)
      ? ''
      : 'Ein Hauptpaket kann nur auf oberster Ebene angelegt werden.'
  }

  if (!targetNode) {
    return 'Bitte zuerst ein Hauptpaket oder Unterpaket auswählen.'
  }

  if (isTaskNode(targetNode)) {
    return 'Unter Aufgaben können keine weiteren Elemente angelegt werden.'
  }

  if (templateType === WBS_NODE_TYPES.SUB_PACKAGE) {
    return canCreateUnder(templateType, targetNode)
      ? ''
      : 'Ein Unterpaket kann nur unter einem Hauptpaket oder Unterpaket angelegt werden.'
  }

  if (templateType === WBS_NODE_TYPES.TASK) {
    return canCreateUnder(templateType, targetNode)
      ? ''
      : 'Eine Aufgabe kann nur unter einem Hauptpaket oder Unterpaket angelegt werden.'
  }

  return 'Ungültiger Knotentyp.'
}

export function getParentIdForCreate(templateType, targetNode) {
  if (templateType === WBS_NODE_TYPES.MAIN_PACKAGE) {
    return null
  }

  return targetNode?.id ?? null
}

export function getDefaultTitleForType(templateType) {
  switch (templateType) {
    case WBS_NODE_TYPES.MAIN_PACKAGE:
      return 'Neues Hauptpaket'
    case WBS_NODE_TYPES.SUB_PACKAGE:
      return 'Neues Unterpaket'
    case WBS_NODE_TYPES.TASK:
      return 'Neue Aufgabe'
    default:
      return 'Neues Element'
  }
}

export function buildCreatePayload({ projectId, templateType, targetNode }) {
  return {
    projectId,
    parentId: getParentIdForCreate(templateType, targetNode),
    title: getDefaultTitleForType(templateType),
    description: '',
    type: templateType,
    plannedStart: null,
    plannedEnd: null,
    plannedHours: null,
    actualHours: null,
    isBlocked: false,
    comment: '',
  }
}

export function isNodeMeaningfullyEmpty(node) {
  if (!node) {
    return true
  }

  const title = typeof node.title === 'string' ? node.title.trim() : ''
  const description = typeof node.description === 'string' ? node.description.trim() : ''
  const comment = typeof node.comment === 'string' ? node.comment.trim() : ''

  const defaultTitles = [
    'Neues Hauptpaket',
    'Neues Unterpaket',
    'Neue Aufgabe',
    'Neues Element',
  ]

  const hasCustomTitle = title !== '' && !defaultTitles.includes(title)
  const hasDescription = description !== ''
  const hasComment = comment !== ''
  const hasPlannedStart = Boolean(node.plannedStart)
  const hasPlannedEnd = Boolean(node.plannedEnd)
  const hasPlannedHours = node.plannedHours !== null && node.plannedHours !== undefined
  const hasActualHours = node.actualHours !== null && node.actualHours !== undefined
  const hasBlocked = node.isBlocked === true

  return !(
    hasCustomTitle ||
    hasDescription ||
    hasComment ||
    hasPlannedStart ||
    hasPlannedEnd ||
    hasPlannedHours ||
    hasActualHours ||
    hasBlocked
  )
}

export function flattenTree(nodes) {
  if (!Array.isArray(nodes)) {
    return []
  }

  const result = []

  function walk(items) {
    items.forEach((item) => {
      result.push(item)

      if (Array.isArray(item.children) && item.children.length > 0) {
        walk(item.children)
      }
    })
  }

  walk(nodes)
  return result
}

export function findNodeInTree(nodes, nodeId) {
  if (!Array.isArray(nodes) || !nodeId) {
    return null
  }

  for (const node of nodes) {
    if (node.id === nodeId) {
      return node
    }

    if (Array.isArray(node.children) && node.children.length > 0) {
      const found = findNodeInTree(node.children, nodeId)
      if (found) {
        return found
      }
    }
  }

  return null
}