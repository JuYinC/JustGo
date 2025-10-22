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

        // Lazy initialization for repositories (singleton per UnitOfWork instance)
        private IBlogRepository? _blog;
        private IPlaceWeatherRepository? _place;
        private IScheduleRepository? _schedule;
        private IUserKeepRepository? _keep;

        public UnitOfWork(IDbConnection con, TravelContext context, ApplicationDbContext applicationDbContext)
        {
            _con = con;
            _context = context;
            _UserComtext = applicationDbContext;
        }

        // Properties now return singleton instances instead of creating new ones each time
        public IBlogRepository blog
        {
            get
            {
                if (_blog == null)
                {
                    _blog = new BlogRepository(_con, _context, _UserComtext);
                }
                return _blog;
            }
        }

        public IPlaceWeatherRepository place
        {
            get
            {
                if (_place == null)
                {
                    _place = new PlaceWeatherRepository(_context, _con);
                }
                return _place;
            }
        }

        public IScheduleRepository schedule
        {
            get
            {
                if (_schedule == null)
                {
                    _schedule = new ScheduleRepository(_context, _con);
                }
                return _schedule;
            }
        }

        public IUserKeepRepository keep
        {
            get
            {
                if (_keep == null)
                {
                    _keep = new UserKeepRepository(_con, _context);
                }
                return _keep;
            }
        }
    }
}
