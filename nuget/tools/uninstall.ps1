param($installPath, $toolsPath, $package, $project)

# Get the open solution.
$solution = Get-Interface $dte.Solution ([EnvDTE80.Solution2])

# Get Solution Items folder
$solutionFolder = $solution.Projects | where-object { $_.ProjectName -eq "Solution Items" } | select -first 1

# Determine if other projects are have the same package
$remove = $TRUE
foreach ($proj in Get-Project -All) {
    $b = Get-Package -ProjectName $proj.ProjectName -Filter "RefRestrict"
    if($b -and $proj.ProjectName -ne $project.ProjectName) { 
        $remove = $FALSE 
    }
}

if($remove -and $solutionFolder) {
    $configFile = $solutionFolder.ProjectItems | where-object { $_.Name -eq "RefRestrict.config.xml" } | select -first 1
    $configFile.Remove()
}

$solPath = Split-Path -parent $dte.Solution.FileName
$configfile = $solPath + "\RefRestrict.config.xml"
if($remove) {
    $delExePath = $solPath + "\RefRestrict.exe"
    Remove-Item $delExePath
    Remove-Item $configfile
} else {
    $configXml = New-Object XML
    $configXml.Load($configfile)

    #Just remove the reference in the XML
    $nodeToDelete = $configXml.SelectSingleNode("rrconfig/rules[@project='" + $project.ProjectName + "']")
    if($nodeToDelete) {
        $nodeToDelete.ParentNode.RemoveChild($nodeToDelete) > $null
	$configXml.Save($configfile)	
    }	
}
