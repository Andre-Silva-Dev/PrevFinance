Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Set-Location "$PSScriptRoot\..\src\backend"
dotnet run --project PrevFinance.Api
