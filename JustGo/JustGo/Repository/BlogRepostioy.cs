using Dapper;
using JustGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using JustGo.ViewModels;
using System.Data;
using System.Diagnostics;

namespace JustGo.Repository
{
    public class BlogRepostioy : IBlogRepostioy
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;
        public BlogRepostioy(IDbConnection con, TravelContext context)
        {
            _con = con;
            _context = context;
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
            string title = "";
            //string describe = "";
            if (vm.Search.Length > 0)
            {
                title = "(Title like '%'+@Search+'%' or Describe like '%'+@Search+'%') and";
                //describe = "and Describe like '%'+@Search+'%'";
            }
            string filterCounty = "";            
            if (vm.selectCounty.Length > 0)
            {
                filterCounty = "and Region in @selectCounty";
            }
            string filterAcitivity = "";
            if (vm.selectAcitivity.Length > 0)
            {
                filterAcitivity = "and Class in @selectAcitivity";
            }            
            string strSQL = $"select * from Blog as b where {title} (Select count(DetailsID) from BlogDetails as bd where b.BlogID = BlogID and(select COUNT(PlaceID) from Place where bd.PlaceID = PlaceID {filterCounty} {filterAcitivity})>0)>0";
            List<Blog> mList = _con.Query<Blog>(strSQL, vm).ToList();
            List<BlogVM> vmList = new List<BlogVM>();
            if (mList.Count > 0)
            {                
                foreach (var item in mList)
                {
                    vmList.Add(modeltoVM(item));
                }
            }
            
            return vmList;
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
