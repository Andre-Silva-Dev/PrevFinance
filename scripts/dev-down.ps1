Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Write-Host 'Stopping local containers...'
docker compose down
Write-Host 'Environment is down.'
