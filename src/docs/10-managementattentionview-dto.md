# 10 - ManagementAttentionView DTO

## Ziel

Dieses Dokument beschreibt das DTO fuer die berechnete Handlungsbedarf-Sicht im MVP.

Scope von Sprint-036:

- DTO definieren
- Felder dokumentieren
- keine Berechnung
- keine Vorschlagslogik-Implementierung
- keine Dashboard-Integration

## DTO-Definition

Technischer Name:

ManagementAttentionViewDto

Ablage:

backend/WbsTool.Api/Modules/Projects/Contracts/ManagementAttentionViewDto.cs

## Felddokumentation

| Feld | Typ | Bedeutung im MVP | Herkunft | Pflegeart |
|---|---|---|---|---|
| Type | string | Typklasse des Handlungsbedarfs (z. B. Risk, Deliverable, CompetencyGap, ResourceGap) | abgeleitet | automatisch |
| SourceType | string | Technische Quellklassifikation | abgeleitet | automatisch |
| SourceId | Guid | Eindeutiger Bezug auf Quellobjekt | Quelle | automatisch |
| Title | string | Kurztitel des Eintrags | Quelle/abgeleitet | automatisch |
| Description | string | Kurzbeschreibung des Kontexts | Quelle/abgeleitet | automatisch |
| Impact | int | Wirkung auf Projektziel (Skala 1-5) | abgeleitet | automatisch |
| Urgency | int | Zeitliche Dringlichkeit (Skala 1-5) | abgeleitet | automatisch |
| DependencyEffect | int | Wirkung auf Abhaengigkeiten (Skala 1-5) | abgeleitet | automatisch |
| Recoverability | int | Kompensierbarkeit (Skala 1-5) | abgeleitet | automatisch |
| Confidence | int | Verlaesslichkeit der Bewertung (Skala 1-5) | abgeleitet | automatisch |
| PriorityScore | decimal | Gewichteter Prioritaetswert fuer Sortierung | abgeleitet | automatisch |
| SuggestedOwnerPersonId | Guid? | Vorgeschlagene verantwortliche Person | abgeleitet | automatisch, spaeter manuell aenderbar |
| SuggestedAction | string | Vorgeschlagene Massnahme | abgeleitet | automatisch, spaeter manuell aenderbar |
| Explanation | string | Erklaerung der Priorisierung und Herleitung | abgeleitet | automatisch |
| ReactionDate | DateOnly? | Vorgeschlagenes spaetestes Reaktionsdatum | abgeleitet | automatisch, spaeter manuell aenderbar |

## MVP-Abgrenzung

Explizit nicht Teil von Sprint-036:

- Berechnungsregeln im Service
- Vorschlagslogik im Service
- API-Erweiterung
- Dashboard-Anzeige
- Persistierung oder Historisierung

## Ergebnis Sprint-036

Mit dem DTO ist das technische Austauschformat fuer die folgenden Sprints vorbereitet.
Die fachliche Semantik der Felder ist dokumentiert, ohne bereits Implementierungslogik einzufuehren.
