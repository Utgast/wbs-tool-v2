# 15 - Ressourcen: Personenliste

## Ziel

Sprint-041.1 macht den Ressourcen-Reiter erstmals nutzbar.

Erste Funktion: Read-only-Übersicht aller aktiven Personen mit Basis-Informationen.

---

## Betroffene Dateien

| Datei | Änderung |
|---|---|
| `src/frontend/wbs-tool-ui/src/services/api.js` | Neue API-Funktion `getPersons()` |
| `src/frontend/wbs-tool-ui/src/pages/ResourcesPage.tsx` | Komplett rewritten: Tabelle mit Personenliste |

**Backend:**
Keine Änderung – `GET /api/persons` existiert bereits.

---

## Verwendete API

### Backend-Endpoint

```
GET /api/persons
```

**Controller:** `PersonsController.cs`  
**Service:** `PersonService.cs`

Liefert alle aktiven Personen (PersonDto):
- Id
- DisplayName
- ShortName
- Email
- IsPlaceholder
- PlaceholderType
- IsActive

---

## Angezeigte Felder

Tabelle mit 5 Spalten:

| Spalte | Quelle | Beschreibung |
|---|---|---|
| Name | PersonDto.DisplayName | Vollständiger Name |
| Kürzel | PersonDto.ShortName | Kurzform (oder "-") |
| Email | PersonDto.Email | E-Mail-Adresse (oder "-") |
| Typ | IsPlaceholder + PlaceholderType | "Real" oder PlaceholderType (z. B. "Resource") |
| Status | IsActive | Badge: "Aktiv" (grün) oder "Inaktiv" (rot) |

---

## MVP-Grenzen

Bewusst nicht Teil von Sprint-041.1:

- Keine Kapazitätsdaten
- Keine Auslastungsberechnung
- Keine Projektzuordnungen
- Keine Ressourcenplanung
- Keine Bearbeitung
- Keine Filter, Suche, Sortierung
- Kein Team-Feld (nicht in PersonDto vorhanden)
- Keine Kompetenz-Übersicht

---

## Build-Ergebnis

```
dotnet build -o bin/_sprint041_1_build → Erfolgreich
npm run build → Erfolgreich
```

---

## Nächster Mikro-Sprint

Sprint-041.2 – Kapazitäten sichtbar machen

Ziel: Zugeordnete Stunden, Kapazität und Auslastung pro Person anzeigen.
