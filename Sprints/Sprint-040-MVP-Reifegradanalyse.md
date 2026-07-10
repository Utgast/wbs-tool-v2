# Sprint-040: MVP-Reifegradanalyse und Roadmap

**Datum:** 2026-07-10  
**Typ:** Dokumentations- und Planungs-Sprint  
**Status:** Abgeschlossen ✓

---

## 1. Ziel

Dokumentieren, warum die Entwicklung von Detailfunktionen zur MVP-Vervollständigung umgesteuert wurde.

### Leitsatz

> **"Nicht mehr Intelligenz, sondern vollständige Nutzbarkeit."**

Die größte Wertsteigerung entsteht aktuell nicht durch:
- Scoring und Komplexität
- Handlungsbedarf und Vorschlagslogik
- Intelligente Automation

sondern durch:
- **Sichtbarkeit** der Kernreiter
- **Nutzbarkeit** der Basisfunktionen
- **Pflegebarkeit** der Hauptdaten

---

## 2. Aktueller MVP-Reifegrad

### Dashboard – 85 %

**Stärken:**
- Alle KPIs funktionsfähig (Stunden, Kosten, Ressourcen)
- Risiken und Liefergegenstände angezeigt
- Handlungsbedarf implementiert und priorisiert
- TOP HANDELN – Top 10 Action Items mit Farbcodierung

**Schwächen:**
- SuggestedOwnerPersonId noch als GUID dargestellt (sollte Name sein)
- Keine bidirektionale Navigation zu Details
- Administration der Vorschlagslogik nur über Code möglich

**Wichtigste MVP-Lücke:**
- SuggestedOwner anzeigen mit Personennamen statt GUID

---

### WBS – 80 %

**Stärken:**
- Vollständige Baumstruktur mit Mutter-Kind-Beziehungen
- Alle WBS-Ebenen darstellbar
- Read-Only funktioniert stabil
- Filter und Suchfunktion vorhanden

**Schwächen:**
- Bearbeitungsmaske nur teilweise implementiert
- Bulk-Operations nicht vorhanden
- Export nicht vorhanden

**Wichtigste MVP-Lücke:**
- Inline-Bearbeitung von WBS-Elementen (Name, Beschreibung)

---

### Ressourcen – 30 %

**Stärken:**
- Personenliste angezeigt (Name, Kürzel, Email, Typ, Status)
- KPI-Cards für Überblick (Total, Aktiv, Inaktiv, Demands)
- Daten aktuell und korrekt

**Schwächen:**
- Keine Ressourcenverfügbarkeit angezeigt
- Keine Detailansicht pro Person
- Keine Kapazitätsplanung
- Keine Kompetenz-Verknüpfung sichtbar

**Wichtigste MVP-Lücke:**
- Verfügbarkeit und Kapazitätsload pro Person anzeigen

---

### Prozesse – 40 %

**Stärken:**
- LPH-Struktur (Leistungsphasen) angezeigt
- Code, Name, Reihenfolge, Status sortiert
- Basis für Deliverables-Zuordnung vorhanden

**Schwächen:**
- Deliverables nicht angezeigt
- Keine Detailansicht pro LPH
- Keine Phase-Verantwortlichkeiten sichtbar
- Keine Meilensteine

**Wichtigste MVP-Lücke:**
- Deliverables zu LPH-Phasen zuordnen und anzeigen

---

### Kompetenzen – 40 %

**Stärken:**
- Controller vorhanden und getestet
- PersonCompetencies-Beziehung strukturiert

**Schwächen:**
- Kompetenzkatalog nicht angezeigt
- Personenkompetenzen nicht sichtbar
- Keine Kategorisierung sichtbar
- Keine Level/Bewertung

**Wichtigste MVP-Lücke:**
- Kompetenzkatalog (Code, Name, Kategorie) sichtbar machen
- Personenkompetenzen pro Person anzeigen

---

### Administration – 30 %

**Stärken:**
- Grundstruktur für Admin-Funktionen vorhanden
- Konfiguration für Vorschlagslogik vorbereitet

**Schwächen:**
- Keine Admin-UI für Konfiguration
- ManagementAttentionDefaults nur über Code änderbar
- Keine Auditierung oder Logging
- Keine Benutzer-/Rollenmanagement

**Wichtigste MVP-Lücke:**
- Admin-Panel für Vorschlagslogik-Konfiguration bauen

---

## 3. Wichtigste Erkenntnisse

### Problem

In Sprint-038 und Sprint-039 wurde zunächst **Intelligenz** in den Vordergrund gestellt:
- ManagementAttentionService mit Scoring-Logik
- SuggestedAction, Explanation, ReactionDate
- Priorisierung und Automatisierung

### Umlenkung

Die größte **Wertsteigerung** für MVP entsteht nicht dort, sondern durch:
- **Sichtbarkeit:** Alle Hauptreiter müssen grundlegend sichtbar sein
- **Nutzbarkeit:** Benutzer können ihre Kernarbeit erledigen
- **Pflegebarkeit:** Daten sind einfach lesbar, verstanden und nachverfolgbar

### Konsequenz

**Neue Entwicklungsstrategie:**
1. Erst: **Vollständige Sichtbarkeit** (1 Reiter pro Sprint)
2. Dann: **Basis-Bearbeitung** (Inline-Edits)
3. Später: **Intelligenz** (Scoring, Automation)

---

## 4. Top MVP-Lücken (priorisiert)

| Priorität | Lücke | Impact | Aufwand |
|---|---|---|---|
| **1** | Ressourcen sichtbar machen | Ressourcenmanagement unmöglich ohne Übersicht | Mittel |
| **2** | Prozesse + Deliverables sichtbar machen | Projektstrukturierung nicht nachvollziehbar | Mittel |
| **3** | Kompetenzen sichtbar machen | Skill-Match und Besetzung nicht möglich | Mittel |
| **4** | Administration nutzen | Vorschlagslogik unveränderbar ohne Code | Gering |

---

## 5. Bisherige Roadmap (abgeschlossen)

### Sprint-041.1 ✅
**Personen sichtbar machen**
- Tabelle mit Name, Kürzel, Email, Typ, Status
- getPersons() API-Funktion
- Reifegradverbesserung Ressourcen: 15% → 20%

### Sprint-041.4 ✅
**Ressourcen-KPIs**
- 4 KPI-Cards: Total Personen, Aktiv, Inaktiv, Demands
- Paralleles Laden (Promise.all)
- Reifegradverbesserung Ressourcen: 20% → 30%

### Sprint-042.1 ✅
**LPH sichtbar machen**
- Tabelle mit Code, Name, Reihenfolge, Status
- getProcessPhases() API-Funktion
- Sortierung nach Reihenfolge
- Reifegradverbesserung Prozesse: 20% → 40%

---

## 6. Nächste Mikro-Sprints

### Sprint-042.2: Deliverables sichtbar machen

**Ziel:** Deliverables im Prozesse-Reiter anzeigen

**Scope:**
- Tabelle mit: Name, Typ, Status, DueDate, Owner, Zugeordnete Phase
- Read-Only
- Sortierung nach Phase

**Dateien:** 2 Änderungen (api.js + ProcessesPage.tsx)  
**API:** 1 Endpunkt (getProjectDeliverables oder getDeliverables)  
**Reifegradverbesserung Prozesse:** 40% → 60%

---

### Sprint-043.1: Kompetenzkatalog sichtbar machen

**Ziel:** Kompetenzübersicht im Kompetenzen-Reiter

**Scope:**
- Tabelle mit: Code, Name, Kategorie, Aktiv/Inaktiv
- Read-Only
- Sortierung nach Code oder Kategorie

**Dateien:** 2 Änderungen (api.js + CompetenciesPage.tsx)  
**API:** 1 Endpunkt (getCompetencies)  
**Reifegradverbesserung Kompetenzen:** 40% → 60%

---

### Sprint-043.2: Personenkompetenzen sichtbar machen

**Ziel:** Kompetenz-Profil für jede Person

**Scope:**
- Tabelle/Liste mit: Person, Kompetenz, Level
- Read-Only
- Filterable nach Person oder Kompetenz

**Dateien:** 2 Änderungen (CompetenciesPage.tsx oder neue PersonCompetenciesPanel)  
**API:** 1 Endpunkt (getPersonCompetencies)  
**Reifegradverbesserung Kompetenzen:** 60% → 75%

---

## 7. Neue Entwicklungsregel: MVP-Fertigstellungsmodus

### Ein Mikro-Sprint = ein sichtbarer Nutzerwert

```
1 Mikro-Sprint
  ↓
  = 1 sichtbarer Nutzerwert
  ↓
  = 1–3 Dateien geändert
  ↓
  = maximal 1 neuer API-Endpunkt
```

### Keine Groß-Sprints mehr

- Jeder Sprint ist in sich geschlossen
- Maximale Klarheit über Umfang
- Regelmäßige Validierung (Build-Tests, Dokumentation)
- Schnelle Iteration möglich

### Folge

Sprints 042.2, 043.1, 043.2 folgen **exakt** diesem Muster:
1. Lesen: Existierende Controller und DTO
2. Schreiben: 1 API-Funktion in api.js
3. Bauen: 1 neue oder erweiterte React-Komponente
4. Dokumentieren: Neue docs/XX-*.md
5. Validieren: Backend Build ✓ + Frontend Build ✓

---

## 8. Abschlussbewertung

### Aktueller Engpass

Der größte Engpass für MVP-Fertigstellung ist **nicht**:
- ❌ Dashboard-Lieferfähigkeit (85% = sehr gut)
- ❌ Handlungsbedarf-Logik (funktioniert)
- ❌ Liefergegenstände (sind angezeigt)

sondern:

Der größte Engpass für MVP-Fertigstellung ist:
- ✅ **Ressourcen-Reiter** (30% = unvollständig)
- ✅ **Prozesse-Reiter** (40% = unvollständig)
- ✅ **Kompetenzen-Reiter** (40% = unvollständig)

### Begründung

Benutzer können derzeit nicht:
- Die verfügbaren Ressourcen überblicken
- Deliverables Prozess-Phasen zuordnen
- Kompetenzenprofile ihrer Mitarbeiter sehen

Diese drei Basisfunktionen sind **kritisch** für die tägliche Nutzung.

### Empfehlung nach Sprint-043.2

Nach Abschluss von Sprint-043.2 (Personenkompetenzen):
1. Erneute MVP-Reifegradanalyse durchführen
2. Prüfen, ob neue Engpässe entstanden sind
3. Administration ggf. als Sprint 044 priorisieren
4. Oder: Basis-Bearbeitung (Edit-Funktionen) priorisieren

---

## 9. Versionierung

- **Sprint-Nr.:** 040
- **Titel:** MVP-Reifegradanalyse und Roadmap
- **Codeänderungen:** 0
- **Dokumentationen:** 1 (diese Datei)
- **Build-Validierung:** N/A (keine Code-Änderungen)

---

## 10. Nächster Schritt

**Sprint-042.2 bereit zur Ausführung**

Folgender Sprint kann zu beliebiger Zeit starten:
- Alle Anforderungen dokumentiert
- API-Struktur klar
- Komponenten-Pattern etabliert
- Kein Blocking-Issue

Warte auf explizite Nutzer-Instruktion vor Start.

