import { useEffect, useState } from 'react'
import { getPersons, getResourceDemands } from '../services/api'

type Person = {
  id: string
  displayName: string
  shortName?: string | null
  email?: string | null
  isPlaceholder: boolean
  placeholderType?: string | null
  isActive: boolean
}

type ResourceDemand = {
  id: string
  projectId: string
  plannedHours?: number | null
  assignedHours?: number | null
  isActive: boolean
}

export default function ResourcesPage() {
  const [persons, setPersons] = useState<Person[]>([])
  const [resourceDemands, setResourceDemands] = useState<ResourceDemand[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  useEffect(() => {
    async function loadData() {
      setLoading(true)
      setError('')

      try {
        const [personsData, demandsData] = await Promise.all([
          getPersons(),
          getResourceDemands().catch(() => null),
        ])

        const safePersons = Array.isArray(personsData) ? personsData : []
        const safeDemands = Array.isArray(demandsData) ? demandsData : []

        setPersons(safePersons)
        setResourceDemands(safeDemands)
      } catch (err) {
        setError(err?.message || 'Fehler beim Laden der Ressourcendaten')
      } finally {
        setLoading(false)
      }
    }

    loadData()
  }, [])

  return (
    <section style={{ display: 'flex', flexDirection: 'column', gap: '20px', padding: '20px' }}>
      <div>
        <h2 style={{ margin: '0 0 12px 0', color: '#111827' }}>Ressourcen</h2>
        <p style={{ margin: 0, color: '#6b7280', fontSize: '14px' }}>Übersicht der verfügbaren Personen</p>
      </div>

      {!loading && !error && (persons.length > 0 || resourceDemands.length > 0) && (
        <div
          style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))',
            gap: '14px',
          }}
        >
          <KpiCard
            label="Personen insgesamt"
            value={persons.length}
            status="info"
          />
          <KpiCard
            label="Aktive Personen"
            value={persons.filter((p) => p.isActive).length}
            status="success"
          />
          <KpiCard
            label="Inaktive Personen"
            value={persons.filter((p) => !p.isActive).length}
            status="warning"
          />
          <KpiCard
            label="Ressourcenbedarfe"
            value={resourceDemands.length}
            status={resourceDemands.length > 0 ? 'info' : 'success'}
          />
        </div>
      )}

      {error && (
        <div style={{ padding: '12px', backgroundColor: '#fee', color: '#c00', borderRadius: '8px' }}>
          {error}
        </div>
      )}

      {loading && (
        <div style={{ padding: '20px', textAlign: 'center', color: '#6b7280' }}>
          Personen werden geladen...
        </div>
      )}

      {!loading && !error && persons.length === 0 && (
        <div style={{ padding: '20px', textAlign: 'center', color: '#6b7280' }}>
          Keine Personen vorhanden.
        </div>
      )}

      {!loading && persons.length > 0 && (
        <div
          style={{
            borderRadius: '12px',
            border: '1px solid #e5e7eb',
            overflow: 'hidden',
            boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
          }}
        >
          <table
            style={{
              width: '100%',
              borderCollapse: 'collapse',
              fontSize: '14px',
            }}
          >
            <thead>
              <tr style={{ backgroundColor: '#f9fafb', borderBottom: '1px solid #e5e7eb' }}>
                <th
                  style={{
                    textAlign: 'left',
                    padding: '12px 16px',
                    fontWeight: '700',
                    color: '#6b7280',
                    textTransform: 'uppercase',
                    fontSize: '12px',
                    letterSpacing: '0.5px',
                  }}
                >
                  Name
                </th>
                <th
                  style={{
                    textAlign: 'left',
                    padding: '12px 16px',
                    fontWeight: '700',
                    color: '#6b7280',
                    textTransform: 'uppercase',
                    fontSize: '12px',
                    letterSpacing: '0.5px',
                  }}
                >
                  Kürzel
                </th>
                <th
                  style={{
                    textAlign: 'left',
                    padding: '12px 16px',
                    fontWeight: '700',
                    color: '#6b7280',
                    textTransform: 'uppercase',
                    fontSize: '12px',
                    letterSpacing: '0.5px',
                  }}
                >
                  Email
                </th>
                <th
                  style={{
                    textAlign: 'left',
                    padding: '12px 16px',
                    fontWeight: '700',
                    color: '#6b7280',
                    textTransform: 'uppercase',
                    fontSize: '12px',
                    letterSpacing: '0.5px',
                  }}
                >
                  Typ
                </th>
                <th
                  style={{
                    textAlign: 'center',
                    padding: '12px 16px',
                    fontWeight: '700',
                    color: '#6b7280',
                    textTransform: 'uppercase',
                    fontSize: '12px',
                    letterSpacing: '0.5px',
                  }}
                >
                  Status
                </th>
              </tr>
            </thead>
            <tbody>
              {persons.map((person) => (
                <tr
                  key={person.id}
                  style={{
                    borderBottom: '1px solid #e5e7eb',
                    transition: 'background-color 0.15s ease',
                  }}
                  onMouseEnter={(e) => (e.currentTarget.style.backgroundColor = '#f9fafb')}
                  onMouseLeave={(e) => (e.currentTarget.style.backgroundColor = 'transparent')}
                >
                  <td style={{ padding: '12px 16px', color: '#111827', fontWeight: '500' }}>
                    {person.displayName}
                  </td>
                  <td style={{ padding: '12px 16px', color: '#6b7280' }}>
                    {person.shortName || '-'}
                  </td>
                  <td style={{ padding: '12px 16px', color: '#6b7280' }}>
                    {person.email || '-'}
                  </td>
                  <td style={{ padding: '12px 16px', color: '#6b7280' }}>
                    {person.isPlaceholder && person.placeholderType ? (
                      <span
                        style={{
                          backgroundColor: '#e0e7ff',
                          color: '#3730a3',
                          padding: '4px 8px',
                          borderRadius: '6px',
                          fontSize: '12px',
                          fontWeight: '500',
                        }}
                      >
                        {person.placeholderType}
                      </span>
                    ) : (
                      <span>Real</span>
                    )}
                  </td>
                  <td style={{ padding: '12px 16px', textAlign: 'center' }}>
                    {person.isActive ? (
                      <span
                        style={{
                          backgroundColor: '#dcfce7',
                          color: '#166534',
                          padding: '4px 8px',
                          borderRadius: '6px',
                          fontSize: '12px',
                          fontWeight: '600',
                        }}
                      >
                        Aktiv
                      </span>
                    ) : (
                      <span
                        style={{
                          backgroundColor: '#fee2e2',
                          color: '#991b1b',
                          padding: '4px 8px',
                          borderRadius: '6px',
                          fontSize: '12px',
                          fontWeight: '600',
                        }}
                      >
                        Inaktiv
                      </span>
                    )}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      <div style={{ color: '#6b7280', fontSize: '12px', fontStyle: 'italic', marginTop: '12px' }}>
        {persons.length > 0 && `Gesamt: ${persons.length} Person${persons.length === 1 ? '' : 'en'}`}
      </div>
    </section>
  )
}

function KpiCard({ label, value, status }) {
  const statusConfig = {
    success: { bg: '#dcfce7', color: '#166534', accent: '#22c55e' },
    warning: { bg: '#fef3c7', color: '#92400e', accent: '#f59e0b' },
    danger: { bg: '#fee2e2', color: '#991b1b', accent: '#ef4444' },
    info: { bg: '#dbeafe', color: '#1e40af', accent: '#3b82f6' },
  }

  const config = statusConfig[status] || statusConfig.info

  return (
    <div
      style={{
        backgroundColor: config.bg,
        border: `1px solid ${config.accent}`,
        borderLeft: `4px solid ${config.accent}`,
        borderRadius: '8px',
        padding: '16px',
        display: 'flex',
        flexDirection: 'column',
        gap: '8px',
      }}
    >
      <div style={{ fontSize: '12px', fontWeight: '700', color: config.color, textTransform: 'uppercase', letterSpacing: '0.5px' }}>
        {label}
      </div>
      <div style={{ fontSize: '24px', fontWeight: '800', color: config.color }}>
        {value}
      </div>
    </div>
  )
}