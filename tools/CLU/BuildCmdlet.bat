echo off
Setlocal EnableDelayedExpansion

set DebugCLU=
set root=%~dp0..\..
if not "%1"=="" (
    @powershell -file %~dp0\BuildDrop.ps1 -commandPackagesToBuild %1 --excludeaz
    %root%\drop\az\win7-x64\az.exe --install %1
) else (
    @powershell -file %~dp0\BuildDrop.ps1 -excludeaz
    %root%\drop\az\win7-x64\az.exe --install
    %root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Profile
    %root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Resources
    %root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Resources.Cmdlets
    %root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Websites
    %root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Network
    %root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Management.Storage
)