using JustGo.Models;
namespace JustGo.ViewModels
{
    public class ScheduleVM
    {
        
        public int ScheduleId { get; set; }
        public string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? WeatherWarning { get; set; }

        public IList<IList<ScheduleDetailVM>>? Details { get; set; }
    }
    public class ScheduleDetailVM
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }        
        public bool? WeatherWarning { get; set; }
        public int? Pop { get; set; }
        public int? Temperature { get; set; }
        public int? Uvi { get; set; }

        public Place? Place { get; set; }
    }

}
