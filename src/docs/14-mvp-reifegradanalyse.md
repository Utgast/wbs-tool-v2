# 14 - MVP-Reifegradanalyse

## Ziel

Bewertung des aktuellen MVP-Reifegrads aller Hauptreiter im WBS Tool / ERP MVP.
Identifikation der groessten funktionalen Luecken.
Grundlage fuer die priorisierte Roadmap der naechsten Sprints.

---

## Dashboard

### Was funktioniert bereits

- Projektkennzahlen: Fortschritt, Plan/Ist-Stunden, Kosten
- Lieferfaehigkeitsampel (Red / Yellow / Green) mit Begruendung
- Handlungsbedarf-Sektion: TOP HANDELN mit bis zu 10 priorisierten Eintraegen
- HandlungsbedarfPanel: Ueberfaellig, Blockiert, offene Stunden, Auslastung, Kompetenz-Gaps
- Offene und kritische Risiken: KPI + Drilldown mit Statusaenderung
- Offene und ueberfaellige Deliverables: KPI + Drilldown mit Statusaenderung
- Ressourcen-KPIs: Bedarf, Zugeordnet, Kapazitaet, Auslastung
- Kompetenz-KPIs: Deckung, fehlende Kompetenzen
- WBS-Hauptpakete als Navigationskacheln
- Top Risiken und kritische Deliverables als kompakte Listen
- Detail-Panel bei Kachelklick mit Kontext und Bewertungshinweisen

### Was fehlt fuer den MVP

- Keine direkte Navigation von Dashboard-Eintraegen zu Detailseiten (ausser WBS)
- Handlungsbedarf-Karte zeigt SuggestedOwnerPersonId als rohes GUID, nicht als Name
- ReactionDate-Berechnung relativ zu heute funktioniert, aber Darstellung koennte mit "in N Tagen" verbessert werden

### Nice-to-have

- Historische Trendansicht der KPIs
- Export als PDF
- Ampellogik administrierbar aus Backend

### Reifegrad

**85 %**

Begruendung: Vollstaendig bedienbare Uebersicht mit Risiken, Deliverables, Handlungsbedarf, Ressourcen und Kompetenzen. Kleine Darstellungsluecken bei Owner-Name und Verlinkungen.

---

## WBS

### Was funktioniert bereits

- WBS-Baum mit allen Ebenen (Hauptpakete, Unterpakete, Aufgaben)
- Node erstellen per Template-Palette (Drag-and-Drop)
- Node bearbeiten: Titel, Stunden, Kosten, Verantwortlicher, RateCategory, Status, Termine
- Node deaktivieren
- Ressourcenzuordnungen pro Node (erstellen, bearbeiten)
- Fortschrittsberechnung aus Ist-Stunden
- Blockierte und ueberfaellige Nodes im Dashboard sichtbar

### Was fehlt fuer den MVP

- Kein visuelles Ampel-Feedback pro Node im Baum (Status-Farbe je Node)
- Keine Massenbearbeitung (z. B. alle Kinder-Nodes auf Status setzen)
- Kein Import/Export

### Nice-to-have

- Gantt-Darstellung
- Abhängigkeiten zwischen Nodes

### Reifegrad

**80 %**

Begruendung: Vollstaendige CRUD-Funktionalitaet, Template-Palette, Ressourcenzuordnung. Fehlt: Status-Ampel im Baum selbst.

---

## Ressourcen

### Was funktioniert bereits

- Ressourcen-KPIs im Dashboard: Bedarf, Zugeordnet, Kapazitaet, Auslastung
- ResourceAssignments pro WBS-Node (im WBS-Reiter)
- Kapazitaetsdaten werden berechnet und im Dashboard angezeigt

### Was fehlt fuer den MVP

- Kein dedizierter Ressourcen-Reiter: aktuell nur Placeholder "Dieses Modul ist im aktuellen Stand noch nicht erweitert."
- Keine Personen-Uebersicht (wer ist verfuegbar, welche Kapazitaet)
- Keine Kapazitaetspflege ueber UI (CapacityAllocations nur per Seed/API)
- Keine ResourceDemand-Pflege ueber UI (ResourceDemandsController vorhanden aber nicht im Frontend eingebunden)
- Keine Auslastungsansicht pro Person

### Nice-to-have

- Ressourcen-Kalenderansicht
- Skill-basierte Ressourcenvorschlaege

### Reifegrad

**15 %**

Begruendung: Backend vollstaendig vorhanden (ResourceDemandsController, ResourceAssignmentsController, CapacityAllocations), Frontend-Reiter ist leerer Placeholder. KPIs im Dashboard vorhanden, aber keine Datenpflege.

---

## Kompetenzen

### Was funktioniert bereits

- Kompetenzliste sichtbar (Name, Code)
- Klick auf Kompetenz zeigt zugeordnete Personen mit Proficiency-Level (Grundlagen / Fortgeschritten / Experte)
- Deckungsgrad wird im Dashboard als KPI angezeigt (MissingCompetencies, CompetencyCoveragePercent)

### Was fehlt fuer den MVP

- Keine Pflege von Kompetenzen (Neu anlegen, Bearbeiten, Deaktivieren) ueber UI
- Keine Zuweisung von Personen zu Kompetenzen ueber UI (PersonCompetenciesController vorhanden)
- Keine WBS-Anforderungspflege ueber UI (welche Kompetenz benoetigt welches Paket)
- Keine Gesamtuebersicht Person → Kompetenzen (nur Kompetenz → Personen)
- Keine Pflegemoeglickeit fuer Proficiency-Level

### Nice-to-have

- Kompetenzmatrix (Person × Kompetenz)
- Gap-Analyse pro Person

### Reifegrad

**40 %**

Begruendung: Lesezugriff vorhanden, Dashboard-KPIs vorhanden. Backend fuer Zuweisung vorhanden. Pflege-UI fehlt komplett.

---

## Prozesse

### Was funktioniert bereits

- Deliverables im Dashboard: offene und ueberfaellige Deliverables als KPI und Drilldown
- Statusaenderung von Deliverables direkt aus dem Dashboard-Panel
- DeliverablesController vorhanden: Full CRUD
- ProcessPhases-Modul vorhanden (Backend)

### Was fehlt fuer den MVP

- Kein dedizierter Prozesse-Reiter: aktuell nur Placeholder
- Keine Deliverable-Uebersicht nach LPH (Leistungsphasen)
- Kein Anlegen von Deliverables ueber UI
- Keine LPH-Struktur sichtbar
- Keine Prozessphase waehlbar beim Erstellen von Deliverables

### Nice-to-have

- Gantt fuer Deliverables nach LPH
- Meilenstein-Tracking

### Reifegrad

**20 %**

Begruendung: Backend und Dashboard-Integration vorhanden. Eigenstaendiger Reiter ist Placeholder. Kein Deliverable-CRUD ueber UI.

---

## Administration

### Was funktioniert bereits

- Ampel-Konfiguration fuer Terminabweichung, Auslastung, Ueberfaelligkeit
- Live-Vorschau der konfigurierten Schwellwerte mit Testwerten
- Aktivierung/Deaktivierung einzelner Ampelsysteme

### Was fehlt fuer den MVP

- Keine Persistierung der Einstellungen (alles nur lokaler State, keine Backend-Anbindung)
- Keine Personen-Stammdatenpflege (PersonsController ist Read-only)
- Keine Kompetenz-Stammdatenpflege
- Keine RateCategories-Pflege
- Keine TaskStatuses-Pflege

### Nice-to-have

- Rollenkonzept
- Benutzerverwaltung
- Audit-Log

### Reifegrad

**30 %**

Begruendung: Konfigurationsseite visuell vorhanden, aber ohne Persistierung wirkungslos. Stammdatenpflege komplett fehlend.

---

## Reifegradbewertung

| Reiter | Reifegrad | Status |
|---|---|---|
| Dashboard | 85 % | MVP-fertig (kleine Verbesserungen moeglich) |
| WBS | 80 % | MVP-fertig |
| Kompetenzen | 40 % | Nicht MVP-fertig |
| Administration | 30 % | Nicht MVP-fertig |
| Prozesse | 20 % | Nicht MVP-fertig |
| Ressourcen | 15 % | Nicht MVP-fertig |

---

## Top 10 MVP-Luecken

Sortiert nach: (1) Nutzen fuer Projektleiter, (2) MVP-Relevanz, (3) Umsetzungsaufwand

| # | Luecke | Nutzen | MVP-Relevant | Aufwand |
|---|---|---|---|---|
| 1 | Ressourcen-Reiter: Personen + Kapazitaeten sichtbar machen | Sehr hoch | Ja | Mittel |
| 2 | Prozesse-Reiter: Deliverable-Uebersicht nach LPH | Sehr hoch | Ja | Mittel |
| 3 | Kompetenzen: Zuweisung Person → Kompetenz ueber UI | Hoch | Ja | Klein |
| 4 | Admin: Schwellwert-Konfiguration persistieren (Backend-Anbindung) | Hoch | Ja | Mittel |
| 5 | Prozesse: Deliverables anlegen und bearbeiten ueber UI | Hoch | Ja | Mittel |
| 6 | Kompetenzen: Kompetenz anlegen und bearbeiten ueber UI | Mittel | Ja | Klein |
| 7 | WBS: Status-Ampel pro Node im Baum sichtbar | Mittel | Ja | Klein |
| 8 | Ressourcen: Kapazitaeten und ResourceDemands ueber UI pflegbar | Mittel | Ja | Gross |
| 9 | Admin: Personen-Stammdaten anlegen und bearbeiten | Mittel | Ja | Mittel |
| 10 | Dashboard: SuggestedOwnerPersonId als Name statt GUID anzeigen | Niedrig | Nein | Klein |

---

## Empfohlene Roadmap

### Sprint-041

**Ressourcen-Reiter: Personen und Kapazitaeten sichtbar**

- Personen aus PersonsController laden und anzeigen
- Zugeordnete Stunden pro Person aus ResourceAssignments aggregieren
- Kapazitaet pro Person aus CapacityAllocations anzeigen
- Einfache Read-only-Tabelle genuegt fuer MVP

Aufwand: Mittel

---

### Sprint-042

**Prozesse-Reiter: Deliverable-Uebersicht**

- Deliverables laden und nach ProcessPhase / LPH gruppiert anzeigen
- Status, Faelligkeit, Verantwortlicher sichtbar
- Bestehenden DeliverablesController nutzen

Aufwand: Mittel

---

### Sprint-043

**Kompetenzen vervollstaendigen**

- Zuweisung Person → Kompetenz ueber UI (PersonCompetenciesController nutzen)
- Kompetenz anlegen und bearbeiten

Aufwand: Klein bis Mittel

---

### Sprint-044

**Administration: Persistierung und Stammdaten**

- Schwellwert-Konfiguration im Backend speichern (neue Endpoint-Gruppe)
- Personen-Stammdaten anlegen und bearbeiten

Aufwand: Mittel

---

### Sprint-045

**WBS-Qualitaet und Dashboard-Verfeinerung**

- Status-Ampel pro Node im Baum sichtbar
- Dashboard: Owner-Name statt GUID bei Handlungsbedarf
- Kleinere Usability-Verbesserungen auf Basis Praxistests

Aufwand: Klein bis Mittel

---

## Wichtigste Erkenntnisse

1. **Backend ist dem Frontend weit voraus.** Ressourcen, Deliverables, Kompetenzen und Prozesse haben vollstaendige Backend-APIs. Die Frontend-Reiter sind Placeholder.

2. **Dashboard und WBS sind MVP-reif.** Diese beiden Reiter koennen produktiv genutzt werden.

3. **Ressourcen-Reiter ist der groesste blinde Fleck.** Kapazitaeten und Personendaten fehlen im Frontend komplett, obwohl im Dashboard bereits KPIs daraus berechnet werden.

4. **Administration ohne Persistierung ist wirkungslos.** Die Konfigurationsseite existiert visuell, speichert aber nichts.

5. **Prozesse-Reiter hat den hoechsten Nachholbedarf nach Ressourcen.** Deliverables werden im Dashboard bereits verwaltet, aber ein eigenstaendiger LPH-orientierter Ueberblick fehlt.
