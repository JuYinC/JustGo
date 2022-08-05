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
    public class ScheduleRepostioy : IScheduleRepostioy
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;

        public ScheduleRepostioy(TravelContext context, IDbConnection con)
        {
            _con = con;
            _context = context;            
        }

        public bool createScedule(ScheduleVM vm)
        {            
            try{
                _context.Add(viewToModel(vm));
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }            
            return true;
        }

        public bool deleteScedule(int SceduleId)
        {
            Schedule deleSchedule = _context.Schedule.Where(e => e.ScheduleId==SceduleId).Include(e=>e.ScheduleDetails).First();

            _context.Remove(deleSchedule);
            _context.SaveChanges();

            return true;
        }

        public bool editScedule(ScheduleVM vm)
        {            
              _context.RemoveRange(_context.ScheduleDetails.Where(e => e.ScheduleId == vm.ScheduleId));
            _context.SaveChanges();
           
            Schedule schedule = viewToModel(vm);
            _context.Update(schedule);
            _context.SaveChanges();

            return true;
        }

        public ScheduleVM selectScedule(int SceduleId ,string UserId)
        {
            return modelToView(_context.Schedule.Where(e => e.UserId == UserId).Include(b => b.ScheduleDetails).SingleOrDefault(e => e.ScheduleId == SceduleId) ?? new Schedule());
            //return modelToView(_context.Schedule.Include(b => b.ScheduleDetails).SingleOrDefault(e => e.ScheduleId == SceduleId) ?? new Schedule());
        }

        public IList<ScheduleVM> selectUserSchedule(string UserId)
        {
            List<ScheduleVM> vmList = new List<ScheduleVM>();            
            foreach (Schedule item in _context.Schedule.Where(e => e.UserId == UserId).ToList())
            {
                vmList.Add(modelToView(item));
            }            
            return vmList;
        }

        private Schedule viewToModel(ScheduleVM vm)
        {
            List<ScheduleDetails> modelList = new List<ScheduleDetails>();
            if (vm.Details != null)
            {
                foreach (List<ScheduleDetailVM> detail in vm.Details)
                {
                    foreach (ScheduleDetailVM item in detail)
                    {
                        if (item.Place == null)
                        {
                            continue;
                        }
                        ScheduleDetails details = new ScheduleDetails()
                        {
                            StartTime = item.StartTime,
                            EndtTime = item.EndTime,
                            PlaceId = item.Place.PlaceId,
                            Town = item.Place.Town,
                            WeatherWarning = item.WeatherWarning,
                            Pop = item.Pop,
                            Temperature = item.Temperature,
                            Uvi = item.Uvi
                        };
                        modelList.Add(details);
                    }
                }
            }
            Schedule model = new Schedule()
            {
                ScheduleId = vm.ScheduleId,
                UserId = vm.UserId,
                Title = vm.Title,
                StartDate = vm.StartDate.AddHours(8),
                EndDate = vm.EndDate.AddHours(8),
                WeatherWarning = vm.WeatherWarning,

                ScheduleDetails = modelList,
            };
            return model;
        }

        private ScheduleVM modelToView(Schedule model)
        {
            ScheduleVM vm = new ScheduleVM()
            {                
                ScheduleId = model.ScheduleId,
                UserId = model.UserId,
                Title = model.Title,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                WeatherWarning = model.WeatherWarning,
            };
            //model.StartDate.AddHours(-8);
            //model.EndDate.AddHours(-8);
            if (model.ScheduleDetails.Count > 0)
            {
                vm.Details = new List<IList<ScheduleDetailVM>>();                
                for (int i = 0; i <= (model.EndDate-model.StartDate).Days; i++)
                {
                    List<ScheduleDetailVM> vmList = new List<ScheduleDetailVM>();
                    var list = model.ScheduleDetails.Where(e => e.StartTime.Day == model.StartDate.Day+i);                       
                    foreach (ScheduleDetails item in list)
                    {                        
                        ScheduleDetailVM vmDetail = new ScheduleDetailVM()
                        {
                            StartTime = item.StartTime,
                            EndTime = item.EndtTime,
                            WeatherWarning = item.WeatherWarning,
                            Pop = item.Pop,
                            Temperature = item.Temperature,
                            Uvi = item.Uvi,
                            Place = _context.Place.SingleOrDefault(e => e.PlaceId == item.PlaceId) ?? new Place(),
                        };
                        vmList.Add(vmDetail);
                    }
                    vm.Details.Add(vmList);
                }                
            }
            return vm;
        }
    }    
}
