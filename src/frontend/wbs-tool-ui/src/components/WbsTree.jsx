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
  if (!Array.isArray(nodes) || nodes.length === 0) {
    return null
  }

  return (
    <div className="wbs-tree-list" role={level === 0 ? 'tree' : 'group'}>
      {nodes.map((node) => {
        const isSelected = selectedNodeId === node.id
        const hasChildren = Array.isArray(node.children) && node.children.length > 0

        return (
          <div
            key={node.id}
            className={`wbs-tree-item level-${level}`}
            role="treeitem"
            aria-selected={isSelected}
            aria-expanded={hasChildren ? true : undefined}
          >
            <WbsTreeNodeCard
              node={node}
              isSelected={isSelected}
              level={level}
              onSelect={onSelectNode}
              onEditNode={onEditNode}
              onDeactivateNode={onDeactivateNode}
              activeDragTemplateType={activeDragTemplateType}
            />

            {hasChildren && (
              <div className="wbs-tree-children">
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

export default WbsTree