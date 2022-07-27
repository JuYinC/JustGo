using JustGo.Models;
using JustGo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace JustGo.Repository
{
    public class BlogRepostioy : IBlogRepostioy
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;
        readonly IPlaceWeatherRepostiory _pr;
        public BlogRepostioy(IDbConnection con, TravelContext context, IPlaceWeatherRepostiory weatherRepostiory)
        {
            _con = con;
            _context = context;
            _pr = weatherRepostiory;
        }

        public bool createBlog(BlogVM vm)
        {
            try{
                _context.Blog.Add(VMtoModel(vm));
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }           
            return true;
        }

        public BlogVM createScheduleToBlog(int scheduleId)
        {
            var schedule = _context.Schedule.Include(b => b.ScheduleDetails).SingleOrDefault(e => e.ScheduleId == scheduleId);

            BlogVM blogVm = new BlogVM()
            {
                Like = 0,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate
            };

            List<BlogDetailsVM> vmList = new List<BlogDetailsVM>();

            foreach (ScheduleDetails item in schedule.ScheduleDetails)
            {
                var p = _context.Place.SingleOrDefault(e=>e.PlaceId == item.PlaceId);
                var vm = new BlogDetailsVM() { 
                    StartTime = item.StartTime,
                    EndtTime  = item.EndtTime,
                    PlaceId = item.PlaceId,
                    P_Name = p.Name,
                    P_tel = p.Tel,
                    P_Add = p.Add,
                    
                };
                vmList.Add(vm);
            }

            blogVm.Details = vmList;

            return blogVm;
        }

        public bool deleteBlog(BlogVM vm)
        {
            _context.Remove(_context.Blog.Include(b=>b.BlogDetails).SingleOrDefault(e=>e.BlogId==vm.BlogId)??new Blog());

            return true;
        }

        public bool editBlog(BlogVM vm)
        {
            _context.RemoveRange(_context.BlogDetails.Where(b=>b.BlogId==vm.BlogId));

            _context.Update(VMtoModel(vm));
            _context.SaveChanges();
            return true;
        }

        public ICollection<BlogVM> getBlogFilter(SelectPlaceVM vm)
        {
            ICollection<Place> p_list = _pr.getPlaceFilter(vm);
            List<BlogVM> result = new List<BlogVM>();
            foreach (Place p in p_list)
            {
                var item = _context.Blog.SingleOrDefault();
            }
            return result;
        }

        public ICollection<BlogVM> getBlogRank()
        {
            throw new NotImplementedException();
        }

        public BlogVM selectBlog(int blogId)
        {
            return modeltoVM(_context.Blog.Include(b => b.BlogDetails).SingleOrDefault(b => b.BlogId == blogId)??new Blog());
        }

        public ICollection<BlogVM> selectUserBlog(string userId)
        {
            var vmList = new List<BlogVM>();
            foreach(Blog item in _context.Blog.Where(b => b.UserId == userId))
            {
                vmList.Add(modeltoVM(item));
            }
            
            return vmList;
        }

        BlogVM modeltoVM(Blog model)
        {
            BlogVM vm = new BlogVM()
            {
                BlogId = model.BlogId,
                UserId = model.UserId,
                Title = model.Title,
                Describe = model.Describe,
                CoverImageName = model.ImageName,
                Like = model.Like,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Details = new List<BlogDetailsVM>()
            };
            if(model.BlogDetails.Count>0)
            {
                foreach (BlogDetails item in model.BlogDetails)
                {
                    var p = _context.Place.SingleOrDefault(e => e.PlaceId == item.PlaceId);
                    vm.Details.Add(new BlogDetailsVM()
                    {
                        StartTime = item.StartTime,
                        EndtTime  = item.EndtTime,
                        PlaceId = item.PlaceId,
                        P_Name = p.Name,
                        P_Add  = p.Add,
                        P_tel = p.Name,
                        Describe = item.Describe,
                        Images = new List<string>(),
                        Score = item.Score
                    });
                }
            }

            return vm;
        }
        Blog VMtoModel(BlogVM vm)
        {
            Blog model = new Blog()
            {
                BlogId = vm.BlogId,
                UserId = vm.UserId,
                Describe = vm.Describe,
                ImageName = vm.CoverImageName,
                Like = vm.Like,
                StartDate=vm.StartDate,
                EndDate = vm.EndDate,
            };
            if (vm.Details.Count > 0)
            {
                model.BlogDetails = new List<BlogDetails>();
                foreach (BlogDetailsVM item in vm.Details)
                {
                    model.BlogDetails.Add(
                        new BlogDetails()
                        {
                            StartTime=item.StartTime,EndtTime=item.EndtTime,Describe = item.Describe,Images = item.Images.ToString(),
                            Score = item.Score
                        }
                    );
                }
            }
            return model;            
        }
    }
}
