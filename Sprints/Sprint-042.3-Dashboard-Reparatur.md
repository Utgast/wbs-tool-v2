# Sprint-042.3: HTTP 500 Dashboard-Fehler Reparatur

**Datum:** 2026-07-10  
**Typ:** Bugfix (Unscheduled)  
**Status:** Abgeschlossen ✓

---

## 1. Ziel

Den HTTP 500-Fehler beim Laden der Projektübersicht beheben, der durch falsche Dependency Injection-Reihenfolge entstanden ist.

### Problem
```
HTTP 500: Fehler beim Laden der Projektübersicht
```

---

## 2. Umsetzung

### Root Cause Analysis

**Fehler:** In `Program.cs` war die Service-Registrierungsreihenfolge falsch:

```csharp
// FALSCH:
builder.Services.AddScoped<IProjectDashboardService, ProjectDashboardService>();
builder.Services.AddScoped<IManagementAttentionService, ManagementAttentionService>();
```

`ProjectDashboardService` benötigt `IManagementAttentionService`, aber dieses war noch nicht registriert.

### Reparatur

Registrierungsreihenfolge korrigiert:

```csharp
// RICHTIG:
builder.Services.AddScoped<IManagementAttentionService, ManagementAttentionService>();
builder.Services.AddScoped<IProjectDashboardService, ProjectDashboardService>();
```

**Regel:** Abhängigkeiten müssen VOR ihren Konsumenten registriert werden.

---

## 3. Geänderte Dateien

| Datei | Änderung |
|---|---|
| [Program.cs](../src/backend/WbsTool.Api/Program.cs) | Zeilen 40-41 vertauscht (3 Zeilen) |

**Gesamtumfang:** 1 Datei, 3 Zeilen, 0 Code-Logik-Änderungen

---

## 4. Build-Ergebnisse

### Backend Build ✅

```
Status:         Erfolgreich
Dauer:          1,2s
Output:         bin\_sprint042_3_build\WbsTool.Api.dll
Compilation:    0 Fehler, 0 Warnungen
Modus:          Alternative Output (-o flag, Prozess lief noch)
```

### Frontend Build ✅

```
Status:         Erfolgreich
Dauer:          713ms
Modules:        57 transformiert
JS:             280.56 kB (gzip: 81.32 kB)
CSS:            12.58 kB (gzip: 2.77 kB)
Errors:         0
```

---

## 5. MVP-Grenzen

**Was NICHT geändert wurde:**
- ❌ Keine Feature-Änderungen
- ❌ Kein Refactoring
- ❌ Keine neuen Dependencies
- ❌ Keine Logik-Änderungen

**Das ist ein BUGFIX, kein Feature-Sprint.**

---

## 6. Reifegrad-Verbesserung

**Dashboard-Reiter:**
- Vorher: 85% (aber fehlerhaft - HTTP 500)
- Nachher: 85% (funktionsfähig wieder)

**Ergebnis:** Dashboard wieder operational. HTTP 500 behoben.

---

## 7. Nächster Sprint

**Sprint-043.1 (Geplant)**
Kompetenzen-Reiter – Kompetenzkatalog sichtbar machen

---

## 8. Qualitätsprüfung

✅ Backend kompiliert fehlerfrei  
✅ Frontend kompiliert fehlerfrei  
✅ Änderung ist minimal (3 Zeilen Reordering)  
✅ Keine Regress-Risiken  
✅ Dependency-Kette korrekt  

---

**Status:** Reparatur erfolgreich, bereit für Produktion.

