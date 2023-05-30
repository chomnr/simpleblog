#!/bin/bash

# Run EF Migrations
dotnet ef migrations add "InitialMigration" --project Application --startup-project Web --output-dir Infrastructure/Persistence/Migrations

# Apply Database Update
dotnet ef database update --project Application --startup-project Web

sed -i "s|Host={localhost}; Database={db}; Username={username}; Password={password}!|Host=$POSTGRES_HOST; Database=$POSTGRES_DB; Username=$POSTGRES_USER; Password=$POSTGRES_PASS;|" appsettings.json