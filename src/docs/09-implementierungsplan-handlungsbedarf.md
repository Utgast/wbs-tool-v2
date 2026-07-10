# 09 - Implementierungsplan Handlungsbedarf MVP

## 1. Ziel

Dieses Dokument beschreibt, wie Handlungsbedarf im WBS Tool / ERP MVP mit minimalem Risiko und in kleinen, kontrollierbaren Sprints umgesetzt werden kann.

Leitprinzip fuer die Umsetzung:

- kleine Inkremente
- klare Abnahme je Sprint
- keine fruehe Komplexitaet
- maximaler Steuerungswert fuer Projektleiter

## 2. Ausgangslage

Vorhanden:

- Risiken
- Deliverables
- Kompetenzdeckung
- Kapazitaetsdaten
- Lieferfaehigkeit
- Projektleiter-Cockpit

Noch nicht vorhanden:

- Handlungsbedarfberechnung
- Vorschlagslogik
- Attention-Liste

Fachliche Einordnung:

Die notwendige Datengrundlage ist bereits in den Kernmodulen vorhanden. Es fehlt die quellenuebergreifende Berechnung, Priorisierung und strukturierte Vorschlagserzeugung fuer Management-Entscheidungen.

## 3. MVP-Grenzen

Explizite Grenzen fuer Sprint 1 der Umsetzung:

- keine Persistierung
- keine CRUD-Oberflaeche
- keine Historie
- keine automatische Anlage verbindlicher Objekte
- nur berechnete Sicht

Zusatzklarstellung:

- keine neue Entity
- keine eigene CRUD-Domaene
- keine workflowbasierte Lebenszyklussteuerung im MVP

## 4. Technische Bausteine

### Baustein A: ManagementAttentionView als DTO

Zweck:

- einheitliche, berechnete Sicht je Handlungsbedarf-Eintrag
- standardisierte Felder fuer Typ, Quelle, Prioritaet, Vorschlag und Erklaerung

Ergebnis:

- konsistentes Austauschformat fuer Berechnung und Cockpit-Darstellung

### Baustein B: ManagementAttentionService berechnet

Zweck:

- Aggregation der Daten aus Risiken, Deliverables, Kompetenz- und Ressourcenlage
- Ermittlung von Impact, Urgency, DependencyEffect, Recoverability, Confidence
- Berechnung von PriorityScore und ReactionDate

Ergebnis:

- quellenuebergreifende, priorisierte Attention-Eintraege

### Baustein C: Dashboard Integration Top Handlungsbedarf

Zweck:

- Einbindung der Top-Liste in das bestehende Projektleiter-Cockpit
- Begrenzung auf die steuerungsrelevantesten Eintraege (Top-N)

Ergebnis:

- sichtbarer Management-Mehrwert im bestehenden Cockpit-Kontext

### Baustein D: Vorschlagslogik

Zweck:

- Vorschlaege fuer Owner, Prioritaet und Reaktionsdatum
- spaeter erweiterbar um differenzierte Massnahmenvorschlaege

Ergebnis:

- systematische Entscheidungsunterstuetzung bei voller menschlicher Entscheidungshoheit

## 5. Sprint-Zerlegung

Vorgeschlagene technische Umsetzung:

### Sprint-036: ManagementAttentionView DTO

Umfang:

- Feldschnitt definieren
- Quellmapping je Typ festlegen
- Datenvollstaendigkeit und Pflichtfelder dokumentieren

Abnahmefokus:

- einheitliches DTO-Modell als Grundlage aller Folgebausteine

### Sprint-037: ManagementAttentionService

Umfang:

- Berechnungsfluss fuer alle vier Typen (Risiko, Deliverable, Kompetenz-Gap, Ressourcen-Gap)
- Prioritaetsberechnung mit Score-Regeln
- Erklaerungstexte und Confidence-Fallbacks

Abnahmefokus:

- reproduzierbare, nachvollziehbare Berechnung aus vorhandenen Daten

### Sprint-038: Owner-Vorschlaege

Umfang:

- Owner-Ableitungslogik je Typ
- Rollenfallbacks bei fehlendem Owner
- Kennzeichnung unsicherer Vorschlaege

Abnahmefokus:

- belastbare, erklaerbare Owner-Vorschlaege mit klarer Aenderbarkeit

### Sprint-039: Dashboard Integration

Umfang:

- Top-Handlungsbedarf als Bereich im Cockpit
- Prioritaet, Typ, Titel, Auswirkung, Vorschlag, Owner, Reaktionsdatum
- Fokus auf Top-N zur Vermeidung von Ueberfrachtung

Abnahmefokus:

- nutzbare Management-Sicht fuer operative Steuerung

### Sprint-040: Review und Validierung

Umfang:

- Regelreview mit Fachsicht
- Datenqualitaets- und Plausibilitaetspruefung
- Abgleich mit Steuerungswert und Pflegeleichtigkeit

Abnahmefokus:

- MVP-Qualitaet gesichert, Risiken transparent, naechste Ausbaustufe vorbereitet

## 6. Risiken

### Datenqualitaet

- Risiko: inkonsistente oder unvollstaendige Eingaben verfalschen Prioritaeten.
- Steuerung: Pflichtdaten je Typ, Plausibilitaetsregeln, Confidence-Kennzeichnung.

### Fehlende Daten

- Risiko: Ableitungen werden zu grob.
- Steuerung: konservative Fallback-Regeln und explizite Hinweise auf Unsicherheit.

### Zu viele Vorschlaege

- Risiko: Informationsueberlastung statt Steuerung.
- Steuerung: Top-N-Begrenzung und klare Priorisierungsschwellen.

### Falsche Priorisierung

- Risiko: unpassende Reihenfolge der Handlungsbedarfe.
- Steuerung: transparente Scoring-Regeln, Tie-Breaker, Review in Sprint-040.

### Akzeptanzprobleme

- Risiko: Projektleiter sehen keinen Mehrwert oder empfinden Zusatzaufwand.
- Steuerung: arbeitsnahe Datennutzung, erklaerbare Vorschlaege, Mensch entscheidet.

## 7. Definition of Done

Handlungsbedarf MVP gilt als fertig, wenn:

1. Eine berechnete, quellenuebergreifende Handlungsbedarf-Sicht verfuegbar ist.
2. Alle vier Typen (Risiko, Deliverable, Kompetenz-Gap, Ressourcen-Gap) in die Berechnung eingehen.
3. PriorityScore transparent aus dokumentierten Faktoren entsteht.
4. Vorschlaege fuer Owner, Prioritaet und Reaktionsdatum erzeugt werden.
5. Top-Handlungsbedarf im Cockpit fokussiert darstellbar ist.
6. Fallback- und Confidence-Regeln bei fehlenden Daten greifen.
7. Keine Persistierung, keine eigene CRUD-Domaene und keine Historie im MVP eingefuehrt wurden.

## 8. Roadmap nach MVP

Spaetere Optionen:

- Historisierung
- Manuelle Handlungsbedarfe
- Massnahmenmanagement
- Eskalationen
- Benachrichtigungen
- Workflow

Reihenfolgeempfehlung nach MVP:

1. Historisierung und Auswertung der Entscheidungswirksamkeit
2. Manuelle Ergaenzungen fuer Sonderfaelle
3. strukturiertes Massnahmenmanagement
4. Eskalations- und Benachrichtigungslogik
5. optionale Workflow-Vertiefung

## 9. Empfehlung

Die minimale Umsetzung mit groesstem Nutzen fuer den Projektleiter ist:

- berechnete Attention-Sicht aus bestehenden Modulen
- transparente Priorisierung mit klarem Score
- Top-N-Darstellung im Cockpit
- Vorschlaege fuer Owner, Prioritaet und Reaktionsdatum
- konsequente Beibehaltung von Mensch entscheidet

Warum diese Minimalumsetzung den groessten Nutzen liefert:

- hoher Steuerungswert bei geringem technischem Risiko
- kein neuer Pflegeprozess und keine Persistenzkomplexitaet
- schnelle Einfuehrung mit iterativer Verbesserung ueber kurze Sprints

Naechster technischer Schritt:

Sprint-036 - ManagementAttentionView DTO
