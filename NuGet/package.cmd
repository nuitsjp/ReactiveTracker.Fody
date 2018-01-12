echo off
copy /Y ..\Source\Interception.Fody\Interception.Fody\bin\Debug\netstandard2.0\Interception.Fody.dll .
copy /Y ..\Source\Interception.Fody\Interception.Fody\bin\Debug\netstandard2.0\Interception.Fody.pdb .
nuget.exe pack Interception.Fody.nuspec -Exclude nuget.exe -Exclude package.cmd