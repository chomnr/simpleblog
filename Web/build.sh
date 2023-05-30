#!/bin/bash

#sed -i "s#Host=localhost; Database=blog; Username=postgres; Password=postgres!| Database=$POSTGRES_DB; Username=$POSTGRES_USER; Password=$POSTGRES_PASS;|" appsettings.json

psql -U postgres -c "CREATE DATABASE blog;"

# Run EF Migrations
dotnet ef migrations add "InitialMigration" --project 'Application' --startup-project 'Web' --output-dir 'Infrastructure/Persistence/Migrations'

# Apply Database Update
dotnet ef database update --project 'Application' --startup-project 'Web'
