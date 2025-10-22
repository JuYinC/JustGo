# JustGo 測試資料說明

## 概述

本專案提供完整的台灣旅遊測試資料，包含：
- 31 個台灣熱門景點（繁體中文）
- 3 個測試使用者帳號
- 5 篇旅遊部落格文章
- 天氣資料

## 快速匯入資料

### 方法一：使用自動化腳本（推薦）

```bash
# 確保 Docker SQL Server 已啟動
docker-compose up -d

# 執行匯入腳本
./import-seed-data.sh
```

這個腳本會自動完成：
1. 檢查 SQL Server 是否運行
2. 執行 EF Core 資料庫遷移
3. 匯入所有測試資料（景點、使用者、部落格）

### 方法二：手動匯入

```bash
# 1. 啟動 SQL Server
docker-compose up -d

# 2. 建立資料庫架構
cd JustGo
dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context TravelContext
cd ..

# 3. 匯入所有測試資料
docker exec -i justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YOUR_SQL_PASSWORD_HERE" -C < database/seed-all-data.sql
```

## 測試帳號

### 帳號 1: 台灣旅遊達人
- **Email**: `taiwan.lover@justgo.com`
- **密碼**: `Test@123`
- **姓名**: 台灣旅遊達人
- **城市**: 台北市
- **部落格**: 台北週末輕旅行、九份山城浪漫遊

### 帳號 2: 登山小隊長
- **Email**: `mountain.hiker@justgo.com`
- **密碼**: `Test@123`
- **姓名**: 登山小隊長
- **城市**: 南投縣
- **部落格**: 台中文青景點巡禮、花蓮太魯閣峽谷探險

### 帳號 3: 美食探險家
- **Email**: `food.explorer@justgo.com`
- **密碼**: `Test@123`
- **姓名**: 美食探險家
- **城市**: 台南市
- **部落格**: 台南美食古蹟之旅

## 景點資料（31 個台灣景點）

### 台北市 (7 個景點)
1. **台北101** - 台北地標，觀景台與購物中心
2. **國立故宮博物院** - 世界四大博物館之一
3. **西門町** - 台北最熱鬧的流行商圈
4. **士林夜市** - 台北最大的夜市
5. **陽明山國家公園** - 自然生態與溫泉
6. **龍山寺** - 台北最古老的廟宇之一
7. **貓空纜車** - 茶園與觀景餐廳

### 新北市 (4 個景點)
8. **九份老街** - 山城懷舊老街
9. **淡水老街** - 河岸夕陽與歷史
10. **野柳地質公園** - 女王頭等海蝕地形
11. **平溪天燈** - 天燈祈福

### 台中市 (4 個景點)
12. **逢甲夜市** - 創新小吃發源地
13. **彩虹眷村** - 繽紛彩繪打卡聖地
14. **高美濕地** - 夕陽與風車美景
15. **宮原眼科** - 復古華麗冰淇淋店

### 台南市 (4 個景點)
16. **安平古堡** - 台灣最古老的城堡
17. **赤崁樓** - 國家一級古蹟
18. **花園夜市** - 台南最大流動夜市
19. **四草綠色隧道** - 台版亞馬遜河

### 高雄市 (4 個景點)
20. **駁二藝術特區** - 藝術與文化空間
21. **西子灣風景區** - 夕陽海灣
22. **六合夜市** - 國際觀光夜市
23. **旗津海岸公園** - 海鮮與海灘

### 花蓮縣 (3 個景點)
24. **太魯閣國家公園** - 世界級峽谷景觀
25. **七星潭風景區** - 礫石灘與太平洋
26. **東大門夜市** - 花蓮最大夜市

### 南投縣 (3 個景點)
27. **日月潭** - 台灣最大淡水湖
28. **清境農場** - 高山牧場風光
29. **溪頭自然教育園區** - 森林步道

### 宜蘭縣 (2 個景點)
30. **羅東夜市** - 包心粉圓等特色小吃
31. **梅花湖風景區** - 環湖步道

## 景點分類說明

景點 `Class` 欄位分類：
- **1**: 地標建築
- **2**: 博物館/藝術館
- **3**: 商圈/老街
- **4**: 夜市/美食區
- **5**: 自然景觀/公園
- **6**: 寺廟/古蹟
- **7**: 纜車/交通景點
- **8**: 特色景點/打卡點

## 部落格文章

### 1. 台北週末輕旅行
- 作者：台灣旅遊達人
- 日期：2025-10-15 ~ 2025-10-16
- 景點：台北101、西門町、故宮博物院、士林夜市、龍山寺
- 讚數：42

### 2. 九份山城浪漫遊
- 作者：台灣旅遊達人
- 日期：2025-10-10
- 景點：九份老街
- 讚數：68

### 3. 台中文青景點巡禮
- 作者：登山小隊長
- 日期：2025-10-05 ~ 2025-10-06
- 景點：彩虹眷村、宮原眼科、高美濕地、逢甲夜市
- 讚數：55

### 4. 台南美食古蹟之旅
- 作者：美食探險家
- 日期：2025-09-28 ~ 2025-09-30
- 景點：赤崁樓、安平古堡、四草綠色隧道、花園夜市
- 讚數：89

### 5. 花蓮太魯閣峽谷探險
- 作者：登山小隊長
- 日期：2025-09-20 ~ 2025-09-22
- 景點：太魯閣國家公園、七星潭、東大門夜市
- 讚數：73

## 資料庫結構

### Place (景點)
- PlaceId: 景點ID
- Name: 景點名稱（繁體中文）
- Description: 景點描述
- Tel: 電話
- Add: 地址（完整繁體中文地址）
- Lat/Lng: 經緯度座標
- Region: 縣市
- Town: 鄉鎮市區
- Class: 景點分類
- Opentime/Closetime: 營業時間
- Timestay: 建議停留時間（分鐘）

### Blog (部落格)
- BlogId: 部落格ID
- UserId: 作者ID
- Title: 標題
- Describe: 描述
- ImageName: 封面圖片
- Like: 讚數
- StartDate/EndDate: 旅遊日期

### BlogDetails (部落格詳細內容)
- DetailsId: 詳細內容ID
- BlogId: 所屬部落格
- PlaceId: 景點ID
- StartTime/EndTime: 時間範圍
- Describe: 詳細描述（繁體中文遊記）
- Images: 圖片檔名
- Score: 評分

## 驗證資料匯入

```bash
# 連接到 SQL Server
docker exec -it justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YOUR_SQL_PASSWORD_HERE" -C

# 檢查景點數量
SELECT COUNT(*) as PlaceCount FROM Travel.dbo.Place;

# 檢查使用者數量
SELECT COUNT(*) as UserCount FROM Travel.dbo.AspNetUsers;

# 檢查部落格數量
SELECT COUNT(*) as BlogCount FROM Travel.dbo.Blog;

# 檢查部落格詳細內容數量
SELECT COUNT(*) as BlogDetailCount FROM Travel.dbo.BlogDetails;

# 查看所有景點
SELECT PlaceId, Name, Region, Town FROM Travel.dbo.Place;

# 查看所有部落格
SELECT BlogId, Title, Name FROM Travel.dbo.Blog
JOIN Travel.dbo.AspNetUsers ON Blog.UserID = AspNetUsers.Id;
```

## 重置資料

如果需要重新匯入資料：

```bash
# 方法 1: 刪除資料庫重新建立
docker exec justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YOUR_SQL_PASSWORD_HERE" -C \
  -Q "DROP DATABASE Travel"

# 重新匯入
./import-seed-data.sh

# 方法 2: 只清除資料保留架構
# SQL 腳本已經包含 DELETE 語句，直接重新執行即可
docker exec -i justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YOUR_SQL_PASSWORD_HERE" -C < database/seed-all-data.sql
```

## 注意事項

1. **圖片檔案**: 部落格中的圖片檔名是示範資料，實際檔案不存在。如需測試圖片功能，請另外準備圖片檔案。

2. **密碼安全**: 測試帳號密碼為 `Test@123`，僅供開發測試使用。

3. **座標精確度**: 景點座標為真實的經緯度資料，可直接用於地圖顯示。

4. **營業時間**: 景點的開放時間為參考資料，實際請以官方公告為準。

5. **繁體中文**: 所有資料都使用繁體中文，符合台灣在地化需求。

## 擴充資料

如需新增更多測試資料，可以編輯：
- `database/seed-all-data.sql` - 包含所有測試資料（景點、使用者、部落格）

檔案結構清晰，分為五個主要區塊：
1. 清除現有資料
2. 台灣景點資料（31 個景點）
3. 天氣資料（5 筆）
4. 測試使用者帳號（3 個帳號）
5. 旅遊部落格與詳細內容（5 篇部落格，22 筆詳細內容）

## 故障排除

### SQL Server 連接失敗
```bash
# 檢查容器狀態
docker ps

# 查看 SQL Server 日誌
docker logs justgo-sqlserver

# 重啟容器
docker-compose restart
```

### 匯入資料失敗
```bash
# 檢查資料庫是否存在
docker exec justgo-sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "YOUR_SQL_PASSWORD_HERE" -C \
  -Q "SELECT name FROM sys.databases"

# 手動建立資料庫
cd JustGo
dotnet ef database update
```

## 授權與使用

此測試資料僅供開發與測試使用。景點資訊來源於公開資訊，僅作為範例資料。
