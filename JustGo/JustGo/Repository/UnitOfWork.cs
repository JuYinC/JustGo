using JustGo.Data;
using JustGo.Models;
using System.Data;

namespace JustGo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;
        readonly ApplicationDbContext _UserComtext;
        public UnitOfWork(IDbConnection con, TravelContext context, ApplicationDbContext applicationDbContext)
        {
            _con = con;
            _context = context;
            _UserComtext = applicationDbContext;
        }

        public IBlogRepostioy blog => new BlogRepostioy(_con, _context, _UserComtext);

        public IPlaceWeatherRepostiory place => new PlaceWeatherRepostiory(_context, _con);

        public IScheduleRepostioy schedule => new ScheduleRepostioy(_context, _con);

        public IUserKeepRepostiory keep => new UserKeepRepostiory(_con,_context);
    }
}
