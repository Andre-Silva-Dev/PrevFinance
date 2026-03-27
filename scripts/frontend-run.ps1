Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Set-Location "$PSScriptRoot\..\src\frontend\prevfinance-web"
npm install
npm run start
