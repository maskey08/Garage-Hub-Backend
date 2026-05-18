# Script to recreate the garagehub database
$connectionString = "Host=localhost;Port=5432;Username=postgres;Password=admin123"
$database = "garagehub"

# Use psql via the postgresql server
# Since psql is not available, we'll create a SQL script instead

$sqlScript = @"
-- Drop the existing database if it exists
DROP DATABASE IF EXISTS $database;

-- Create a fresh database
CREATE DATABASE $database;
"@

# Save the script to a file
$sqlScript | Out-File -FilePath "db_setup.sql" -Encoding UTF8

Write-Output "Database cleanup script created. You can run it with psql:"
Write-Output "psql -h localhost -U postgres -f db_setup.sql"
Write-Output ""
Write-Output "Or execute manually in psql:"
Write-Output "DROP DATABASE IF EXISTS garagehub;"
Write-Output "CREATE DATABASE garagehub;"
