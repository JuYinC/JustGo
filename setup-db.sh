#!/bin/bash

# JustGo Database Setup Script for Docker SQL Server

# Load environment variables from .env file
if [ -f .env ]; then
    export $(grep -v '^#' .env | xargs)
fi

# Default password if not set in .env
SA_PASSWORD=${SA_PASSWORD:-"JustGo2025!DevSQL#Secure"}

echo "================================================"
echo "  JustGo - Database Setup Script"
echo "================================================"
echo ""

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Error: Docker is not running. Please start Docker first."
    exit 1
fi

echo "✓ Docker is running"
echo ""

# Start SQL Server container
echo "🚀 Starting SQL Server container..."
docker-compose up -d

echo ""
echo "⏳ Waiting for SQL Server to start (30 seconds)..."
sleep 30

echo ""
echo "✓ SQL Server container is running!"
echo ""

# Display connection info
echo "================================================"
echo "  SQL Server Connection Information"
echo "================================================"
echo "Server: localhost,1433"
echo "Database: Travel (will be created by migrations)"
echo "User: sa"
echo "Password: (set in .env file)"
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "⚠️  .NET SDK not found. Please install .NET 6.0 SDK first."
    echo ""
    echo "To install .NET SDK:"
    echo "  Visit: https://dotnet.microsoft.com/download/dotnet/6.0"
    echo ""
    exit 1
fi

echo "✓ .NET SDK found: $(dotnet --version)"
echo ""

# Navigate to project directory
cd JustGo

# Restore packages
echo "📦 Restoring NuGet packages..."
dotnet restore

echo ""
echo "🔨 Building project..."
dotnet build

echo ""
echo "🗄️  Running database migrations..."
dotnet ef database update

echo ""
echo "================================================"
echo "  ✅ Setup Complete!"
echo "================================================"
echo ""
echo "To run the application:"
echo "  cd JustGo"
echo "  dotnet run"
echo ""
echo "Or press F5 in VS Code to debug"
echo ""
echo "To stop SQL Server:"
echo "  docker-compose down"
echo ""
echo "To stop and remove data:"
echo "  docker-compose down -v"
echo ""
