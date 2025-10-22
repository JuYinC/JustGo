#!/bin/bash

# JustGo - Import Seed Data Script
# 匯入台灣旅遊測試資料

echo "================================================"
echo "  JustGo - 匯入測試資料"
echo "================================================"
echo ""

# Check if SQL Server container is running
if ! docker ps | grep justgo-sqlserver > /dev/null; then
    echo "❌ Error: SQL Server container is not running."
    echo "   Please start it first: docker-compose up -d"
    exit 1
fi

echo "✓ SQL Server container is running"
echo ""

# Wait for SQL Server to be ready
echo "⏳ Checking SQL Server connection..."
for i in {1..10}; do
    if docker exec justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "SELECT 1" > /dev/null 2>&1; then
        echo "✓ SQL Server is ready"
        break
    fi
    if [ $i -eq 10 ]; then
        echo "❌ Error: Cannot connect to SQL Server"
        exit 1
    fi
    echo "   Waiting for SQL Server... ($i/10)"
    sleep 3
done

echo ""
echo "================================================"
echo "  Step 1: Creating database and running migrations"
echo "================================================"
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK not found. Please install .NET 6.0 SDK first."
    exit 1
fi

cd JustGo

# Add dotnet tools to PATH
export PATH="$PATH:$HOME/.dotnet/tools"

# Run EF Core migrations to create database schema
echo "🗄️  Running Entity Framework migrations..."
echo "   Creating ApplicationDbContext (Identity)..."
dotnet ef database update --context ApplicationDbContext

if [ $? -ne 0 ]; then
    echo "❌ Error: Failed to run ApplicationDbContext migrations"
    exit 1
fi

echo "   Creating TravelContext (Travel data)..."
dotnet ef database update --context TravelContext

if [ $? -ne 0 ]; then
    echo "❌ Error: Failed to run TravelContext migrations"
    exit 1
fi

echo "✓ Database schema created successfully"
echo ""

cd ..

echo "================================================"
echo "  Step 2: Importing all seed data"
echo "         (景點、使用者、部落格)"
echo "================================================"
echo ""

# Import all seed data from consolidated file
docker exec -i justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C < database/seed-all-data.sql

if [ $? -ne 0 ]; then
    echo "❌ Error: Failed to import seed data"
    exit 1
fi

echo "✓ All seed data imported successfully"

echo ""
echo "================================================"
echo "  ✅ All data imported successfully!"
echo "================================================"
echo ""
echo "Database Summary:"
echo "  • 31 台灣景點 (Taiwan places)"
echo "  • 5 天氣資料 (Weather data)"
echo "  • 3 使用者帳號 (User accounts)"
echo "  • 5 旅遊部落格 (Travel blogs)"
echo "  • 20 部落格詳細內容 (Blog details)"
echo ""
echo "================================================"
echo "  Test User Accounts (測試帳號)"
echo "================================================"
echo ""
echo "1. Email: taiwan.lover@justgo.com"
echo "   Password: Test@123"
echo "   Name: 台灣旅遊達人"
echo ""
echo "2. Email: mountain.hiker@justgo.com"
echo "   Password: Test@123"
echo "   Name: 登山小隊長"
echo ""
echo "3. Email: food.explorer@justgo.com"
echo "   Password: Test@123"
echo "   Name: 美食探險家"
echo ""
echo "================================================"
echo ""
echo "You can now run the application:"
echo "  cd JustGo"
echo "  dotnet run"
echo ""
echo "Or press F5 in VS Code to start debugging"
echo ""
