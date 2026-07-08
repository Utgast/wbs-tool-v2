# KPI-Katalog
## WBS Tool / ERP MVP

Version: 1.1  
Stand: 08.07.2026

---

# 1. Ziel

Dieser KPI-Katalog definiert die fachlichen Kennzahlen des WBS Tool / ERP MVP.

Ziele:

- Einheitliche KPI-Definition
- Nachvollziehbare Berechnungsregeln
- Einheitliche Dashboard-Darstellung
- Vermeidung widersprüchlicher Auswertungen
- Grundlage für API-Erweiterungen
- Grundlage für Dashboard-ViewModels
- Grundlage für spätere Testfälle
- Grundlage für spätere Ampellogik

Das Dashboard soll sich von einem reinen Kennzahlencockpit zu einer Projektleitstelle entwickeln.

Es soll nicht nur anzeigen:

```text
Was ist geplant?
Was wurde verbraucht?
```

sondern zusätzlich beantworten:

```text
Sind Aufgaben fachlich besetzbar?
Sind benötigte Kompetenzen vorhanden?
Sind Kapazitäten ausreichend?
Wo besteht Handlungsbedarf?
Sind die Daten belastbar?
```

---

# 2. KPI-Ebenen

Das System unterscheidet drei fachliche Auswertungsebenen:

```text
Projekt
    ↓
Hauptpaket
    ↓
Aufgabe
```

Die Aggregation erfolgt grundsätzlich:

```text
Aufgabe
→ Hauptpaket
→ Projekt
```

---

# 3. Fachliche Grunddefinitionen

## 3.1 WBS-Knoten

Ein WBS-Knoten ist jedes Element der WBS-Hierarchie.

Beispiele:

```text
1 Arbeitsvorbereitung
1.1 Unterlagen sichten
1.1.1 Genehmigungsunterlagen prüfen
```

Ein WBS-Knoten kann sein:

- Hauptpaket
- Unterpaket
- Aufgabe
- Strukturknoten

---

## 3.2 Hauptpaket

Ein Hauptpaket ist ein WBS-Knoten der obersten fachlichen Arbeitsebene.

MVP-Regel:

```text
Hauptpaket = WbsNode.Level == 1
```

Hauptpakete dienen im Dashboard als zentrale Steuerungskacheln.

---

## 3.3 Unterpaket

Ein Unterpaket ist ein WBS-Knoten unterhalb eines Hauptpakets, der selbst weitere Knoten enthalten kann.

Unterpakete dienen primär der Strukturierung.

---

## 3.4 Aufgabe

Eine Aufgabe ist ein operativ planbarer WBS-Knoten.

Für operative KPIs gilt im MVP:

```text
Aufgabe = Blattknoten
```

oder:

```text
Aufgabe = WBS-Knoten ohne aktive Child-Knoten
```

Alternativ kann ein WBS-Knoten auch dann als Aufgabe gelten, wenn er operative Planungsinformationen trägt, z. B.:

```text
PlannedHours
PlannedEnd
Status
ResourceAssignment
RequiredCompetency
```

Die bevorzugte MVP-Regel lautet jedoch:

```text
Operative KPIs zählen nur Blattknoten.
```

---

## 3.5 Blattknoten

Ein Blattknoten ist ein WBS-Knoten ohne aktive untergeordnete WBS-Knoten.

```text
Blattknoten = keine aktiven Child-Knoten
```

Blattknoten sind die bevorzugte Basis für operative KPIs.

---

## 3.6 Struktur-KPI vs. operative KPI

Struktur-KPIs zählen WBS-Strukturen.

Beispiele:

```text
Hauptpakete
Unterpakete
Elemente gesamt
```

Operative KPIs bewerten bearbeitbare Aufgaben.

Beispiele:

```text
Fortschritt
Überfälligkeit
Blockierung
Kompetenzdeckung
Besetzbarkeit
Handlungsbedarf
```

MVP-Regel:

```text
Struktur-KPIs:
Alle aktiven WBS-Knoten

Operative KPIs:
Nur aktive Blattknoten
```

---

## 3.7 Aktive Aufgabe

Eine Aufgabe gilt als aktiv, wenn:

```text
WbsNode.IsActive = true
```

und sie nicht fachlich abgeschlossen ist.

Abgeschlossene Status können sein:

```text
Done
Closed
Delivered
Abgeschlossen
```

---

## 3.8 Aktive Person

Eine Person gilt als aktiv, wenn:

```text
Person.IsActive = true
```

Platzhalter-Personen können aktiv sein, gelten aber als gesonderte Kategorie.

Beispiele für Platzhalter:

```text
TBD
External
Owner
Contractor
Unassigned
```

---

## 3.9 Pflichtkompetenz

Eine Pflichtkompetenz ist eine Kompetenz, die für die Bearbeitung einer Aufgabe zwingend erforderlich ist.

Technische Grundlage:

```text
WbsRequiredCompetency
```

MVP-Regel:

Wenn kein eigenes Pflichtfeld vorhanden ist, gelten alle WbsRequiredCompetency-Einträge zunächst als erforderlich.

---

## 3.10 Kompetenzdeckung

Kompetenzdeckung beschreibt, ob benötigte Kompetenzen im verfügbaren Personen-/Kompetenzmodell vorhanden sind.

Basis:

```text
WbsRequiredCompetency
↔
PersonCompetency
```

---

## 3.11 Fachliche Besetzbarkeit

Eine Aufgabe ist fachlich besetzbar, wenn:

```text
alle Pflichtkompetenzen der Aufgabe
durch mindestens eine aktive Person abgedeckt werden
```

Kapazität wird hierbei noch nicht berücksichtigt.

---

## 3.12 Kapazitive Besetzbarkeit

Eine Aufgabe ist kapazitiv besetzbar, wenn:

```text
fachlich besetzbar
UND
im relevanten Zeitraum ausreichend Kapazität vorhanden
```

Kapazitive Besetzbarkeit ist eine spätere Ausbaustufe und nicht zwingend Teil der ersten KPI-Erweiterung.

---

## 3.13 Blockierte Aufgabe

Eine Aufgabe gilt als blockiert, wenn:

```text
IsBlocked = true
```

oder der Status einem blockierenden Status entspricht, z. B.:

```text
Blocked
OnHold
```

---

## 3.14 Kritische Aufgabe

Eine Aufgabe gilt als kritisch, wenn sie ein erhöhtes Risiko trägt.

Beispiele:

```text
Status = Critical
RiskLevel = High
```

MVP-Hinweis:

```text
Blockiert und kritisch sind getrennte fachliche Zustände.
```

Eine kritische Aufgabe ist nicht automatisch blockiert.

---

## 3.15 Handlungsbedarf

Handlungsbedarf liegt vor, wenn eine Aufgabe oder ein Hauptpaket steuerungsrelevant auffällig ist.

Beispiele:

```text
überfällig
blockiert
kritisch
Kompetenz fehlt
nicht fachlich besetzbar
Kapazität fehlt
Datenlage unvollständig
```

---

## 3.16 Datenqualität

Datenqualität beschreibt, ob eine KPI belastbar berechnet werden kann.

Beispiele für unvollständige Daten:

```text
fehlendes PlannedEnd
fehlende PlannedHours
fehlendes Kompetenzmapping
ungeklärte MatchQuality
fehlende Ressourcenzuordnung
```

---

# 4. Grundregeln für Aggregation

## 4.1 Aggregation Aufgabe → Hauptpaket

Ein Hauptpaket aggregiert alle operativen Aufgaben unterhalb dieses Hauptpakets.

MVP-Regel:

```text
Alle aktiven Blattknoten unterhalb des Hauptpakets
```

---

## 4.2 Aggregation Hauptpaket → Projekt

Das Projekt aggregiert alle Hauptpakete beziehungsweise alle operativen Aufgaben des Projekts.

---

## 4.3 Summen-KPIs

Summen werden addiert.

Beispiele:

```text
Planstunden
Ist-Stunden
Plankosten
Ist-Kosten
Ressourcenbedarf
Zugeordnete Stunden
Kapazitätsstunden
```

Formel:

```text
Summe aller relevanten Einzelwerte
```

---

## 4.4 Prozent-KPIs

Prozentwerte werden nicht gemittelt.

Falsch:

```text
Durchschnitt der Prozentwerte einzelner Aufgaben
```

Richtig:

```text
Prozentwert aus aggregierten Basiswerten neu berechnen
```

Beispiel:

```text
Sum(ActualHours)
/
Sum(PlannedHours)
*
100
```

---

## 4.5 Umgang mit fehlenden Basiswerten

Wenn notwendige Basiswerte fehlen, darf keine scheinpräzise KPI erzeugt werden.

Beispiele:

```text
PlannedHours = 0
CapacityHours = 0
RequiredCompetencies = 0
```

MVP-Regel:

```text
Wenn Divisor = 0:
KPI = null oder 0 gemäß fachlicher Definition
Status = Grau, wenn Aussage nicht belastbar ist
```

---

# 5. KPI-Kategorien

## Kategorie A – Struktur

Beantwortet:

```text
Wie groß und wie tief ist das Projekt strukturiert?
```

---

## Kategorie B – Fortschritt

Beantwortet:

```text
Wie weit sind wir fachlich?
```

---

## Kategorie C – Aufwandsverbrauch

Beantwortet:

```text
Wie viel Aufwand wurde im Verhältnis zur Planung verbraucht?
```

---

## Kategorie D – Aufwand / Kosten

Beantwortet:

```text
Was wurde geplant?
Was wurde verbraucht?
Welche Abweichungen entstehen?
```

---

## Kategorie E – Ressourcenzuordnung

Beantwortet:

```text
Welche geplanten Bedarfe sind bereits Personen oder Rollen zugeordnet?
```

---

## Kategorie F – Kapazität

Beantwortet:

```text
Welche Kapazität ist vorhanden?
Wie stark ist sie belegt?
```

---

## Kategorie G – Kompetenz

Beantwortet:

```text
Sind die benötigten Fähigkeiten vorhanden?
```

---

## Kategorie H – Besetzbarkeit

Beantwortet:

```text
Können Aufgaben fachlich und später kapazitiv besetzt werden?
```

---

## Kategorie I – Risiko / Handlungsbedarf

Beantwortet:

```text
Wo muss der Projektleiter handeln?
```

---

## Kategorie J – Datenqualität

Beantwortet:

```text
Sind die KPI belastbar?
```

---

# 6. Struktur-KPI

## KPI: Hauptpakete

### Beschreibung

Anzahl aktiver Hauptpakete.

### Quelle

```text
WbsNode
```

### Formel

```text
Count(WbsNode where IsActive = true and Level = 1)
```

### Ebene

```text
Projekt
```

### MVP

```text
Ja
```

---

## KPI: Unterpakete

### Beschreibung

Anzahl aktiver Unterpakete unterhalb eines Hauptpakets.

### Quelle

```text
WbsNode
```

### Formel

```text
Count(aktive Child-Knoten unterhalb Hauptpaket, ohne Hauptpaket selbst)
```

### Ebene

```text
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Aufgabenanzahl

### Beschreibung

Anzahl operativer Aufgaben.

### Quelle

```text
WbsNode
```

### Formel

```text
Count(aktive Blattknoten)
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Elemente gesamt

### Beschreibung

Alle aktiven WBS-Knoten unterhalb eines Hauptpakets.

### Quelle

```text
WbsNode
```

### Formel

```text
Count(alle aktiven Nachfolger)
```

### Ebene

```text
Hauptpaket
```

### MVP

```text
Ja
```

---

# 7. Fortschritts-KPI

## KPI: Fortschritt %

### Beschreibung

Fachlicher Bearbeitungsfortschritt.

### Bevorzugte Quelle

```text
WbsNode.ProgressPercent
```

oder vergleichbares Fortschrittsfeld.

### Formel

Wenn Fortschrittswerte vorhanden sind:

```text
gewichteter Fortschritt auf Basis PlannedHours
```

Beispiel:

```text
Sum(ProgressPercent * PlannedHours)
/
Sum(PlannedHours)
```

### Fallback

Falls kein fachlicher Fortschrittswert vorhanden ist, darf ersatzweise der Aufwandsverbrauch separat angezeigt werden.

Wichtig:

```text
Fortschritt ist nicht identisch mit Aufwandsverbrauch.
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja, sofern Daten vorhanden
```

---

## KPI: Aufwandsverbrauch %

### Beschreibung

Verhältnis von Ist-Stunden zu Planstunden.

### Quelle

```text
WbsNode.PlannedHours
WbsNode.ActualHours
```

### Formel

```text
ActualHours
/
PlannedHours
*
100
```

### Bedeutung

Diese KPI zeigt nicht zwingend fachlichen Fortschritt, sondern die Verbrauchsquote des geplanten Aufwands.

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

# 8. Termin-KPI

## KPI: Überfällige Aufgaben

### Beschreibung

Aktive operative Aufgaben, deren geplantes Ende überschritten ist und die nicht abgeschlossen sind.

### Quelle

```text
WbsNode.PlannedEnd
WbsNode.Status
```

### Formel

```text
PlannedEnd vorhanden
UND PlannedEnd < aktuelles Tagesdatum
UND Status nicht in [Done, Closed, Delivered, Abgeschlossen]
```

### Datum

MVP-Regel:

```text
aktuelles lokales Tagesdatum
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Aufgaben ohne Planende

### Beschreibung

Aktive operative Aufgaben ohne geplantes Enddatum.

### Quelle

```text
WbsNode.PlannedEnd
```

### Formel

```text
Count(aktive Aufgaben where PlannedEnd is null)
```

### Bedeutung

Diese Aufgaben werden nicht als überfällig gezählt, erzeugen aber ein Datenqualitätsrisiko.

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja, Datenqualität
```

---

# 9. Blockierungs- und Kritikalitäts-KPI

## KPI: Blockierte Aufgaben

### Beschreibung

Aufgaben, die aktuell nicht weiterbearbeitet werden können.

### Quelle

```text
WbsNode.IsBlocked
WbsNode.Status
```

### Formel

```text
IsBlocked = true
ODER Status in [Blocked, OnHold]
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Kritische Aufgaben

### Beschreibung

Aufgaben mit erhöhtem Risiko oder kritischem Status.

### Quelle

```text
WbsNode.Status
```

### Formel

```text
Status = Critical
```

oder zukünftig:

```text
RiskLevel = High
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Optional
```

---

# 10. Aufwand- und Kosten-KPI

## KPI: Planstunden

### Beschreibung

Geplante Stunden.

### Quelle

```text
WbsNode.PlannedHours
```

### Formel

```text
Sum(PlannedHours)
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Ist-Stunden

### Beschreibung

Tatsächlich verbuchte Stunden.

### Quelle

```text
WbsNode.ActualHours
```

### Formel

```text
Sum(ActualHours)
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Offene Stunden

### Beschreibung

Differenz zwischen Planstunden und Ist-Stunden.

### Formel

```text
PlannedHours - ActualHours
```

### Hinweis

Negative Werte weisen auf eine Überschreitung hin.

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Plan-Kosten

### Quelle

```text
WbsNode.PlannedCost
```

oder importierte Plankosten.

### Formel

```text
Sum(PlannedCost)
```

### MVP

```text
Ja, wenn Daten vorhanden
```

---

## KPI: Ist-Kosten

### Quelle

```text
WbsNode.ActualCost
```

oder importierte Ist-Kosten.

### Formel

```text
Sum(ActualCost)
```

### MVP

```text
Ja, wenn Daten vorhanden
```

---

## KPI: Kostenabweichung

### Formel

```text
ActualCost - PlannedCost
```

### MVP

```text
Optional
```

---

# 11. Ressourcenzuordnungs-KPI

## KPI: Ressourcenbedarf

### Beschreibung

Geplanter Ressourcenbedarf.

### Quelle

```text
ResourceDemand
```

### Formel

```text
Sum(ResourceDemand.PlannedHours)
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Zugeordnete Stunden

### Beschreibung

Bereits zugeordnete Stunden aus Assignments.

### Quelle

```text
ResourceAssignment
```

### Formel

```text
Sum(ResourceAssignment.PlannedHours)
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Nicht zugeordnete Stunden

### Formel

```text
ResourceDemand.PlannedHours
-
ResourceAssignment.PlannedHours
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Aufgaben ohne Assignment

### Beschreibung

Operative Aufgaben ohne Ressourcenzuordnung.

### Formel

```text
Count(aktive Aufgaben ohne aktives ResourceAssignment)
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

# 12. Kapazitäts-KPI

## KPI: Verfügbare Kapazität

### Beschreibung

Verfügbare geplante Kapazität im Projektkontext.

### Quelle

```text
CapacityAllocation
```

### Formel

```text
Sum(CapacityHours)
```

Falls technisch aktuell `PlannedHours` verwendet wird, ist fachlich zu klären, ob dieses Feld tatsächlich verfügbare Kapazität darstellt.

### Ebene

```text
Projekt
```

### MVP

```text
Ja
```

---

## KPI: Auslastung %

### Beschreibung

Verhältnis von zugeordneten Stunden zu verfügbarer Kapazität.

### Formel

```text
AssignedHours
/
CapacityHours
*
100
```

### Sonderfall

```text
Wenn CapacityHours = 0:
Auslastung nicht belastbar
Status = Grau
```

### Ebene

```text
Projekt
```

### MVP

```text
Ja
```

---

## KPI: Kapazitätslücke

### Beschreibung

Differenz zwischen Ressourcenbedarf und zugeordneter beziehungsweise verfügbarer Kapazität.

### MVP-Formel

```text
PlannedDemandHours - AssignedHours
```

### Erweiterte Formel später

```text
PlannedDemandHours - AvailableCapacityHours
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

# 13. Kompetenz-KPI

## KPI: Benötigte Kompetenzen

### Beschreibung

Anzahl unterschiedlicher benötigter Kompetenzen.

### Quelle

```text
WbsRequiredCompetency
```

### Formel

```text
Distinct Count(CompetencyId)
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Abgedeckte Kompetenzen

### Beschreibung

Benötigte Kompetenzen, die mindestens einer aktiven Person zugeordnet sind.

### Quelle

```text
WbsRequiredCompetency
PersonCompetency
Person
```

### Formel

```text
Required CompetencyId
INTERSECT
aktive PersonCompetency.CompetencyId
```

### Bedingung

```text
Person.IsActive = true
PersonCompetency.IsActive = true
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Fehlende Kompetenzen

### Beschreibung

Benötigte Kompetenzen ohne aktive Person mit dieser Kompetenz.

### Formel

```text
RequiredCompetencies
MINUS
CoveredCompetencies
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Kompetenzdeckung %

### Beschreibung

Anteil der abgedeckten benötigten Kompetenzen.

### Formel

```text
CoveredCompetencies
/
RequiredCompetencies
*
100
```

### Sonderfall

```text
Wenn RequiredCompetencies = 0:
KPI nicht belastbar oder 100 %, abhängig von fachlicher Entscheidung
```

MVP-Empfehlung:

```text
Wenn keine RequiredCompetencies gepflegt sind:
Status = Grau
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Kompetenzvollständigkeit je Aufgabe

### Beschreibung

Eine Aufgabe gilt kompetenzseitig vollständig abgedeckt, wenn alle Pflichtkompetenzen erfüllt sind.

### Formel

```text
Alle mandatory RequiredCompetencies der Aufgabe
sind durch aktive PersonCompetencies abgedeckt
```

### Status

```text
vollständig
teilweise
nicht abgedeckt
ungeklärt
```

### MVP

```text
Ja, für Besetzbarkeit relevant
```

---

## KPI: Unsicher zugeordnete Kompetenzen

### Beschreibung

Kompetenzzuordnungen mit niedriger oder ungeklärter MatchQuality.

### Quelle

```text
WbsRequiredCompetency
MatchQuality
```

### Formel

```text
Count(MatchQuality in [niedrig, ungeklärt])
```

### MVP

```text
Optional, aber empfohlen
```

---

# 14. Besetzbarkeits-KPI

## KPI: Fachlich besetzbare Aufgaben

### Beschreibung

Aufgaben, deren Pflichtkompetenzen vollständig durch aktive Personen abgedeckt sind.

### Formel

```text
Aufgabe hat RequiredCompetencies
UND
alle Pflichtkompetenzen sind durch aktive Personen abgedeckt
```

### Nicht berücksichtigt

```text
Verfügbarkeit
Kapazitätszeitraum
Stundenlücke
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Fachlich nicht besetzbare Aufgaben

### Beschreibung

Aufgaben, bei denen mindestens eine Pflichtkompetenz nicht abgedeckt ist.

### Formel

```text
Mindestens eine Pflichtkompetenz ohne aktive PersonCompetency
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Fachliche Besetzbarkeitsquote

### Formel

```text
Fachlich besetzbare Aufgaben
/
Aufgaben mit RequiredCompetencies
*
100
```

### Sonderfall

```text
Wenn keine RequiredCompetencies gepflegt:
Status = Grau
```

### Ebene

```text
Projekt
Hauptpaket
```

### MVP

```text
Ja
```

---

## KPI: Kapazitiv besetzbare Aufgaben

### Beschreibung

Aufgaben, die fachlich besetzbar sind und für die im relevanten Zeitraum ausreichend Kapazität vorhanden ist.

### Formel später

```text
fachlich besetzbar
UND
CapacityAvailable >= RequiredHours
```

### MVP

```text
Nein, spätere Ausbaustufe
```

---

# 15. Risiko- und Handlungsbedarf-KPI

## KPI: Operativer Handlungsbedarf

### Beschreibung

Aufgaben mit unmittelbarem operativem Problem.

### Bedingungen

```text
überfällig
ODER
blockiert
ODER
fachlich nicht besetzbar
ODER
keine Ressourcenzuordnung
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Struktureller Handlungsbedarf

### Beschreibung

Aufgaben oder Hauptpakete mit unvollständiger fachlicher Grundlage.

### Bedingungen

```text
fehlende Kompetenz
ODER
fehlende Kapazität
ODER
ungeklärte MatchQuality
ODER
fehlende Planwerte
```

### Ebene

```text
Projekt
Hauptpaket
Aufgabe
```

### MVP

```text
Ja
```

---

## KPI: Handlungsbedarf gesamt

### Beschreibung

Verdichtete Kennzahl für steuerungsrelevante Auffälligkeiten.

### Formel

```text
Operativer Handlungsbedarf
+
struktureller Handlungsbedarf
```

### Hinweis

Eine Aufgabe soll bei Bedarf nur einmal als handlungsrelevant gezählt werden, auch wenn mehrere Gründe vorliegen.

### MVP

```text
Ja
```

---

# 16. Datenqualitäts-KPI

## KPI: Aufgaben ohne Planende

### Formel

```text
Count(aktive Aufgaben where PlannedEnd is null)
```

### Bedeutung

Termin-KPIs sind für diese Aufgaben nicht vollständig belastbar.

---

## KPI: Aufgaben ohne Planstunden

### Formel

```text
Count(aktive Aufgaben where PlannedHours is null or PlannedHours = 0)
```

### Bedeutung

Fortschritt, Aufwandsverbrauch und Kapazitätslogik sind eingeschränkt belastbar.

---

## KPI: Aufgaben ohne Kompetenzmapping

### Formel

```text
Count(aktive Aufgaben ohne WbsRequiredCompetency)
```

### Bedeutung

Kompetenzdeckung und Besetzbarkeit sind nicht berechenbar.

---

## KPI: Aufgaben mit ungeklärter MatchQuality

### Formel

```text
Count(Aufgaben oder Kompetenzzuordnungen mit MatchQuality = ungeklärt)
```

### Bedeutung

Automatische Zuordnungen müssen fachlich geprüft werden.

---

## KPI: Aufgaben ohne Ressourcenzuordnung

### Formel

```text
Count(aktive Aufgaben ohne ResourceAssignment)
```

### Bedeutung

Ressourcensteuerung ist nicht vollständig belastbar.

---

# 17. Ampellogik MVP

## 17.1 Grundprinzip

Die Ampel ist eine fachliche Verdichtung.

Sie darf keine Detailinformationen ersetzen.

Jede Ampel muss über Detailansicht nachvollziehbar sein.

---

## 17.2 Prioritätsregel

Die Ampellogik ist deterministisch.

Priorität:

```text
Grau prüft Datenbelastbarkeit
Rot schlägt Gelb
Gelb schlägt Grün
Grün nur wenn keine Warnbedingung erfüllt ist
```

---

## 17.3 Grau

Grau bedeutet:

```text
KPI nicht belastbar berechenbar
```

Beispiele:

```text
keine RequiredCompetencies gepflegt
keine PlannedHours vorhanden
keine CapacityHours vorhanden
wichtige MatchQuality ungeklärt
```

Grau ist kein guter oder schlechter Zustand, sondern ein Hinweis auf fehlende Datenqualität.

---

## 17.4 Rot

Rot gilt, wenn mindestens eine Bedingung erfüllt ist:

```text
blockierte Aufgabe vorhanden
kritische Aufgabe vorhanden
fachlich nicht besetzbare Pflichtaufgabe vorhanden
Kompetenzdeckung < 60 %
Besetzbarkeitsquote < 60 %
wesentliche Kapazitätslücke vorhanden
```

---

## 17.5 Gelb

Gelb gilt, wenn nicht Rot und mindestens eine Bedingung erfüllt ist:

```text
Kompetenzdeckung 60–89 %
Besetzbarkeitsquote 60–89 %
überfällige Aufgaben vorhanden
Aufgaben ohne Assignment vorhanden
ungeklärte MatchQuality vorhanden
kleinere Kapazitätslücke vorhanden
```

---

## 17.6 Grün

Grün gilt nur, wenn:

```text
keine Rot-Bedingung erfüllt
UND
keine Gelb-Bedingung erfüllt
UND
Datenlage belastbar
```

---

# 18. KPI-Priorisierung MVP

## Phase 1 – Bereits vorhanden / Basisdashboard

```text
Projektfortschritt
Aufwandsverbrauch
Planstunden
Ist-Stunden
Plankosten
Ist-Kosten
Überfällige Aufgaben
Blockierte Aufgaben
Ressourcenbedarf
Zugeordnete Stunden
Offene Stunden
Kapazität
Auslastung
```

---

## Phase 2 – Kompetenz-KPIs

```text
Benötigte Kompetenzen
Abgedeckte Kompetenzen
Fehlende Kompetenzen
Kompetenzdeckung %
Aufgaben ohne Kompetenzmapping
```

---

## Phase 3 – Fachliche Besetzbarkeit

```text
fachlich besetzbare Aufgaben
fachlich nicht besetzbare Aufgaben
fachliche Besetzbarkeitsquote
Kompetenzvollständigkeit je Aufgabe
```

---

## Phase 4 – Handlungsbedarf und Ampellogik

```text
operativer Handlungsbedarf
struktureller Handlungsbedarf
Ampel je Hauptpaket
Ampel auf Projektebene
Detailgründe je Ampel
```

---

## Phase 5 – Kapazitive Besetzbarkeit

```text
Kapazitätslücke je Hauptpaket
Kapazitätslücke je Kompetenz
kapazitiv besetzbare Aufgaben
Verfügbarkeitsprüfung im Zeitraum
```

---

# 19. Zielbild Dashboard

Die empfohlene Reihenfolge der Dashboard-KPIs lautet:

```text
Projektfortschritt
Planstunden
Ist-Stunden
Offene Stunden
Überfällige Aufgaben
Blockierte Aufgaben
Kompetenzdeckung
Fachliche Besetzbarkeit
Kapazitätsdeckung
Handlungsbedarf
Datenqualität
```

---

# 20. Zielbild Hauptpaket-Karte

Beispiel:

```text
Entwurfsplanung

Aufgaben: 18
Fortschritt: 46 %
Aufwandsverbrauch: 44 %

Stunden:
Plan: 320 h
Ist: 140 h
Offen: 180 h

Kompetenzen:
Benötigt: 5
Abgedeckt: 4
Offen: 1

Besetzbarkeit:
14 / 18 Aufgaben fachlich besetzbar

Kapazität:
120 h zugeordnet
60 h offen

Handlungsbedarf:
2 überfällig
1 blockiert
1 Kompetenz offen

Status: Gelb
```

---

# 21. Nicht-Ziele MVP

Nicht Bestandteil des MVP:

```text
automatische Ressourcenoptimierung
KI-gestützte Besetzungsvorschläge ohne Review
personenbezogene Leistungsbewertung
Skill-Ranking von Mitarbeitenden
komplexe Forecasts
Earned Value Management
automatische Terminprognosen
automatische Kostenprognosen
```

---

# 22. Grundsatz zur Personen- und Kompetenzdarstellung

Das System bewertet keine Mitarbeitenden.

Es zeigt ausschließlich:

```text
Planungsbedarf
Kompetenzabdeckung
Kapazitätslage
Besetzbarkeit von Aufgaben
```

Nicht zulässig im MVP:

```text
Performance-Ranking
Mitarbeiterbewertung
Produktivitätsvergleich
automatische Eignungsbewertung als Leistungsurteil
```

Zulässig:

```text
Person besitzt Kompetenz X
Person ist Aufgabe Y zugeordnet
Person hat geplante Kapazität Z
Aufgabe benötigt Kompetenz X
Kompetenz X ist nicht abgedeckt
```

---

# 23. Ergebnis

Der KPI-Katalog definiert die fachliche Grundlage für das Dashboard als Projektleitstelle.

Das Dashboard entwickelt sich damit von:

```text
Kennzahlen anzeigen
```

zu:

```text
Projekt steuerbar machen
```

Der zentrale Mehrwert entsteht durch die Verbindung von:

```text
WBS
→ Aufgaben
→ Kompetenzen
→ Personen
→ Ressourcenbedarf
→ Kapazität
→ Besetzbarkeit
→ Handlungsbedarf
```