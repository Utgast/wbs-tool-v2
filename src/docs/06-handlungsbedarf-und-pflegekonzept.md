# 06 - Datenpflege, Handlungsbedarf und Projektleitstelle

## 1. Gesamtziel

Das WBS Tool / ERP MVP soll keine reine Datensammlung und kein reines Dashboard sein.

Ziel ist eine Projektleitstelle, die Projektleiter unterstuetzt, Projekte planbar, steuerbar und lieferfaehig zu machen.

Leitsatz:

Eine Projektleitstelle, die mit minimalem Pflegeaufwand belastbare Daten erzeugt und daraus nachvollziehbare Handlungsempfehlungen fuer Projektleiter ableitet.

## 2. Zielhierarchie

1. Projekt erfolgreich liefern
2. Projekt wirksam steuern
3. Daten einfach und arbeitsnah pflegen
4. Lieferfaehigkeit bewerten
5. Handlungsbedarf erkennen
6. Massnahmen vorbereiten
7. Projektleiter entscheidet

## 3. Architekturprinzip: Datenpflege ist kein Selbstzweck

Der Benutzer arbeitet am Projekt, nicht am Tool.

Nicht:

"Bitte pflege erst alle Daten, damit das Dashboard funktioniert."

Sondern:

"Waehrend du dein Projekt steuerst, entstehen automatisch die Daten fuer Auswertung und Handlungsbedarf."

## 4. Architekturprinzip: Keine Auswertung ohne Eingabestrategie

Fuer jede neue KPI, Auswertung oder Score-Logik muessen kuenftig diese Fragen beantwortet werden:

1. Welche Daten braucht die Auswertung?
2. Wo entstehen diese Daten im Arbeitsalltag?
3. Sind diese Daten bereits im System vorhanden?
4. Falls nein: Wie kann der Benutzer sie mit minimalem Aufwand erfassen?
5. Welche Daten koennen automatisch abgeleitet werden?
6. Welche Eingaben sind wirklich Pflicht?
7. Was passiert, wenn Daten fehlen?

## 5. Architekturprinzip: System schlaegt vor - Mensch entscheidet

Das System unterstuetzt Entscheidungen.
Der Projektleiter trifft Entscheidungen.

Das System darf:

- erkennen
- berechnen
- priorisieren
- vorschlagen
- begruenden
- Alternativen anzeigen

Das System darf im MVP nicht:

- automatisch Personen verbindlich zuweisen
- automatisch Risiken endgueltig anlegen
- automatisch Prioritaeten unsichtbar festlegen
- automatisch Massnahmen verbindlich starten

Vorschlaege muessen sein:

- nachvollziehbar
- erklaerbar
- aenderbar
- bestaetigbar
- ablehnbar
- zurueckstellbar

Leitsatz:

Das System soll Entscheidungen vorbereiten, nicht Entscheidungen ersetzen.

Kurzform:

System schlaegt vor. Mensch entscheidet.

## 6. MVP-Dreieck fuer neue Features

Jedes neue Feature muss kuenftig drei Fragen bestehen:

### Steuerungswert

Hilft es dem Projektleiter, bessere Entscheidungen zu treffen?

### Pflegeleichtigkeit

Kann die Information schnell, zuverlaessig und arbeitsnah gepflegt werden?

### Ableitbarkeit

Kann das System daraus automatisch Mehrwert erzeugen?

Nur wenn alle drei Punkte erfuellt sind, hat das Feature hohen MVP-Wert.

## 7. Handlungsbedarf als Management-Sicht

Handlungsbedarf ist keine einfache Liste von Risiken oder Deliverables.

Handlungsbedarf beantwortet:

- Was gefaehrdet konkret die Lieferung?
- Was passiert, wenn nichts getan wird?
- Bis wann muss reagiert werden?
- Wer sollte handeln?
- Welche Massnahme hilft?

Ein Handlungsbedarf kann entstehen aus:

- Risiko
- Deliverable
- Kompetenz-Gap
- Ressourcen-Gap

Fuer MVP zunaechst als berechnete Sicht betrachten, nicht als persistente Entity.

## 8. Vorschlagslogik

Das System soll aus vorhandenen Daten Vorschlaege erzeugen fuer:

- Bearbeiter
- Prioritaet
- Massnahmen
- Faelligkeiten
- Risiken
- Handlungsbedarfe
- Ressourcenbesetzung

Vorschlaege duerfen nicht verbindlich automatisch umgesetzt werden.

Der Projektleiter kann:

- uebernehmen
- aendern
- ablehnen
- zurueckstellen
- kommentieren

## 9. Beispiel: Kompetenz-Gap

Muster:

Eingabe minimal:

- WBS-Element
- Leistungsphase
- Kompetenz
- Zeitraum
- Aufwand / Kapazitaet

Automatisch abgeleitet:

- passende Personen
- Kompetenzdeckung
- Kapazitaetsluecke
- Handlungsbedarf
- Prioritaet
- Erklaerung

Projektleiter entscheidet:

- vorgeschlagene Person uebernehmen
- andere Person waehlen
- Bedarf splitten
- zurueckstellen
- eskalieren

## 10. Beispiel: Risiko-Kandidat

System erkennt:

- Deliverable ueberfaellig
- Kompetenz fehlt
- Kapazitaetsluecke vorhanden
- kritisches WBS-Element betroffen

System schlaegt vor:

- moeglicher Risikokandidat
- betroffene Lieferung
- moegliche Auswirkung
- vorgeschlagener Owner
- vorgeschlagene Massnahme

Projektleiter entscheidet:

- als Risiko uebernehmen
- ablehnen
- Owner aendern
- Massnahme ergaenzen
- zurueckstellen

## 11. Beispiel: Ressourcenbedarf

System kennt:

- Bedarf
- Kompetenz
- Zeitraum
- verfuegbare Personen
- Personenkompetenzen
- Auslastung

System schlaegt mehrere Bearbeiter vor, sortiert nach:

- Kompetenz-Match
- Verfuegbarkeit
- aktueller Auslastung
- Level / Erfahrung
- Projektkontext

Projektleiter entscheidet:

- Person uebernehmen
- andere Person waehlen
- Bedarf splitten
- Bedarf offen lassen
- Eskalation ausloesen

## 12. Sprint-Gate fuer neue Features

Vor jedem neuen Sprint pruefen:

1. Welchen Steuerungswert erzeugt das Feature?
2. Welche Daten braucht es?
3. Wo entstehen diese Daten?
4. Wie einfach ist die Pflege?
5. Was kann automatisch abgeleitet werden?
6. Wo braucht es menschliche Bestaetigung?
7. Welche Entscheidung wird besser?
8. Was passiert bei fehlenden Daten?

Wenn diese Fragen nicht beantwortet sind, wird das Feature nicht umgesetzt.

## 13. Konsequenz fuer die weitere Roadmap

Der naechste technische Schwerpunkt darf nicht nur mehr Auswertung sein.

Stattdessen:

- einfache Pflege
- arbeitsnahe Eingaben
- automatische Vorschlaege
- erklaerbare Handlungsbedarfe
- Projektleiter behaelt Kontrolle

## 14. Kernaussagen

- Gute Steuerung beginnt nicht im Dashboard, sondern bei einfacher Datenpflege.
- Keine Auswertung ohne Eingabestrategie.
- Datenpflege darf kein eigener Prozess sein.
- System schlaegt vor. Mensch entscheidet.
- Das System soll Entscheidungen vorbereiten, nicht Entscheidungen ersetzen.
- Handlungsbedarf ist die Bruecke zwischen Daten und Projektentscheidung.
- Eine Projektleitstelle braucht nicht mehr Daten, sondern bessere Entscheidungen mit weniger Pflegeaufwand.
