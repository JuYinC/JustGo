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
            var getPlace = _con.Query<Place>("select * from fn_selePlaceDistance(@Lat,@Lng,10)",vm).ToList();            
            Console.WriteLine(getPlace.GetType());

            return getPlace;
        }

        public ICollection<Place> getPlaceFilter(SelectPlaceVM vm)
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
                        sqlStr += "Region in @selectCounty ";
                        i = false;
                    }                    
                }               
                if (vm.selectAcitivity.Length > 0)
                {
                    if (i)
                    {
                        sqlStr += " Class in @selectAcitivity";
                    }
                    else
                    {
                        sqlStr += " and Class in @selectAcitivity";
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
    }    
}
