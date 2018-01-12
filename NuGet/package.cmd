echo off
rmdir /S /Q lib
mkdir lib
cd lib
mkdir netstandard2.0
cd ..
cd ..
rmdir /S /Q Package
mkdir Package
cd NuGet
copy /Y ..\Src\ReactiveTracker.Fody\ReactiveTracker\bin\Release\netstandard2.0\ReactiveTracker.dll .
copy /Y ..\Src\ReactiveTracker.Fody\ReactiveTracker\bin\Release\netstandard2.0\ReactiveTracker.pdb .
copy /Y ..\Src\ReactiveTracker.Fody\ReactiveTracker\bin\Release\netstandard2.0\ReactiveTracker.dll "lib/netstandard2.0"
copy /Y ..\Src\ReactiveTracker.Fody\ReactiveTracker\bin\Release\netstandard2.0\ReactiveTracker.pdb "lib/netstandard2.0"
copy /Y ..\Src\ReactiveTracker.Fody\ReactiveTracker.Fody\bin\Release\netstandard2.0\ReactiveTracker.Fody.dll .
copy /Y ..\Src\ReactiveTracker.Fody\ReactiveTracker.Fody\bin\Release\netstandard2.0\ReactiveTracker.Fody.pdb .
nuget.exe pack ReactiveTracker.Fody.nuspec -OutputDirectory ..\Package -Exclude nuget.exe -Exclude package.cmd