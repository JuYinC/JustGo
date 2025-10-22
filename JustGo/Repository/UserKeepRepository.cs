using JustGo.Models;
using JustGo.ViewModels;
using System.Data;

namespace JustGo.Repository
{
    public class UserKeepRepository : IUserKeepRepository
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;
        public UserKeepRepository(IDbConnection con, TravelContext context)
        {
            _con = con;
            _context = context;
        }
        public bool IsKeep(UserKeepVM vm)
        {
            if (vm == null)
            {
                return false;
            }
            UserKeep? keep;
            try
            {
                keep = _context.UserKeep.SingleOrDefault(e => e.UserId == vm.UserId && e.KeepClass == vm.KeepClass && e.KeepNumber == vm.Id);
            }
            catch
            {
                keep = _context.UserKeep.Where(e => e.UserId == vm.UserId && e.KeepClass == vm.KeepClass && e.KeepNumber == vm.Id).FirstOrDefault();
            }
            if (keep != null)
            {
                return true;
            }
            return false;            
        }

        public bool Keep(UserKeepVM vm)
        {
            UserKeep? keep;
            
                keep = _context.UserKeep.SingleOrDefault(e => e.UserId == vm.UserId && e.KeepClass == vm.KeepClass && e.KeepNumber == vm.Id);
            if (keep != null)
            {
                _context.Remove(keep);
                _context.SaveChanges();
                var blog = _context.Blog.SingleOrDefault(e => e.BlogId == keep.KeepNumber);
                if (blog != null)
                {
                    blog.Like = _context.UserKeep.Where(e => e.KeepClass == 0 && e.KeepNumber == vm.Id).Count();
                    _context.Update(blog);
                    _context.SaveChanges();
                    return true;
                }
            }
            else
            {
                UserKeep userKeep = new UserKeep()
                {
                    KeepClass = vm.KeepClass ?? 0,
                    UserId = vm.UserId ?? "",
                    KeepNumber = vm.Id ?? 0
                };
                _context.Add(userKeep);
                _context.SaveChanges();
                var blog = _context.Blog.SingleOrDefault(e => e.BlogId == vm.Id);
                if (blog != null)
                {
                    blog.Like = _context.UserKeep.Where(e => e.KeepClass == 0 && e.KeepNumber == vm.Id).Count();
                    _context.Update(blog);
                    _context.SaveChanges();
                    return true;
                }
            }                        
            return false;
        }
    }
}
