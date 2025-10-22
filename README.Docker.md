# 使用 Docker SQL Server 執行 JustGo

## 系統需求

1. **Docker Desktop** 已安裝並執行中
2. **.NET 6.0 SDK** 已安裝
3. **VS Code** 含 C# 擴充套件

## 快速開始

### 1. 啟動 SQL Server 容器

```bash
# 賦予設定腳本執行權限
chmod +x setup-db.sh

# 執行設定腳本（自動完成所有設定）
./setup-db.sh
```

或手動執行：

```bash
# 啟動 Docker 中的 SQL Server
docker-compose up -d

# 等待約 30 秒讓 SQL Server 準備就緒
sleep 30

# 進入專案資料夾
cd JustGo

# 還原套件並建置
dotnet restore
dotnet build

# 執行遷移以建立資料庫
dotnet ef database update

# 執行應用程式
dotnet run
```

### 2. 存取應用程式

開啟瀏覽器並前往：
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

## Docker 指令

### 查看執行中的容器
```bash
docker ps
```

### 查看 SQL Server 日誌
```bash
docker logs justgo-sqlserver
```

### 停止 SQL Server（保留資料）
```bash
docker-compose down
```

### 停止 SQL Server 並移除所有資料
```bash
docker-compose down -v
```

### 重新啟動 SQL Server
```bash
docker-compose restart
```

## 連線字串

Docker 連線字串已在 User Secrets 中設定：

```bash
dotnet user-secrets list
# 輸出:
# ConnectionStrings:TravelDocker = Server=localhost,1433;Database=Travel;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=True
```

> 💡 **注意**: 連線字串現在使用 User Secrets 管理，不再硬編碼在 `appsettings.json` 中。詳見 [SECRETS_GUIDE.md](SECRETS_GUIDE.md)

## 連線到 SQL Server

### 使用 Azure Data Studio 或 SQL Server Management Studio

- **伺服器**: localhost,1433
- **驗證方式**: SQL Server Authentication
- **登入名稱**: sa
- **密碼**: YourStrong@Passw0rd
- **資料庫**: Travel

### 使用 Docker CLI

```bash
docker exec -it justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YourStrong@Passw0rd" -C
```

執行 SQL 查詢範例：

```sql
-- 切換到 Travel 資料庫
USE Travel;
GO

-- 查看所有資料表
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
GO

-- 查詢部落格數量
SELECT COUNT(*) AS BlogCount FROM Blog;
GO

-- 查詢景點數量
SELECT COUNT(*) AS PlaceCount FROM Place;
GO
```

## 故障排除

### SQL Server 無法啟動

檢查日誌：
```bash
docker logs justgo-sqlserver
```

常見問題：
- Port 1433 已被佔用
- Docker 分配的記憶體不足
- 密碼不符合複雜度要求

**解決方法**:
```bash
# 檢查 1433 port 是否被佔用
netstat -an | grep 1433  # Linux/Mac
netstat -an | findstr 1433  # Windows

# 如果被佔用，停止佔用的服務或更改 docker-compose.yml 中的 port
```

### 連線被拒絕

確認以下事項：
1. 容器正在執行中: `docker ps`
2. 啟動後等待 30 秒
3. 檢查防火牆設定

```bash
# 檢查容器狀態
docker ps | grep justgo-sqlserver

# 檢查容器健康狀態
docker inspect justgo-sqlserver | grep -A 10 "Health"
```

### 資料庫連線錯誤

如果遇到連線錯誤，請確認 User Secrets 已正確設定：

```bash
cd JustGo
dotnet user-secrets list
```

應該看到：
```
ConnectionStrings:TravelDocker = Server=localhost,1433;Database=Travel;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=True
```

如果沒有，請設定：
```bash
dotnet user-secrets set "ConnectionStrings:TravelDocker" "Server=localhost,1433;Database=Travel;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

### 重置所有設定

```bash
# 停止並移除所有內容
docker-compose down -v

# 重新開始
docker-compose up -d
sleep 30

# 重新匯入測試資料
./import-seed-data.sh
```

## 切換資料庫連線

編輯 `Program.cs` 第 16-30 行來切換：

```csharp
// 使用 Docker（預設）
var connectionString = builder.Configuration.GetConnectionString("TravelDocker");

// 使用 Azure 雲端資料庫
var connectionString = builder.Configuration.GetConnectionString("TravelPssP");

// 使用本機 Windows SQL Server
var connectionString = builder.Configuration.GetConnectionString("TravelWindows");
```

> 💡 **最佳實踐**: 使用環境變數或 User Secrets 管理不同環境的連線字串

## 資料庫遷移 (Migrations)

### 建立新的遷移

```bash
cd JustGo

# 為 TravelContext 建立遷移
dotnet ef migrations add MigrationName --context TravelContext

# 為 ApplicationDbContext 建立遷移
dotnet ef migrations add MigrationName --context ApplicationDbContext
```

### 套用遷移

```bash
# 套用 TravelContext 遷移
dotnet ef database update --context TravelContext

# 套用 ApplicationDbContext 遷移
dotnet ef database update --context ApplicationDbContext
```

### 回復到特定遷移

```bash
dotnet ef database update PreviousMigrationName --context TravelContext
```

### 移除最後一個遷移

```bash
dotnet ef migrations remove --context TravelContext
```

### 查看所有遷移

```bash
# 查看遷移歷史
dotnet ef migrations list --context TravelContext
dotnet ef migrations list --context ApplicationDbContext
```

## 測試資料匯入

本專案已準備完整的台灣旅遊測試資料。

### 自動匯入（推薦）

```bash
# 使用自動化腳本
./import-seed-data.sh
```

此腳本會自動：
1. 檢查 SQL Server 是否執行中
2. 執行 EF Core 資料庫遷移
3. 匯入所有測試資料

### 手動匯入

```bash
# 啟動資料庫
docker-compose up -d

# 執行遷移
cd JustGo
dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context TravelContext
cd ..

# 匯入種子資料
docker exec -i justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YourStrong@Passw0rd" -C < database/seed-all-data.sql
```

### 測試資料內容

匯入後，資料庫將包含：
- **31 個台灣景點** - 涵蓋台北、台中、台南、高雄等地
- **5 筆天氣資料**
- **3 個測試帳號** - 密碼皆為 `Test@123`
- **5 篇旅遊部落格** - 含 22 筆詳細遊記
- **所有內容使用繁體中文**

詳細資訊請參考 [README.SeedData.md](README.SeedData.md)

## 資料庫備份與還原

### 備份資料庫

```bash
# 建立備份
docker exec justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YourStrong@Passw0rd" -C \
  -Q "BACKUP DATABASE [Travel] TO DISK = N'/var/opt/mssql/data/Travel.bak' WITH FORMAT"

# 複製備份檔到本機
docker cp justgo-sqlserver:/var/opt/mssql/data/Travel.bak ./Travel.bak
```

### 還原資料庫

```bash
# 複製備份檔到容器
docker cp ./Travel.bak justgo-sqlserver:/var/opt/mssql/data/Travel.bak

# 還原資料庫
docker exec justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YourStrong@Passw0rd" -C \
  -Q "RESTORE DATABASE [Travel] FROM DISK = N'/var/opt/mssql/data/Travel.bak' WITH REPLACE"
```

## Docker Compose 設定說明

### 檔案: docker-compose.yml

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: justgo-sqlserver
    environment:
      - SA_PASSWORD=${SA_PASSWORD:-YourStrong@Passw0rd}
      - ACCEPT_EULA=Y
    ports:
      - "1433:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql

volumes:
  sqlserver-data:
```

### 環境變數

可以透過 `.env` 檔案設定：

```bash
# .env
SA_PASSWORD=YourStrong@Passw0rd
DB_PORT=1433
```

## 效能優化建議

### 1. 增加 Docker 記憶體配置

在 Docker Desktop 設定中，建議分配至少 4GB RAM。

### 2. 資料庫索引

測試資料已包含以下索引：
- Blog.UserId
- BlogDetails.BlogId, PlaceId
- UserKeep 組合索引

### 3. 連線池設定

連線字串已包含 `MultipleActiveResultSets=True` 以提升效能。

## 正式環境注意事項

**⚠️ 重要**:
- 正式環境務必更改 SA 密碼！
- 使用環境變數或 Azure Key Vault 管理敏感資料
- 考慮使用 Docker secrets 保護密碼
- 建立適當的備份策略
- 啟用 SQL Server 加密連線
- 設定防火牆規則限制存取

### 正式環境建議設定

```yaml
# docker-compose.prod.yml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - SA_PASSWORD=${SA_PASSWORD}  # 從環境變數讀取
      - ACCEPT_EULA=Y
    volumes:
      - sqlserver-data:/var/opt/mssql
      - ./backups:/var/opt/mssql/backups  # 備份目錄
    restart: unless-stopped
    networks:
      - backend
    # 不要暴露 port 到外部
    expose:
      - "1433"
```

## 常見問題 (FAQ)

### Q: 為什麼使用 Docker 而不是安裝 SQL Server？

A: Docker 的優點：
- 不需要完整安裝 SQL Server
- 跨平台（Windows、Mac、Linux）
- 容易清理和重置
- 團隊環境一致
- 隔離性好，不影響系統

### Q: 資料會保留嗎？

A: 會。Docker Volume `sqlserver-data` 會保存所有資料，即使容器重啟或更新。除非執行 `docker-compose down -v`。

### Q: 如何連接其他工具？

A: 使用以下連線資訊：
- Host: `localhost` 或 `127.0.0.1`
- Port: `1433`
- User: `sa`
- Password: `YourStrong@Passw0rd`
- Database: `Travel`

### Q: 可以使用其他資料庫嗎？

A: 可以。專案使用 Entity Framework Core，理論上可以切換到其他資料庫（PostgreSQL、MySQL 等），但需要：
1. 安裝對應的 EF Core Provider
2. 修改連線字串
3. 可能需要調整部分 SQL 語法

## 相關文件

- **[SECRETS_GUIDE.md](SECRETS_GUIDE.md)** - 密碼管理完整指南
- **[README.SeedData.md](README.SeedData.md)** - 測試資料說明
- **[README.md](README.md)** - 專案總覽

## 其他資源

- [Docker SQL Server 官方文件](https://hub.docker.com/_/microsoft-mssql-server)
- [EF Core Migrations 文件](https://docs.microsoft.com/zh-tw/ef/core/managing-schemas/migrations/)
- [SQL Server 效能調校](https://docs.microsoft.com/zh-tw/sql/relational-databases/performance/performance-monitoring-and-tuning-tools)
- [Docker Compose 文件](https://docs.docker.com/compose/)

---

**最後更新**: 2025-10-22
