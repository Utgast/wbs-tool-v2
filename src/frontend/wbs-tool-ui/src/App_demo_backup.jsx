import { useState } from 'react'
import AdministrationPage from './pages/AdministrationPage'
import DashboardPage from './pages/DashboardPage'

export default function App() {
  const [currentTab, setCurrentTab] = useState('dashboard')

  const dashboard = {
    progressPercent: 68,
    totalPlannedHours: 1200,
    totalActualHours: 840,
    plannedDemandHours: 1100,
    assignedHours: 920,
    openHours: 180,
    capacityHours: 1000,
    utilizationPercent: 92,
    blockedNodes: 3,
    overdueNodes: 7,
  }

  return (
    <div
      style={{
        minHeight: '100vh',
        backgroundColor: '#f3f4f6',
        color: '#111827',
        fontFamily: 'Arial, sans-serif',
      }}
    >
      <header
        style={{
          borderBottom: '1px solid #e5e7eb',
          backgroundColor: '#ffffff',
          padding: '16px 24px',
        }}
      >
        <h1
          style={{
            margin: 0,
            fontSize: '24px',
            fontWeight: 'bold',
          }}
        >
          WBS-Tool
        </h1>
      </header>

      <nav
        style={{
          display: 'flex',
          gap: '8px',
          flexWrap: 'wrap',
          padding: '16px 24px',
          borderBottom: '1px solid #e5e7eb',
          backgroundColor: '#ffffff',
        }}
      >
        <TabButton
          label="Dashboard"
          isActive={currentTab === 'dashboard'}
          onClick={() => setCurrentTab('dashboard')}
        />

        <TabButton
          label="WBS"
          isActive={currentTab === 'wbs'}
          onClick={() => setCurrentTab('wbs')}
        />

        <TabButton
          label="Ressourcen"
          isActive={currentTab === 'resources'}
          onClick={() => setCurrentTab('resources')}
        />

        <TabButton
          label="Kompetenzen"
          isActive={currentTab === 'skills'}
          onClick={() => setCurrentTab('skills')}
        />

        <TabButton
          label="Prozesse"
          isActive={currentTab === 'processes'}
          onClick={() => setCurrentTab('processes')}
        />

        <TabButton
          label="⚙️ Administration 🔒"
          isActive={currentTab === 'administration'}
          onClick={() => setCurrentTab('administration')}
        />
      </nav>

      <main
        style={{
          padding: '24px',
        }}
      >
        {currentTab === 'dashboard' && (
          <DashboardPage dashboard={dashboard} />
        )}

        {currentTab === 'wbs' && (
          <PagePlaceholder
            title="WBS"
            text="Die bestehende WBS-Arbeitsansicht bleibt unverändert und wird hier weiterhin separat eingebunden."
          />
        )}

        {currentTab === 'resources' && (
          <PagePlaceholder
            title="Ressourcen"
            text="Dieses Modul ist aktuell nicht Bestandteil der Sprint-2-Anpassung."
          />
        )}

        {currentTab === 'skills' && (
  <main className="workspace-dashboard">
    <CompetenciesPage />
  </main>
)}

        {currentTab === 'processes' && (
          <PagePlaceholder
            title="Prozesse"
            text="Dieses Modul ist aktuell nicht Bestandteil der Sprint-2-Anpassung."
          />
        )}

        {currentTab === 'administration' && <AdministrationPage />}
      </main>
    </div>
  )
}

function TabButton({ label, isActive, onClick }) {
  return (
    <button
      type="button"
      onClick={onClick}
      style={{
        padding: '10px 14px',
        borderRadius: '8px',
        border: isActive ? '1px solid #2563eb' : '1px solid #d1d5db',
        backgroundColor: isActive ? '#dbeafe' : '#ffffff',
        color: isActive ? '#1d4ed8' : '#111827',
        fontWeight: isActive ? 'bold' : 'normal',
        cursor: 'pointer',
      }}
    >
      {label}
    </button>
  )
}

function PagePlaceholder({ title, text }) {
  return (
    <section
      style={{
        backgroundColor: '#ffffff',
        border: '1px solid #e5e7eb',
        borderRadius: '12px',
        padding: '24px',
        boxShadow: '0 1px 3px rgba(0,0,0,0.08)',
      }}
    >
      <h2
        style={{
          marginTop: 0,
          marginBottom: '12px',
        }}
      >
        {title}
      </h2>

      <p
        style={{
          margin: 0,
          color: '#4b5563',
          lineHeight: 1.6,
        }}
      >
        {text}
      </p>
    </section>
  )
}