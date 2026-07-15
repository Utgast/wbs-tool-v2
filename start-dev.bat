@echo off
echo Starte WBS Tool V2...

start "WBS Tool V2 Backend" cmd /k "cd /d C:\Projekte\wbs-tool-v2\src\backend\WbsTool.Api && dotnet run"

start "WBS Tool V2 Frontend" cmd /k "cd /d C:\Projekte\wbs-tool-v2\src\frontend\wbs-tool-ui && npm run dev"

echo.
echo WICHTIG:
echo 1. Backend muss auf http://localhost:5046 laufen
echo 2. Frontend-URL aus dem Vite-Fenster oeffnen, z.B. http://localhost:5173
echo.
pause