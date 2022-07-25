using Dapper;
using JustGo.Models;
using JustGo.ViewModels;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace JustGo.Repository
{
    public class PlaceWeatherRepostiory : IPlaceWeatherRepostiory
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;
        public PlaceWeatherRepostiory(TravelContext context, IDbConnection con)
        {
            _con = con;
            _context = context;
        }

        public IEnumerable<Place> getPlace(int start, int quantity)
        {
            using (var con = _con)
            {
                con.Open();
                string sqlStr = $"select * from Place order by PlaceID offset @start row fetch next @quantity rows only";
                return con.Query<Place>(sqlStr, new { start, quantity }).ToList();                
            }
        }

        public IEnumerable<Place> getPlaceFilter(SelectPlaceVM vm)
        {
            using (var con = _con)
            {
                con.Open();
                string sqlStr = $"select * from Place Where ";
                bool i = true;
                if (vm.selectCounty.Length>0)
                {
                    if (i)
                    {
                        sqlStr += "Region in @region ";
                        i = false;
                    }                    
                }               
                if (vm.selectAcitivity.Length > 0)
                {
                    if (i)
                    {
                        sqlStr += " Class in @Class";
                    }
                    else
                    {
                        sqlStr += " and Class in @Class";
                    }                    
                }
                return con.Query<Place>(sqlStr, vm).ToList();
            }
        }

        public Place getPlaceId(string id)
        {
            using (var con = _con)
            {
                con.Open();
                string sqlStr = $"select * from Place Where PlaceId = @id";
                return con.Query<Place>(sqlStr, new { id }).FirstOrDefault()??new Place();
            }
        }

        public Weather getWeatherByLocation(string location)
        {
            throw new NotImplementedException();
        }
        public void testAddBlog()
        {
            Blog blog = new Blog()
            {
                UserId = "ddd",
                Title = "a",
                ImageName = "b.jpg",
                Describe = "s",
                Like = 0,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
                BlogDetails = new List<BlogDetails>()
                {
                    new BlogDetails() { StartTime = DateTime.Now, EndtTime = DateTime.Now, PlaceId =10000,Describe="adas",Images="s",Score=4.2 },
                    new BlogDetails() { StartTime = DateTime.Now, EndtTime = DateTime.Now, PlaceId =10000,Describe="aada",Images="s",Score=4.2 },
                    new BlogDetails() { StartTime = DateTime.Now, EndtTime = DateTime.Now, PlaceId =10000,Describe="aasdda",Images="s",Score=4.2 },
                    new BlogDetails() { StartTime = DateTime.Now, EndtTime = DateTime.Now, PlaceId =10000,Describe="aass",Images="s",Score=4.2 },
                }
            };

            _context.Add(blog);
            _context.SaveChanges();
        }
    }

    public class p
    {
        public string[]? region { get; set; }
        public string[]? town { get; set; }
        public int[]? Class { get; set; }
    }
}
