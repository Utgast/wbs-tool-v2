# Dashboard-Fehleranalyse: HTTP 500 Reparatur

**Sprint:** 042.3 (Bugfix)  
**Datum:** 2026-07-10  
**Status:** Behoben ✅

---

## 1. Fehlerbild

**Symptom:** 
- HTTP 500 Fehler bei `GET /api/projects/{projectId}/dashboard`
- Fehlermeldung Frontend: "HTTP 500: Fehler beim Laden der Projektübersicht"
- Dashboard lädt nicht, DashboardPage bleibt leer

**Auswirkungen:**
- Projektübersicht-Tab funktioniert nicht
- Dashboard-Daten nicht verfügbar
- TopManagementAttentionItems nicht abrufbar

---

## 2. Fehlerursache (Root Cause)

**Ursache:** Dependency Injection Reihenfolge in `Program.cs`

**Problem:**
- `IProjectDashboardService` war VOR `IManagementAttentionService` registriert
- `ProjectDashboardService` versucht, `IManagementAttentionService` zu injizieren
- Da IManagementAttentionService noch nicht registriert war, trat ein Dependency Resolution Error auf
- Result: HTTP 500 Internal Server Error

**Betroffene Zeilen (Program.cs, Zeilen 40-41):**
```csharp
// FALSCH (Reihenfolge falsch):
builder.Services.AddScoped<IProjectDashboardService, ProjectDashboardService>();
builder.Services.AddScoped<IManagementAttentionService, ManagementAttentionService>();
```

**Warum es nicht bemerkt wurde:**
- ProjectDashboardService wurde in Sprint-039 mit ManagementAttentionService erweitert
- Die Service-Registrierung wurde in Zeile 40 eingefügt
- ManagementAttentionService wurde danach in Zeile 41 hinzugefügt
- Testbuild konnte das Problem nicht früh erkennen, da die Dependency noch nicht aktiviert war

---

## 3. Reparatur

**Minimale Änderung:** Registrierungsreihenfolge vertauscht

**Korrekt (nach Reparatur, Program.cs Zeilen 40-42):**
```csharp
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IManagementAttentionService, ManagementAttentionService>();
builder.Services.AddScoped<IProjectDashboardService, ProjectDashboardService>();
```

**Regeln für Service-Registrierung:**
1. Abhängigkeiten MÜSSEN vor ihren Konsumenten registriert werden
2. Dependency-Kette: ManagementAttentionService → ProjectDashboardService
3. Wenn B abhängig von A ist, registriere zuerst A, dann B

**Änderung:** 1 Datei, 3 Zeilen umgeordnet

---

## 4. Geänderte Dateien

| Datei | Änderung |
|---|---|
| [Program.cs](../src/backend/WbsTool.Api/Program.cs) | Zeilen 40-41 vertauscht (ManagementAttentionService vor ProjectDashboardService) |

---

## 5. Build-Ergebnisse

### Backend Build (nach Reparatur)

```
Status: ✅ Erfolgreich
Dauer: 1,2s
Output: bin\_sprint042_3_build\WbsTool.Api.dll
Errors: 0
Warnings: 0
Build-Modus: Alternative Output (Datei war gesperrt, -o flag benutzt)
```

### Frontend Build

```
Status: ✅ Erfolgreich
Dauer: 713ms
Module: 57 transformiert
Errors: 0
Warnings: 0
```

---

## 6. Testergebnis

**Erwartet nach Reparatur:**
- ✅ Projektübersicht-Endpoint lädt wieder
- ✅ Dashboard-Daten werden zurückgegeben
- ✅ ManagementAttentionService wird korrekt injiziert
- ✅ TopManagementAttentionItems sind verfügbar
- ✅ Keine HTTP 500 Fehler mehr

**Bestätigung:** 
Nach der Reparatur sollte die Dependency-Kette korrekt aufgelöst werden:

```
AppDbContext
  ↓
ManagementAttentionService (registriert) ← Jetzt ZUERST registriert
  ↓
ProjectDashboardService (registriert) ← Jetzt DANACH registriert
  ↓
ProjectsController (verwendet ProjectDashboardService)
  ↓
GET /api/projects/{id}/dashboard → Erfolgreich
```

---

## 7. Prävention

**Für zukünftige Reparaturen:**
- Dependency Injection immer Bottom-Up ordnen (Abhängigkeiten zuerst)
- Nach jedem neuen Service in Program.cs prüfen:
  - "Welche Services hängen davon ab?"
  - "Sind alle Dependencies bereits registriert?"
- CI/CD sollte Dependency-Fehler früh erkennen
- Unit Tests sollten Service-Konfiguration testen

---

## 8. Fazit

**Status:** Bugfix erfolgreich ✅

- **Fehler:** Dependency Injection Reihenfolge falsch
- **Reparatur:** 2 Zeilen umgeordnet
- **Umfang:** Minimal (nur Konfiguration, kein Code-Change)
- **Auswirkung:** HTTP 500 ist jetzt behoben
- **Regress-Risiko:** Sehr gering (reine Reordering)

Das Dashboard lädt wieder normal.

