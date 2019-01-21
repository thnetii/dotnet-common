#! /usr/bin/pwsh

[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$resp = Invoke-WebRequest -Uri "https://github.com/thnetii/azure-msbuild/releases/latest" -Method Head
[uri]$tagUri = $resp.BaseResponse.ResponseUri
[string]$tagName = $tagUri.Segments | Select-Object -Last 1
[string]$fileName = "THNETII.AzureDevOps.MSBuild.Logger.zip"
$filePath = Join-Path $PSScriptRoot $fileName
[uri]$dlUri = New-Object System.Uri @($tagUri, "../download/$tagName/$fileName")
Invoke-WebRequest -Uri $dlUri -OutFile $filePath
Expand-Archive -Path $filePath -DestinationPath $PSScriptRoot
