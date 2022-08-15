using JustGo.Models;
using JustGo.ViewModels;
using System.Data;

namespace JustGo.Repository
{
    public class UserKeepRepostiory : IUserKeepRepostiory
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;
        public UserKeepRepostiory(IDbConnection con, TravelContext context)
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
            if (_context.UserKeep.SingleOrDefault(e => e.UserId == vm.UserId && e.KeepClass == vm.KeepClass && e.KeepNumber == vm.Id) != null)
            {
                return true;
            }
            return false;
        }

        public bool Keep(UserKeepVM vm)
        {
            var keep = _context.UserKeep.SingleOrDefault(e => e.UserId == vm.UserId && e.KeepClass == vm.KeepClass && e.KeepNumber == vm.Id);
            if(keep != null)
            {
                _context.Remove(keep);
                _context.SaveChanges();
                var blog = _context.Blog.SingleOrDefault(e => e.BlogId == keep.KeepNumber);
                if (blog != null)
                {
                    blog.Like -= 1;
                    _context.Update(blog);
                    _context.SaveChanges();
                    return true;
                }                
            }
            else
            {
                UserKeep userKeep = new UserKeep()
                { 
                  KeepClass = vm.KeepClass??0,
                  UserId = vm.UserId??"",
                  KeepNumber = vm.Id??0
                };
                _context.Add(userKeep);
                var blog = _context.Blog.SingleOrDefault(e => e.BlogId == userKeep.KeepNumber);
                if (blog != null)
                {
                    blog.Like += 1;
                    _context.Update(blog);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
    }
}
