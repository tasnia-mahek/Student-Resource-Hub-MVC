# Deployment notes

This repository is prepared for a Render container deployment using Docker.

## Required external configuration

Set these in the hosting UI as environment variables:

- ASPNETCORE_ENVIRONMENT=Production
- ConnectionStrings__DefaultConnection=Server=...;Port=3306;Database=...;User=...;Password=...;

## Important: uploaded videos/files

Uploaded media are currently stored under the app filesystem. On free hosting, this is not durable across redeploys. For a production-safe deployment, keep the app on Render but store uploaded videos in persistent storage or object storage and configure the app to use that root.

## Safe rollback

Keep the current repo state in Git before deploying, and create a branch before any production changes.

## Normal deployment flow

1. Push this repo to GitHub.
2. Create a Render Web Service using the Dockerfile.
3. Add the MySQL connection string as an environment variable.
4. Deploy.
5. Verify the app loads and the database connection works.
6. Upload a test file and verify it can be served.
