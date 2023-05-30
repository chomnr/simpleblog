#!/bin/bash

dotnet ef migrations add "InitialMigration" --project ../Application --startup-project ../Web --output-dir Infrastructure/Persistence/Migrations
