@ECHO OFF

SETLOCAL

IF [%1] == [] (
	SET OUTPUT_PATH=%ProgramFiles%\PostSharp 1.0
) ELSE (
	SET OUTPUT_PATH=%*
)

ECHO This will install Code-o-matic in "%OUTPUT_PATH%". If you didn't install PostSharp in that folder, you need to pass the correct path to this folder.
PAUSE

IF NOT EXIST "%OUTPUT_PATH%" (
	ECHO The PostSharp folder does not exist.
	GOTO :EOF
)

ECHO Copying files...
COPY /Y CodeOMatic.Validation.Core.* "%OUTPUT_PATH%"
COPY /Y CodeOMatic.Validation.CompileTime.* "%OUTPUT_PATH%"
COPY /Y CodeOMatic.Validation.psplugin "%OUTPUT_PATH%"
ECHO Done.
