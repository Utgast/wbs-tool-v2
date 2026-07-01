# Repro / Verification Workflow

Use this workflow after every Copilot implementation slice.

## 1. Repository state
Run from repository root:

```powershell
Set-Location "C:\Projekte\wbs-tool"
git status --short
```

Expected:
- no DB files staged
- no Seed folder staged
- no .vscode staged
- no backup files staged

## 2. Backend build

```powershell
Set-Location "C:\Projekte\wbs-tool\src\backend\WbsTool.Api"
dotnet build
```

Expected:
- Build succeeded
- no new critical warnings

## 3. Frontend build

```powershell
Set-Location "C:\Projekte\wbs-tool\src\frontend\wbs-tool-ui"
npm run build
```

Expected:
- build succeeds
- no CSS warnings caused by accidental PowerShell/text content in CSS

## 4. Backend run

```powershell
Set-Location "C:\Projekte\wbs-tool\src\backend\WbsTool.Api"
dotnet run
```

Expected backend URL:
- http://localhost:5046

## 5. Dashboard regression
Run in second PowerShell window:

```powershell
Invoke-RestMethod "http://localhost:5046/api/projects/7f3faaa5-1245-4d43-978b-88b5bab3a23b/dashboard" |
  ConvertTo-Json -Depth 10
```

Expected values:
- totalPlannedHours = 681
- totalActualHours = 674
- progressPercent approx. 98.97
- blockedNodes = 0

## 6. WBS control node regression
Check WBS-ID 2.1.2 in API/UI.

Expected:
- title = Identifikation Fremdleitungen / Schutzgebiete
- plannedHoursTotal = 16
- actualHoursTotal = 24
- visible resources include Ahmad, Ibrahim, Phine, Victor

## 7. Git add discipline
Never use:

```powershell
git add .
```

Use explicit adds only after reviewing `git status --short`.
