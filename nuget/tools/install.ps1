param($installPath, $toolsPath, $package, $project)

# Define the Ref Restrict specific files
$configFileName = "RefRestrict.config.xml"
$refRestrictExe = "RefRestrict.exe"

# Get the open solution.
$solution = Get-Interface $dte.Solution ([EnvDTE80.Solution2])
$solPath = Split-Path -parent $dte.Solution.FileName
$configPath = $solPath + "/" + $configFileName
$exePath = $solPath + "/" + $refRestrictExe

# Create Solution Items folder if it doesn't exist
$solutionFolder = $solution.Projects | where-object { $_.ProjectName -eq "Solution Items" } | select -first 1
if(!$solutionFolder) {
    $solutionFolder = $solution.AddSolutionFolder("Solution Items")
}

# Copy the exe solution folder (overwrite any existing exe)
$exeToolPath = join-path $installPath ('tools/' + $refRestrictExe)
Copy-Item $exeToolPath $rootdir

# Copy the blank xml file if it doesn't exist
$configPath = $solPath + "/" + $configFileName

if(!(Test-Path $configPath)) {
    # Copy blank config file
    $configToolPath = join-path $installPath ('tools/' + $configFileName)
    Copy-Item $configToolPath $rootdir
    
    # Add a file to the Solution Items folder.
    $solItems = Get-Interface $solutionFolder.ProjectItems ([EnvDTE.ProjectItems])
    $solItems.AddFromFile($configPath) > $null
}

# Add section for project
$configXml = New-Object XML
$configXml.Load($configPath)
$ruleNode = $configXml.CreateElement("rules")
$ruleNode.SetAttribute("project", $project.ProjectName)
$ruleData = $configXml.CreateComment("Add rules for project " + $project.ProjectName + " here")
$ruleNode.AppendChild($ruleData) > $null
$configXml.DocumentElement.AppendChild($ruleNode) > $null
$configXml.Save($configPath)
