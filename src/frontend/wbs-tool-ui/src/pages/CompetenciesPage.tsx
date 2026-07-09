import { useEffect, useMemo, useState } from 'react'
import {
  getCompetencies,
  getPersonsByCompetency,
} from '../services/competenciesApi'

type Competency = {
  id: string
  code: string
  name: string
  description?: string | null
  isActive: boolean
}

type CompetencyPerson = {
  personCompetencyId: string
  personId: string
  personDisplayName: string
  personShortName?: string | null
  personEmail?: string | null
  competencyId: string
  competencyCode: string
  competencyName: string
  proficiencyLevel?: number | null
  comment?: string | null
  isActive: boolean
}

function getLevelLabel(level?: number | null) {
  if (level === 3) return 'Experte'
  if (level === 2) return 'Fortgeschritten'
  if (level === 1) return 'Grundlagen'
  return 'Nicht bewertet'
}

function getLevelBadgeColor(level?: number | null) {
  if (level === 3) return '#166534'
  if (level === 2) return '#92400e'
  if (level === 1) return '#1d4ed8'
  return '#6b7280'
}

export default function CompetenciesPage() {
  const [competencies, setCompetencies] = useState<Competency[]>([])
  const [selectedCompetencyId, setSelectedCompetencyId] = useState<string | null>(null)
  const [persons, setPersons] = useState<CompetencyPerson[]>([])

  const [loadingCompetencies, setLoadingCompetencies] = useState(false)
  const [loadingPersons, setLoadingPersons] = useState(false)

  const [error, setError] = useState('')

  useEffect(() => {
    async function loadCompetencies() {
      setLoadingCompetencies(true)
      setError('')

      try {
        const data = await getCompetencies()
        const safeData = Array.isArray(data) ? data : []

        setCompetencies(safeData)

        if (safeData.length > 0) {
          setSelectedCompetencyId(safeData[0].id)
        }
      } catch (err: unknown) {
        setError(err instanceof Error ? err.message : 'Kompetenzen konnten nicht geladen werden')
      } finally {
        setLoadingCompetencies(false)
      }
    }

    loadCompetencies()
  }, [])

  useEffect(() => {
    async function loadPersons() {
      if (!selectedCompetencyId) {
        setPersons([])
        return
      }

      setLoadingPersons(true)
      setError('')

      try {
        const data = await getPersonsByCompetency(selectedCompetencyId)
        setPersons(Array.isArray(data) ? data : [])
      } catch (err: unknown) {
        setPersons([])
        setError(
          err instanceof Error
            ? err.message
            : 'Personen zur Kompetenz konnten nicht geladen werden'
        )
      } finally {
        setLoadingPersons(false)
      }
    }

    loadPersons()
  }, [selectedCompetencyId])

  const selectedCompetency = useMemo(
    () => competencies.find((item) => item.id === selectedCompetencyId) ?? null,
    [competencies, selectedCompetencyId]
  )

  return (
    <section
      style={{
        width: '100%',
        display: 'grid',
        gridTemplateColumns: '320px minmax(0, 1fr)',
        gap: '20px',
        alignItems: 'start',
      }}
    >
      <aside
        style={{
          background: '#ffffff',
          border: '1px solid #e5e7eb',
          borderRadius: '14px',
          padding: '16px',
          boxShadow: '0 1px 4px rgba(15, 23, 42, 0.08)',
        }}
      >
        <div style={{ marginBottom: '16px' }}>
          <h2 style={{ margin: 0 }}>Kompetenzen</h2>
          <p style={{ margin: '6px 0 0', color: '#6b7280', fontSize: '14px' }}>
            Kompetenzkatalog als f�hrendes Stammdatum
          </p>
        </div>

        {loadingCompetencies && (
          <p style={{ color: '#6b7280' }}>Kompetenzen werden geladen...</p>
        )}

        {!loadingCompetencies && competencies.length === 0 && (
          <p style={{ color: '#6b7280' }}>Noch keine Kompetenzen vorhanden.</p>
        )}

        <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
          {competencies.map((competency) => {
            const isSelected = competency.id === selectedCompetencyId

            return (
              <button
                key={competency.id}
                type="button"
                onClick={() => setSelectedCompetencyId(competency.id)}
                style={{
                  width: '100%',
                  textAlign: 'left',
                  border: isSelected ? '2px solid #f97316' : '1px solid #e5e7eb',
                  background: isSelected ? '#fff7ed' : '#f9fafb',
                  borderRadius: '10px',
                  padding: '10px 12px',
                  cursor: 'pointer',
                }}
              >
                <strong style={{ display: 'block' }}>{competency.name}</strong>
                <span style={{ color: '#6b7280', fontSize: '12px' }}>
                  {competency.code}
                </span>
              </button>
            )
          })}
        </div>
      </aside>

      <main
        style={{
          background: '#ffffff',
          border: '1px solid #e5e7eb',
          borderRadius: '14px',
          padding: '20px',
          boxShadow: '0 1px 4px rgba(15, 23, 42, 0.08)',
        }}
      >
        {error && (
          <div
            style={{
              marginBottom: '16px',
              padding: '10px 12px',
              borderRadius: '10px',
              background: '#fee2e2',
              color: '#991b1b',
              fontWeight: 600,
            }}
          >
            {error}
          </div>
        )}

        {!selectedCompetency && (
          <p style={{ color: '#6b7280' }}>Bitte eine Kompetenz ausw�hlen.</p>
        )}

        {selectedCompetency && (
          <>
            <header
              style={{
                display: 'flex',
                justifyContent: 'space-between',
                gap: '16px',
                alignItems: 'flex-start',
                marginBottom: '20px',
              }}
            >
              <div>
                <h2 style={{ margin: 0 }}>{selectedCompetency.name}</h2>
                <p style={{ margin: '6px 0 0', color: '#6b7280' }}>
                  Code: {selectedCompetency.code}
                </p>
              </div>

              <span
                style={{
                  padding: '6px 10px',
                  borderRadius: '999px',
                  background: selectedCompetency.isActive ? '#dcfce7' : '#f3f4f6',
                  color: selectedCompetency.isActive ? '#166534' : '#4b5563',
                  fontWeight: 700,
                  fontSize: '12px',
                }}
              >
                {selectedCompetency.isActive ? 'Aktiv' : 'Inaktiv'}
              </span>
            </header>

            <section
              style={{
                marginBottom: '24px',
                padding: '14px',
                borderRadius: '12px',
                background: '#f9fafb',
                border: '1px solid #e5e7eb',
              }}
            >
              <h3 style={{ margin: '0 0 8px' }}>Beschreibung</h3>
              <p style={{ margin: 0, color: '#374151' }}>
                {selectedCompetency.description || 'Keine Beschreibung hinterlegt.'}
              </p>
            </section>

            <section>
              <div
                style={{
                  display: 'flex',
                  justifyContent: 'space-between',
                  alignItems: 'center',
                  marginBottom: '12px',
                }}
              >
                <div>
                  <h3 style={{ margin: 0 }}>Zugeordnete Personen</h3>
                  <p style={{ margin: '4px 0 0', color: '#6b7280', fontSize: '14px' }}>
                    Personen, die diese Kompetenz aktuell im System besitzen
                  </p>
                </div>

                <strong>{persons.length} Person(en)</strong>
              </div>

              {loadingPersons && (
                <p style={{ color: '#6b7280' }}>Personen werden geladen...</p>
              )}

              {!loadingPersons && persons.length === 0 && (
                <div
                  style={{
                    padding: '14px',
                    borderRadius: '12px',
                    background: '#fff7ed',
                    border: '1px solid #fed7aa',
                    color: '#9a3412',
                  }}
                >
                  Noch keine Person zugeordnet. Diese Kompetenz kann damit bereits als m�glicher Gap sichtbar werden.
                </div>
              )}

              <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                {persons.map((person) => (
                  <article
                    key={person.personCompetencyId}
                    style={{
                      display: 'grid',
                      gridTemplateColumns: 'minmax(0, 1fr) auto',
                      gap: '12px',
                      alignItems: 'center',
                      padding: '12px',
                      borderRadius: '12px',
                      border: '1px solid #e5e7eb',
                      background: '#ffffff',
                    }}
                  >
                    <div>
                      <strong>{person.personDisplayName}</strong>

                      <div style={{ color: '#6b7280', fontSize: '13px', marginTop: '4px' }}>
                        {person.personEmail || person.personShortName || 'Keine weiteren Kontaktdaten'}
                      </div>

                      {person.comment && (
                        <p style={{ margin: '8px 0 0', color: '#374151' }}>
                          {person.comment}
                        </p>
                      )}
                    </div>

                    <span
                      style={{
                        padding: '6px 10px',
                        borderRadius: '999px',
                        color: '#ffffff',
                        background: getLevelBadgeColor(person.proficiencyLevel),
                        fontSize: '12px',
                        fontWeight: 700,
                        whiteSpace: 'nowrap',
                      }}
                    >
                      {getLevelLabel(person.proficiencyLevel)}
                    </span>
                  </article>
                ))}
              </div>
            </section>
          </>
        )}
      </main>
    </section>
  )
}
