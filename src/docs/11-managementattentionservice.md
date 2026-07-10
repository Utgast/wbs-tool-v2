# Ziel

Erster Berechnungskern fuer Handlungsbedarf als berechnete Sicht, ohne Persistierung und ohne Anbindung an API, Controller oder Dashboard.

Der Service liefert eine Liste von ManagementAttentionViewDto-Eintraegen.

# Betroffene Dateien

- backend/WbsTool.Api/Modules/Projects/Services/IManagementAttentionService.cs
- backend/WbsTool.Api/Modules/Projects/Services/ManagementAttentionService.cs
- backend/WbsTool.Api/Program.cs

# Unterstuetzte Quellen

Im Sprint-037 aktiv umgesetzt:

- Risiken
- Deliverables (nur ueberfaellig)

Im Sprint-037 bewusst noch nicht umgesetzt (nur Platzhalterstruktur):

- Kompetenz-Gaps
- Ressourcen-Gaps

# Berechnungsregeln

Risiko:

- Es werden offene Risiken verarbeitet (Status ungleich Accepted/Closed).
- Kritische Risiken und offene Risiken erzeugen Handlungsbedarf-Eintraege.
- SuggestedOwnerPersonId wird direkt aus dem Risiko-Owner gesetzt.

Deliverable:

- Es werden nur ueberfaellige Deliverables verarbeitet (DueDate < heute und Status ungleich Delivered).
- Ueberfaellige Deliverables erzeugen Handlungsbedarf-Eintraege.
- SuggestedOwnerPersonId wird direkt aus dem Deliverable-Owner gesetzt.

Service-Verhalten:

- Ergebnis ist List<ManagementAttentionViewDto>.
- Sortierung erfolgt absteigend nach PriorityScore, danach aufsteigend nach ReactionDate.

# Priorisierung

MVP-Prioritaeten in Sprint-037:

- Critical Risk: PriorityScore = 100
- Offenes Risk: PriorityScore = 70
- Ueberfaelliges Deliverable: PriorityScore = 90

Begruendung:

- Kritische Risiken erhalten die hoechste unmittelbare Steuerungsprioritaet.
- Ueberfaellige Deliverables sind hochkritisch fuer die Lieferfaehigkeit, aber unter kritischen Risiken eingeordnet.
- Offene nicht-kritische Risiken bleiben steuerungsrelevant, aber mit niedrigerem Startwert.

# Owner-Vorschlaege

MVP-Regeln in Sprint-037:

- Risiko: SuggestedOwnerPersonId = Risiko-Owner
- Deliverable: SuggestedOwnerPersonId = Deliverable-Owner

Prinzip bleibt erhalten:

System schlaegt vor. Mensch entscheidet.

Der Service erzeugt nur Vorschlaege und fuehrt keine Aktionen aus.

# MVP-Grenzen

Bewusst nicht Teil von Sprint-037:

- keine API
- kein Dashboard
- keine Persistierung
- keine Kompetenz-Gaps
- keine Ressourcen-Gaps
- keine komplexe Scoring-Logik
- keine automatische Anlage oder Bearbeitung von Objekten

# Build-Ergebnis

Backend Build-Validierung:

- dotnet build: erwartbar fehlgeschlagen wegen Dateisperre auf laufende WbsTool.Api.exe
- dotnet build -o bin/_sprint037_build: erfolgreich

# Naechster Sprint

Sprint-038 - Vorschlagslogik erweitern
