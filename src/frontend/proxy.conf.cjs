const fs = require('node:fs');
const path = require('node:path');

function parseDotEnv(filePath) {
  if (!fs.existsSync(filePath)) {
    return {};
  }

  const result = {};
  const lines = fs.readFileSync(filePath, 'utf8').split(/\r?\n/);

  for (const rawLine of lines) {
    const line = rawLine.trim();
    if (!line || line.startsWith('#')) {
      continue;
    }

    const separatorIndex = line.indexOf('=');
    if (separatorIndex <= 0) {
      continue;
    }

    const key = line.slice(0, separatorIndex).trim();
    let value = line.slice(separatorIndex + 1).trim();

    if (value.startsWith('"') && value.endsWith('"') && value.length >= 2) {
      value = value.slice(1, -1);
    }

    result[key] = value;
  }

  return result;
}

const workspaceRoot = path.resolve(__dirname, '..');
const envPath = path.join(workspaceRoot, '.env');
const env = parseDotEnv(envPath);

const apiBaseUrl = process.env.FRONTEND_API_BASE_URL || env.FRONTEND_API_BASE_URL || 'http://localhost:5071/api';
const target = apiBaseUrl.endsWith('/api') ? apiBaseUrl.slice(0, -4) : apiBaseUrl;

module.exports = {
  '/api': {
    target,
    secure: false,
    changeOrigin: true,
    logLevel: 'info'
  }
};
