Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Write-Host 'Starting PostgreSQL and Redis containers...'
docker compose up -d postgres redis
Write-Host 'Environment is up.'
