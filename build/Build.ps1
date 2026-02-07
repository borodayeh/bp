param(
    [ValidateSet('Debug', 'Release')]
    [string]$Configuration = 'Release'
)

$ErrorActionPreference = 'Stop'
Write-Host "BP build pipeline bootstrap ($Configuration)"

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    Write-Warning 'dotnet SDK is not installed in this environment. Build steps are scaffolded only.'
    exit 0
}

# Planned pipeline:
# 1) dotnet restore
# 2) dotnet build
# 3) dotnet test
# 4) dotnet publish -r win-x64 --self-contained true
