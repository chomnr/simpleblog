#!/bin/bash

# Run EF Migrations
dotnet ef migrations add "InitialMigration" --project Application --startup-project Web --output-dir Infrastructure/Persistence/Migrations

# Apply Database Update
dotnet ef database update --project Application --startup-project Web