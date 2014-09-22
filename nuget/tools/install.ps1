param($installPath, $toolsPath, $package, $project)

# Get the open solution.
$solution = Get-Interface $dte.Solution ([EnvDTE80.Solution2])

# Create Solution Items folder
$solutionFolder = $solution.Projects | where-object { $_.ProjectName -eq "Solution Items" } | select -first 1

if(!$solutionFolder) {
	$solutionFolder = $solution.AddSolutionFolder("Solution Items")
}

# Copy the exe solution folder (overwrite any existing exe)
$refRestrictExe = join-path $installPath 'tools/RefRestrict.exe'
Copy-Item $refRestrictExe $rootdir

# Copy the blank xml file if it doesn't exist
$solPath = Split-Path -parent $dte.Solution.FileName
$configfile = $solPath + "\RefRestrict.config.xml"

if(!(Test-Path $configfile)) {
    # Copy blank config file
    $refRestrictXml = join-path $installPath 'tools/RefRestrict.config.xml'
    Copy-Item $refRestrictXml $rootdir

    # Add a file to the Solution Items folder.
    $solItems = Get-Interface $solutionFolder.ProjectItems ([EnvDTE.ProjectItems])
    $solItems.AddFromFile($configfile) > $null
}

# Add section for project
$configXml = New-Object XML
$configXml.Load($configfile)
$ruleNode = $configXml.CreateElement("rules")
$ruleNode.SetAttribute("project", $project.ProjectName)
$ruleData = $configXml.CreateComment("Add rules for project " + $project.ProjectName + " here")
$ruleNode.AppendChild($ruleData) > $null
$configXml.DocumentElement.AppendChild($ruleNode) > $null
$configXml.Save($configfile)
