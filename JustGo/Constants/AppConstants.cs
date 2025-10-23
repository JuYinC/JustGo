namespace JustGo.Constants
{
    /// <summary>
    /// Application-wide constants
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// Default user profile image filename
        /// </summary>
        public const string DefaultUserImage = "2208121714164777.jpg";

        /// <summary>
        /// Distance thresholds in kilometers
        /// </summary>
        public static class Distance
        {
            /// <summary>
            /// Nearby distance threshold (15km)
            /// </summary>
            public const int NearbyDistanceKm = 15;

            /// <summary>
            /// Maximum distance for searches (200km)
            /// </summary>
            public const int MaxDistanceKm = 200;

            /// <summary>
            /// Default distance when not specified (15km)
            /// </summary>
            public const int DefaultDistanceKm = 15;
        }

        /// <summary>
        /// User keep/favorite classes
        /// </summary>
        public static class KeepClass
        {
            /// <summary>
            /// Blog keep class
            /// </summary>
            public const int Blog = 0;

            /// <summary>
            /// Schedule keep class
            /// </summary>
            public const int Schedule = 1;
        }

        /// <summary>
        /// Place classification constants
        /// </summary>
        public static class PlaceClass
        {
            /// <summary>
            /// Maximum class value for attractions (景點上限)
            /// </summary>
            public const int AttractionMaxClass = 15;

            /// <summary>
            /// Maximum class value for restaurants
            /// </summary>
            public const int RestaurantMaxClass = 15;

            /// <summary>
            /// Hotel class value
            /// </summary>
            public const int HotelClass = 16;
        }

        /// <summary>
        /// Search and pagination defaults
        /// </summary>
        public static class SearchDefaults
        {
            /// <summary>
            /// Default page size for initial load (3 items)
            /// </summary>
            public const int DefaultPageSize = 3;

            /// <summary>
            /// Page size for search results (12 items)
            /// </summary>
            public const int SearchPageSize = 12;

            /// <summary>
            /// Maximum number of nearby places to return (500)
            /// </summary>
            public const int NearbyPlacesLimit = 500;

            /// <summary>
            /// Maximum number of random places to return (200)
            /// </summary>
            public const int RandomPlacesLimit = 200;
        }

        /// <summary>
        /// File upload validation constants
        /// </summary>
        public static class FileUpload
        {
            /// <summary>
            /// Maximum file size in bytes (5MB)
            /// </summary>
            public const int MaxFileSizeBytes = 5 * 1024 * 1024;

            /// <summary>
            /// Maximum file size in MB (5MB)
            /// </summary>
            public const int MaxFileSizeMB = 5;

            /// <summary>
            /// Allowed image extensions
            /// </summary>
            public static readonly string[] AllowedImageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

            /// <summary>
            /// Allowed MIME types for images
            /// </summary>
            public static class MimeTypes
            {
                public const string Png = "data:image/png;base64";
                public const string Jpeg = "data:image/jpeg;base64";
                public const string Gif = "data:image/gif;base64";
            }
        }
    }
}
