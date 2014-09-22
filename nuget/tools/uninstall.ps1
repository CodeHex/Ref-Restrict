param($installPath, $toolsPath, $package, $project)

# Define the Ref Restrict specific files
$configFileName = "RefRestrict.config.xml"
$refRestrictExe = "RefRestrict.exe"

# Get the open solution.
$solution = Get-Interface $dte.Solution ([EnvDTE80.Solution2])
$solPath = Split-Path -parent $dte.Solution.FileName
$configPath =$solPath + "/" + $configFileName
$exePath = $solPath + "/" + $refRestrictExe

# Determine if other projects are have the Ref Restrict package. If none of them have it,
# indicate that you want to remove Ref Restrict completely.
$remove = $TRUE
foreach ($proj in Get-Project -All) {
    $refRestrictPackage = Get-Package -ProjectName $proj.ProjectName -Filter "RefRestrict"
    if($refRestrictPackage -and $proj.ProjectName -ne $project.ProjectName) { 
        $remove = $FALSE 
    }
}

# If we need to remove Ref Restrict completely...
if($remove) {
    # If the solution items folder exist...
    $solItems = $solution.Projects | where-object { $_.ProjectName -eq "Solution Items" } | select -first 1
    if($solItems) {
        #...and the config file is in this folder, then remove it from the folder
        $configFile = $solItems.ProjectItems | where-object { $_.Name -eq $configFileName } | select -first 1
        if($configFile) { $configFile.Delete() }
    }

    # Delete Ref Restrict files
    Remove-Item $configPath
    Remove-Item $exePath 
    
} else {
    # Load the config file
    $configXml = New-Object XML
    $configXml.Load($configPath)

    #Just remove the reference in the config XML file
    $nodeToDelete = $configXml.SelectSingleNode("rrconfig/rules[@project='" + $project.ProjectName + "']")
    if($nodeToDelete) {
        $nodeToDelete.ParentNode.RemoveChild($nodeToDelete) > $null
        $configXml.Save($configPath)    
    }   
}
 
