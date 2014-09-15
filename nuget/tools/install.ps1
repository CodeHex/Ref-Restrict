param($installPath, $toolsPath, $package, $project)

# Get the open solution.
$solution = Get-Interface $dte.Solution ([EnvDTE80.Solution2])

# Create Solution Items folder
$solutionFolder = $solution.Projects | where-object { $_.ProjectName -eq "Solution Items" } | select -first 1

if(!$solutionFolder) {
	$solutionFolder = $solution.AddSolutionFolder("Solution Items")
}
$solItems = Get-Interface $solutionFolder.ProjectItems ([EnvDTE.ProjectItems])


# Copy the exe and xml to the solution folder
$refRestrictExe = join-path $installPath 'tools/RefRestrict.exe'
Copy-Item $refRestrictExe $rootdir

$refRestrictXml = join-path $installPath 'tools/RefRestrict.config.xml'
Copy-Item $refRestrictXml $rootdir

# Add a file to the child solution folder.
$solPath = Split-Path -parent $dte.Solution.FileName
$configfile = $solPath + "\RefRestrict.config.xml"

$solItems.AddFromFile($configfile) > $null

# Remove placeholder file
$placeholder = $project.ProjectItems | where-object { $_.Name -eq "rrproj.txt" } | select -first 1
$placeholder.Delete()


