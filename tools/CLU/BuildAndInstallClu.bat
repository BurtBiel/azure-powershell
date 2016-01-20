echo off
setlocal
set root=%~dp0..\..
where dotnet.exe
if ERRORLEVEL 1 (
    echo Please install 'dotnet', say from 'https://azureclu.blob.core.windows.net/tools/dotnet-win-x64.latest.zip', unzip, then add its bin folder to the PATH
    exit /B 1
)

pushd
cd %root%\src\CLU
call dotnet restore -s https://api.nuget.org/v3/index.json -s "%root%\tools\LocalFeed"
if ERRORLEVEL 1 (
    echo "dnu.cmd restore" failed under folder of "%root%\src\CLU"
    popd
    exit /B 1
)
popd

@powershell -file %~dp0\BuildDrop.ps1

REM cook a msclu.cfg with a correct local repro path. 
set mscluCfg=%root%\drop\az\win7-x64\msclu.cfg
if not exist %mscluCfg% (
    copy /Y %root%\src\CLU\az\msclu.cfg %root%\drop\az\win7-x64
)
echo ^(Get-Content "%mscluCfg%"^) ^| ForEach-Object { $_ -replace "TOFILL", "%root%\drop\CommandRepo" } ^| Set-Content "%mscluCfg%"^ >"%temp%\Rep.ps1"
@powershell -file %temp%\Rep.ps1

%root%\drop\az\win7-x64\az.exe --install
%root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Profile
%root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Resources
%root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Resources.Cmdlets
%root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Websites
%root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Network
%root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Management.Storage
%root%\drop\az\win7-x64\az.exe --install Microsoft.Azure.Commands.Compute

rename %root%\drop\az\win7-x64\azure.bat az.bat

REM setup osx and linux bits which can be xcopied and run. 
REM note, for known nuget bugs, skip --install by copying over cmdlet packages.
xcopy %root%\drop\az\win7-x64\pkgs %root%\drop\az\osx.10.10-x64\pkgs /S /Q /I /Y
copy /Y %root%\drop\az\win7-x64\azure.lx %root%\drop\az\osx.10.10-x64
copy /Y %root%\drop\az\win7-x64\msclu.cfg %root%\drop\az\osx.10.10-x64

REM: copy over the pre-cooked az.sh and ensure correct line endings
copy /Y %~dp0\az.sh %root%\drop\az\osx.10.10-x64\az
set azuresh=%root%\drop\az\osx.10.10-x64\az
echo Get-ChildItem %azuresh% ^| ForEach-Object { >  %temp%\fixLineEndings.ps1
echo $contents = [IO.File]::ReadAllText($_) -replace "`r`n?", "`n" >> %temp%\fixLineEndings.ps1 
echo [IO.File]::WriteAllText($_, $contents) >> %temp%\fixLineEndings.ps1 
echo } >> %temp%\fixLineEndings.ps1
@powershell -file %temp%\fixLineEndings.ps1

xcopy %root%\drop\az\win7-x64\pkgs %root%\drop\az\ubuntu.14.04-x64\pkgs /S /Q /I /Y
copy /Y %root%\drop\az\win7-x64\azure.lx %root%\drop\az\ubuntu.14.04-x64
copy /Y %root%\drop\az\win7-x64\msclu.cfg %root%\drop\az\ubuntu.14.04-x64
copy /Y %azuresh% %root%\drop\az\ubuntu.14.04-x64\az

REM, windows version also needs it for bash based testing
copy /Y %~dp0\az.win.sh %root%\drop\az\win7-x64\az
