<#
.Synopsis
	Builds a .NET Core project locally and optionally deploys it to a local nuget share.  This is incredibly useful when developing local nuget packages.

.Description
	Builds a .NET Core project locally and optionally deploys it to a local nuget share.  This is incredibly useful when developing local nuget packages.

.Parameter $configuration
	The current configuration.  This script is intended to be run locally; therefore, it will only run if the current config is DEBUG.

.Parameter $projectName
	The project name
.Parameter $nugetPath
	The path to your local nuget repo.
.Example
	Run-Build -configuration:DEBUG -projectName:MyProject -nugetPath:"C:\LocalNuget"
#>
[CmdletBinding(SupportsShouldProcess=$true)]
param(
[Parameter(HelpMessage="The current configuration.  This script will only run in DEBUG")]
[string] $configuration,
[Parameter(HelpMessage="The project name. ")]
[string] $projectName,
[Parameter(HelpMessage="Local Nuget Gallery.  Pass null to skip this step")]
[string] $nugetPath

)
<#
Removes any existing nuget packages from the build folder
#>
function RemoveExistingNuGetPackages(){
	logger "Deleting NuGet Packages From .\\bin\\$configuration\\*.nupkg for project $projectName"
	Remove-Item ".\\bin\\$configuration\\*.nupkg"
}
<#
	call dotnet pack on the current project
#>
function GenerateNugetPackage(){
      Write-Host "Building NugetPackage"
     dotnet pack --no-build --configuration $configuration
}
<#
	If a path was passed, attempts to copy the newly created nuget package to a local repository
#>
function DeployToLocalNuGetShare(){
	Write-Host "Deploying Package To Local Nuget Share $nugetPath"
	 Copy-Item ".\\bin\\$configuration\\*.nupkg"  $nugetPath -Force
}
<#
	delete the entire NuGet package folder from the user's local package store.  Consumers of this package will need to issue an Install-Package <projectName> once the script
	completes.
#>
function DeleteNuGetPackageFolder(){
	$path = $env:userprofile
	$path = [io.path]::combine($path,".nuget", "packages", $projectName)
	if( Test-Path $path){
		Write-Host "Deleting cached Nuget Folder $path"
		Remove-Item $path -Recurse -Force
	 }
}
function logger(){
  param
    (
        [Parameter(Position=0,ValueFromPipeline=$true)]
        [string]$msg,
        [string]$BackgroundColor = "Yellow",
        [string]$ForegroundColor = "Black"
    )

    Write-Host -BackgroundColor $BackgroundColor -ForegroundColor $ForegroundColor $msg;
}

if( $configuration -eq "DEBUG"  -or $configuration -eq "debug"){
	RemoveExistingNuGetPackages
	GenerateNugetPackage
	if( [string]::IsNullOrEmpty($nugetPath) -eq $false){
		DeployToLocalNuGetShare
		DeleteNuGetPackageFolder
	}
}
else{
	Write-Host "Configuration Is Not Debug.  PostCompile step will be skipped."
}