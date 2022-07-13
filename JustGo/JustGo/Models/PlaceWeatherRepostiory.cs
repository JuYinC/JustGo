using Dapper;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace JustGo.Models
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
            using(var con = _con)
            {
                con.Open();
                string sqlStr = $"select * from Place order by PlaceID offset @start row fetch next @quantity rows only";
                return con.Query<Place>(sqlStr,new { start , quantity }).ToList();
            }          
        }

        public IEnumerable<Place> getPlaceFilter(string[] placeClass, string[] region, string[] town, int[] Class)
        {
            using (var con = _con)
            {
                con.Open();
                string sqlStr = $"select * from Place Where ";
                p p = new p();
                bool i = true;
                if (region.Length > 0)
                {
                    if (i)
                    {
                        sqlStr += "Region in @region ";
                        i = false;
                    }
                    p.region = region;
                }
                if (town.Length > 0)
                {
                    if (i)
                    {
                        sqlStr += " Town in @town";
                    }
                    else
                    {
                        sqlStr += " and Town in @town";
                    }
                    p.town = town;
                }
                if (Class.Length > 0)
                {
                    if (i)
                    {
                        sqlStr += " Class in @Class";
                    }
                    else
                    {
                        sqlStr += " and Class in @Class";
                    }
                    p.Class = Class;
                }
                return con.Query<Place>(sqlStr, p).ToList();
            }
        }

        public Place getPlaceId(string id)
        {
            using (var con = _con)
            {
                con.Open();
                string sqlStr = $"select * from Place Where PlaceId = @id";
                return con.Query<Place>(sqlStr, new { id }).FirstOrDefault();
            }
        }

        public Weather getWeatherByLocation(string location)
        {
            throw new NotImplementedException();
        }
    }

    public class p
    {
        public string[]? region { get; set; }
        public string[]? town { get; set; }
        public int[]? Class { get; set; }
    }
}
