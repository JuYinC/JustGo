using JustGo.Models;
using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IScheduleRepostioy
    {
        //搜尋使用者行程表
        public ICollection<ScheduleVM> selectUserSchedule(int UserId);

        //搜尋行程表細項
        public ScheduleVM selectSceduleDetails(int SceduleId);

        public bool editScedule(Schedule schedule, ICollection<ScheduleDetails> scheduleDetails);

        public bool createScedule(Schedule schedule, ICollection<ScheduleDetails> scheduleDetails);

        public bool deleteScedule(int SceduleId);
    }
}
