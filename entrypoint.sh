#!/bin/bash
set -e

echo "Starting application..."
echo "Current directory: $(pwd)"
echo "Listing files:"
ls -la

# Check if database exists
if [ -f "/app/data/myapi.db" ]; then
    echo "Database exists at /app/data/myapi.db"
else
    echo "Database does not exist yet, will be created by migrations"
fi

# Start the application (migrations will run automatically via Program.cs)
echo "Starting .NET application..."
exec dotnet MyApi.dll
