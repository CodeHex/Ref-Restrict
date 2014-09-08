param($installPath, $toolsPath, $package, $project)

# Get the open solution.
$solution = Get-Interface $dte.Solution ([EnvDTE80.Solution2])
$deployFolder = $solution.Projects | where-object { $_.ProjectName -eq "RefRestrict" } | select -first 1

if(!$deployFolder) {
	$deployFolder = $solution.AddSolutionFolder("RefRestrict")
}

# Ref exe
$deploySource = join-path $installPath 'lib/RefRestrict.exe'
Copy-Item $deploySource $rootdir



# Add a file to the child solution folder.
$childSolutionFolder = Get-Interface $deployFolder.Object ([EnvDTE80.SolutionFolder])
$solfolder = Split-Path -parent $dte.Solution.FileName
$fileName = $solfolder + "\RefRestrict.exe"
"I'm here"
$fileName
$projectFile = $childSolutionFolder.AddFromFile($fileName)


