namespace JustGo.ViewModels
{
    public class SelectPlaceVM
    {        
        public string? selectType { get; set; }
        public string[]? selectCounty { get; set; }
        public int[]? selectAcitivity { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public string? Search { get; set; }
        public double? Distance { get; set; }
        public int ? SearchNumber { get; set; }
    }
}
