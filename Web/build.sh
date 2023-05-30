#!/bin/bash

dotnet ef migrations add "InitialMigration" --project ../Application --startup-project ../Web --output-dir Infrastructure/Persistence/Migrations
dotnet ef database update --project ../Application --startup-project Web
