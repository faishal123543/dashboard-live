@echo off
echo ============================================================
echo  Loan Dashboard MVC  —  http://localhost:5100
echo ============================================================
cd /d "%~dp0src\LoanDashboard.MVC"
dotnet run --launch-profile http
