@ECHO OFF

SETLOCAL ENABLEEXTENSIONS ENABLEDELAYEDEXPANSION

IF [%1] == [/?] (
	GOTO :DO_USAGE
)

IF [%1] == [-?] (
	GOTO :DO_USAGE
)

SET GEN_DOC=0
IF /I [%1] == [-doc] (
	SET GEN_DOC=1
	SHIFT
)

SET SCRIPT_PATH=%~dp0

PUSHD "%SCRIPT_PATH%\..\.."

SET VERSION=1.0
IF [%1] == [] (
	FOR /F %%I IN (version.txt) DO (
		SET VERSION=%%I
	)
) ELSE (
	SET VERSION=%1
)

SET REVISION=0
IF [%2] == [] (
	FOR /F "tokens=1,2" %%I IN ('svn info https://code-o-matic.googlecode.com/svn/trunk') DO (
		IF [%%I]==[Revision:] (
			SET REVISION=%%J
		)
	)
) ELSE (
	SET REVISION=%2
)

POPD

SET RELEASE_NAME=%VERSION%.%REVISION%

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

IF /I [%GEN_DOC%] == [1] (
	SandcastleBuilderConsole.exe CodeOMatic.shfb
)

POPD

MD "%RELEASES_PATH%" 2> NUL

SET OUTPUT_FOLDER=%RELEASES_PATH%\%RELEASE_NAME%
SET OUTPUT_BIN_ARCHIVE=%OUTPUT_FOLDER%\CodeOMatic_bin_%RELEASE_NAME%.zip
SET OUTPUT_SRC_ARCHIVE=%OUTPUT_FOLDER%\CodeOMatic_src_%RELEASE_NAME%.zip
SET OUTPUT_ASSEMBLIES=%OUTPUT_FOLDER%\bin
SET OUTPUT_SRC=%OUTPUT_FOLDER%\src

RD /S /Q "%OUTPUT_FOLDER%" > NUL

MD %OUTPUT_FOLDER%
MD %OUTPUT_ASSEMBLIES%
MD %OUTPUT_SRC%

FOR %%I IN (Validation Validation.CompileTime Validation.Core Web) DO (
	COPY "%SOURCE_PATH%\CodeOMatic.%%I\bin\Release\CodeOMatic.%%I.dll" "%OUTPUT_ASSEMBLIES%"
	COPY "%SOURCE_PATH%\CodeOMatic.%%I\bin\Release\CodeOMatic.%%I.pdb" "%OUTPUT_ASSEMBLIES%"
)

FOR %%I IN (CodeOMatic.Validation.psplugin PostsharpRequired.cs) DO (
	COPY "%SOURCE_PATH%\CodeOMatic.Installer\bin\Release\%%I" "%OUTPUT_ASSEMBLIES%"
)

COPY "%SOURCE_PATH%\CodeOMatic.Installer\bin\Release\CodeOMatic.exe" "%OUTPUT_FOLDER%\CodeOMatic.%RELEASE_NAME%.exe"

"%SCRIPT_PATH%\tools\7z.exe" a -tzip "%OUTPUT_BIN_ARCHIVE%" "%OUTPUT_ASSEMBLIES%\*.*"

PUSHD "%OUTPUT_SRC%"
SET EXCLUSIONS_FILE=exclusions.txt

ECHO. > %EXCLUSIONS_FILE%

FOR %%I IN (_ReSharper \bin \obj \.svn Debug Release .cache .user .suo .resharper) DO (
	ECHO %%I >> %EXCLUSIONS_FILE%
)

XCOPY "%SOURCE_PATH%" "%OUTPUT_SRC%" /EXCLUDE:%EXCLUSIONS_FILE% /E /I /Q

DEL %EXCLUSIONS_FILE%
POPD

IF /I [%GEN_DOC%] == [1] (
	COPY "%DOC_PATH%\CodeOMatic.chm" "%OUTPUT_FOLDER%\CodeOMatic.%RELEASE_NAME%.chm"
)

"%SCRIPT_PATH%\tools\7z.exe" a -tzip "%OUTPUT_SRC_ARCHIVE%" "%OUTPUT_SRC%\*.*"

POPD

GOTO :EOF

:DO_USAGE

ECHO.
ECHO USAGE: make_release [-doc] ^<version^> ^<revision^>
ECHO   version is the version number of the release.
ECHO   revision is the revision number of the release.
ECHO   -doc specifies that documentation should be generated.
ECHO.
ECHO EXAMPLES:
ECHO   make_release 1.0
ECHO   make_release 3.1 55
ECHO   make_release -doc 3.1
ECHO   make_release -doc 3.1 55
ECHO.