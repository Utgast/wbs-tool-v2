# 16 - Ressourcen: KPI-Übersicht

## Ziel

Sprint-041.4 erweitert den Ressourcen-Reiter um eine einfache KPI-Übersicht.

Der Reiter zeigt jetzt auf einen Blick, wie viele Personen vorhanden sind, wie viele aktiv sind und wie viele Ressourcenbedarfe offen sind.

---

## Betroffene Dateien

| Datei | Änderung |
|---|---|
| `src/frontend/wbs-tool-ui/src/services/api.js` | Neue Funktion `getResourceDemands()` |
| `src/frontend/wbs-tool-ui/src/pages/ResourcesPage.tsx` | Erweiterung: KPI-Sektion mit 4 Karten, paralleles Laden von Personen + ResourceDemands |

**Backend:**
Keine Änderung – existierende Endpoints werden verwendet.

---

## Angezeigte KPIs

| KPI | Berechnung | Quelle |
|---|---|---|
| Personen insgesamt | count(PersonDto) | GET /api/persons |
| Aktive Personen | count(PersonDto where IsActive=true) | GET /api/persons |
| Inaktive Personen | count(PersonDto where IsActive=false) | GET /api/persons |
| Ressourcenbedarfe | count(ResourceDemandDto) | GET /api/resourcedemands |

---

## Verwendete Datenquellen

### Personen
Endpoint: `GET /api/persons`  
Quelle: `PersonsController.GetAllActive()`  
Rückgabe: Liste aktiver Personen (PersonDto)

### Ressourcenbedarfe
Endpoint: `GET /api/resourcedemands`  
Quelle: `ResourceDemandsController.GetAll()`  
Rückgabe: Liste aller Ressourcenbedarfe (ResourceDemandDto)

---

## Fehlende Daten

Technisch nicht aggregierbar ohne zusätzliche API:

- **Zugeordnete Stunden pro Person**: ResourceAssignmentsController hat keine GetAll()-Methode, nur pro WBS-Node. Aggregation würde neuen Endpoint erfordern.
- **Verfügbare Kapazität pro Person**: CapacityAllocationsController hat GetAll(), aber keine Verbindung zu Personen im DTO.
- **Auslastung**: Würde Kombination von mehreren Quellen erfordern.

Diese werden in Sprint-041.2+ hinzugefügt.

---

## MVP-Grenzen

Bewusst nicht Teil von Sprint-041.4:

- Keine Kapazitätsplanung
- Keine Forecast
- Keine Bearbeitung
- Keine Ressourcenoptimierung
- Keine automatischen Vorschläge
- Kein Kompetenzabgleich
- Kein Handlungsbedarf
- Keine Stunden-Aggregation pro Person

---

## Build-Ergebnis

```
dotnet build -o bin/_sprint041_4_build → Erfolgreich
npm run build → Erfolgreich
```

---

## Reifegradverbesserung

**Ressourcen-Reiter vor Sprint-041.4:** 15 %  
**Ressourcen-Reiter nach Sprint-041.4:** 30 %

Grund: Erstmals Überblick über Personenpool und Ressourcenbedarfe möglich. Basis für weitere Aggregationen gelegt.

---

## Nächster Mikro-Sprint

Sprint-042.1 – Prozesse-Reiter: LPH sichtbar machen

Ziel: Prozessphasen / LPH im Prozesse-Reiter anzeigen.
