# Instroduction #

This page contains walkthrough steps that will get you started with code-o-matic.

# Using the validation library #

  1. Install the validation plug-in by running **CodeOMatic.x.y.z.exe**. This will also install PostSharp.
  1. Open your project.
  1. Add references to the following assemblies (in **C:\Program Files\PostSharp 1.0**):
    * PostSharp.Core.dll
    * PostSharp.Public.dll
  1. Add references to the following assemblies (in **C:\Program Files\PostSharp 1.0\CodeOMatic**):
    * CodeOMatic.Core.dll
    * CodeOMatic.Validation.dll
    * CodeOMatic.Validation.Core.dll
  1. Apply validation attributes to your method and properties, as described in   ValidationExamples.