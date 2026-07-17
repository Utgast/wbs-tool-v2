import { useState } from 'react'

export default function AdministrationPage() {
  const [preview, setPreview] = useState(null)

  const [testValues, setTestValues] = useState({
    deviation: 7,
    utilization: 92,
    overdue: 7,
  })

  const [settings, setSettings] = useState({
    enableScheduleTrafficLight: true,
    enableCapacityTrafficLight: true,
    enableOverdueTrafficLight: true,

    scheduleGreenTolerance: 5,
    scheduleYellowTolerance: 15,

    capacityGreenLimit: 90,
    capacityYellowLimit: 110,

    overdueGreenLimit: 0,
    overdueYellowLimit: 5,
  })

  function calculatePreview() {
    const result = {}

    if (settings.enableScheduleTrafficLight) {
      if (testValues.deviation > settings.scheduleYellowTolerance) {
        result.schedule = 'red'
      } else if (testValues.deviation > settings.scheduleGreenTolerance) {
        result.schedule = 'yellow'
      } else {
        result.schedule = 'green'
      }
    }

    if (settings.enableCapacityTrafficLight) {
      if (testValues.utilization > settings.capacityYellowLimit) {
        result.capacity = 'red'
      } else if (testValues.utilization > settings.capacityGreenLimit) {
        result.capacity = 'yellow'
      } else {
        result.capacity = 'green'
      }
    }

    if (settings.enableOverdueTrafficLight) {
      if (testValues.overdue > settings.overdueYellowLimit) {
        result.overdue = 'red'
      } else if (testValues.overdue > settings.overdueGreenLimit) {
        result.overdue = 'yellow'
      } else {
        result.overdue = 'green'
      }
    }

    setPreview(result)
  }

  return (
    <section
      style={{
        display: 'flex',
        flexDirection: 'column',
        gap: '24px',
      }}
    >
      <HeaderCard />

      <div
        style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))',
          gap: '20px',
          alignItems: 'start',
        }}
      >
        <AdminCard
          title="Aktivierungen"
          subtitle="Ampelsysteme ein- oder ausschalten"
          accentColor="#F58220"
        >
          <div
            style={{
              display: 'flex',
              flexDirection: 'column',
              gap: '14px',
            }}
          >
            <CheckboxSetting
              label="Terminampel aktiv"
              description="Bewertet später Soll- gegen Ist-Fortschritt."
              checked={settings.enableScheduleTrafficLight}
              onChange={(checked) =>
                setSettings({
                  ...settings,
                  enableScheduleTrafficLight: checked,
                })
              }
            />

            <CheckboxSetting
              label="Kapazitätsampel aktiv"
              description="Bewertet Auslastung gegen verfügbare Kapazität."
              checked={settings.enableCapacityTrafficLight}
              onChange={(checked) =>
                setSettings({
                  ...settings,
                  enableCapacityTrafficLight: checked,
                })
              }
            />

            <CheckboxSetting
              label="Überfälligkeitsampel aktiv"
              description="Bewertet überfällige Arbeitspakete und Aufgaben."
              checked={settings.enableOverdueTrafficLight}
              onChange={(checked) =>
                setSettings({
                  ...settings,
                  enableOverdueTrafficLight: checked,
                })
              }
            />
          </div>
        </AdminCard>

        <AdminCard
          title="Schwellwerte"
          subtitle="Grenzwerte für grün, gelb und rot"
          accentColor="#2563eb"
        >
          <ThresholdGroup title="Terminampel">
            <ParameterInput
              label="Grün-Toleranz (%)"
              value={settings.scheduleGreenTolerance}
              onChange={(value) =>
                setSettings({
                  ...settings,
                  scheduleGreenTolerance: Number(value),
                })
              }
            />

            <ParameterInput
              label="Gelb-Toleranz (%)"
              value={settings.scheduleYellowTolerance}
              onChange={(value) =>
                setSettings({
                  ...settings,
                  scheduleYellowTolerance: Number(value),
                })
              }
            />
          </ThresholdGroup>

          <ThresholdGroup title="Kapazitätsampel">
            <ParameterInput
              label="Grün bis (%)"
              value={settings.capacityGreenLimit}
              onChange={(value) =>
                setSettings({
                  ...settings,
                  capacityGreenLimit: Number(value),
                })
              }
            />

            <ParameterInput
              label="Gelb bis (%)"
              value={settings.capacityYellowLimit}
              onChange={(value) =>
                setSettings({
                  ...settings,
                  capacityYellowLimit: Number(value),
                })
              }
            />
          </ThresholdGroup>

          <ThresholdGroup title="Überfälligkeitsampel">
            <ParameterInput
              label="Grün bis"
              value={settings.overdueGreenLimit}
              onChange={(value) =>
                setSettings({
                  ...settings,
                  overdueGreenLimit: Number(value),
                })
              }
            />

            <ParameterInput
              label="Gelb bis"
              value={settings.overdueYellowLimit}
              onChange={(value) =>
                setSettings({
                  ...settings,
                  overdueYellowLimit: Number(value),
                })
              }
            />
          </ThresholdGroup>
        </AdminCard>
      </div>

      <div
        style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(320px, 1fr))',
          gap: '20px',
          alignItems: 'start',
        }}
      >
        <AdminCard
          title="Testsimulation"
          subtitle="Beispielwerte für die Ampelvorschau"
          accentColor="#7c3aed"
        >
          <div
            style={{
              display: 'grid',
              gridTemplateColumns: 'repeat(auto-fit, minmax(180px, 1fr))',
              gap: '16px',
            }}
          >
            <ParameterInput
              label="Terminabweichung (%)"
              value={testValues.deviation}
              onChange={(value) =>
                setTestValues({
                  ...testValues,
                  deviation: Number(value),
                })
              }
            />

            <ParameterInput
              label="Auslastung (%)"
              value={testValues.utilization}
              onChange={(value) =>
                setTestValues({
                  ...testValues,
                  utilization: Number(value),
                })
              }
            />

            <ParameterInput
              label="Überfällige Vorgänge"
              value={testValues.overdue}
              onChange={(value) =>
                setTestValues({
                  ...testValues,
                  overdue: Number(value),
                })
              }
            />
          </div>

          <button
            type="button"
            onClick={calculatePreview}
            style={{
              marginTop: '20px',
              width: '100%',
              padding: '12px 16px',
              borderRadius: '10px',
              border: 'none',
              backgroundColor: '#F58220',
              color: '#ffffff',
              fontWeight: '700',
              cursor: 'pointer',
              boxShadow: '0 4px 10px rgba(245,130,32,0.35)',
            }}
          >
            Ampelvorschau berechnen
          </button>
        </AdminCard>

        <AdminCard
          title="Ampelvorschau"
          subtitle="Ergebnis aus Schwellwerten und Testwerten"
          accentColor="#16a34a"
        >
          {!preview && (
            <EmptyPreview />
          )}

          {preview && (
            <div
              style={{
                display: 'grid',
                gridTemplateColumns: 'repeat(auto-fit, minmax(180px, 1fr))',
                gap: '16px',
              }}
            >
              {preview.schedule && (
                <PreviewCard
                  title="Termin"
                  status={preview.schedule}
                />
              )}

              {preview.capacity && (
                <PreviewCard
                  title="Kapazität"
                  status={preview.capacity}
                />
              )}

              {preview.overdue && (
                <PreviewCard
                  title="Überfällig"
                  status={preview.overdue}
                />
              )}
            </div>
          )}
        </AdminCard>
      </div>

      <InfoCard />
    </section>
  )
}

function HeaderCard() {
  return (
    <div
      style={{
        backgroundColor: '#ffffff',
        border: '1px solid #e5e7eb',
        borderRadius: '14px',
        padding: '24px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
      }}
    >
      <div
        style={{
          display: 'flex',
          justifyContent: 'space-between',
          gap: '16px',
          flexWrap: 'wrap',
          alignItems: 'center',
        }}
      >
        <div>
          <h2
            style={{
              margin: 0,
              color: '#111827',
            }}
          >
            Administration
          </h2>

          <p
            style={{
              margin: '8px 0 0 0',
              color: '#6b7280',
              maxWidth: '760px',
            }}
          >
            Konfigurationszentrale für Dashboard, Ampellogik und spätere
            Projektsteuerungsparameter.
          </p>
        </div>

        <div
          style={{
            padding: '8px 12px',
            borderRadius: '999px',
            backgroundColor: '#fff7ed',
            color: '#c2410c',
            fontWeight: '700',
            border: '1px solid #fed7aa',
          }}
        >
          MVP: Frontend-Konfiguration
        </div>
      </div>
    </div>
  )
}

function AdminCard({
  title,
  subtitle,
  accentColor,
  children,
}) {
  return (
    <section
      style={{
        backgroundColor: '#ffffff',
        borderRadius: '14px',
        border: '1px solid #e5e7eb',
        borderTop: `5px solid ${accentColor}`,
        padding: '22px',
        boxShadow: '0 2px 8px rgba(0,0,0,0.08)',
      }}
    >
      <h3
        style={{
          margin: 0,
          color: '#111827',
        }}
      >
        {title}
      </h3>

      <p
        style={{
          margin: '6px 0 20px 0',
          color: '#6b7280',
          fontSize: '14px',
        }}
      >
        {subtitle}
      </p>

      {children}
    </section>
  )
}

function ThresholdGroup({ title, children }) {
  return (
    <div
      style={{
        marginBottom: '20px',
        paddingBottom: '16px',
        borderBottom: '1px solid #e5e7eb',
      }}
    >
      <h4
        style={{
          margin: '0 0 12px 0',
          color: '#374151',
        }}
      >
        {title}
      </h4>

      <div
        style={{
          display: 'grid',
          gridTemplateColumns: 'repeat(auto-fit, minmax(150px, 1fr))',
          gap: '12px',
        }}
      >
        {children}
      </div>
    </div>
  )
}

function CheckboxSetting({
  label,
  description,
  checked,
  onChange,
}) {
  return (
    <label
      style={{
        display: 'flex',
        gap: '12px',
        padding: '14px',
        borderRadius: '12px',
        border: checked ? '1px solid #F58220' : '1px solid #e5e7eb',
        backgroundColor: checked ? '#fff7ed' : '#f9fafb',
        cursor: 'pointer',
      }}
    >
      <input
        type="checkbox"
        checked={checked}
        onChange={(e) => onChange(e.target.checked)}
        style={{
          marginTop: '4px',
        }}
      />

      <span>
        <strong
          style={{
            display: 'block',
            color: '#111827',
            marginBottom: '4px',
          }}
        >
          {label}
        </strong>

        <span
          style={{
            color: '#6b7280',
            fontSize: '13px',
          }}
        >
          {description}
        </span>
      </span>
    </label>
  )
}

function ParameterInput({
  label,
  value,
  onChange,
}) {
  return (
    <div>
      <label
        style={{
          display: 'block',
          marginBottom: '6px',
          fontWeight: '700',
          color: '#374151',
          fontSize: '14px',
        }}
      >
        {label}
      </label>

      <input
        type="number"
        value={value}
        onChange={(e) => onChange(e.target.value)}
        style={{
          width: '100%',
          boxSizing: 'border-box',
          padding: '10px 12px',
          borderRadius: '10px',
          border: '1px solid #d1d5db',
          fontSize: '15px',
          backgroundColor: '#ffffff',
        }}
      />
    </div>
  )
}

function PreviewCard({ title, status }) {
  const config = getStatusConfig(status)

  return (
    <div
      style={{
        backgroundColor: config.backgroundColor,
        border: `1px solid ${config.borderColor}`,
        padding: '18px',
        borderRadius: '14px',
        textAlign: 'center',
      }}
    >
      <div
        style={{
          fontSize: '14px',
          color: '#374151',
          marginBottom: '10px',
          fontWeight: '700',
        }}
      >
        {title}
      </div>

      <div
        style={{
          fontSize: '32px',
          marginBottom: '8px',
        }}
      >
        {config.icon}
      </div>

      <strong
        style={{
          color: config.textColor,
          fontSize: '18px',
        }}
      >
        {config.label}
      </strong>
    </div>
  )
}

function EmptyPreview() {
  return (
    <div
      style={{
        padding: '28px',
        borderRadius: '14px',
        border: '1px dashed #d1d5db',
        backgroundColor: '#f9fafb',
        textAlign: 'center',
        color: '#6b7280',
      }}
    >
      Noch keine Vorschau berechnet.
      <br />
      Bitte Testwerte prüfen und die Ampelvorschau berechnen.
    </div>
  )
}

function InfoCard() {
  return (
    <div
      style={{
        padding: '18px',
        borderRadius: '14px',
        backgroundColor: '#f9fafb',
        border: '1px dashed #d1d5db',
        color: '#374151',
      }}
    >
      <strong>Fachlicher Hinweis:</strong> Die spätere Terminampel soll nicht
      dauerhaft auf einfachen Fortschrittsgrenzen basieren. Ziel ist ein
      Soll-/Ist-Vergleich über die Zeitachse zwischen PlannedStart, PlannedEnd
      und dem aktuellen Fortschritt.
    </div>
  )
}

function getStatusConfig(status) {
  if (status === 'green') {
    return {
      label: 'GRÜN',
      icon: '🟢',
      backgroundColor: '#dcfce7',
      borderColor: '#86efac',
      textColor: '#166534',
    }
  }

  if (status === 'yellow') {
    return {
      label: 'GELB',
      icon: '🟡',
      backgroundColor: '#fef9c3',
      borderColor: '#fde68a',
      textColor: '#854d0e',
    }
  }

  return {
    label: 'ROT',
    icon: '🔴',
    backgroundColor: '#fee2e2',
    borderColor: '#fca5a5',
    textColor: '#991b1b',
  }
}