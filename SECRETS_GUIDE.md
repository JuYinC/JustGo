# JustGo 密碼與環境變數管理指南

> 本指南說明如何安全地管理 JustGo 專案的敏感資訊（API Keys、資料庫密碼等）

---

## 📋 目錄

1. [概述](#概述)
2. [User Secrets（推薦）](#user-secrets)
3. [.env 檔案](#env-檔案)
4. [何時使用哪一種？](#何時使用哪一種)
5. [實際操作指南](#實際操作指南)
6. [安全檢查清單](#安全檢查清單)
7. [團隊協作](#團隊協作)
8. [故障排除](#故障排除)

---

## 概述

本專案使用**兩種互補的方式**管理敏感資訊：

| 方式 | 用途 | 適用情境 |
|------|------|---------|
| **User Secrets** | .NET 應用程式密碼 | API Keys, 連線字串 |
| **.env 檔案** | Docker 容器設定 | 容器環境變數 |

### 為什麼需要兩種？

- **User Secrets** → .NET 原生，最適合 C# 程式碼使用
- **.env** → Docker Compose 標準，容器啟動時需要

---

## User Secrets

### 什麼是 User Secrets?

User Secrets 是 .NET 內建的開發環境密碼管理工具，將敏感資訊儲存在**專案外部**，避免提交到版本控制系統。

### ✅ 已設定的 Secrets

```bash
cd JustGo
dotnet user-secrets list
```

輸出：
```
Google:MapsApiKey = YOUR_GOOGLE_MAPS_API_KEY_HERE
ConnectionStrings:TravelDocker = Server=localhost,1433;Database=Travel;User Id=sa;Password=YOUR_SQL_PASSWORD_HERE;TrustServerCertificate=True;MultipleActiveResultSets=True
```

### 📍 儲存位置

User Secrets 儲存在系統的使用者設定檔中，**不在專案資料夾內**：

- **Windows**: `%APPDATA%\Microsoft\UserSecrets\<user_secrets_id>\secrets.json`
- **Linux/Mac**: `~/.microsoft/usersecrets/<user_secrets_id>/secrets.json`

### 🔧 常用指令

```bash
# 查看所有 secrets
dotnet user-secrets list

# 新增/修改 secret
dotnet user-secrets set "Google:MapsApiKey" "your-api-key"
dotnet user-secrets set "ConnectionStrings:TravelDocker" "your-connection-string"

# 刪除單一 secret
dotnet user-secrets remove "Google:MapsApiKey"

# 清除所有 secrets
dotnet user-secrets clear

# 初始化 User Secrets（通常已完成）
dotnet user-secrets init
```

### 💡 在程式碼中使用

#### 1. Views (check.cshtml, itinerary.cshtml)

```cshtml
@inject IConfiguration Configuration

<!-- 使用 Google Maps API Key -->
<script async src="https://maps.googleapis.com/maps/api/js?key=@Configuration["Google:MapsApiKey"]"></script>
```

#### 2. Program.cs

```csharp
var connectionString = builder.Configuration.GetConnectionString("TravelDocker");

builder.Services.AddDbContext<TravelContext>(options =>
    options.UseSqlServer(connectionString));
```

#### 3. Controllers

```csharp
public class MyController : Controller
{
    private readonly IConfiguration _configuration;

    public MyController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        var apiKey = _configuration["Google:MapsApiKey"];
        var connectionString = _configuration.GetConnectionString("TravelDocker");
        // ...
    }
}
```

### ⚙️ appsettings.json 設定

`appsettings.json` 只保留設定結構，**不含敏感資訊**：

```json
{
  "ConnectionStrings": {
    "TravelWindows": "Data Source=.;Initial Catalog=Travel;Integrated Security=True",
    "TravelPssP": "",
    "TravelDocker": ""
  },
  "Google": {
    "MapsApiKey": ""
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

User Secrets 會在執行時**自動覆蓋**這些空值。

### 📊 設定優先順序

.NET Configuration 的讀取順序（後面的會覆蓋前面的）：

1. `appsettings.json`
2. `appsettings.{Environment}.json`
3. **User Secrets** (只在 Development 環境)
4. Environment Variables
5. Command-line arguments

### ✅ User Secrets 的優點

- ✅ .NET 原生支援，無需額外套件
- ✅ 敏感資訊不會提交到 Git
- ✅ 每個開發者可以有自己的設定
- ✅ 自動整合到 Configuration 系統
- ✅ 跨專案共用（使用相同 UserSecretsId）

### ⚠️ User Secrets 的限制

- ⚠️ **只適用於 Development 環境**
- ⚠️ 不適合正式環境（Production）
- ⚠️ 需要手動設定（團隊成員需各自設定）
- ⚠️ 僅限本機開發

---

## .env 檔案

### 📄 檔案說明

| 檔案 | 用途 | 是否提交到 Git |
|------|------|---------------|
| `.env.example` | 範例檔案，顯示需要哪些變數 | ✅ 是（範本） |
| `.env` | 實際的環境變數值 | ❌ 否（已加入 .gitignore） |

### 📝 設定步驟

1. **複製範例檔案**:
   ```bash
   cp .env.example .env
   ```

2. **編輯 .env 填入實際值**:
   ```bash
   # SQL Server Configuration (Docker)
   SA_PASSWORD=YOUR_SQL_PASSWORD_HERE
   DB_SERVER=localhost
   DB_PORT=1433
   DB_NAME=Travel
   DB_USER=sa
   ```

3. **確認 .gitignore 包含 .env**:
   ```gitignore
   # Environment variables
   .env
   .env.local
   .env.*.local
   ```

### 🐳 Docker Compose 使用

在 `docker-compose.yml` 中使用 `.env` 變數：

```yaml
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: justgo-sqlserver
    environment:
      - SA_PASSWORD=${SA_PASSWORD}
      - ACCEPT_EULA=Y
    ports:
      - "${DB_PORT}:1433"
    volumes:
      - sqlserver-data:/var/opt/mssql

volumes:
  sqlserver-data:
```

### ✅ .env 的優點

- ✅ Docker Compose 原生支援
- ✅ 容易與團隊分享（用 .env.example）
- ✅ 跨平台標準
- ✅ 簡單明瞭

### ⚠️ .env 的注意事項

- ⚠️ 必須加入 `.gitignore`，否則會被提交
- ⚠️ 每個開發者需要自己建立 `.env`
- ⚠️ 不會自動整合到 .NET Configuration（除非另外載入）

---

## 何時使用哪一種？

### 使用 User Secrets 的情況 ⭐

✅ **.NET 應用程式的密碼**
- Google Maps API Key
- SendGrid API Key
- 資料庫連線字串（從 .NET 連線時）
- OAuth Client Secrets
- JWT Signing Keys
- 任何在 C# 程式碼中使用的敏感資訊

**範例**:
```bash
dotnet user-secrets set "Google:MapsApiKey" "YOUR_GOOGLE_MAPS_API_KEY_HERE"
dotnet user-secrets set "SendGrid:ApiKey" "YOUR_SENDGRID_API_KEY_HERE"
dotnet user-secrets set "ConnectionStrings:TravelDocker" "Server=localhost,1433;Database=Travel;User Id=sa;Password=YOUR_SQL_PASSWORD_HERE;..."
```

### 使用 .env 的情況

✅ **Docker/容器化設定**
- Docker Compose 環境變數
- 容器啟動參數
- 多語言專案的統一設定
- CI/CD Pipeline 變數

**範例 (.env)**:
```bash
SA_PASSWORD=YOUR_SQL_PASSWORD_HERE
DB_SERVER=localhost
DB_PORT=1433
COMPOSE_PROJECT_NAME=justgo
```

### 兩者搭配使用（推薦） 🎯

最佳實踐是**兩者都用**：

```
┌─────────────────────────────────────┐
│  User Secrets                       │
│  ├─ Google:MapsApiKey               │ → .NET 應用程式使用
│  └─ ConnectionStrings:TravelDocker  │
└─────────────────────────────────────┘

┌─────────────────────────────────────┐
│  .env                               │
│  ├─ SA_PASSWORD                     │ → Docker Compose 使用
│  ├─ DB_PORT                         │
│  └─ DB_SERVER                       │
└─────────────────────────────────────┘
```

---

## 實際操作指南

### 新開發者加入專案

完整的設定步驟：

```bash
# 1. Clone 專案
git clone <repository-url>
cd JustGo

# 2. 建立 .env 檔案（給 Docker 用）
cp .env.example .env
# 編輯 .env，通常不需要改（使用預設值即可）

# 3. 設定 User Secrets（給 .NET 用）
cd JustGo
dotnet user-secrets set "Google:MapsApiKey" "YOUR_GOOGLE_MAPS_API_KEY_HERE"
dotnet user-secrets set "ConnectionStrings:TravelDocker" "Server=localhost,1433;Database=Travel;User Id=sa;Password=YOUR_SQL_PASSWORD_HERE;TrustServerCertificate=True;MultipleActiveResultSets=True"

# 4. 驗證設定
dotnet user-secrets list

# 5. 啟動 Docker SQL Server
cd ..
docker-compose up -d

# 6. 執行資料庫遷移和匯入資料
./import-seed-data.sh

# 7. 執行專案
cd JustGo
dotnet run
```

### 已經在開發，想要改用 User Secrets

```bash
# 1. 設定 User Secrets
cd JustGo
dotnet user-secrets set "Google:MapsApiKey" "YOUR_GOOGLE_MAPS_API_KEY_HERE"
dotnet user-secrets set "ConnectionStrings:TravelDocker" "Server=localhost,1433;Database=Travel;User Id=sa;Password=YOUR_SQL_PASSWORD_HERE;..."

# 2. 清空 appsettings.json 的敏感資訊
# 編輯 appsettings.json，將密碼改為空字串 ""

# 3. 驗證
dotnet user-secrets list
dotnet run  # 測試是否正常運作
```

---

## 安全檢查清單

### ✅ 本專案已完成

- [x] User Secrets 已初始化並設定
- [x] `.env` 已加入 `.gitignore`
- [x] `appsettings.json` 移除硬編碼密碼
- [x] Views 改用 `@Configuration` 讀取 API Key
- [x] `.env.example` 已建立並包含說明
- [x] 移除臨時密碼產生工具
- [x] `.gitignore` 包含敏感檔案規則

### 🔒 安全最佳實踐

#### 永遠不要 ❌

- ❌ 把 `.env` 提交到 Git
- ❌ 在 `appsettings.json` 寫密碼
- ❌ 在程式碼中硬編碼 API Keys
- ❌ 在 HTML/JavaScript 中暴露後端密碼
- ❌ 將 User Secrets 用於正式環境
- ❌ 分享 User Secrets 的實際檔案內容

#### 應該要 ✅

- ✅ 使用 User Secrets（開發環境）
- ✅ 使用環境變數（正式環境）
- ✅ 定期更換密碼和 API Keys
- ✅ 限制 API Key 的使用範圍（HTTP referrers）
- ✅ 提交前檢查是否有敏感資訊
- ✅ 使用 `.env.example` 作為範本

#### 提交前檢查 🔍

```bash
# 檢查是否有敏感資訊
git diff --cached | grep -i "password\|apikey\|secret\|token"

# 確認 .env 不會被提交
git status --ignored

# 查看即將提交的檔案
git diff --cached --name-only

# 檢查 appsettings.json 是否乾淨
cat JustGo/appsettings.json | grep -i "password"
```

---

## 團隊協作

### 給新成員的快速設定指令

將以下內容分享給新加入的團隊成員：

```bash
# JustGo 專案快速設定

# 1. Clone 專案
git clone <repository-url>
cd JustGo

# 2. 建立 .env（Docker 用）
cp .env.example .env

# 3. 設定 User Secrets（.NET 用）
cd JustGo
dotnet user-secrets set "Google:MapsApiKey" "YOUR_GOOGLE_MAPS_API_KEY_HERE"
dotnet user-secrets set "ConnectionStrings:TravelDocker" "Server=localhost,1433;Database=Travel;User Id=sa;Password=YOUR_SQL_PASSWORD_HERE;TrustServerCertificate=True;MultipleActiveResultSets=True"

# 4. 驗證
dotnet user-secrets list

# 5. 啟動資料庫
cd ..
docker-compose up -d

# 6. 匯入測試資料
./import-seed-data.sh

# 7. 執行專案
cd JustGo
dotnet run
```

### 如何共享密碼？

**不要透過 Git 或聊天工具直接傳送密碼！**

建議方式：
1. 使用 `.env.example` 和 README 說明結構
2. 透過安全的密碼管理工具（1Password, LastPass）分享
3. 面對面口頭告知
4. 使用一次性連結（如 onetimesecret.com）

---

## 故障排除

### 問題 1: User Secrets 讀取不到

**症狀**:
- 程式執行時找不到 API Key
- Google Maps 無法載入
- 資料庫連線失敗

**檢查步驟**:

```bash
# 1. 確認環境是 Development
echo $ASPNETCORE_ENVIRONMENT  # Linux/Mac
echo %ASPNETCORE_ENVIRONMENT%  # Windows
# 應該輸出: Development

# 2. 確認 User Secrets 已設定
cd JustGo
dotnet user-secrets list
# 應該看到 Google:MapsApiKey 和 ConnectionStrings:TravelDocker

# 3. 確認 Key 名稱完全一致（包含大小寫）
dotnet user-secrets list | grep "Google:MapsApiKey"

# 4. 確認專案已初始化 User Secrets
cat JustGo.csproj | grep "UserSecretsId"
# 應該看到 <UserSecretsId>...</UserSecretsId>
```

**解決方法**:

```bash
# 重新設定 User Secrets
cd JustGo
dotnet user-secrets clear
dotnet user-secrets set "Google:MapsApiKey" "YOUR_GOOGLE_MAPS_API_KEY_HERE"
dotnet user-secrets set "ConnectionStrings:TravelDocker" "Server=localhost,1433;Database=Travel;User Id=sa;Password=YOUR_SQL_PASSWORD_HERE;TrustServerCertificate=True;MultipleActiveResultSets=True"
```

---

### 問題 2: .env 檔案不存在

**症狀**:
- Docker Compose 啟動失敗
- 環境變數找不到

**解決方法**:

```bash
# 從範例建立 .env
cp .env.example .env

# 檢查內容
cat .env

# 確認 .gitignore 包含 .env
cat .gitignore | grep "^\.env"
```

---

### 問題 3: 找不到 UserSecretsId

**錯誤訊息**:
```
Could not find the global property 'UserSecretsId' in MSBuild project
```

**解決方法**:

```bash
cd JustGo
dotnet user-secrets init
```

這會在 `JustGo.csproj` 加入：
```xml
<PropertyGroup>
  <UserSecretsId>生成的唯一ID</UserSecretsId>
</PropertyGroup>
```

---

### 問題 4: 不確定密碼是否已外洩到 Git

**檢查 Git 歷史**:

```bash
# 搜尋是否曾提交密碼
git log -p | grep -i "password"
git log -p | grep -i "apikey"

# 搜尋特定檔案的歷史
git log -p -- JustGo/appsettings.json | grep -i "password"

# 如果發現已提交，需要：
# 1. 立即更換所有密碼和 API Keys
# 2. 使用 git-filter-repo 或 BFG Repo-Cleaner 清除歷史
```

**清除 Git 歷史中的敏感資訊**:

```bash
# 使用 BFG Repo-Cleaner（推薦）
# 1. 安裝 BFG
# https://rtyley.github.io/bfg-repo-cleaner/

# 2. 建立檔案 passwords.txt，列出要刪除的密碼
echo "YOUR_ACTUAL_LEAKED_PASSWORD" > passwords.txt
echo "YOUR_ACTUAL_LEAKED_API_KEY" >> passwords.txt

# 3. 執行 BFG
bfg --replace-text passwords.txt

# 4. 清理
git reflog expire --expire=now --all
git gc --prune=now --aggressive

# 5. Force push（警告：會改寫歷史）
git push --force
```

---

### 問題 5: 不同專案想共用 User Secrets

**需求**: JustGo 專案和測試專案共用相同的 secrets

**解決方法**:

在兩個專案的 `.csproj` 設定相同的 `UserSecretsId`：

```xml
<!-- JustGo/JustGo.csproj -->
<PropertyGroup>
  <UserSecretsId>共用的-UserSecretsId</UserSecretsId>
</PropertyGroup>

<!-- JustGo.Tests/JustGo.Tests.csproj -->
<PropertyGroup>
  <UserSecretsId>共用的-UserSecretsId</UserSecretsId>
</PropertyGroup>
```

---

## 正式環境 (Production)

**⚠️ User Secrets 只適用於開發環境！**

### 正式環境應使用的方案

#### 1. Azure App Service

```bash
# 使用 Azure CLI 設定
az webapp config appsettings set \
  --name justgo-app \
  --resource-group justgo-rg \
  --settings "Google__MapsApiKey=YOUR_API_KEY" "ConnectionStrings__TravelDb=YOUR_CONNECTION_STRING"
```

或使用 **Azure Key Vault**:

```csharp
// Program.cs
if (!builder.Environment.IsDevelopment())
{
    var keyVaultUrl = new Uri(builder.Configuration["KeyVaultUrl"]);
    builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());
}
```

#### 2. Docker 部署

使用 **Docker Secrets** 或 **Environment Variables**:

```yaml
# docker-compose.prod.yml
services:
  app:
    image: justgo:latest
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - Google__MapsApiKey=${GOOGLE_MAPS_API_KEY}
      - ConnectionStrings__TravelDb=${DB_CONNECTION_STRING}
    secrets:
      - db_password

secrets:
  db_password:
    external: true
```

#### 3. Kubernetes

```yaml
# secret.yaml
apiVersion: v1
kind: Secret
metadata:
  name: justgo-secrets
type: Opaque
data:
  google-maps-api-key: <base64-encoded>
  connection-string: <base64-encoded>
```

```yaml
# deployment.yaml
env:
  - name: Google__MapsApiKey
    valueFrom:
      secretKeyRef:
        name: justgo-secrets
        key: google-maps-api-key
```

#### 4. 其他服務

- **AWS**: AWS Secrets Manager
- **GCP**: Google Secret Manager
- **通用**: HashiCorp Vault

---

## 參考資源

### 官方文件

- [Safe storage of app secrets in ASP.NET Core](https://docs.microsoft.com/aspnet/core/security/app-secrets)
- [Configuration in ASP.NET Core](https://docs.microsoft.com/aspnet/core/fundamentals/configuration)
- [Docker Compose: Environment Variables](https://docs.docker.com/compose/environment-variables/)
- [Azure Key Vault](https://docs.microsoft.com/azure/key-vault/)

### 相關檔案

- [IMPROVEMENTS.md](IMPROVEMENTS.md) - 安全性改善建議
- [README.Docker.md](README.Docker.md) - Docker 設定說明
- [README.SeedData.md](README.SeedData.md) - 測試資料說明

### 工具

- [BFG Repo-Cleaner](https://rtyley.github.io/bfg-repo-cleaner/) - 清除 Git 歷史中的敏感資訊
- [git-filter-repo](https://github.com/newren/git-filter-repo) - Git 歷史重寫工具
- [1Password](https://1password.com/) - 團隊密碼管理
- [Azure Key Vault](https://azure.microsoft.com/services/key-vault/) - 雲端密碼管理

---

## 總結

### 快速參考

| 需求 | 使用方式 | 指令 |
|------|---------|------|
| 設定 .NET 密碼 | User Secrets | `dotnet user-secrets set "Key" "Value"` |
| 查看已設定密碼 | User Secrets | `dotnet user-secrets list` |
| 設定 Docker 環境 | .env | 編輯 `.env` 檔案 |
| 新成員設定 | 兩者都要 | 參考[實際操作指南](#實際操作指南) |
| 檢查安全性 | Git | `git diff --cached \| grep -i password` |

### 核心原則

1. ✅ **開發環境**: User Secrets + .env
2. ✅ **永遠**: 將敏感資訊加入 `.gitignore`
3. ✅ **正式環境**: Azure Key Vault 或雲端 Secrets Manager
4. ✅ **團隊**: 使用 `.env.example` 作為範本
5. ✅ **提交前**: 檢查是否有密碼

---

**最後更新**: 2025-10-22
**維護者**: JustGo 開發團隊
