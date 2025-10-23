-- ============================================
-- Function: fn_selePlaceDistance
-- Description: Calculate distance between a given coordinate and all places
--              Returns places within specified distance using Haversine formula
-- Parameters:
--   @Lat: Latitude of the search location
--   @Lng: Longitude of the search location
--   @Distance: Maximum distance in kilometers
-- Returns: Place records with calculated Distance column (in km)
-- ============================================

USE Travel;
GO

-- Drop function if it exists
IF OBJECT_ID('dbo.fn_selePlaceDistance', 'TF') IS NOT NULL
    DROP FUNCTION dbo.fn_selePlaceDistance;
GO

CREATE FUNCTION dbo.fn_selePlaceDistance
(
    @Lat DECIMAL(10, 7),
    @Lng DECIMAL(10, 7),
    @Distance DECIMAL(10, 2)
)
RETURNS @ResultTable TABLE
(
    PlaceId INT,
    Name NVARCHAR(MAX),
    Description NVARCHAR(MAX),
    Tel NVARCHAR(MAX),
    [Add] NVARCHAR(MAX),
    Lat DECIMAL(10, 7),
    Lng DECIMAL(10, 7),
    Region NVARCHAR(MAX),
    Town NVARCHAR(MAX),
    Class INT,
    Opentime NVARCHAR(MAX),
    IntOpentime INT,
    Closetime NVARCHAR(MAX),
    IntClosetime INT,
    Timestay INT,
    Distance DECIMAL(10, 2)
)
AS
BEGIN
    -- Earth's radius in kilometers
    DECLARE @EarthRadius DECIMAL(10, 2) = 6371.0;

    INSERT INTO @ResultTable
    SELECT
        PlaceId,
        Name,
        Description,
        Tel,
        [Add],
        Lat,
        Lng,
        Region,
        Town,
        Class,
        Opentime,
        IntOpentime,
        Closetime,
        IntClosetime,
        Timestay,
        -- Haversine formula to calculate distance in kilometers
        @EarthRadius * 2 * ATN2(
            SQRT(
                POWER(SIN((RADIANS(Lat) - RADIANS(@Lat)) / 2), 2) +
                COS(RADIANS(@Lat)) * COS(RADIANS(Lat)) *
                POWER(SIN((RADIANS(Lng) - RADIANS(@Lng)) / 2), 2)
            ),
            SQRT(
                1 - (
                    POWER(SIN((RADIANS(Lat) - RADIANS(@Lat)) / 2), 2) +
                    COS(RADIANS(@Lat)) * COS(RADIANS(Lat)) *
                    POWER(SIN((RADIANS(Lng) - RADIANS(@Lng)) / 2), 2)
                )
            )
        ) AS Distance
    FROM Place
    WHERE
        -- Pre-filter using a bounding box for better performance
        -- Approximately 1 degree latitude ≈ 111 km
        Lat BETWEEN @Lat - (@Distance / 111.0) AND @Lat + (@Distance / 111.0)
        AND Lng BETWEEN @Lng - (@Distance / (111.0 * COS(RADIANS(@Lat)))) AND @Lng + (@Distance / (111.0 * COS(RADIANS(@Lat))));

    -- Filter to only include places within the specified distance
    DELETE FROM @ResultTable WHERE Distance > @Distance;

    RETURN;
END;
GO

PRINT '✓ Function fn_selePlaceDistance created successfully';
GO

-- ============================================
-- Test the function with example coordinates
-- ============================================
PRINT '';
PRINT 'Testing fn_selePlaceDistance with Taipei 101 coordinates...';
PRINT '------------------------------------------------------------';

-- Test: Find places within 5km of Taipei 101 (25.0339639, 121.5644722)
SELECT TOP 10
    Name,
    Region,
    Town,
    CAST(Distance AS DECIMAL(10, 2)) AS DistanceKm,
    Lat,
    Lng
FROM fn_selePlaceDistance(25.0339639, 121.5644722, 5.0)
ORDER BY Distance;

PRINT '';
PRINT '✓ Test completed - showing nearby places within 5km of Taipei 101';
GO
