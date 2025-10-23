# Database Scripts

This directory contains SQL scripts for the JustGo travel planning application.

## Files

### seed-all-data.sql
Complete seed data script including:
- 31 Taiwan travel places
- Weather data samples
- Test user accounts (password: `Test@123`)
- Travel blogs with details

Execute this after running EF Core migrations to populate the database with test data.

### fn_selePlaceDistance.sql
SQL table-valued function that calculates distances between coordinates using the Haversine formula.

**Function Signature:**
```sql
fn_selePlaceDistance(@Lat DECIMAL, @Lng DECIMAL, @Distance DECIMAL)
```

**Parameters:**
- `@Lat`: Latitude of search location
- `@Lng`: Longitude of search location
- `@Distance`: Maximum distance in kilometers

**Returns:**
All Place table columns plus a calculated `Distance` column (in kilometers).

**Usage Example:**
```sql
-- Find attractions within 10km of coordinates
SELECT *
FROM fn_selePlaceDistance(25.0339639, 121.5644722, 10.0)
WHERE Class < 8
ORDER BY Distance;
```

## Deployment Order

1. **Run EF Core migrations** (creates database schema)
2. **Execute fn_selePlaceDistance.sql** (creates the distance calculation function)
3. **Execute seed-all-data.sql** (populates test data)

## Deployment Instructions

### Using SQL Server Management Studio (SSMS)
1. Open SSMS and connect to your SQL Server instance
2. Open the script file (File → Open → File)
3. Select the target database (`Travel`) from the dropdown
4. Press F5 or click Execute

### Using Azure Data Studio
1. Connect to your SQL Server instance
2. Open the script file
3. Ensure `USE Travel;` is at the top of the script
4. Run the script (Ctrl+Shift+E or click Run)

### Using Command Line (sqlcmd)
```bash
sqlcmd -S your_server_name -d Travel -i fn_selePlaceDistance.sql
sqlcmd -S your_server_name -d Travel -i seed-all-data.sql
```

### Using dotnet CLI with EF Core
If using migrations, you can execute SQL files after migration:
```bash
dotnet ef database update
sqlcmd -S your_server_name -d Travel -i database/fn_selePlaceDistance.sql
sqlcmd -S your_server_name -d Travel -i database/seed-all-data.sql
```

## Testing

After deployment, verify the function works:

```sql
-- Test: Find places within 5km of Taipei 101
SELECT Name, Region, Town, Distance
FROM fn_selePlaceDistance(25.0339639, 121.5644722, 5.0)
ORDER BY Distance;
```

Expected results should include nearby Taipei landmarks like 台北101, 國父紀念館, etc.

## Performance Notes

The function uses a bounding box pre-filter before applying the Haversine formula to improve performance:
- Approximately 1 degree latitude ≈ 111 km
- Longitude adjustment accounts for latitude using `COS(RADIANS(@Lat))`

This optimization significantly reduces the number of distance calculations needed.
