using Dapper;
using JustGo.Models;
using JustGo.ViewModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using JustGo.Constants;

namespace JustGo.Repository
{
    public class PlaceWeatherRepository : IPlaceWeatherRepository
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;
        public PlaceWeatherRepository(TravelContext context, IDbConnection con)
        {
            _con = con;
            _context = context;
        }

        public ICollection<Place> getPlace(SelectPlaceVM vm)
        {
            if (vm.Distance != null)
            {
                if(vm.Distance <= AppConstants.Distance.NearbyDistanceKm)
                {
                    return _con.Query<Place>($"select top({AppConstants.SearchDefaults.NearbyPlacesLimit}) * from fn_selePlaceDistance(@Lat,@Lng,@Distance) where Class < {AppConstants.PlaceClass.AttractionMaxClass} order by Distance", vm).ToList();
                }
                else
                {
                    return _con.Query<Place>($"select top({AppConstants.SearchDefaults.RandomPlacesLimit}) * from fn_selePlaceDistance(@Lat,@Lng,@Distance) where Class < {AppConstants.PlaceClass.AttractionMaxClass} order by NEWID()", vm).ToList();
                }
            }
            return _con.Query<Place>($"select top({AppConstants.SearchDefaults.RandomPlacesLimit}) * from fn_selePlaceDistance(@Lat,@Lng,{AppConstants.Distance.DefaultDistanceKm}) where Class < {AppConstants.PlaceClass.AttractionMaxClass} order by NEWID()", vm).ToList();
        }

        public ICollection<Place> getPlaceFilter(SelectPlaceVM vm)
        {
            string sqlStr;
            //sqlStr = $"select * from Place ";
            //sqlStr = $"select * from fn_selePlaceDistance(22.6397082860113,120.30264837097221,40) ";
            sqlStr = $"select top({AppConstants.SearchDefaults.NearbyPlacesLimit}) * from fn_selePlaceDistance(@Lat,@Lng,@Distance) ";
            bool i = true;
            if (vm.selectCounty != null && vm.selectCounty.Length > 0)
            {
                sqlStr = $"select top({AppConstants.SearchDefaults.NearbyPlacesLimit}) * from Place Where Region in @selectCounty";
                i = false;

            }
            switch (vm.selectType)
            {
                case "景點":
                    if (vm.selectAcitivity != null && vm.selectAcitivity.Length > 0)
                    {
                        if (i)
                        {
                            sqlStr += "Where  Class in @selectAcitivity ";
                        }
                        else
                        {
                            sqlStr += " and Class in @selectAcitivity ";
                        }
                    }
                    sqlStr += $" and Class <{AppConstants.PlaceClass.AttractionMaxClass}";
                    break;
                case "餐飲":
                    if (i)
                    {
                        sqlStr += $"Where Class = '{AppConstants.PlaceClass.RestaurantMaxClass}' ";
                    }
                    else
                    {
                        sqlStr += $" and Class = '{AppConstants.PlaceClass.RestaurantMaxClass}' ";
                    }
                    break;
                case "旅宿":
                    if (i)
                    {
                        sqlStr += $"Where Class = '{AppConstants.PlaceClass.HotelClass}' ";
                    }
                    else
                    {
                        sqlStr += $" and Class = '{AppConstants.PlaceClass.HotelClass}' ";
                    }
                    break;
                default:
                    sqlStr += $"Where Class <{AppConstants.PlaceClass.AttractionMaxClass}";
                    break;
            }
            if (vm.selectCounty != null && vm.selectCounty.Length > 0 || vm.Distance>AppConstants.Distance.NearbyDistanceKm)
            {
                sqlStr += " order by NEWID()";
            }
            else
            {
                sqlStr += " order by Distance";
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
