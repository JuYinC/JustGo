# JustGo 旅遊規劃平台

> 一個專為台灣旅遊愛好者設計的旅遊規劃與分享平台

[![Video](https://img.shields.io/badge/介紹影片-YouTube-red)](https://youtu.be/AyUdJXsSMq0)
[![.NET](https://img.shields.io/badge/.NET-6.0-purple)](https://dotnet.microsoft.com/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-blue)](https://www.microsoft.com/sql-server)

## 📺 專案介紹影片

🎥 [觀看專案介紹影片](https://youtu.be/AyUdJXsSMq0)

---

## ✨ 功能特色

### 🗺️ 旅遊部落格
- 撰寫和分享旅遊遊記
- 上傳旅遊照片
- 按讚與收藏功能
- 景點標籤分類

### 📅 行程規劃
- 拖拽式行程編輯
- 整合 Google Maps 顯示路線
- 自動計算行程時間
- 將行程轉換為部落格文章

### 📍 景點探索
- 31+ 台灣熱門景點資訊
- 即時天氣資訊整合
- 景點分類搜尋
- 地圖定位顯示

### 👤 使用者系統
- ASP.NET Identity 身份驗證
- 個人化旅遊收藏
- 用戶檔案管理

---

## 🛠️ 技術架構

### 後端技術
- **框架**: ASP.NET Core 6.0 MVC
- **ORM**: Entity Framework Core 6.0.7
- **資料庫**: SQL Server 2022
- **查詢工具**: Dapper (混合使用)
- **身份驗證**: ASP.NET Core Identity

### 前端技術
- **框架**: Vue.js 2.6.14
- **UI**: Bootstrap 5.1
- **地圖**: Google Maps JavaScript API
- **HTTP**: Axios
- **拖拽**: VueDraggable

### 開發工具
- **容器化**: Docker & Docker Compose
- **IDE**: Visual Studio Code
- **版本控制**: Git

### 架構模式
- Repository Pattern
- Unit of Work Pattern
- MVC (Model-View-Controller)

---

## 🚀 快速開始

### 系統需求

- .NET SDK 6.0 或更高版本
- Docker Desktop
- Git

### 安裝步驟

1. **Clone 專案**
   ```bash
   git clone <repository-url>
   cd JustGo
   ```

2. **建立環境變數檔案**
   ```bash
   cp .env.example .env
   ```

3. **設定 User Secrets**
   ```bash
   cd JustGo
   dotnet user-secrets set "Google:MapsApiKey" "your-google-maps-api-key"
   dotnet user-secrets set "ConnectionStrings:TravelDocker" "Server=localhost,1433;Database=Travel;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=True"
   cd ..
   ```

4. **啟動 Docker SQL Server**
   ```bash
   docker-compose up -d
   ```

5. **執行資料庫遷移並匯入測試資料**
   ```bash
   ./import-seed-data.sh
   ```

6. **執行專案**
   ```bash
   cd JustGo
   dotnet run
   ```

7. **開啟瀏覽器**
   ```
   https://localhost:7127
   ```

---

## 🧪 測試帳號

系統已預先建立測試帳號，可直接登入使用：

| 帳號 | 密碼 | 描述 |
|------|------|------|
| taiwan.lover@justgo.com | Test@123 | 台灣旅遊達人 |
| mountain.hiker@justgo.com | Test@123 | 登山小隊長 |
| food.explorer@justgo.com | Test@123 | 美食探險家 |

---

## 📁 專案結構

```
JustGo/
├── JustGo/                          # 主要應用程式
│   ├── Controllers/                 # MVC 控制器
│   │   ├── HomeController.cs
│   │   ├── BlogController.cs
│   │   ├── ScheduleController.cs
│   │   └── LoginController.cs
│   ├── Models/                      # 資料模型
│   │   ├── Blog.cs
│   │   ├── Schedule.cs
│   │   ├── Place.cs
│   │   └── ApplicationUser.cs
│   ├── ViewModels/                  # 視圖模型
│   ├── Views/                       # Razor 視圖
│   │   ├── Home/
│   │   └── Login/
│   ├── Repository/                  # 資料存取層
│   │   ├── BlogRepostioy.cs
│   │   ├── ScheduleRepostioy.cs
│   │   └── UnitOfWork.cs
│   ├── Services/                    # 服務層
│   ├── Areas/Identity/              # Identity 頁面
│   └── wwwroot/                     # 靜態檔案
├── database/                        # 資料庫腳本
│   └── seed-all-data.sql
├── docker-compose.yml               # Docker 設定
├── .env.example                     # 環境變數範例
├── import-seed-data.sh             # 資料匯入腳本
└── README.md                        # 本檔案
```

---

## 📚 文件

- **[SECRETS_GUIDE.md](SECRETS_GUIDE.md)** - 密碼與環境變數管理完整指南
- **[README.Docker.md](README.Docker.md)** - Docker SQL Server 設定說明
- **[README.SeedData.md](README.SeedData.md)** - 測試資料詳細說明

---

## 🗄️ 資料庫架構

### 主要資料表

#### Blog (部落格)
- `BlogId` - 部落格 ID (PK)
- `UserId` - 使用者 ID (FK)
- `Title` - 標題
- `Describe` - 描述
- `StartDate` / `EndDate` - 旅遊日期
- `Like` - 讚數
- `ImageName` - 封面圖片

#### Schedule (行程)
- `ScheduleId` - 行程 ID (PK)
- `UserId` - 使用者 ID (FK)
- `Title` - 行程標題
- `StartDate` / `EndDate` - 行程日期

#### Place (景點)
- `PlaceId` - 景點 ID (PK)
- `Name` - 景點名稱
- `Region` - 縣市
- `Town` - 鄉鎮
- `Lat` / `Lng` - 經緯度
- `Class` - 景點分類

#### UserKeep (收藏)
- `Id` - 收藏 ID (PK)
- `UserId` - 使用者 ID (FK)
- `KeepClass` - 收藏類型 (0: Blog, 1: Schedule)
- `KeepNumber` - 收藏項目 ID

---

## 🔧 常用指令

### 開發環境

```bash
# 啟動 SQL Server
docker-compose up -d

# 停止 SQL Server
docker-compose down

# 查看資料庫日誌
docker logs justgo-sqlserver

# 執行 EF Core 遷移
cd JustGo
dotnet ef database update --context TravelContext
dotnet ef database update --context ApplicationDbContext

# 查看 User Secrets
dotnet user-secrets list

# 重新匯入測試資料
./import-seed-data.sh

# 執行專案
dotnet run

# 建置專案
dotnet build

# 執行測試 (如有)
dotnet test
```

### 資料庫操作

```bash
# 連線到 SQL Server
docker exec -it justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C

# 執行 SQL 查詢
docker exec justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P "YourStrong@Passw0rd" -C -Q "SELECT COUNT(*) FROM Travel.dbo.Blog"
```

---

## 🌟 主要功能展示

### 1. 部落格撰寫
- 使用 Quill 富文本編輯器
- 支援多張照片上傳
- 景點自動標籤

### 2. 行程規劃
- Vue.js 拖拽介面
- Google Maps 路線規劃
- 即時行程調整

### 3. 景點搜尋
- 縣市分類篩選
- 關鍵字搜尋
- 地圖視覺化呈現

### 4. 收藏功能
- 一鍵收藏部落格和行程
- 個人收藏列表管理

---

## 🔐 安全性

- ✅ ASP.NET Core Identity 身份驗證
- ✅ User Secrets 管理敏感資訊
- ✅ HTTPS 強制加密
- ✅ CSRF 防護
- ✅ SQL Injection 防護 (使用 EF Core 參數化查詢)

---

## 🤝 貢獻

本專案為團隊協作開發的學習專案。

---

## 📝 授權

本專案僅供學習與展示使用。

---

## 🙏 致謝

- 使用的景點資料來源於台灣公開資訊
- Google Maps API 提供地圖服務
- Bootstrap 和 Vue.js 社群

---

## 📞 聯絡資訊

如有任何問題或建議，歡迎透過 GitHub Issues 聯絡。

---

**最後更新**: 2025-10-22
