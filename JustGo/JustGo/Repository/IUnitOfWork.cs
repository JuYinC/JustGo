namespace JustGo.Repository
{
    public interface IUnitOfWork
    {
        IBlogRepostioy blog { get;}
        IPlaceWeatherRepostiory place { get;}
        IScheduleRepostioy schedule { get;}
        IUserKeepRepostiory keep { get;}        
    }
}
