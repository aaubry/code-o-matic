# code-o-matic
The code-o-matic library leverages the [http://www.postsharp.org/ PostSharp] platform to implement code injection mechanisms for .NET. Code is injected through custom attributes that perform various tasks, such as parameter validation, common patterns implementation, etc.

The current version of code-o-matic offers the following mechanisms:

  * *Validation* - Validate method parameters and properties by applying custom attributes such as NotNull or Pattern.
  * *Automatic properties* - Automatically implement ViewState and Session properties on your ASP.NET controls and pages.

See the GettingStarted page to get started.

## Requirements

Code-o-matic is based on [http://www.postsharp.org/ PostSharp] 1.0. You will need to download and install it for code-o-matic to work.

An installer is supplied that can optionally install the correct version of [http://www.postsharp.org/ PostSharp] for you.

*NOTE:* Postsharp is only required for compiling the code. The production environment does not need to have Postsharp installed.

For performance reasons, the validation library is implemented as a Postsharp plugin. An installer is provided that copies the plugin files in the correct folder. It is also possible to copy the files manually. See the CompilingInstructions page for more informations.

## Documentation

The following pages contain examples of use for the various parts of code-o-matic.

  * ValidationExamples


