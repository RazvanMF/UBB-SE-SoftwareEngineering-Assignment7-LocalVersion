Write-Host ("[i] Running coverage script...");
[array] $commandOutput = dotnet test --collect:"XPlat Code Coverage";

while (!$commandOutput[43].Trim().Contains("coverage.cobertura.xml")) {
    Write-Host ("[-] Unexpected error has occurred. Reattempting operation...");
    Write-Host ("[i] Running coverage script...");
    [array] $commandOutput = dotnet test --collect:"XPlat Code Coverage";
}

Write-Host ("[i] Test results can be found in " + $commandOutput[43].Trim())
$coverageCoberturaFileLocation = $commandOutput[43].Trim()

Write-Host ("[i] Generating report...")
reportgenerator -reports:$coverageCoberturaFileLocation -targetdir:"coveragereport" -reporttypes:Html > $null
Write-Host ("[+] Report successfully generated.")

$currentFileLocation = Get-Location
$nameOfCurrentFile = $coverageCoberturaFileLocation.Split('\')[8]
$testEntriesFolderLocation = $currentFileLocation.ToString().Trim() + "\NamespaceGPT.Tests\TestResults"
$noFolders = (Get-ChildItem -Path $testEntriesFolderLocation -Directory).Count
Write-Host("[i] Repeated uses can lead to numerous and outdated entries in the TestResults folder.")
If ($noFolders -eq 1) {
    Write-Host("[i] Currently, there is 1 folder inside the 'TestResults' directory.")
}
Else {
    Write-Host("[i] Currently, there are " + $noFolders + " folders inside the 'TestResults' directory.")
}
$option = (Read-Host "[?] Do you want to erase the contents of this folder, except the newest one? (Y/n)").ToLower()
If ($option -eq "y") {
    foreach ($folder in $(Get-ChildItem -Path $testEntriesFolderLocation -Directory)) {
        Remove-Item  $($testEntriesFolderLocation + "\" + $folder) -Force -Recurse -Exclude $nameOfCurrentFile
        if ($folder.ToString() -ne $nameOfCurrentFile.ToString()) {
            Write-Host ("[i] Deleted entry " + $folder + " from 'TestResults'")
        }
    }
}
Write-Host ("[+] Successfully deleted old coverage tests.")

Write-Host ("[i] Opening report file...")
Invoke-Item ($currentFileLocation.ToString().Trim() + "\coveragereport\index.html")

Read-Host -Prompt "[+] All Done! Press Enter to exit"