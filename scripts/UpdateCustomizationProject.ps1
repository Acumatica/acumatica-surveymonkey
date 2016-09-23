Add-Type -A System.IO.Compression.FileSystem
$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition
$zipExists = test-path "${scriptPath}\..\SurveyMonkeyIntegration.zip"
if ($zipExists -eq $true)
{
	Remove-Item "${scriptPath}\..\SurveyMonkeyIntegration.zip"
}
[IO.Compression.ZipFile]::CreateFromDirectory("${scriptPath}\..\SurveyMonkeyIntegration", "${scriptPath}\..\SurveyMonkeyIntegration.zip")
