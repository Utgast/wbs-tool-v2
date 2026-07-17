export default function NavigationTabs({
  currentTab,
  onChange,
}) {
  const tabs = [
    { key: 'dashboard', label: ' Dashboard' },
    { key: 'wbs', label: ' WBS' },
    { key: 'resources', label: ' Ressourcen' },
    { key: 'competencies', label: ' Kompetenzen' },
    { key: 'processes', label: ' Prozesse' },
    { key: 'administration', label: ' Administration' },
  ]

  return (
    <nav
      style={{
        display: 'flex',
        flexWrap: 'wrap',
        gap: '10px',
        padding: '16px',
        backgroundColor: '#ffffff',
        borderRadius: '12px',
        marginBottom: '24px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
      }}
    >
      {tabs.map((tab) => {
        const active = currentTab === tab.key

        return (
          <button
            key={tab.key}
            onClick={() => onChange(tab.key)}
            style={{
              border: 'none',
              cursor: 'pointer',
              padding: '12px 18px',
              borderRadius: '10px',
              transition: 'all 0.2s ease',

              backgroundColor: active
                ? '#F58220'
                : '#f3f4f6',

              color: active
                ? '#ffffff'
                : '#374151',

              fontWeight: active ? '700' : '500',

              boxShadow: active
                ? '0 4px 10px rgba(245,130,32,0.35)'
                : 'none',
            }}
          >
            {tab.label}
          </button>
        )
      })}
    </nav>
  )
}