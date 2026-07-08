# Fachmodell Projektsteuerung
## WBS Tool / ERP MVP

Version: 1.0  
Stand: 08.07.2026

---

# 1. Zielbild

## Vision

Das System entwickelt sich von einem reinen WBS-Tool zu einem integrierten Projektsteuerungs- und Ressourcenmanagementsystem.

Der fachliche Kern bleibt die WBS.

Alle weiteren Module bauen auf der Projektstruktur auf.

---

## Leitfrage

Das System soll nicht nur beantworten:

- Welche Aufgaben existieren?
- Wie viele Stunden wurden geplant?

sondern:

- Sind die Aufgaben fachlich besetzt?
- Sind die benötigten Kompetenzen vorhanden?
- Haben die verfügbaren Mitarbeiter ausreichend Kapazität?
- Wo entstehen Risiken?
- Welche Arbeitspakete gefährden Termin- oder Projektziele?

---

# 2. Grundprinzipien

## WBS ist die führende Datenquelle

Alle fachlichen Informationen werden einer WBS-Aufgabe zugeordnet.

```text
Projekt
    ↓
WBS
    ↓
Aufgabe
```

Die WBS bleibt der fachliche Anker für:

- Termine
- Stunden
- Kosten
- Fortschritt
- Ressourcenbedarf
- Kompetenzen
- Risiken

---

## Dashboard enthält keine Primärdaten

Das Dashboard speichert keine fachlichen Daten.

Es aggregiert ausschließlich Informationen aus:

- WBS
- Kompetenzen
- Ressourcen
- Kapazität
- Assignments

---

## Transparenz vor Automatisierung

Unsichere Zuordnungen werden sichtbar gemacht.

Das System darf niemals den Eindruck erwecken, mehr Sicherheit zu besitzen als tatsächlich vorhanden.

---

# 3. Fachliche Schlüssel

## Aufgaben-ID

Die Aufgaben-ID ist der primäre fachliche Schlüssel.

```text
Aufgaben-ID
```

Verwendung:

- Fortschritt
- Stunden
- Kosten
- Ressourcenbedarf
- Kompetenzen
- Assignments

---

## Person_Key

Verknüpft Aufgaben mit Personen.

Beispiele:

```text
tobias
ahmad
ibrahim
raghu
dennis
```

---

## Task_norm

Normalisierter Aufgabenname.

Dient zur Verknüpfung von:

```text
WBS
↔ Kompetenzmatrix
↔ Ablaufplan
```

---

## Match_Qualität

Beschreibt die Sicherheit einer automatischen Verknüpfung.

Werte:

```text
hoch
mittel
niedrig
ungeklärt
```

alternativ:

```text
1.00
0.75
0.50
0.00
```

---

## Offene_Punkte

Nicht auflösbare Zuordnungen.

Werden explizit ausgewiesen und niemals automatisch geschlossen.

---

# 4. Fachlicher Hauptprozess

## Projektsteuerung

```text
Projekt
    ↓
WBS-Aufgabe
    ↓
Benötigte Kompetenz
    ↓
Mögliche Personen
    ↓
Verfügbare Kapazität
    ↓
Besetzbarkeit
    ↓
Risiko
    ↓
Dashboard
```

---

# 5. Kernobjekte

---

## Project

Beschreibt ein Projekt.

### Attribute

```text
Id
ProjectNumber
Name
Description
Status
Customer
StartDate
EndDate
```

---

## WbsNode

Zentrales fachliches Objekt.

### Attribute

```text
Id
ProjectId
ParentId

VisibleWbsId
Title
Description

Status

PlannedStart
PlannedEnd

ActualStart
ActualEnd

PlannedHours
ActualHours

PlannedCost
ActualCost

ProgressPercent

Responsible

MatchQuality
```

### Beziehungen

```text
Project → WbsNode

WbsNode → WbsNode

WbsNode → ResourceDemand

WbsNode → RequiredCompetence

WbsNode → Assignment
```

---

## RequiredCompetence

Beschreibt fachliche Anforderungen einer Aufgabe.

### Attribute

```text
Id

WbsNodeId

CompetenceName

Source

IsMandatory

Note

MatchQuality
```

### Beispiele

```text
FM-Profil
CAD
GIS
EMF
Genehmigungsmanagement
Projektmanagement
Grundstücksmanagement
```

---

## Competence

Katalog aller Kompetenzen.

### Attribute

```text
Id

Name

Category

Description
```

### Beispiele

```text
Planung
CAD
FM-Profil
GIS
EMF
Bauleitung
Projektmanagement
Genehmigungsmanagement
```

---

## Person

Interne oder externe Ressource.

### Attribute

```text
Id

PersonKey

Name

Organisation

Role

Location
```

---

## PersonCompetence

Verknüpfung Person ↔ Kompetenz.

### Attribute

```text
Id

PersonId

CompetenceId

Level

IsPrimary

Note
```

---

## ResourceDemand

Beschreibt fachlichen Bedarf.

### Attribute

```text
Id

WbsNodeId

CompetenceId

RequiredHours

RequiredFrom

RequiredTo

AssignedHours

CoveragePercent
```

### Zweck

Verbindet:

```text
Aufgabe
↔ Kompetenz
↔ Stundenbedarf
```

---

## Assignment

Zuordnung von Personen zu Aufgaben.

### Attribute

```text
Id

WbsNodeId

PersonId

Role

PlannedHours

ActualHours

StartDate

EndDate

HourRate

HourRateCategory
```

---

## CapacityEntry

Kapazitätsplanung.

### Attribute

```text
Id

PersonId

ProjectId

StartDate

EndDate

AvailabilityPercent

AllocatedPercent

AssignmentContext

Note
```

---

# 6. Fachlicher Kernbegriff: Besetzbarkeit

## Definition

Eine Aufgabe gilt als besetzbar, wenn:

```text
Kompetenz vorhanden
+
Person vorhanden
+
Kapazität vorhanden
```

---

## Status Grün

```text
Kompetenz vorhanden
Person vorhanden
Kapazität ausreichend
```

---

## Status Gelb

```text
Kompetenz vorhanden

aber

Kapazität eingeschränkt

oder

Match_Qualität mittel/niedrig
```

---

## Status Rot

```text
Keine passende Kompetenz

oder

keine Person

oder

relevante Stunden unbesetzt
```

---

## Status Grau

```text
Daten unvollständig

oder

Zuordnung ungeklärt
```

---

# 7. KPI-Katalog MVP

---

## Struktur-KPI

### Projekt

```text
Hauptpakete
Unterpakete
Aufgaben
Elemente gesamt
```

---

## Fortschritt

### Projekt

```text
Fortschritt gesamt
```

### Hauptpaket

```text
Fortschritt Hauptpaket
```

---

## Stunden

### Projekt

```text
Planstunden
Ist-Stunden
Offene Stunden
```

### Hauptpaket

```text
Planstunden
Ist-Stunden
Reststunden
```

---

## Kompetenzdeckung

### Projekt

```text
Benötigte Kompetenzen
Abgedeckte Kompetenzen
Offene Kompetenzen
```

### Hauptpaket

```text
Kompetenzdeckung %
```

---

## Kapazitätsdeckung

### Projekt

```text
Verfügbare Kapazität
Zugeordnete Kapazität
Kapazitätslücke
```

### Hauptpaket

```text
Bedarf
Zugeordnet
Lücke
```

---

## Risiken

### Projekt

```text
Terminrisiken
Kompetenzrisiken
Kapazitätsrisiken
```

### Hauptpaket

```text
Ampelstatus
```

---

# 8. Aggregationsregeln

---

## Aufgabe → Hauptpaket

Aggregation erfolgt über alle untergeordneten Knoten.

Grundregel:

```text
Root-Knoten aggregiert alle Nachfolger
```

---

## Stunden

```text
Summe PlannedHours

Summe ActualHours
```

aller Nachfolger.

---

## Kosten

```text
Summe PlannedCost

Summe ActualCost
```

aller Nachfolger.

---

## Fortschritt

Gewichteter Durchschnitt.

Gewichtung:

```text
PlannedHours
```

---

## Kompetenzdeckung

```text
Abgedeckte Kompetenzen
/
Benötigte Kompetenzen
```

---

## Kapazitätsdeckung

```text
Zugeordnete Stunden
/
Benötigte Stunden
```

---

# 9. Dashboardkonzept

## Cockpit-Leiste

Zeigt Projektübersicht.

### KPI

```text
Fortschritt

Planstunden

Ist-Stunden

Terminrisiken

Kompetenzlücken

Kapazitätslücken
```

---

## Hauptpaket-Karten

### Beispiel

```text
Entwurfsplanung

Aufgaben: 18

Fortschritt: 46 %

Stunden
Plan: 320 h
Ist: 140 h
Offen: 180 h

Kompetenzen
4 / 5 abgedeckt

Kapazität
120 h zugeordnet
60 h offen

Status: Gelb
```

---

## Detailpanel

Beim Klick auf ein Hauptpaket.

### Inhalte

```text
Aufgaben

Kompetenzlücken

Kapazitätslücken

Unsichere Zuordnungen

Handlungsbedarf
```

---

# 10. Ampellogik MVP

---

## Grün

```text
Kompetenzdeckung >= 90 %

Kapazitätsdeckung >= 90 %
```

und

```text
keine offenen Risiken
```

---

## Gelb

```text
Kompetenzdeckung 60–89 %

oder

Kapazitätsdeckung 60–89 %
```

---

## Rot

```text
Kompetenzdeckung < 60 %

oder

Kapazitätsdeckung < 60 %
```

---

## Grau

```text
Daten unvollständig

oder

Match_Qualität ungeklärt
```

---

# 11. MVP-Abgrenzung

## Bestandteil MVP

```text
WBS

Fortschritt

Stunden

Kompetenzdeckung

Kapazitätsdeckung

Besetzbarkeit

Ampellogik

Dashboard

Drilldown
```

---

## Nicht Bestandteil MVP

```text
Automatische Ressourcenoptimierung

KI-gestützte Besetzungsvorschläge

Forecasting

Earned Value Management

Automatische Terminprognosen

Personenbezogene Leistungsbewertung

Kompetenzranking von Mitarbeitern
```

---

# 12. Zielarchitektur Version 1.0

```text
Project
    ↓
WbsNode
    ↓
ResourceDemand
    ↓
RequiredCompetence
    ↓
Assignment
    ↓
Person
    ↓
Capacity

═══════════════════════

Dashboard

(Aggregationsebene)
```

---

# Ergebnis

Das System entwickelt sich von:

```text
WBS + Dashboard
```

zu:

```text
Projekt
→ Aufgaben
→ Kompetenzen
→ Ressourcen
→ Kapazität
→ Besetzbarkeit
→ Risiko
→ Projektsteuerung
```

Das Dashboard wird damit zur zentralen Projektleitstelle, während die WBS die operative Arbeitsansicht bleibt.