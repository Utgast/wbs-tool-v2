# 07 - Technisches Zielbild Handlungsbedarf

## 1. Ziel

Dieses Dokument beschreibt, wie Handlungsbedarf im WBS Tool / ERP MVP technisch entstehen soll, ohne neue persistente Fachstruktur einzufuehren.

Fokus des Zielbilds:

- berechnen
- aggregieren
- priorisieren
- vorschlagen

Nicht Bestandteil dieses Zielbilds:

- Persistierung als neue Entity
- CRUD-Lifecycle fuer Handlungsbedarf
- Workflow-Engine
- automatische verbindliche Entscheidungen

Technische Zielrichtung fuer MVP:

Handlungsbedarf wird als berechnete Management-Sicht aus bestehenden Modulen erzeugt und im Projektkontext priorisiert dargestellt.

## 2. Datenquellen

### 2.1 Risiken (Module Risks)

Vorhandene Daten:

- Risk-Titel
- Severity
- Status
- OwnerPersonId
- CreatedAt
- Projektbezug

Fehlende Daten fuer vollstaendige Handlungsbedarf-Bewertung:

- explizites AffectedObjective
- explizite Recoverability-Bewertung
- explizite Confidence-Bewertung

Pflichtdaten im MVP aus dieser Quelle:

- SourceId
- Severity
- Status
- Projektbezug

Automatisch ableitbar:

- Impact (aus Severity)
- Urgency (aus Status + zeitlichem Kontext)
- SuggestedAction (aus Regelkatalog je Severity/Status)
- SuggestedOwner (primaer aus OwnerPersonId)

### 2.2 Deliverables (Module Deliverables)

Vorhandene Daten:

- Name
- Type
- Status
- DueDate
- OwnerPersonId
- Projektbezug

Fehlende Daten:

- explizite Wirkungsklasse auf Projektziel
- explizite DependencyEffect-Bewertung

Pflichtdaten im MVP:

- SourceId
- Status
- DueDate
- Projektbezug

Automatisch ableitbar:

- Urgency (insbesondere ueberfaellig / nahes Faelligkeitsfenster)
- Impact (ueber Typ + Status + zeitliche Kritikalitaet)
- ReactionDate (aus DueDate und definierter Fristlogik)

### 2.3 Kompetenzdeckung (Competency Coverage)

Vorhandene Daten:

- RequiredCompetencies
- CoveredCompetencies
- MissingCompetencies
- CompetencyCoveragePercent

Fehlende Daten:

- direkte Zuordnung auf priorisierte Projektziele je Gap
- Gap-Schwere auf einzelner WBS-Objektebene als standardisierte Kennzahl

Pflichtdaten im MVP:

- MissingCompetencies
- CompetencyCoveragePercent

Automatisch ableitbar:

- Typ Kompetenz-Gap
- Impact/Urgency ueber Schwellwerte
- SuggestedAction (z. B. Besetzung, Umschichtung, Eskalation)

### 2.4 Ressourcenbedarf und Kapazitaet (Resource Demands/Assignments/Capacity)

Vorhandene Daten:

- PlannedDemandHours
- AssignedHours
- OpenHours
- CapacityHours
- UtilizationPercent

Fehlende Daten:

- standardisierte Ursache fuer OpenHours (fehlende Person vs. fehlende Kompetenz)
- explizite Abhaengigkeitswirkung pro Luecke

Pflichtdaten im MVP:

- OpenHours
- UtilizationPercent
- Projektbezug

Automatisch ableitbar:

- Ressourcen-Gap-Typ
- Urgency (OpenHours + Ueberlastung)
- SuggestedAction (Reallokation, Splitting, Priorisierung)

### 2.5 Lieferfaehigkeit (Delivery Status)

Vorhandene Daten:

- DeliveryStatus (Green/Yellow/Red)
- DeliveryStatusReason
- DeliveryStatusTrigger

Fehlende Daten:

- feinere Ursachenaufschluesselung auf Handlungsbedarf-Eintragsebene

Pflichtdaten im MVP:

- DeliveryStatus
- DeliveryStatusTrigger

Automatisch ableitbar:

- globale Priorisierungsvorspannung (z. B. Red-Status als Verstaerker)
- Explanation-Bausteine fuer Handlungsbedarf

### 2.6 WBS (Struktur- und Terminbezug)

Vorhandene Daten:

- WBS-Nodes mit Projektbezug
- Statusbezug (inkl. Blocked/Critical-Kontext)
- PlannedStart/PlannedEnd-Kontext

Fehlende Daten:

- einheitliche kritischer-Pfad-Kennzeichnung im MVP

Pflichtdaten im MVP:

- Projektbezug
- Terminbezug (falls vorhanden)

Automatisch ableitbar:

- DependencyEffect-Hinweise
- Terminische Urgency-Hinweise

## 3. Handlungsbedarfstypen MVP

### A) Risiko

Erkennungsregel:

- Risiko ist offen (nicht Accepted/Closed)
- Priorisiert bei hoher Severity und/oder kritischem Trigger

Auswirkung:

- direkte Gefaehrdung von Lieferziel, Kosten, Termin oder Qualitaet

Reaktionsfrist:

- kurz bei hoher Severity, sonst regelbasiert nach Status und Naehe zu betroffenen Terminen

Vorschlag:

- Mitigation definieren/aktualisieren, Eskalation pruefen, Folgeaufgaben terminieren

Owner-Vorschlag:

- primär vorhandener Risk-Owner, alternativ verantwortliche Projektrolle

### B) Deliverable

Erkennungsregel:

- Deliverable ist offen und ueberfaellig oder statuskritisch

Auswirkung:

- Lieferblockade oder Verzugsrisiko bei Abhaengigkeiten

Reaktionsfrist:

- sofort bei ueberfaellig, sonst vor DueDate nach Fristlogik

Vorschlag:

- Freigabe priorisieren, Bearbeitung absichern, Engpass beseitigen

Owner-Vorschlag:

- vorhandener Deliverable-Owner, alternativ fachlich zustaendige Rolle

### C) Kompetenz-Gap

Erkennungsregel:

- MissingCompetencies > 0 oder CompetencyCoveragePercent unter Schwelle

Auswirkung:

- Leistung kann nicht qualitaets- oder termingerecht erbracht werden

Reaktionsfrist:

- kurzfristig bei kritischer Unterdeckung, sonst stufenweise nach Auspraegung

Vorschlag:

- geeignete Personen vorschlagen, Bedarf splitten, temporaere Sicherung

Owner-Vorschlag:

- Projektleitung oder Ressourcenverantwortung je Organisationsregel

### D) Ressourcen-Gap

Erkennungsregel:

- OpenHours > 0 und/oder UtilizationPercent > 100

Auswirkung:

- Lieferverzug durch fehlende oder ueberlastete Kapazitaet

Reaktionsfrist:

- priorisiert nach Hoehe der Luecke und zeitlicher Naehe

Vorschlag:

- Reallokation, Priorisierung, Umfangsanpassung, Eskalation

Owner-Vorschlag:

- Ressourcenverantwortung mit Projektleiter-Abstimmung

## 4. Technisches Modell

Es wird keine neue persistente Entity eingefuehrt.

Arbeitsname der berechneten Sicht:

ManagementAttentionView

### Feldkatalog mit Herkunft und Berechnung

| Feld | Herkunft | Berechnung/Regel | Manuell oder automatisch |
|---|---|---|---|
| Type | Regelwerk | Typklasse (Risk, Deliverable, CompetencyGap, ResourceGap) | automatisch |
| SourceType | Quellmodul | technische Quellenkennzeichnung | automatisch |
| SourceId | Quellobjekt | ID des ausloesenden Datensatzes | automatisch |
| Title | Quellobjekt | Titel/Name oder generierter Titel | automatisch |
| Description | Quellobjekt + Regeltext | Kurzkontext + Hinweistext | automatisch, manuell ergaenzbar spaeter |
| Impact | Severity/KPI | Mapping auf Skala 1-5 | automatisch |
| Urgency | DueDate/Status/Trigger | Fristnahe + Kritikalitaet auf Skala 1-5 | automatisch |
| DependencyEffect | WBS/Trigger | Abhaengigkeitswirkung auf Skala 1-5 | automatisch |
| Recoverability | Regelwerk + Datensituation | Kompensierbarkeit auf Skala 1-5 | automatisch (MVP), spaeter manuell verfeinerbar |
| Confidence | Datenvollstaendigkeit | Bewertungsvertrauen auf Skala 1-5 | automatisch |
| PriorityScore | Bewertungsfaktoren | gewichtetes Scoring | automatisch |
| SuggestedOwner | Owner/Projektrolle | vorhandener Owner oder Rollenfallback | automatisch, manuell aenderbar |
| SuggestedAction | Regelkatalog | typ- und zustandsabhaengiger Vorschlag | automatisch, manuell aenderbar |
| Explanation | Trigger + Regelwerk | erklaerbarer Grundtext zur Priorisierung | automatisch |
| ReactionDate | DueDate + Fristregeln | spaetester Reaktionszeitpunkt | automatisch, manuell anpassbar |

## 5. Vorschlagslogik

Architekturprinzip:

System schlaegt vor. Mensch entscheidet.

### 5.1 Vorschlag Bearbeiter

Datenbasis:

- vorhandene Owner-Daten
- Projektrolle
- Kompetenz- und Kapazitaetsindikatoren

Berechnungslogik:

- primaer bestehender Owner
- fallback auf zustaendige Projektrolle
- optional Priorisierung nach Verfuegbarkeit/Match

Aenderbarkeit:

- voll aenderbar durch Projektleiter

### 5.2 Vorschlag Massnahmen

Datenbasis:

- Typ, Status, Trigger, Schweregrad

Berechnungslogik:

- regelbasierter Massnahmenkatalog je Typ
- kritische Trigger erzeugen eskalierende Vorschlaege

Aenderbarkeit:

- uebernehmbar, editierbar, ablehnbar, zurueckstellbar

### 5.3 Vorschlag Prioritaeten

Datenbasis:

- Impact, Urgency, DependencyEffect, Recoverability, Confidence

Berechnungslogik:

- gewichteter PriorityScore
- Tie-Breaker: frueheres ReactionDate, dann hoeherer Impact

Aenderbarkeit:

- System berechnet transparent; Projektleiter kann Priorisierung bewusst ueberschreiben

### 5.4 Vorschlag Reaktionsdatum

Datenbasis:

- DueDate, Status, Trigger, Typ

Berechnungslogik:

- regelbasierte Vorlaufzeit pro Typ
- bei Ueberfaelligkeit unmittelbare Reaktion

Aenderbarkeit:

- durch Projektleiter verschiebbar mit Begruendung

## 6. Datenpflege-Matrix

| Handlungsbedarfstyp | Benoetigte Daten | Quelle | Manuelle Eingabe | Automatische Ableitung | Pflegeaufwand | Fallback bei fehlenden Daten |
|---|---|---|---|---|---|---|
| Risiko | Titel, Severity, Status, Owner | Risks | Risikoanlage/-pflege | Impact, Urgency, SuggestedAction, Score | mittel | konservative Default-Bewertung + Kennzeichnung niedriger Confidence |
| Deliverable | Name, Status, DueDate, Owner | Deliverables | Deliverable-Pflege | Urgency, ReactionDate, SuggestedAction, Score | niedrig bis mittel | Default-ReactionDate + Prioritaet ueber Status ohne Feinkontext |
| Kompetenz-Gap | Required/Covered/Missing, Coverage% | Competency/Project Dashboard | arbeitsnahe Bedarfspflege in WBS-Kontext | Gap-Typ, Impact/Urgency, Suggestions, Score | niedrig | Gap als beobachtungspflichtig mit erhoehtem Unsicherheitsflag |
| Ressourcen-Gap | PlannedDemand, Assigned, OpenHours, Utilization | Resource Demands/Assignments/Capacity | Bedarf und Zuordnung im Arbeitsprozess | Urgency, SuggestedOwner, SuggestedAction, Score | niedrig bis mittel | Warnstufe aus OpenHours/Utilization auch bei Teilinformationen |

## 7. Prioritaetsmodell MVP

Bewertungsdimensionen auf Skala 1 bis 5:

- Impact
- Urgency
- DependencyEffect
- Recoverability
- Confidence

Vorgeschlagenes MVP-Scoring:

PriorityScore =
Impact * 0.35 +
Urgency * 0.25 +
DependencyEffect * 0.20 +
Recoverability * 0.10 +
Confidence * 0.10

Vorteile:

- quellenuebergreifend vergleichbar
- transparent und erklaerbar
- auf bestehende KPI-Lage abbildbar
- schnell MVP-faehig

Risiken:

- Gewichtung zunaechst heuristisch
- begrenzte Qualitaet bei unvollstaendigen Quelldaten
- Recoverability/Confidence anfangs nur regelbasiert

Fehlende Informationen:

- verbindliche Mapping-Tabelle fuer alle Schwellen
- standardisierte Ableitung von AffectedObjective
- organisationsspezifische Eskalationsregeln

Spaetere Erweiterungen:

- feinere Abhaengigkeitsmetriken
- lernende Gewichtungsanpassung
- historisierte Wirksamkeitsauswertung der Entscheidungen

## 8. Dashboard-Zielbild

Neuer fachlicher Bereich:

TOP HANDELN

Rahmen:

- maximal 10 Eintraege
- quellenuebergreifende Priorisierung
- keine typgetrennten Toplisten

Pro Eintrag:

- Prioritaet
- Typ
- Titel
- Auswirkung
- Vorschlag
- Owner
- Reaktionsdatum

Hinweis:

Dies ist ein technisches Zielbild fuer Datenaufbereitung und Informationsstruktur, keine UI-Implementierung.

## 9. Berechnete Sicht vs. Entity

### A) Berechnete Sicht

Aufwand:

- niedrig bis mittel

Nachvollziehbarkeit:

- hoch bei dokumentierten Regeln und Explanation-Feld

Datenqualitaet:

- gut bei klaren Quellenregeln, sensibel bei fehlenden Daten

Pflegeaufwand:

- moderat, Schwerpunkt auf Regelpflege statt Objektpflege

Historisierung:

- eingeschraenkt, nur indirekt ohne Zusatzmechanismus

### B) Persistente Entity

Aufwand:

- hoch (Modell, CRUD, Lifecycle, Governance)

Nachvollziehbarkeit:

- sehr hoch durch Zustandshistorie

Datenqualitaet:

- hoch, aber nur mit zusaetzlichem Pflegeaufwand

Pflegeaufwand:

- hoch (eigener Prozess droht)

Historisierung:

- sehr gut

Klare Empfehlung:

Fuer MVP wird Handlungsbedarf als berechnete Sicht umgesetzt.

## 10. Architekturentscheidung

ADR-Entscheidung (MVP):

Handlungsbedarf wird zunaechst als berechnete Sicht aus bestehenden Projektdaten bereitgestellt.

Begruendung:

- schnellster Weg zu Steuerungsmehrwert
- konsistent mit bestehender Dashboard-Architektur
- kein neuer Pflegeprozess fuer eigene Handlungsbedarf-Objekte
- Prinzipienkonform zu Datenpflege, Ableitbarkeit und Projektleiterkontrolle

Vorteile:

- schnelle Umsetzbarkeit
- klare Nachvollziehbarkeit ueber Regeln und Erklaerung
- geringe Einstiegskomplexitaet
- kein Schema-/Migrationsbedarf

Bewusst akzeptierte Nachteile:

- keine native Historie je Handlungsbedarf-Eintrag
- begrenzte Lifecycle-Abbildung im MVP
- Genauigkeit abhaengig von Datenqualitaet der Quellsysteme

## 11. Offene Fragen

- Wie wird AffectedObjective im MVP standardisiert aus vorhandenen Feldern abgeleitet?
- Welche konkrete Mapping-Matrix gilt fuer Impact/Urgency/DependencyEffect pro Typ?
- Welche verbindliche Reaktionsfristlogik gilt je Typ und Kritikalitaetsstufe?
- Wie wird Confidence bei teilweiser oder fehlender Datenlage konsistent gesetzt?
- Welche Rollenfallbacks gelten organisationsweit fuer SuggestedOwner?
- Ab welcher Schwelle wird aus wiederkehrenden Eintraegen eine Pflichteskalation?
- Wie wird ohne neue Entity eine minimale Verlaufssicht (Snapshots) bereitgestellt?
- Welche Regel hat Vorrang bei Konflikt zwischen Score und fachlicher Leitungsentscheidung?

## 12. Empfehlung

Klare MVP-Empfehlung:

Sprint-034 fokussiert einen Implementierungsplan fuer Handlungsbedarf als berechnete Dashboard-Sicht.

Begruendung:

- Das fachliche Zielbild ist definiert.
- Die Datenquellen sind identifiziert und grob bewertet.
- Das Prioritaets- und Vorschlagsprinzip ist festgelegt.
- Offene Punkte sind klar isoliert und sprintfaehig.

Empfohlener Sprinttitel:

Sprint-034 - Implementierungsplan Handlungsbedarf (berechnete Dashboard-Sicht)
