Param(
    [string] $InstallDir,
    [string] $Manifest
)

$ErrorActionPreference = 'Stop'

$env:DOTNET_ROOT="$InstallDir"

Write-Host "Installing maui-check..."
& dotnet tool update --global redth.net.maui.check

Write-Host "Installing Maui..."
& maui-check `
  --manifest "$Manifest" `
  --verbose --ci --fix --non-interactive `
  --skip androidsdk `
  --skip xcode `
  --skip vswin `
  --skip vsmac

exit $LASTEXITCODE
