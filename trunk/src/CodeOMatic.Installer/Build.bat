@ECHO OFF

SETLOCAL

SET SCRIPT_PATH=%~dp0
SET OUTPUT_PATH=%1

PUSHD %OUTPUT_PATH%

"%SCRIPT_PATH%\nsis\makensis.exe" Installer.nsi
