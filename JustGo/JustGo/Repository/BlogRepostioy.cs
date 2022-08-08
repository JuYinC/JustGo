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
                        var p = _context.Place.SingleOrDefault(e => e.PlaceId == item.PlaceId);
                        var vm = new BlogDetailsVM()
                        {
                            StartTime = item.StartTime,
                            EndtTime = item.EndtTime,
                            PlaceId = item.PlaceId,
                            P_Name = p.Name,
                            P_tel = p.Tel,
                            P_Add = p.Add,

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
            string strSerarch = "";            
            if (vm.Search.Length > 0)
            {
                strSerarch = "(Title like '%'+@Search+'%' or Describe like '%'+@Search+'%') and";                
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
            string strSQL = $"select * from Blog as b where {strSerarch} exists(Select DetailsID from BlogDetails as bd where b.BlogID = BlogID and exists(select PlaceID from Place where bd.PlaceID = PlaceID {filterCounty} {filterAcitivity}))";
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
            var blogs = _context.Blog.OrderBy(e => e.Like).Take(20);
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

        BlogVM modeltoVM(Blog model)
        {
            BlogVM vm = new BlogVM()
            {
                BlogId = model.BlogId,
                UserId = model.UserId,
                Title = model.Title,
                Describe = model.Describe,
                CoverImage = new blogImage() { name=model.ImageName},
                Like = model.Like,
                StartDate = model.StartDate.AddHours(8),
                EndDate = model.EndDate.AddHours(8),
                Details = new List<IList<BlogDetailsVM>>()
            };
            if(model.BlogDetails.Count>0)
            {
                for (int i = 0; i <= (model.EndDate - model.StartDate).Days; i++)
                {
                    foreach (BlogDetails item in model.BlogDetails.Where(e => e.StartTime.Day == model.StartDate.Day + i))
                    {
                        vm.Details.Add(new List<BlogDetailsVM>());
                        var p = _context.Place.SingleOrDefault(e => e.PlaceId == item.PlaceId);
                        vm.Details[i].Add(new BlogDetailsVM()
                        {
                            StartTime = item.StartTime.AddHours(8),
                            EndtTime = item.EndtTime.AddHours(8),
                            PlaceId = item.PlaceId,
                            P_Name = p.Name,
                            P_Add = p.Add,
                            P_tel = p.Name,
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
                Like = vm.Like,
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
                        Console.WriteLine(JsonConvert.SerializeObject(item.Images));
                        model.BlogDetails.Add(                            
                            new BlogDetails()
                            {
                                PlaceId = item.PlaceId,
                                StartTime = item.StartTime,
                                EndtTime = item.EndtTime,
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
