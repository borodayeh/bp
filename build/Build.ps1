param(
    [ValidateSet('Debug', 'Release')]
codex/create-bp-architecture-and-delivery-specifications-jcqw5n
    [string]$Configuration = 'Release',
    [string]$Runtime = 'win-x64',
    [string]$Version = '0.1.0'
)

$ErrorActionPreference = 'Stop'
$Root = Split-Path -Parent $PSScriptRoot
$ArtifactsRoot = Join-Path $Root 'artifacts'
$PublishDir = Join-Path $ArtifactsRoot $Runtime

Write-Host "BP build pipeline ($Configuration, $Runtime, $Version)"
=======
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
Write-Host "BP build pipeline bootstrap ($Configuration)"
main

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Warning 'dotnet SDK is not installed in this environment. Build steps are scaffolded only.'
    exit 0
}

codex/create-bp-architecture-and-delivery-specifications-jcqw5n
dotnet restore "$Root/src/BP.App/BP.App.csproj"
dotnet build "$Root/src/BP.App/BP.App.csproj" -c $Configuration --no-restore
dotnet test "$Root/tests/BP.UnitTests/BP.UnitTests.csproj" -c $Configuration

dotnet publish "$Root/src/BP.App/BP.App.csproj" `
  -c $Configuration `
  -r $Runtime `
  -p:PublishSingleFile=true `
  -p:SelfContained=true `
  -p:Version=$Version `
  -o $PublishDir

if (Get-Command iscc -ErrorAction SilentlyContinue) {
    & iscc "$Root/installer/inno/bp.iss" /DMyAppVersion=$Version /DSourceDir=$PublishDir /O"$ArtifactsRoot"
} else {
    Write-Warning 'Inno Setup (iscc) not found. Skipping installer generation.'
}
=======
# Planned pipeline:
# 1) dotnet restore
# 2) dotnet build
# 3) dotnet test
# 4) dotnet publish -r win-x64 --self-contained true
main
