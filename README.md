# Student Resource Hub

For the complete Windows/MySQL setup from a fresh machine, see [DATABASE_SETUP.md](DATABASE_SETUP.md).

## MySQL setup

This project uses Pomelo Entity Framework Core with MySQL. Create a MySQL database and configure the connection locally with User Secrets:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost;Port=3306;Database=StudentResourceHubDb;User=YOUR_USER;Password=YOUR_PASSWORD;" --project .\student-resource-hub.csproj
dotnet ef database update --project .\student-resource-hub.csproj --startup-project .\student-resource-hub.csproj
```

The connection string is intentionally kept out of tracked files. For deployment, provide the same value through the host's connection-string or environment-variable configuration.

## Video lectures

Lecture uploads accept common video containers including MP4, M4V, MOV, AVI, WMV, MKV, WebM, FLV, 3GP, OGV, TS, MTS, M2TS, VOB, and ASF. The application does not impose a lecture-specific size limit, but the hosting platform, reverse proxy, available disk, and persistent storage still apply their own limits.

Lecture files are stored under `wwwroot/uploads/lectures` and streamed with HTTP range support for browser-compatible formats. Production deployment should mount persistent storage or use object storage; otherwise uploaded files can disappear when the app is redeployed.