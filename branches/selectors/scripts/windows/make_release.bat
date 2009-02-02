@ECHO OFF

SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

IF [%1] == [] (
	GOTO :DO_USAGE
)

SET SCRIPT_PATH=%~dp0
SET SOURCE_PATH=%SCRIPT_PATH%\..\..\src
SET DOC_PATH=%SCRIPT_PATH%\..\..\Doc
SET TOOLS_PATH=%SCRIPT_PATH%\tools
SET RELEASES_PATH=%SCRIPT_PATH%\..\..\releases

SET LOCAL_CONFIG_FILE=%SCRIPT_PATH%\setup_local_config.bat
IF NOT EXIST %LOCAL_CONFIG_FILE% (
	ECHO @ECHO OFF > %LOCAL_CONFIG_FILE%
	ECHO REM Configure machine-dependent settings here>> %LOCAL_CONFIG_FILE%
	ECHO SET PATH=%%PATH%%;C:\program files\Microsoft Visual Studio 9.0\Common7\Tools>> %LOCAL_CONFIG_FILE%
	ECHO SET PATH=%%PATH%%;C:\Program Files\EWSoftware\Sandcastle Help File Builder>> %LOCAL_CONFIG_FILE%
)

CALL %LOCAL_CONFIG_FILE%

PUSHD "%SOURCE_PATH%"
CALL VSVARS32.bat
MSBUILD /p:Configuration=Release /v:n CodeOMatic.sln

IF /I [%2] == [-doc] (
	SandcastleBuilderConsole.exe CodeOMatic.shfb
)

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
	COPY "%SOURCE_PATH%\CodeOMatic.%%I\bin\Release\CodeOMatic.%%I.pdb" "%OUTPUT_ASSEMBLIES%
)
COPY "%SOURCE_PATH%\CodeOMatic.Validation.Installer\Release\CodeOMatic.Validation.Installer.msi" "%OUTPUT_ASSEMBLIES%

"%SCRIPT_PATH%\tools\7z.exe" a -tzip "%OUTPUT_BIN_ARCHIVE%" "%OUTPUT_ASSEMBLIES%\*.*"

IF /I [%2] == [-doc] (
	COPY "%DOC_PATH%\*.chm" "%OUTPUT_FOLDER%"
)

POPD

GOTO :EOF

:DO_USAGE

ECHO.
ECHO USAGE: make_release ^<version^> [-doc]
ECHO   version is the version number of the release.
ECHO   -doc specifies that documentation should be generated.
ECHO.
ECHO EXAMPLES:
ECHO   make_release 1.0
ECHO   make_release 3.1 -doc
ECHO.