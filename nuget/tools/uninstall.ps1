param($installPath, $toolsPath, $package, $project)

# Get the open solution.
$solution = Get-Interface $dte.Solution ([EnvDTE80.Solution2])

# Get Solution Items folder
$solutionFolder = $solution.Projects | where-object { $_.ProjectName -eq "Solution Items" } | select -first 1

if($solutionFolder) {
	$configFile = $solutionFolder.ProjectItems | where-object { $_.Name -eq "RefRestrict.config.xml" } | select -first 1
	$configFile.Remove()
	#if({$solutionFolder.ProjectItems.Count -eq 0}) {
	#	$solProj = $solution.Projects | where-object {$_.Name -eq "Solution Items"}
	#	$solution.Remove($solProj)
	#}
}

$solPath = Split-Path -parent $dte.Solution.FileName
$delExePath = $solPath + "\RefRestrict.exe"
$delXmlPath = $solPath + "\RefRestrict.config.xml"
Remove-Item $delExePath
Remove-Item $delXmlPath
