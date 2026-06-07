#!/usr/bin/env bash
# Bootstraps the local SQL Server container with both schema scripts.
# Usage:  ./seed-mac.sh
set -euo pipefail

CONTAINER="loan-dashboard-mssql"
SA_PASSWORD="YourStrong!Passw0rd"

if ! docker ps --format '{{.Names}}' | grep -q "^${CONTAINER}$"; then
  echo "Starting SQL Server container..."
  docker compose up -d sqlserver
  echo "Waiting for SQL Server to accept connections..."
  for i in {1..30}; do
    if docker exec "${CONTAINER}" /opt/mssql-tools18/bin/sqlcmd \
        -S localhost -U sa -P "${SA_PASSWORD}" -C -Q "SELECT 1" >/dev/null 2>&1; then
      echo "  ready."
      break
    fi
    sleep 2
  done
fi

SQLCMD="/opt/mssql-tools18/bin/sqlcmd"

run_sql() {
  local file="$1"
  echo ">>> Running ${file}"
  docker exec -i "${CONTAINER}" "${SQLCMD}" \
      -S localhost -U sa -P "${SA_PASSWORD}" -C \
      -i "/scripts/${file}"
}

docker exec "${CONTAINER}" mkdir -p /scripts
docker cp init-db-sqlserver.sql  "${CONTAINER}":/scripts/init-db-sqlserver.sql
docker cp init-ep101-schema.sql  "${CONTAINER}":/scripts/init-ep101-schema.sql

run_sql init-db-sqlserver.sql
run_sql init-ep101-schema.sql

echo
echo "✓ Database seeded. Now run:"
echo "    cd src/LoanDashboard.MVC && dotnet run"
