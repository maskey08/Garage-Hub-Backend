# PostgreSQL Configuration Guide

## Issue Fixed
The application was failing with: `28P01: password authentication failed for user "postgres"`

This occurred because the password in `appsettings.json` (`12345678`) did not match the actual PostgreSQL password configured on the server.

## Solution
The application now supports loading the connection string from an environment variable, keeping sensitive credentials out of version control.

## Setup Instructions

### Option 1: Using Environment Variable (Recommended for Development)

1. **Windows (PowerShell)**
   ```powershell
   $env:GARAGEHUB_DB_CONNECTION = "Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=YOUR_ACTUAL_PASSWORD"
   ```

2. **Windows (Command Prompt)**
   ```cmd
   set GARAGEHUB_DB_CONNECTION=Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=YOUR_ACTUAL_PASSWORD
   ```

3. **Linux/macOS (Bash)**
   ```bash
   export GARAGEHUB_DB_CONNECTION="Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=YOUR_ACTUAL_PASSWORD"
   ```

4. Replace `YOUR_ACTUAL_PASSWORD` with your PostgreSQL "postgres" user password

5. Run the application:
   ```bash
   dotnet run
   ```

### Option 2: Using appsettings.Development.json (For Local Development)

Edit `appsettings.Development.json`:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore.Database.Command": "Information"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=YOUR_ACTUAL_PASSWORD"
  }
}
```

### Option 3: Using User Secrets (Recommended for Local Development)

1. Initialize user secrets (if not already done):
   ```bash
   dotnet user-secrets init
   ```

2. Set the connection string:
   ```bash
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=YOUR_ACTUAL_PASSWORD"
   ```

3. Or set via environment variable:
   ```bash
   dotnet user-secrets set "GARAGEHUB_DB_CONNECTION" "Host=localhost;Port=5432;Database=garagehub;Username=postgres;Password=YOUR_ACTUAL_PASSWORD"
   ```

## Finding Your PostgreSQL Password

If you don't remember your PostgreSQL "postgres" password:

1. **On Windows:**
   - PostgreSQL typically installs with the password you set during installation
   - Check your installation notes or try common defaults
   - You may need to reset it via PostgreSQL tools

2. **On Linux/macOS:**
   - Check PostgreSQL documentation for your system
   - Connect as the postgres system user to reset the password if needed

## Testing the Connection

After setting the environment variable, you can test the connection by running:
```bash
dotnet run
```

The application will attempt to connect during startup and seed the admin user. Look for success messages in the debug output.

## Important Security Notes

⚠️ **Never commit actual passwords to version control**
- `appsettings.json` uses a placeholder password for safety
- Actual passwords should only be in environment variables or User Secrets
- `.gitignore` should already exclude `appsettings.Development.json` and user secrets
