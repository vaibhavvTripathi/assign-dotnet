# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["MyApi.csproj", "./"]
RUN dotnet restore "MyApi.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "MyApi.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "MyApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the runtime image for running the app
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install SQLite (in case you need the CLI tools)
RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

# Create directory for SQLite database
RUN mkdir -p /app/data

EXPOSE 8080
EXPOSE 8081

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MyApi.dll"]
