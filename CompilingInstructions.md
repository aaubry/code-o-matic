# Windows instructions #

  1. Install PostSharp 1.0 with the default options: http://www.postsharp.org/download (requires registration :( )
  1. Build the solution using Visual Studio or MSBUILD.
  1. Either
    * install CodeOMatic.exe from the CodeOMatic.Installer\bin\Release folder.
> > or
    * manually copy the following files to c:\program files\PostSharp 1.0 :
      * CodeOMatic.Validation.psplugin
      * CodeOMatic.Core.dll
      * CodeOMatic.Validation.CompileTime.dll
      * CodeOMatic.Validation.Core.dll
  1. Run the unit tests with MbUnit - http://www.mbunit.com/ (optional).