# 08 - Datenflussmodell Handlungsbedarf

## 1. Ziel

Dieses Dokument beschreibt, wie aus operativen Projektdaten belastbarer Handlungsbedarf entsteht.

Zentrale Frage:

Wie entstehen entscheidungsfaehige Vorschlaege mit minimalem Pflegeaufwand?

Leitidee:

- Datenpflege bleibt arbeitsnah.
- Ableitungen erfolgen automatisch.
- Das System bereitet Entscheidungen vor.
- Der Projektleiter trifft die verbindliche Entscheidung.

## 2. Gesamtdatenfluss

Vollstaendiger Fluss:

Projektarbeit -> Datenerfassung -> Automatische Ableitungen -> Vorschlaege -> Menschliche Entscheidung -> Handlungsbedarf -> Projektleiter-Cockpit

Ablaufkette im Detail:

1. Projektarbeit
- Operative Steuerung in WBS, Risiken, Deliverables, Ressourcen und Kompetenzen.

2. Datenerfassung
- Es werden nur die im Arbeitsalltag ohnehin entstehenden Kerndaten gepflegt.

3. Automatische Ableitungen
- Das System berechnet Schwere, Dringlichkeit, Abhaengigkeitseffekt und Datenvertrauen aus vorhandenen Signalen.

4. Vorschlaege
- Das System erstellt Vorschlaege fuer Owner/Bearbeiter, Prioritaet, Massnahmen und Reaktionsdatum.

5. Menschliche Entscheidung
- Projektleiter uebernimmt, aendert, lehnt ab oder stellt zurueck.

6. Handlungsbedarf
- Ergebnis ist eine priorisierte Management-Sicht ueber alle Typen hinweg.

7. Projektleiter-Cockpit
- Darstellung der Top-Themen fuer schnelle Steuerung, ohne neue persistente Domae ne.

## 3. Handlungsbedarfstyp Risiko

Eingabedaten:

- Risiko
- Status
- Severity
- Bezug (Projekt/WBS/Termin)

Automatische Ableitung:

- Kritikalitaet (z. B. ueber Severity und Statuslage)
- Lieferfaehigkeitswirkung (z. B. Einfluss auf DeliveryStatus/Trigger)
- Priorisierung im quellenuebergreifenden Score

System schlaegt vor:

- Owner
- Massnahme
- Reaktionsdatum

Projektleiter entscheidet:

- uebernehmen
- aendern
- ablehnen
- zurueckstellen

## 4. Handlungsbedarfstyp Deliverable

Eingabedaten:

- Deliverable
- DueDate
- Status

Automatische Ableitung:

- Ueberfaelligkeit
- Kritikalitaet (z. B. ueber Status, Termin, Abhaengigkeit)
- Folgeauswirkung auf Lieferung

System schlaegt vor:

- Owner
- Prioritaet
- Massnahme

Projektleiter entscheidet:

- uebernehmen
- aendern
- ablehnen
- zurueckstellen

## 5. Handlungsbedarfstyp Kompetenz-Gap

Minimale Eingaben:

- Kompetenz
- Zeitraum
- Bedarf

Automatische Ableitung:

- Kompetenzdeckung
- verfuegbare Personen
- moegliche Kandidaten

System schlaegt vor:

- Bearbeiter
- Alternativen
- Prioritaet

Projektleiter entscheidet:

- uebernehmen
- aendern
- ablehnen
- zurueckstellen

## 6. Handlungsbedarfstyp Ressourcen-Gap

Minimale Eingaben:

- Stundenbedarf
- Zeitraum

Automatische Ableitung:

- Kapazitaetsluecke
- verfuegbare Ressourcen

System schlaegt vor:

- moegliche Besetzung
- Eskalation
- Prioritaet

Projektleiter entscheidet:

- uebernehmen
- aendern
- ablehnen
- zurueckstellen

## 7. Datenpflege-Matrix

| Handlungsbedarfstyp | Daten | Entstehung | Pflicht | Automatisch | Pflegeaufwand | Fallback |
|---|---|---|---|---|---|---|
| Risiko | Titel, Severity, Status, Owner, Bezug | Risikoarbeit im Projektalltag | Severity, Status, Bezug | Kritikalitaet, Wirkung, Prioritaet, Vorschlag | mittel | konservative Priorisierung + Confidence-Absenkung |
| Deliverable | Name, DueDate, Status, Owner | Lieferobjektpflege in Planung/Umsetzung | DueDate, Status | Ueberfaelligkeit, Folgewirkung, Prioritaet, Vorschlag | niedrig bis mittel | terminnahe Default-Regel + Hinweis auf fehlenden Kontext |
| Kompetenz-Gap | Kompetenz, Zeitraum, Bedarf | WBS-nahe Bedarfsdefinition | Kompetenz, Zeitraum | Deckung, Kandidaten, Prioritaet, Vorschlag | niedrig | Gap als beobachtungspflichtig markieren, Prioritaet ueber Schwelle ableiten |
| Ressourcen-Gap | Stundenbedarf, Zeitraum, Zuordnung | Ressourcenplanung im laufenden Projekt | Stundenbedarf, Zeitraum | OpenHours/Utilization, Besetzungsvorschlag, Prioritaet | niedrig bis mittel | Warnstufe aus Teilwerten + manuelle Klaerung markieren |

## 8. Vorschlagsmodell

Das System erzeugt Vorschlaege fuer:

- Owner
- Bearbeiter
- Prioritaet
- Massnahme
- Reaktionsdatum

### Owner

- Datenbasis: vorhandene Owner-Felder, Projektrollen, Verantwortungsbezug.
- Ableitungslogik: primaer vorhandener Owner, sonst Rollenfallback.
- Aenderbarkeit: voll aenderbar durch Projektleiter.

### Bearbeiter

- Datenbasis: Kompetenz- und Kapazitaetslage, Verfuegbarkeit.
- Ableitungslogik: Match + Verfuegbarkeit + Auslastung.
- Aenderbarkeit: uebernehmbar oder frei ersetzbar.

### Prioritaet

- Datenbasis: Impact, Urgency, DependencyEffect, Recoverability, Confidence.
- Ableitungslogik: gewichteter 1-5-Score mit Tie-Break ueber ReactionDate.
- Aenderbarkeit: korrigierbar mit bewusster Managemententscheidung.

### Massnahme

- Datenbasis: Typ, Status, Trigger, Schwere.
- Ableitungslogik: regelbasierter Massnahmenkatalog je Typ.
- Aenderbarkeit: editierbar, ablehnbar, zurueckstellbar.

### Reaktionsdatum

- Datenbasis: DueDate, Trigger, Status, Typ.
- Ableitungslogik: fristlogische Berechnung je Typ/Kritikalitaet.
- Aenderbarkeit: anpassbar durch Projektleiter.

## 9. Menschliche Entscheidung

Folgende Entscheidungen bleiben immer menschlich:

- Bearbeiter bestaetigen
- Prioritaet korrigieren
- Massnahme festlegen
- Handlungsbedarf akzeptieren
- Handlungsbedarf ablehnen
- Handlungsbedarf zurueckstellen

Begruendung:

- Die finale Verantwortung fuer Projektentscheidungen liegt bei der Projektleitung.
- Vorschlaege koennen in Grenzfaellen unvollstaendig oder kontextblind sein.
- Managemententscheidungen beinhalten Risikoappetit, Stakeholderlage und strategische Abwaegung, die nicht vollautomatisch ersetzbar sind.

## 10. Datenqualitaet

Kritische Daten:

- Severity/Status bei Risiken
- DueDate/Status bei Deliverables
- Kompetenzbedarf und Kompetenzdeckung
- OpenHours/Utilization bei Ressourcen
- belastbarer Projekt-/WBS-Bezug

Daten, die im MVP fehlen duerfen (mit markierter Unsicherheit):

- fein granularer DependencyEffect
- formalisiertes AffectedObjective je Eintrag
- ausdifferenzierte Recoverability-Bewertung

Fallback-Regeln:

- konservative Priorisierung bei fehlender Schluesselinformation
- Confidence-Absenkung bei Datenluecken
- explizite Explanation-Hinweise auf fehlende Eingaben
- Vorschlaege weiterhin moeglich, aber mit Unsicherheitskennzeichnung

Vorschlaege, die trotzdem moeglich bleiben:

- Basis-Prioritaet
- Owner/Bearbeiter-Fallback
- Standard-Massnahme je Typ
- vorlaeufiges Reaktionsdatum

## 11. Risiken des Ansatzes

### Risiko 1: Datenqualitaet uneinheitlich
- Ursache: unterschiedlich disziplinierte Pflege in den Quellmodulen.
- Auswirkung: inkonsistente Prioritaeten.
- Gegenmassnahme: klare Pflichtfelder, Plausibilitaetsregeln, sichtbare Confidence.

### Risiko 2: Fehlende Daten
- Ursache: lueckenhafte Erfassung bei Zeitdruck.
- Auswirkung: schwache Ableitung oder Fehlpriorisierung.
- Gegenmassnahme: Fallback-Logik, Unsicherheitsflag, gezielte Nachpflegehinweise.

### Risiko 3: Falsche Vorschlaege
- Ursache: zu grobe Heuristiken im MVP.
- Auswirkung: sinkendes Vertrauen der Projektleiter.
- Gegenmassnahme: erklaerbare Regeln, manuelle Korrektur, iterative Verfeinerung.

### Risiko 4: Zu viele Vorschlaege
- Ursache: geringe Filterung und fehlende Prioritaetsdisziplin.
- Auswirkung: Ueberlastung statt Entlastung.
- Gegenmassnahme: Top-N-Ansatz, klare Schwellwerte, Fokus auf steuerungsrelevante Items.

### Risiko 5: Akzeptanzprobleme
- Ursache: Eindruck von Zusatzprozess statt Arbeitserleichterung.
- Auswirkung: geringe Nutzung und niedriger Steuerungswert.
- Gegenmassnahme: arbeitsnahe Datenerfassung, sichtbarer Nutzen, Prinzip Mensch entscheidet.

## 12. Architektur-Empfehlung

Handlungsbedarf wird im MVP als berechnete Sicht umgesetzt, nicht als Entity und nicht als eigene CRUD-Domaene.

Begruendung:

- schnellster Weg zu Steuerungswert ohne zusaetzliche Prozesslast
- kompatibel mit bestehender Dashboard- und Modulstruktur
- vermeidet fruehe Persistenzkomplexitaet
- staerkt Ableitbarkeit aus operativen Daten
- haelt Projektleiter in finaler Entscheidungshoheit

Vorteile:

- geringe technische Einstiegskomplexitaet
- hohe Transparenz der Priorisierungslogik
- guter Fit zum MVP-Dreieck (Steuerungswert, Pflegeleichtigkeit, Ableitbarkeit)

## 13. Roadmap

Sprint-035:

MVP-Implementierungsplan Handlungsbedarf

Inhaltlicher Fokus von Sprint-035:

- konkrete Umsetzungsreihenfolge
- Regelkatalog fuer Ableitungen und Vorschlaege
- technische Integrationspunkte in bestehende Dashboard-Berechnung
- Validierungskriterien fuer Datenqualitaet und Nutzbarkeit

Hinweis:

Noch keine Implementierung im Rahmen dieses Dokuments.
