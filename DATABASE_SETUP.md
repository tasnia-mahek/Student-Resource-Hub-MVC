# Student Resource Hub Database Setup

This guide sets up a local MySQL database for the Student Resource Hub MVC application on Windows. It assumes a fresh machine with neither MySQL nor the .NET development tools configured.

## What the application expects

The application uses:

- MySQL Server on `localhost:3306`
- Database name: `StudentResourceHubDb`
- Database character set: `utf8mb4`
- EF Core migrations stored in the repository
- Connection-string key: `ConnectionStrings:DefaultConnection`

Do not edit the tracked `appsettings.json` to add a personal password. Each developer should store their local connection string in .NET User Secrets.

## 1. Install the prerequisites

### Install .NET 10 SDK

Install the .NET 10 SDK from the official Microsoft download page:

<https://dotnet.microsoft.com/download/dotnet/10.0>

After installing, open a new PowerShell window and verify it:

```powershell
dotnet --version
```

The result should be `10.x`.

### Install MySQL Server

The simplest command-line option is WinGet. Run PowerShell as an administrator:

```powershell
winget install --id Oracle.MySQL -e
```

If that package is not available on your machine, download and run the official MySQL Installer Community package instead:

<https://dev.mysql.com/downloads/installer/>

During installation, choose **MySQL Server**. MySQL Workbench is optional; the commands in this guide use the MySQL command-line client. When the installer asks for configuration values, use these local-development settings:

- Port: `3306`
- Windows service: enabled
- Service name: `MySQL80` (the default)
- Authentication: the recommended strong password authentication
- Root password: create a password and keep it private

Close and reopen PowerShell after installation, then verify the client is available:

```powershell
mysql --version
```

If PowerShell cannot find `mysql`, add the MySQL `bin` directory to the current PATH. The default location is usually:

```powershell
$env:Path += ";C:\Program Files\MySQL\MySQL Server 8.0\bin"
mysql --version
```

This changes PATH only for the current PowerShell window. Reopen PowerShell after adding the directory permanently through **System Properties > Environment Variables** if needed.

### Confirm that the MySQL service is running

```powershell
Get-Service -Name MySQL* 
```

If the service is stopped, start the default service from an elevated PowerShell window:

```powershell
Start-Service -Name MySQL80
```

If the installed service has a different name, use the name shown by `Get-Service -Name MySQL*`.

## 2. Create the application database and user

Sign in to MySQL with the root password created during installation:

```powershell
mysql -u root -p
```

At the `mysql>` prompt, run the following SQL. Replace `YOUR_APP_PASSWORD` with a strong local password. Do not use angle brackets in the actual password.

```sql
CREATE DATABASE IF NOT EXISTS StudentResourceHubDb
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

CREATE USER IF NOT EXISTS 'studenthub_app'@'localhost'
  IDENTIFIED BY 'YOUR_APP_PASSWORD';

ALTER USER 'studenthub_app'@'localhost'
  IDENTIFIED BY 'YOUR_APP_PASSWORD';

GRANT ALL PRIVILEGES ON StudentResourceHubDb.*
  TO 'studenthub_app'@'localhost';

FLUSH PRIVILEGES;
```

Exit the MySQL client:

```sql
EXIT;
```

The application user needs access to this database so EF Core can create and update tables. It does not need global privileges over every database on the server.

## 3. Configure the connection string locally

From the repository root, restore the project and create the local User Secrets store:

```powershell
cd path\to\Student-Resource-Hub-MVC
dotnet restore
dotnet user-secrets init --project .\student-resource-hub.csproj
```

Set the connection string. Use the same password entered in the SQL commands above:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=StudentResourceHubDb;User=studenthub_app;Password=YOUR_APP_PASSWORD;" --project .\student-resource-hub.csproj
```

Confirm that the secret exists without printing its password:

```powershell
dotnet user-secrets list --project .\student-resource-hub.csproj
```

The value is stored outside the repository. Never commit a real password, and do not paste one into source files, screenshots, issues, or chat.

## 4. Apply the EF Core schema

The repository contains the initial schema migration and the later academic-catalog migrations. Apply all pending migrations with:

```powershell
dotnet ef database update --project .\student-resource-hub.csproj --startup-project .\student-resource-hub.csproj
```

If `dotnet ef` is not recognized, install the CLI tool that matches the EF Core package version used by this project:

```powershell
dotnet tool install --global dotnet-ef --version 9.0.0
```

Then rerun the migration command. If the tool is already installed, update it with:

```powershell
dotnet tool update --global dotnet-ef --version 9.0.0
```

Check the applied migrations:

```powershell
dotnet ef migrations list --project .\student-resource-hub.csproj --startup-project .\student-resource-hub.csproj
```

You should see `20260918090705_InitialMySqlSchema`, `20260918133713_AddAcademicTermsAndCourses`, and `20260918145307_SeedDistinctSemesterCourses` in the project migration list. The database should also contain an `__EFMigrationsHistory` table. The last migration adds nine distinct courses to each fixed semester for every department.

## 5. Verify the database connection

Check that the application user can connect to the new database:

```powershell
mysql -u studenthub_app -p -D StudentResourceHubDb -e "SHOW TABLES;"
```

After the migration, the command should list the application tables. If it returns an access-denied error, verify the username, password, host (`localhost`), and the `GRANT` statement from step 2.

## 6. Run the application

Start the application from the repository root:

```powershell
dotnet run
```

Open the URL printed in the terminal. With the included development launch settings, the usual URLs are:

- <http://localhost:5041>
- <https://localhost:7141>

Keep MySQL running while the application is running.

## Common fixes

### `Unable to connect to any of the specified MySQL hosts`

Confirm that the service is running and that MySQL is listening on port `3306`:

```powershell
Get-Service -Name MySQL*
Test-NetConnection localhost -Port 3306
```

### `Access denied for user`

Log in as root and reset the application user password. Then update the matching User Secret:

```sql
ALTER USER 'studenthub_app'@'localhost' IDENTIFIED BY 'YOUR_APP_PASSWORD';
GRANT ALL PRIVILEGES ON StudentResourceHubDb.* TO 'studenthub_app'@'localhost';
FLUSH PRIVILEGES;
```

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=StudentResourceHubDb;User=studenthub_app;Password=YOUR_APP_PASSWORD;" --project .\student-resource-hub.csproj
```

### `dotnet ef` cannot be found

Install the global tool from step 4, then close and reopen PowerShell so the global tools directory is on PATH. Verify with:

```powershell
dotnet ef --version
```

### The schema is out of date

Run the migration command again:

```powershell
dotnet ef database update --project .\student-resource-hub.csproj --startup-project .\student-resource-hub.csproj
```

Do not delete migration files or manually recreate tables unless the team has agreed on a migration change.

## Resetting a local database

This permanently deletes local data. Only run it when you intentionally want a clean database:

```powershell
mysql -u root -p -e "DROP DATABASE IF EXISTS StudentResourceHubDb; CREATE DATABASE StudentResourceHubDb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;"
dotnet ef database update --project .\student-resource-hub.csproj --startup-project .\student-resource-hub.csproj
```

Never run the reset command against a shared, staging, or production database.