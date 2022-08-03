using JustGo.Models;
using JustGo.ViewModels;

namespace JustGo.Repository
{
    public interface IScheduleRepostioy
    {
        //搜尋使用者行程表
        public IList<ScheduleVM> selectUserSchedule(string UserId);

        //搜尋行程表細項
        public ScheduleVM selectScedule(int SceduleId, string UserId);

        public bool editScedule(ScheduleVM vm);

        public bool createScedule(ScheduleVM vm);

        public bool deleteScedule(int SceduleId);
    }
}
