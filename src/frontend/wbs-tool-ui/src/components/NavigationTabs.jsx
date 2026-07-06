export default function NavigationTabs({ currentTab, onChange }) {
  return (
    <div className="navigation-tabs">
      <button
        type="button"
        className={currentTab === 'dashboard' ? 'active' : ''}
        onClick={() => onChange('dashboard')}
      >
        Dashboard
      </button>

      <button
        type="button"
        className={currentTab === 'wbs' ? 'active' : ''}
        onClick={() => onChange('wbs')}
      >
        WBS
      </button>

      <button
        type="button"
        className={currentTab === 'resources' ? 'active' : ''}
        onClick={() => onChange('resources')}
      >
        Ressourcen
      </button>

      <button
        type="button"
        className={currentTab === 'competencies' ? 'active' : ''}
        onClick={() => onChange('competencies')}
      >
        Kompetenzen
      </button>

      <button
        type="button"
        className={currentTab === 'processes' ? 'active' : ''}
        onClick={() => onChange('processes')}
      >
        Prozesse
      </button>

      <button
        type="button"
        className={currentTab === 'administration' ? 'active' : ''}
        onClick={() => onChange('administration')}
      >
        Administration
      </button>
    </div>
  )
}