param([string]$dropLocation, [string]$packageVersion="0.0.1", [string] $commandPackagesToBuild = "*", [string] $exceptCommandPackagesToBuild, [switch] $excludeaz)

$thisScriptDirectory = Split-Path $MyInvocation.MyCommand.Path -Parent

$workspaceDirectory = $env:WORKSPACE
if (!($workspaceDirectory))
{
    $workspaceDirectory = (Resolve-Path "$thisScriptDirectory\..\..").Path
    $env:WORKSPACE = $workspaceDirectory
}

if (!($dropLocation))
{
    $dropLocation = "$workspaceDirectory\drop"
}

if (!(Test-Path -Path $dropLocation -PathType Container))
{
    mkdir "$dropLocation"
}

if (!(Test-Path -Path "$dropLocation\CommandRepo" -PathType Container))
{
    mkdir "$dropLocation\CommandRepo"
}

if (!(Test-Path -Path "$dropLocation\az" -PathType Container))
{
    mkdir "$dropLocation\az"
}

$buildPackageScriptPath = "`"$thisScriptDirectory\BuildPackage.ps1`"" # Guard against spaces in the path
$sourcesRoot = "$workspaceDirectory\src\clu"

# Grab all command packages to build.
# Get-ChildItem -path $sourcesRoot -Filter '*.nuspec.template' -Recurse -File
# We'll assume that all directories that contain a *.nuspec.template file is a command package and that the name of the package is everything leading up to .nuspec.template
$commandPackages =  Get-ChildItem -path $sourcesRoot  | Get-ChildItem -File -Filter "*.nuspec.template" | Where -FilterScript {$_ -ne $null -and $_.Name -ne $null -and $_.Name.Contains(".nuspec.template")} |
                        ForEach-Object { New-Object PSObject -Property @{Directory=$_.DirectoryName; Package=$_.Name.Substring(0, $_.Name.Length - ".nuspec.template".Length)} } | 
                        Where-Object -Property Package -Like -Value $commandPackagesToBuild  |
                        Where-Object -Property Package -NotLike -Value $exceptCommandPackagesToBuild

foreach($commandPackage in $commandPackages)
{
    $commandPackageName = $commandPackage.Package
    $commandPackageDir  = $commandPackage.Directory
    $buildOutputDirectory = Join-Path -path $commandPackageDir -ChildPath "bin\Debug\publish"

    Invoke-Expression "& $buildPackageScriptPath $commandPackageDir $commandPackageName $buildOutputDirectory $packageVersion $dropLocation\CommandRepo"
}

if (!($excludeaz))
{
    foreach ($runtime in @("win7-x64", "osx.10.10-x64", "ubuntu.14.04-x64"))
    {
        $azOutput = "$dropLocation\az\$runtime"
        dotnet publish "$sourcesRoot\az" --framework dnxcore50 --runtime $runtime --output $azOutput

        if (!($runtime.StartsWith("win")))
        {
            # use released coreconsole file from https://github.com/dotnet/cli
            Copy-Item -Path "$workspaceDirectory\tools\CLU\$runtime\coreconsole" -Destination "$azOutput\az" -Force

            # Remove all extra exes that end up in the output directory...
            Get-ChildItem -Path "$azOutput" -Filter "*.exe" | Remove-Item
        }
        else 
        {
            # Remove all extra exes that end up in the output directory...
            Get-Childitem -path "$azOutput" -Filter *.exe | Where-Object -Property "Name" -Value "az.exe" -NotMatch | Remove-Item
        }
    }
}