using Dapper;
using JustGo.Models;
using JustGo.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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

        public ICollection<Place> getPlace(SelectPlaceVM vm)
        {
            var getPlace = _con.Query<Place>("select * from fn_selePlaceDistance(@Lat,@Lng,10) where class < 15", vm).ToList();
            Console.WriteLine(getPlace.GetType());

            return getPlace;
        }

        public ICollection<Place> getPlaceFilter(SelectPlaceVM vm)
        {
            string sqlStr;
            sqlStr = $"select * from Place Where ";
            //sqlStr = $"select * from fn_selePlaceDistance(22.6397082860113,120.30264837097221,40) Where ";
            //sqlStr = $"select * from fn_selePlaceDistance(@Lat,@Lng,@Distance) Where ";
            bool i = true;
            if (vm.selectCounty.Length > 0)
            {
                if (i)
                {
                    sqlStr += "Region in @selectCounty ";
                    i = false;
                }
            }
            switch (vm.selectType)
            {

                case "景點":
                    if (vm.selectAcitivity.Length > 0)
                    {
                        if (i)
                        {
                            sqlStr += " Class in @selectAcitivity ";
                        }
                        else
                        {
                            sqlStr += " and Class in @selectAcitivity ";
                        }
                    }
                    sqlStr += " and Class <15";                    
                    break;
                case "餐飲":
                    if (i)
                    {
                        sqlStr += " Class = '15' ";
                    }
                    else
                    {
                        sqlStr += " and Class = '15' ";
                    }
                    break;
                case "旅宿":
                    if (i)
                    {
                        sqlStr += " Class = '16' ";
                    }
                    else
                    {
                        sqlStr += " and Class = '16' ";
                    }
                    break;
            }
            return _con.Query<Place>(sqlStr, vm).ToList();
        }

        public Place getPlaceId(string id)
        {
            string sqlStr = $"select * from Place Where PlaceId = @id";
            return _con.Query<Place>(sqlStr, new { id }).FirstOrDefault() ?? new Place();

        }

        public ICollection<Weather> getWeatherByLocation(string location)
        {
            return _con.Query<Weather>("select * from weather").ToList();
        }
    }
}
