@ECHO OFF

SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

IF [%1] == [] (
	GOTO :DO_USAGE
)

SET SCRIPT_PATH=%~dp0
SET SOURCE_PATH=%SCRIPT_PATH%\..\..\src
SET TOOLS_PATH=%SCRIPT_PATH%\tools
SET RELEASES_PATH=%SCRIPT_PATH%\..\..\releases

PUSHD "%SOURCE_PATH%"
CALL VSVARS32.bat
MSBUILD /p:Configuration=Release /v:n CodeOMatic.sln
POPD

MD "%RELEASES_PATH%" 2> NUL

SET OUTPUT_FOLDER=%RELEASES_PATH%\%1
SET OUTPUT_BIN_ARCHIVE=%OUTPUT_FOLDER%\CodeOMatic_bin_%1.zip
SET OUTPUT_ASSEMBLIES=%OUTPUT_FOLDER%\bin

RD /S /Q "%OUTPUT_FOLDER%" > NUL

MD %OUTPUT_FOLDER%
MD %OUTPUT_ASSEMBLIES%

FOR %%I IN (Validation Validation.CompileTime Validation.Core Web) DO (
	COPY "%SOURCE_PATH%\CodeOMatic.%%I\bin\Release\CodeOMatic.%%I.dll" "%OUTPUT_ASSEMBLIES%
)
COPY "%SOURCE_PATH%\CodeOMatic.Validation.Installer\Release\CodeOMatic.Validation.Installer.msi" "%OUTPUT_ASSEMBLIES%

"%SCRIPT_PATH%\tools\7z.exe" a -tzip "%OUTPUT_BIN_ARCHIVE%" "%OUTPUT_ASSEMBLIES%\*.*"

POPD

GOTO :EOF

:DO_USAGE

ECHO.
ECHO USAGE: make_release ^<version^>
ECHO   version is the version number of the release.
ECHO.
ECHO EXAMPLE:
ECHO   make_release 1.0
ECHO.