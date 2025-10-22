namespace JustGo.Repository
{
    public interface IUnitOfWork
    {
        IBlogRepository blog { get;}
        IPlaceWeatherRepository place { get;}
        IScheduleRepository schedule { get;}
        IUserKeepRepository keep { get;}
    }
}
