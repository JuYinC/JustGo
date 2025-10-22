using Dapper;
using JustGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using Newtonsoft.Json;
using JustGo.ViewModels;
using System.Data;
using System.Diagnostics;
using JustGo.Data;

namespace JustGo.Repository
{
    public class BlogRepository : IBlogRepository
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;
        readonly ApplicationDbContext _UserComtext;
        public BlogRepository(IDbConnection con, TravelContext context ,ApplicationDbContext applicationDbContext)
        {
            _con = con;
            _context = context;
            _UserComtext = applicationDbContext;
        }

        public bool createBlog(BlogVM vm)
        {
            try
            {
                _context.Blog.Add(VMtoModel(vm));
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Log the error with details for debugging
                Console.WriteLine($"Error creating blog: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                Console.WriteLine($"Unexpected error creating blog: {ex.Message}");
                return false;
            }
        }

        public BlogVM createScheduleToBlog(int scheduleId ,string userId)
        {
            var schedule = _context.Schedule.Include(b => b.ScheduleDetails).SingleOrDefault(e => e.ScheduleId == scheduleId)??new Schedule();
            var user = _UserComtext.ApplicationUsers.Single(e => e.Id == userId);
            if (user == null)
            {
                return new BlogVM();
            }
            BlogVM blogVm = new BlogVM()
            {
                UserName = user.Name,
                UserImage = "2208121714164777.jpg",
                Like = 0,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate
            };            
            schedule.StartDate.AddHours(-8);
            schedule.EndDate.AddHours(-8);
            if (schedule.ScheduleDetails.Count > 0)
            {
                blogVm.Details = new List<IList<BlogDetailsVM>>();
                for(int i = 0; i <= schedule.EndDate.Day - schedule.StartDate.Day; i++)
                {
                    blogVm.Details.Add(new List<BlogDetailsVM>());
                    foreach (ScheduleDetails item in schedule.ScheduleDetails.Where(e => e.StartTime.Day == schedule.StartDate.Day + i))
                    {
                        var p = _context.Place.SingleOrDefault(e => e.PlaceId == item.PlaceId)??new Place();
                        var vm = new BlogDetailsVM()
                        {
                            StartTime = item.StartTime.AddHours(8),
                            EndTime = item.EndtTime.AddHours(8),
                            PlaceId = item.PlaceId,
                            P_Name = p.Name,
                            P_tel = p.Tel,
                            P_Add = p.Add,
                            Describe = "",
                            Score=1,
                        };
                        blogVm.Details[i].Add(vm);
                    }
                }                
            }
            return blogVm;
        }

        public bool deleteBlog(BlogVM vm)
        {
            _context.Remove(_context.Blog.Include(b=>b.BlogDetails).SingleOrDefault(e=>e.BlogId==vm.BlogId)??new Blog());
            _context.SaveChanges();
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
            // Use EF Core LINQ with proper joins to prevent SQL injection
            var query = _context.Blog
                .Include(b => b.BlogDetails)
                .AsQueryable();

            // Apply search filter (Title or Describe contains search term)
            if (!string.IsNullOrEmpty(vm.Search))
            {
                query = query.Where(b =>
                    EF.Functions.Like(b.Title, $"%{vm.Search}%") ||
                    EF.Functions.Like(b.Describe, $"%{vm.Search}%"));
            }

            // Apply county filter (join with Place table via PlaceId)
            if (vm.selectCounty != null && vm.selectCounty.Length > 0)
            {
                query = query.Where(b =>
                    b.BlogDetails.Any(bd =>
                        _context.Place.Any(p => p.PlaceId == bd.PlaceId && vm.selectCounty.Contains(p.Region))));
            }

            // Apply activity filter (join with Place table via PlaceId for Class property)
            if (vm.selectAcitivity != null && vm.selectAcitivity.Length > 0)
            {
                query = query.Where(b =>
                    b.BlogDetails.Any(bd =>
                        _context.Place.Any(p => p.PlaceId == bd.PlaceId && vm.selectAcitivity.Contains(p.Class))));
            }

            // Determine page size based on whether it's initial load or search
            int pageSize = vm.SearchNumber == null ? 3 : 12;
            int skipCount = vm.SearchNumber ?? 0;

            // Apply pagination and order
            var mList = query
                .OrderBy(b => b.BlogId)
                .Skip(skipCount)
                .Take(pageSize)
                .ToList();

            // Convert to ViewModels
            var vmList = new List<BlogVM>();
            foreach (var item in mList)
            {
                vmList.Add(modeltoVM(item));
            }

            return vmList;
        }

        public ICollection<BlogVM> getBlogRank()
        {
            var blogs = _context.Blog.OrderByDescending(e => e.Like).Take(4);
            List<BlogVM> vmList = new List<BlogVM>();
            foreach(Blog item in blogs)
            {
                vmList.Add(modeltoVM(item));
            }
            return vmList;
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

        public ICollection<BlogVM> getKeepBlog(UserKeepVM vm)
        {                        
            string strSql = "select * from blog as b where exists(Select KeepNumber from UserKeep where KeepClass = 0 and UserId = @userId and KeepNumber = b.BlogId )";            
            var vmList = new List<BlogVM>();
            foreach (Blog item in _con.Query<Blog>(strSql, vm))
            {
                vmList.Add(modeltoVM(item));
            }
            return vmList;
        }

        string tagSQl = "select Class from Place as p where Class< 15 and exists(select DetailsID from BlogDetails as bd where PlaceID = p.PlaceID and exists(select BlogID where BlogID = @id and bd.BlogID = BlogID)) group by Class";

        BlogVM modeltoVM(Blog model)
        {
            
            var user = _UserComtext.ApplicationUsers.Single(e => e.Id == model.UserId);
            var tagList = _con.Query<int>(tagSQl, new { id = model.BlogId }).ToList();
            string UserImage = "2208121714164777.jpg";
            BlogVM vm = new BlogVM()
            {
                BlogId = model.BlogId,
                UserId = model.UserId,
                UserName = user.Name,
                UserImage = UserImage,
                Title = model.Title,
                Describe = model.Describe,
                CoverImage = new blogImage() { name=model.ImageName},
                TagList = tagList,
                Like = model.Like,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Details = new List<IList<BlogDetailsVM>>()
            };
            if(model.BlogDetails.Count>0)
            {                
                for (int i = 0; i <= (model.EndDate - model.StartDate).Days; i++)
                {
                    vm.Details.Add(new List<BlogDetailsVM>());
                    foreach (BlogDetails item in model.BlogDetails.Where(e => e.StartTime.Day == model.StartDate.Day + i))
                    {                        
                        var p = _context.Place.SingleOrDefault(e => e.PlaceId == item.PlaceId)??new Place();
                        vm.Details[i].Add(new BlogDetailsVM()
                        {
                            StartTime = item.StartTime,
                            EndTime = item.EndtTime,
                            PlaceId = item.PlaceId,
                            P_Name = p.Name,
                            P_Add = p.Add,
                            P_tel = p.Tel,
                            Describe = item.Describe,
                            Images = JsonConvert.DeserializeObject<List<blogImage>>(item.Images),
                            Score = item.Score
                        });
                    }
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
                ImageName = "",
                Like = _context.UserKeep.Where(e=>e.KeepClass==1&&e.KeepId==vm.BlogId).Count(),
                Title = vm.Title,
                StartDate=vm.StartDate,
                EndDate = vm.EndDate,
            };
            if (vm.CoverImage != null)
            {
                model.ImageName = vm.CoverImage.name;
            }
            if (vm.Details!=null&&vm.Details.Count>0)
            {
                model.BlogDetails = new List<BlogDetails>();
                for(int i = 0; i < vm.Details.Count; i++)
                {
                    foreach (BlogDetailsVM item in vm.Details[i])
                    {                       
                        model.BlogDetails.Add(                            
                            new BlogDetails()
                            {
                                PlaceId = item.PlaceId,
                                StartTime = item.StartTime,
                                EndtTime = item.EndTime,
                                Describe = item.Describe,
                                Images = JsonConvert.SerializeObject(item.Images),
                                Score = item.Score
                            }
                        );
                    }
                }                
            }            
            return model;            
        }

        
    }
}
