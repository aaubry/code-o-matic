@ECHO OFF

PUSHD %1

ECHO.
ECHO -------------------------------------------------------
ECHO.

COCO.EXE -namespace CodeOMatic.Validation.CompileTime.Parser SelectorParser.atg

ECHO.
ECHO -------------------------------------------------------
ECHO.

MOVE /Y Parser.cs SelectorParser.Generated.cs
MOVE /Y Scanner.cs SelectorScanner.Generated.cs

POPD