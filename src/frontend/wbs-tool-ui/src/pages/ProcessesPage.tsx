import { useEffect, useState } from 'react'
import { getPersons, getProcessPhases, getProjectDeliverables } from '../services/api'

type ProcessPhase = {
  id: string
  code: string
  name: string
  goal?: string | null
  description?: string | null
  defaultResponsibility?: string | null
  sortOrder: number
  isActive: boolean
}

type Deliverable = {
  id: string
  name: string
  type: string
  status: string
  dueDate: string
  ownerPersonId: string
  processPhaseId?: string | null
  wbsNodeId?: string | null
  projectId: string
  description?: string | null
  createdAt: string
}

type Person = {
  id: string
  displayName: string
  shortName?: string | null
  email?: string | null
  isPlaceholder: boolean
  placeholderType?: string | null
  isActive: boolean
}

interface ProcessesPageProps {
  projectId?: string
}

export default function ProcessesPage({ projectId }: ProcessesPageProps) {
  const [phases, setPhases] = useState<ProcessPhase[]>([])
  const [deliverables, setDeliverables] = useState<Deliverable[]>([])
  const [persons, setPersons] = useState<Person[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  useEffect(() => {
    async function loadData() {
      setLoading(true)
      setError('')

      try {
        const [phasesData, deliverablesData, personsData] = await Promise.all([
          getProcessPhases(),
          projectId ? getProjectDeliverables(projectId) : Promise.resolve([]),
          getPersons(),
        ])

        setPhases(Array.isArray(phasesData) ? phasesData : [])
        setDeliverables(Array.isArray(deliverablesData) ? deliverablesData : [])
        setPersons(Array.isArray(personsData) ? personsData : [])
      } catch (err) {
        setError(err?.message || 'Fehler beim Laden der Prozessphasen')
      } finally {
        setLoading(false)
      }
    }

    loadData()
  }, [projectId])

  return (
    <section style={{ display: 'flex', flexDirection: 'column', gap: '20px', padding: '20px' }}>
      <div>
        <h2 style={{ margin: '0 0 12px 0', color: '#111827' }}>Prozesse</h2>
        <p style={{ margin: 0, color: '#6b7280', fontSize: '14px' }}>Leistungsphasen (LPH) und Prozessablauf</p>
      </div>

      {error && (
        <div style={{ padding: '12px', backgroundColor: '#fee', color: '#c00', borderRadius: '8px' }}>
          {error}
        </div>
      )}

      {loading && (
        <div style={{ padding: '20px', textAlign: 'center', color: '#6b7280' }}>
          Prozessphasen werden geladen...
        </div>
      )}

      {!loading && !error && phases.length === 0 && (
        <div style={{ padding: '20px', textAlign: 'center', color: '#6b7280' }}>
          Keine Prozessphasen vorhanden.
        </div>
      )}

      {!loading && phases.length > 0 && (
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
                  Code
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
                  Name
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
                  Reihenfolge
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
              {phases
                .sort((a, b) => a.sortOrder - b.sortOrder)
                .map((phase) => (
                  <tr
                    key={phase.id}
                    style={{
                      borderBottom: '1px solid #e5e7eb',
                      transition: 'background-color 0.15s ease',
                    }}
                    onMouseEnter={(e) => (e.currentTarget.style.backgroundColor = '#f9fafb')}
                    onMouseLeave={(e) => (e.currentTarget.style.backgroundColor = 'transparent')}
                  >
                    <td
                      style={{
                        padding: '12px 16px',
                        color: '#111827',
                        fontWeight: '600',
                        fontFamily: 'monospace',
                        fontSize: '13px',
                      }}
                    >
                      {phase.code}
                    </td>
                    <td style={{ padding: '12px 16px', color: '#111827', fontWeight: '500' }}>
                      {phase.name}
                    </td>
                    <td style={{ padding: '12px 16px', textAlign: 'center', color: '#6b7280' }}>
                      {phase.sortOrder}
                    </td>
                    <td style={{ padding: '12px 16px', textAlign: 'center' }}>
                      {phase.isActive ? (
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
        {phases.length > 0 && `Gesamt: ${phases.length} Phase${phases.length === 1 ? '' : 'n'}`}
      </div>

      {projectId && (
        <>
          <div style={{ marginTop: '24px' }}>
            <h3 style={{ margin: '0 0 12px 0', color: '#111827', fontSize: '16px' }}>Deliverables</h3>
            <p style={{ margin: 0, color: '#6b7280', fontSize: '14px' }}>Liefergegenstände und Ergebnisse</p>
          </div>

          {!loading && !error && deliverables.length === 0 && (
            <div style={{ padding: '20px', textAlign: 'center', color: '#6b7280' }}>
              Keine Deliverables vorhanden.
            </div>
          )}

          {!loading && deliverables.length > 0 && (
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
                      Typ
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
                      Status
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
                      Due Date
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
                      Phase
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
                      Owner
                    </th>
                  </tr>
                </thead>
                <tbody>
                  {deliverables.map((deliverable) => {
                    const phaseName = deliverable.processPhaseId
                      ? phases.find((p) => p.id === deliverable.processPhaseId)?.name || 'Nicht zugeordnet'
                      : 'Nicht zugeordnet'

                    const ownerName = deliverable.ownerPersonId
                      ? persons.find((p) => p.id === deliverable.ownerPersonId)?.displayName || 'Unbekannt'
                      : 'Unbekannt'

                    const formattedDate = deliverable.dueDate
                      ? new Date(deliverable.dueDate).toLocaleDateString('de-DE', {
                          year: 'numeric',
                          month: '2-digit',
                          day: '2-digit',
                        })
                      : 'Nicht definiert'

                    return (
                      <tr
                        key={deliverable.id}
                        style={{
                          borderBottom: '1px solid #e5e7eb',
                          transition: 'background-color 0.15s ease',
                        }}
                        onMouseEnter={(e) => (e.currentTarget.style.backgroundColor = '#f9fafb')}
                        onMouseLeave={(e) => (e.currentTarget.style.backgroundColor = 'transparent')}
                      >
                        <td style={{ padding: '12px 16px', color: '#111827', fontWeight: '500' }}>
                          {deliverable.name}
                        </td>
                        <td style={{ padding: '12px 16px', color: '#6b7280', fontSize: '13px' }}>
                          {deliverable.type || 'N/A'}
                        </td>
                        <td style={{ padding: '12px 16px', color: '#6b7280', fontSize: '13px' }}>
                          {deliverable.status || 'N/A'}
                        </td>
                        <td style={{ padding: '12px 16px', color: '#6b7280', fontSize: '13px' }}>
                          {formattedDate}
                        </td>
                        <td style={{ padding: '12px 16px', color: '#6b7280', fontSize: '13px' }}>
                          {phaseName}
                        </td>
                        <td style={{ padding: '12px 16px', color: '#6b7280', fontSize: '13px' }}>
                          {ownerName}
                        </td>
                      </tr>
                    )
                  })}
                </tbody>
              </table>
            </div>
          )}

          <div style={{ color: '#6b7280', fontSize: '12px', fontStyle: 'italic', marginTop: '12px' }}>
            {deliverables.length > 0 && `Gesamt: ${deliverables.length} Deliverable${deliverables.length === 1 ? '' : 's'}`}
          </div>
        </>
      )}
    </section>
  )
}