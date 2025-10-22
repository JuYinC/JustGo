-- ============================================
-- JustGo 完整測試資料 (Complete Seed Data)
-- Taiwan Travel Planning Application
-- ============================================
--
-- This script includes:
-- 1. Taiwan travel places (31 locations)
-- 2. Weather data (5 sample records)
-- 3. Test user accounts (3 users)
-- 4. Travel blogs (5 blogs with 20+ details)
--
-- Password for all test users: Test@123
--
-- Execute this script after running EF Core migrations
-- ============================================

USE Travel;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- ============================================
-- SECTION 1: Clear Existing Data
-- ============================================
PRINT '清除現有測試資料...';

DELETE FROM BlogDetails;
DELETE FROM Blog;
DELETE FROM ScheduleDetails;
DELETE FROM Schedule;
DELETE FROM UserKeep;
DELETE FROM Weather;
DELETE FROM Place;

PRINT '✓ 現有資料已清除';
GO

-- ============================================
-- SECTION 2: 台灣景點資料 (Taiwan Places)
-- ============================================
PRINT '正在匯入台灣景點資料...';

SET IDENTITY_INSERT Place ON;

INSERT INTO Place (PlaceId, Name, Description, Tel, [Add], Lat, Lng, Region, Town, Class, Opentime, IntOpentime, Closetime, IntClosetime, Timestay)
VALUES
-- 台北市景點
(1, N'台北101', N'台北地標，擁有觀景台和購物中心，可俯瞰整個台北市景色。', '02-81018800', N'台北市信義區信義路五段7號', 25.0339639, 121.5644722, N'台北市', N'信義區', 1, '09:00', 900, '22:00', 2200, 120),
(2, N'國立故宮博物院', N'世界四大博物館之一，收藏眾多中國古代文物與藝術品。', '02-28812021', N'台北市士林區至善路二段221號', 25.1023553, 121.5484127, N'台北市', N'士林區', 2, '08:30', 830, '18:30', 1830, 180),
(3, N'西門町', N'台北最熱鬧的流行商圈，充滿年輕活力與創意文化。', NULL, N'台北市萬華區西門町', 25.0420264, 121.5067117, N'台北市', N'萬華區', 3, '10:00', 1000, '23:00', 2300, 120),
(4, N'士林夜市', N'台北最大的夜市之一，各式台灣小吃美食應有盡有。', NULL, N'台北市士林區基河路101號', 25.0879569, 121.5240581, N'台北市', N'士林區', 4, '17:00', 1700, '01:00', 100, 120),
(5, N'陽明山國家公園', N'台北近郊的自然生態寶庫，四季景色各異，溫泉資源豐富。', '02-28613601', N'台北市北投區竹子湖路1-20號', 25.1621856, 121.5446094, N'台北市', N'北投區', 5, '09:00', 900, '16:30', 1630, 240),
(6, N'龍山寺', N'台北最古老的廟宇之一，香火鼎盛，建築雕刻精美。', '02-23025162', N'台北市萬華區廣州街211號', 25.0366846, 121.4998591, N'台北市', N'萬華區', 6, '06:00', 600, '22:00', 2200, 60),
(7, N'貓空纜車', N'搭乘纜車欣賞台北景色，終點站貓空有茶園與觀景餐廳。', '02-21812345', N'台北市文山區新光路二段8號', 24.9986439, 121.5769381, N'台北市', N'文山區', 7, '09:00', 900, '21:00', 2100, 180),

-- 新北市景點
(8, N'九份老街', N'依山而建的山城老街，懷舊氛圍濃厚，夜景迷人。', NULL, N'新北市瑞芳區九份', 25.1095629, 121.8449009, N'新北市', N'瑞芳區', 3, '08:00', 800, '22:00', 2200, 180),
(9, N'淡水老街', N'淡水河畔的歷史老街，可欣賞河岸夕陽，品嚐特色小吃。', NULL, N'新北市淡水區中正路', 25.1697547, 121.4390469, N'新北市', N'淡水區', 3, '10:00', 1000, '22:00', 2200, 180),
(10, N'野柳地質公園', N'獨特的海蝕地形景觀，女王頭是最著名的景點。', '02-24922016', N'新北市萬里區野柳里港東路167-1號', 25.2054263, 121.6893129, N'新北市', N'萬里區', 5, '08:00', 800, '17:00', 1700, 120),
(11, N'平溪天燈', N'平溪老街與天燈祈福活動聞名，充滿浪漫氣息。', NULL, N'新北市平溪區平溪街', 25.0253339, 121.7394528, N'新北市', N'平溪區', 8, '09:00', 900, '19:00', 1900, 120),

-- 台中市景點
(12, N'逢甲夜市', N'台中最大的夜市，創新小吃發源地，人潮洶湧。', NULL, N'台中市西屯區文華路', 24.1791733, 120.6449464, N'台中市', N'西屯區', 4, '17:00', 1700, '02:00', 200, 150),
(13, N'彩虹眷村', N'充滿童趣彩繪的眷村，色彩繽紛，是拍照打卡聖地。', '04-23803366', N'台中市南屯區春安路56巷25號', 24.1399956, 120.6125867, N'台中市', N'南屯區', 8, '08:00', 800, '18:00', 1800, 60),
(14, N'高美濕地', N'台中海線著名景點，可觀賞風車與夕陽，生態豐富。', '04-26562810', N'台中市清水區美堤街', 24.3132519, 120.5496439, N'台中市', N'清水區', 5, '10:00', 1000, '20:00', 2000, 120),
(15, N'宮原眼科', N'日治時期建築改建的冰淇淋店，復古華麗的裝潢超吸睛。', '04-22271927', N'台中市中區中山路20號', 24.1376708, 120.6793106, N'台中市', N'中區', 4, '10:00', 1000, '22:00', 2200, 60),

-- 台南市景點
(16, N'安平古堡', N'台灣最古老的城堡，見證台灣歷史變遷。', '06-2267348', N'台南市安平區國勝路82號', 23.0015106, 120.1606872, N'台南市', N'安平區', 6, '08:30', 830, '17:30', 1730, 90),
(17, N'赤崁樓', N'國家一級古蹟，荷蘭時期的歷史建築。', '06-2205647', N'台南市中西區民族路二段212號', 22.9974217, 120.2025769, N'台南市', N'中西區', 6, '08:30', 830, '17:30', 1730, 60),
(18, N'花園夜市', N'台南最大的流動夜市，美食種類眾多。', NULL, N'台南市北區海安路三段', 23.0092522, 120.1969164, N'台南市', N'北區', 4, '17:00', 1700, '01:00', 100, 120),
(19, N'四草綠色隧道', N'台版亞馬遜河，搭乘竹筏穿梭在紅樹林間，生態豐富。', '06-2841610', N'台南市安南區大眾路360號', 23.0418542, 120.1405486, N'台南市', N'安南區', 5, '08:30', 830, '17:00', 1700, 90),

-- 高雄市景點
(20, N'駁二藝術特區', N'港口倉庫改建的藝術空間，充滿創意與文化氣息。', '07-5214899', N'高雄市鹽埕區大勇路1號', 22.6200892, 120.2820397, N'高雄市', N'鹽埕區', 2, '10:00', 1000, '18:00', 1800, 120),
(21, N'西子灣風景區', N'高雄著名海灣，夕陽美景與海洋大學相映成趣。', '07-5252000', N'高雄市鼓山區蓮海路70號', 22.6268972, 120.2645722, N'高雄市', N'鼓山區', 5, '00:00', 0, '23:59', 2359, 120),
(22, N'六合夜市', N'高雄最具代表性的觀光夜市，國際遊客必訪。', NULL, N'高雄市新興區六合二路', 22.6295711, 120.3014714, N'高雄市', N'新興區', 4, '17:00', 1700, '02:00', 200, 120),
(23, N'旗津海岸公園', N'搭渡輪到旗津，享受海鮮與海灘風情。', '07-5718920', N'高雄市旗津區旗津三路990號', 22.6027406, 120.2748483, N'高雄市', N'旗津區', 5, '00:00', 0, '23:59', 2359, 180),

-- 花蓮縣景點
(24, N'太魯閣國家公園', N'世界級的峽谷景觀，大理石峭壁壯麗迷人。', '03-8621100', N'花蓮縣秀林鄉富世291號', 24.1938869, 121.4906311, N'花蓮縣', N'秀林鄉', 5, '08:30', 830, '17:00', 1700, 240),
(25, N'七星潭風景區', N'花蓮必訪海岸，礫石灘與太平洋海景令人心曠神怡。', '03-8221592', N'花蓮縣新城鄉海岸路', 24.0416597, 121.6268119, N'花蓮縣', N'新城鄉', 5, '00:00', 0, '23:59', 2359, 120),
(26, N'東大門夜市', N'花蓮最大的夜市，結合多個夜市攤位。', NULL, N'花蓮縣花蓮市中山路50號', 23.9780806, 121.6067256, N'花蓮縣', N'花蓮市', 4, '17:30', 1730, '23:30', 2330, 120),

-- 南投縣景點
(27, N'日月潭', N'台灣最大的淡水湖泊，湖光山色美不勝收。', '049-2855668', N'南投縣魚池鄉中山路599號', 23.8591439, 120.9155083, N'南投縣', N'魚池鄉', 5, '00:00', 0, '23:59', 2359, 240),
(28, N'清境農場', N'高山牧場風光，綿羊秀與歐式建築是特色。', '049-2802748', N'南投縣仁愛鄉仁和路170號', 24.0474167, 121.1651528, N'南投縣', N'仁愛鄉', 5, '08:00', 800, '17:00', 1700, 180),
(29, N'溪頭自然教育園區', N'竹林幽徑與森林步道，是避暑休閒的好去處。', '049-2612111', N'南投縣鹿谷鄉森林巷9號', 23.6706408, 120.7961756, N'南投縣', N'鹿谷鄉', 5, '07:00', 700, '17:00', 1700, 180),

-- 宜蘭縣景點
(30, N'羅東夜市', N'宜蘭最熱鬧的夜市，包心粉圓等特色小吃聞名。', NULL, N'宜蘭縣羅東鎮民生路', 24.6761197, 121.7693964, N'宜蘭縣', N'羅東鎮', 4, '17:00', 1700, '01:00', 100, 120),
(31, N'梅花湖風景區', N'湖形似梅花的天然湖泊，環湖步道適合散步騎車。', '03-9615576', N'宜蘭縣冬山鄉得安村環湖路', 24.6406594, 121.7457267, N'宜蘭縣', N'冬山鄉', 5, '08:00', 800, '18:00', 1800, 120);

SET IDENTITY_INSERT Place OFF;

PRINT '✓ 已匯入 31 個台灣景點';
GO

-- ============================================
-- SECTION 3: 天氣資料 (Weather Data)
-- ============================================
PRINT '正在匯入天氣資料...';

SET IDENTITY_INSERT weather ON;

INSERT INTO weather (ID, location, locationsName, startTime, endTime, wx, pop12h, minT, maxT, uvi)
VALUES
(1, N'台北市', N'信義區', '2025-10-21 06:00:00', '2025-10-21 18:00:00', N'多雲時晴', 20, 22, 28, 7),
(2, N'台北市', N'信義區', '2025-10-21 18:00:00', '2025-10-22 06:00:00', N'多雲', 30, 20, 24, 0),
(3, N'台中市', N'西屯區', '2025-10-21 06:00:00', '2025-10-21 18:00:00', N'晴時多雲', 10, 24, 30, 9),
(4, N'高雄市', N'鹽埕區', '2025-10-21 06:00:00', '2025-10-21 18:00:00', N'晴', 10, 26, 32, 10),
(5, N'花蓮縣', N'秀林鄉', '2025-10-21 06:00:00', '2025-10-21 18:00:00', N'多雲時晴', 20, 23, 29, 8);

SET IDENTITY_INSERT weather OFF;

PRINT '✓ 已匯入 5 筆天氣資料';
GO

-- ============================================
-- SECTION 4: 測試使用者帳號 (Test Users)
-- ============================================
PRINT '正在建立測試使用者...';

-- Password: Test@123 (ASP.NET Core Identity hashed)
INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, Discriminator, Name, City)
VALUES
('user-taiwan-travel-001', 'taiwan.lover@justgo.com', 'TAIWAN.LOVER@JUSTGO.COM', 'taiwan.lover@justgo.com', 'TAIWAN.LOVER@JUSTGO.COM', 1, 'AQAAAAEAACcQAAAAEFOpbvEeERhF1VkvLg+QgofybbAIx6OdRNU9TEVFJHzqkHkMPNKJXxDVNdcNg7AhmA==', 'RANDOMSECURITYSTAMP001', 'RANDOMCONCURRENCYSTAMP001', 0, 0, 1, 0, 'ApplicationUser', N'台灣旅遊達人', N'台北市'),
('user-taiwan-travel-002', 'mountain.hiker@justgo.com', 'MOUNTAIN.HIKER@JUSTGO.COM', 'mountain.hiker@justgo.com', 'MOUNTAIN.HIKER@JUSTGO.COM', 1, 'AQAAAAEAACcQAAAAEFOpbvEeERhF1VkvLg+QgofybbAIx6OdRNU9TEVFJHzqkHkMPNKJXxDVNdcNg7AhmA==', 'RANDOMSECURITYSTAMP002', 'RANDOMCONCURRENCYSTAMP002', 0, 0, 1, 0, 'ApplicationUser', N'登山小隊長', N'南投縣'),
('user-taiwan-travel-003', 'food.explorer@justgo.com', 'FOOD.EXPLORER@JUSTGO.COM', 'food.explorer@justgo.com', 'FOOD.EXPLORER@JUSTGO.COM', 1, 'AQAAAAEAACcQAAAAEFOpbvEeERhF1VkvLg+QgofybbAIx6OdRNU9TEVFJHzqkHkMPNKJXxDVNdcNg7AhmA==', 'RANDOMSECURITYSTAMP003', 'RANDOMCONCURRENCYSTAMP003', 0, 0, 1, 0, 'ApplicationUser', N'美食探險家', N'台南市');

PRINT '✓ 已建立 3 個測試使用者';
GO

-- ============================================
-- SECTION 5: 旅遊部落格 (Travel Blogs)
-- ============================================
PRINT '正在建立旅遊部落格...';

DECLARE @UserId1 NVARCHAR(450) = 'user-taiwan-travel-001';
DECLARE @UserId2 NVARCHAR(450) = 'user-taiwan-travel-002';
DECLARE @UserId3 NVARCHAR(450) = 'user-taiwan-travel-003';

DECLARE @BlogId1 INT, @BlogId2 INT, @BlogId3 INT, @BlogId4 INT, @BlogId5 INT;

-- Blog 1: 台北週末輕旅行
INSERT INTO Blog (UserID, Title, Describe, ImageName, [Like], StartDate, EndDate)
VALUES (@UserId1, N'台北週末輕旅行', N'兩天一夜的台北之旅，走訪經典景點，品嚐道地美食，感受台北的人文風情與現代氣息。', 'taipei-weekend.jpg', 42, '2025-10-15', '2025-10-16');
SET @BlogId1 = SCOPE_IDENTITY();

INSERT INTO BlogDetails (StartTime, EndtTime, PlaceID, Describe, Images, Score, BlogID)
VALUES
('2025-10-15 09:00:00', '2025-10-15 11:30:00', 1, N'一早來到台北101，搭乘高速電梯登上觀景台，360度俯瞰整個台北盆地。天氣晴朗，視野極佳，連遠方的陽明山都清晰可見。', '101-view-1.jpg,101-view-2.jpg', 5.0, @BlogId1),
('2025-10-15 12:00:00', '2025-10-15 13:30:00', 3, N'午餐時間來到西門町覓食，嚐試了阿宗麵線和鴨肉扁，每一口都充滿台灣味。街頭藝人的表演也很精采。', 'ximending-food.jpg', 4.5, @BlogId1),
('2025-10-15 14:00:00', '2025-10-15 16:00:00', 2, N'下午參觀故宮博物院，館藏真的令人嘆為觀止！從翠玉白菜到肉形石，每件文物都訴說著歷史故事。', 'palace-museum.jpg', 5.0, @BlogId1),
('2025-10-15 18:00:00', '2025-10-15 21:00:00', 4, N'晚上當然要逛士林夜市！豪大大雞排、蚵仔煎、臭豆腐...每一攤都想吃！', 'shilin-night-market.jpg', 4.8, @BlogId1),
('2025-10-16 09:00:00', '2025-10-16 11:00:00', 6, N'第二天一早到龍山寺參拜，感受台灣傳統宗教文化。廟宇建築雕刻精緻，香火鼎盛。', 'longshan-temple.jpg', 4.5, @BlogId1);

-- Blog 2: 九份山城浪漫遊
INSERT INTO Blog (UserID, Title, Describe, ImageName, [Like], StartDate, EndDate)
VALUES (@UserId1, N'九份山城浪漫遊', N'走進宮崎駿動畫場景般的山城，品茗賞景，感受懷舊時光。夜幕降臨時，燈籠亮起的那一刻最讓人著迷。', 'jiufen-cover.jpg', 68, '2025-10-10', '2025-10-10');
SET @BlogId2 = SCOPE_IDENTITY();

INSERT INTO BlogDetails (StartTime, EndtTime, PlaceID, Describe, Images, Score, BlogID)
VALUES
('2025-10-10 10:00:00', '2025-10-10 13:00:00', 8, N'從台北搭火車到瑞芳，再轉公車上山。九份老街保留了許多日治時期的建築，阿柑姨芋圓是必吃美食！', 'jiufen-street.jpg,jiufen-food.jpg', 5.0, @BlogId2),
('2025-10-10 14:00:00', '2025-10-10 16:00:00', 8, N'在茶樓品茗，眺望東北角海岸線。遠方的基隆山、陰陽海盡收眼底。', 'jiufen-teahouse.jpg', 4.8, @BlogId2),
('2025-10-10 17:30:00', '2025-10-10 19:30:00', 8, N'傍晚時分，老街燈籠逐漸亮起，整個山城變得更加迷人。真的有種走進千與千尋場景的錯覺。', 'jiufen-night.jpg,jiufen-lantern.jpg', 5.0, @BlogId2);

-- Blog 3: 台中文青之旅
INSERT INTO Blog (UserID, Title, Describe, ImageName, [Like], StartDate, EndDate)
VALUES (@UserId2, N'台中文青景點巡禮', N'探訪台中最夯的文青景點，從彩虹眷村到高美濕地，每個地方都超好拍！', 'taichung-artsy.jpg', 55, '2025-10-05', '2025-10-06');
SET @BlogId3 = SCOPE_IDENTITY();

INSERT INTO BlogDetails (StartTime, EndtTime, PlaceID, Describe, Images, Score, BlogID)
VALUES
('2025-10-05 10:00:00', '2025-10-05 11:30:00', 13, N'彩虹眷村真的超級繽紛！黃永阜爺爺的彩繪充滿童趣與創意，每個角落都是拍照打卡點。', 'rainbow-village-1.jpg,rainbow-village-2.jpg', 4.7, @BlogId3),
('2025-10-05 12:30:00', '2025-10-05 14:00:00', 15, N'午餐選在宮原眼科，這裡的冰淇淋真的名不虛傳！華麗的復古建築，搭配多種口味選擇。', 'miyahara-icecream.jpg', 5.0, @BlogId3),
('2025-10-05 16:00:00', '2025-10-05 18:30:00', 14, N'傍晚時分到高美濕地看夕陽。踩在濕地上，看著夕陽緩緩落下，風車在遠方轉動，這畫面美得像明信片。', 'gaomei-sunset.jpg,gaomei-windmill.jpg', 5.0, @BlogId3),
('2025-10-05 19:30:00', '2025-10-05 22:00:00', 12, N'晚上當然要來逢甲夜市！這裡是創新小吃的發源地，官芝霖大腸包小腸、黃金右腿、章魚燒...每樣都想吃！', 'fengjia-night.jpg', 4.8, @BlogId3);

-- Blog 4: 台南古都慢慢遊
INSERT INTO Blog (UserID, Title, Describe, ImageName, [Like], StartDate, EndDate)
VALUES (@UserId3, N'台南美食古蹟之旅', N'台南真的是美食與歷史的天堂！從早餐開始就停不下來的美食馬拉松，三天兩夜根本不夠玩！', 'tainan-trip.jpg', 89, '2025-09-28', '2025-09-30');
SET @BlogId4 = SCOPE_IDENTITY();

INSERT INTO BlogDetails (StartTime, EndtTime, PlaceID, Describe, Images, Score, BlogID)
VALUES
('2025-09-28 09:00:00', '2025-09-28 11:00:00', 17, N'第一站來到赤崁樓，這座荷蘭時期的歷史建築保存得很好。導覽解說很詳細，了解了許多台南的歷史故事。', 'chihkan-tower.jpg', 4.5, @BlogId4),
('2025-09-28 12:00:00', '2025-09-28 13:30:00', 17, N'中午在國華街一帶覓食，台南的小吃真的太厲害了！蝦仁飯、肉燥飯、碗粿、魚羹...每一家都大排長龍。', 'tainan-food-1.jpg', 5.0, @BlogId4),
('2025-09-28 14:00:00', '2025-09-28 16:00:00', 16, N'下午參觀安平古堡，台灣最古老的城堡，見證了台灣400年的歷史變遷。', 'anping-fort.jpg', 4.6, @BlogId4),
('2025-09-29 09:00:00', '2025-09-29 11:00:00', 19, N'第二天一早到四草綠色隧道，搭竹筏穿梭在紅樹林水道中。兩側的紅樹林形成綠色隧道，美得像仙境。', 'green-tunnel.jpg', 5.0, @BlogId4),
('2025-09-29 18:00:00', '2025-09-29 21:00:00', 18, N'晚上逛花園夜市（週四六日限定），果然名不虛傳！二師兄滷味、阿美芭樂、統大雞排...每攤都大排長龍。', 'garden-night-market.jpg', 4.9, @BlogId4);

-- Blog 5: 花蓮太魯閣壯遊
INSERT INTO Blog (UserID, Title, Describe, ImageName, [Like], StartDate, EndDate)
VALUES (@UserId2, N'花蓮太魯閣峽谷探險', N'親眼見證大自然的鬼斧神工，太魯閣的壯麗景色令人震撼。搭配七星潭的海景與東大門夜市的美食，完美的花蓮之旅！', 'hualien-taroko.jpg', 73, '2025-09-20', '2025-09-22');
SET @BlogId5 = SCOPE_IDENTITY();

INSERT INTO BlogDetails (StartTime, EndtTime, PlaceID, Describe, Images, Score, BlogID)
VALUES
('2025-09-20 09:00:00', '2025-09-20 15:00:00', 24, N'太魯閣國家公園真的是世界級的景觀！燕子口、九曲洞、長春祠...每個景點都讓人讚嘆。大理石峭壁高聳入雲，氣勢磅礡。', 'taroko-1.jpg,taroko-2.jpg,taroko-3.jpg', 5.0, @BlogId5),
('2025-09-20 16:30:00', '2025-09-20 18:30:00', 25, N'傍晚時分來到七星潭，這裡的礫石灘很特別。海浪拍打石頭的聲音很療癒。夕陽西下時，天空染成橘紅色，美得令人屏息。', 'qixingtan-sunset.jpg', 4.9, @BlogId5),
('2025-09-20 19:00:00', '2025-09-20 21:30:00', 26, N'晚上到東大門夜市覓食，原住民烤肉、炸彈蔥油餅、妙不可言果汁...每樣都好吃！第一烤肉串的山豬肉串必點！', 'dongdamen-night.jpg', 4.7, @BlogId5);

PRINT '✓ 已建立 5 篇旅遊部落格';
PRINT '✓ 已建立 22 筆部落格詳細內容';
GO

-- ============================================
-- 完成訊息
-- ============================================
PRINT '';
PRINT '========================================';
PRINT '  ✅ 資料匯入完成！';
PRINT '========================================';
PRINT '';
PRINT '資料統計:';
PRINT '  • 31 個台灣景點';
PRINT '  • 5 筆天氣資料';
PRINT '  • 3 個測試使用者';
PRINT '  • 5 篇旅遊部落格';
PRINT '  • 22 筆部落格詳細內容';
PRINT '';
PRINT '========================================';
PRINT '  測試帳號 (Test Accounts)';
PRINT '========================================';
PRINT '';
PRINT '1. Email: taiwan.lover@justgo.com';
PRINT '   Password: Test@123';
PRINT '   Name: 台灣旅遊達人';
PRINT '';
PRINT '2. Email: mountain.hiker@justgo.com';
PRINT '   Password: Test@123';
PRINT '   Name: 登山小隊長';
PRINT '';
PRINT '3. Email: food.explorer@justgo.com';
PRINT '   Password: Test@123';
PRINT '   Name: 美食探險家';
PRINT '';
PRINT '========================================';
GO
