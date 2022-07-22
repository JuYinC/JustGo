using Dapper;
using JustGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using JustGo.ViewModels;
using System.Data;

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
            Schedule test = viewToModel(new ScheduleVM());
            ScheduleVM testVM = modelToView(test);
            Console.WriteLine(test.ScheduleId.ToString());
            Console.WriteLine(testVM.ScheduleId.ToString());
        }

        public bool createScedule(Schedule schedule, ICollection<ScheduleDetails> scheduleDetails)
        {
            schedule.ScheduleDetails=scheduleDetails;

            try{
                _context.Add(schedule);
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
            Schedule deleSchedule = _context.Schedule.Where(e=>e.ScheduleId == SceduleId).Include(e=>e.ScheduleDetails).First();

            _context.Remove(deleSchedule);
            _context.SaveChanges();

            return true;
        }

        public bool editScedule(Schedule schedule, ICollection<ScheduleDetails> scheduleDetails)
        {
            List<ScheduleDetails> delDetails = _context.ScheduleDetails.Where(e => e.ScheduleId == schedule.ScheduleId).ToList();

            foreach(ScheduleDetails detail in delDetails)
            {
                _context.Remove(detail);
            }

            _context.SaveChanges();

            schedule.ScheduleDetails = scheduleDetails;
            _context.SaveChanges();

            return true;
        }

        public ScheduleVM selectSceduleDetails(int SceduleId)
        {            
            return modelToView(_context.Schedule.SingleOrDefault(e => e.ScheduleId == SceduleId)??new Schedule());
        }

        public ICollection<ScheduleVM> selectUserSchedule(int UserId)
        {
            List<ScheduleVM> vmList = new List<ScheduleVM>();
            foreach (Schedule item in _context.Schedule.Where(e => e.UserId == UserId).ToList())
            {
                vmList.Add(modelToView(item));
            }
            return vmList;
        }

        Schedule viewToModel(ScheduleVM vm)
        {
            List<ScheduleDetails> modelList = new List<ScheduleDetails>();
            if(vm.Details != null)
            {
                foreach (ScheduleDetailVM item in vm.Details)
                {
                    ScheduleDetails details = new ScheduleDetails()
                    {
                        StartTime = item.StartTime,
                        EndtTime = item.EndtTime,
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
            

            Schedule model = new Schedule()
            {
                ScheduleId = vm.ScheduleId,
                UserId = vm.UserId,
                StartDate = vm.StartDate,
                EndDate = vm.EndDate,
                WeatherWarning = vm.WeatherWarning,

                ScheduleDetails = modelList,
            };            

            

            return model;
        }



        ScheduleVM modelToView(Schedule model)
        {
            List<ScheduleDetailVM> vmList = new List<ScheduleDetailVM>();
            if(model.ScheduleDetails != null)
            {
                foreach (ScheduleDetails item in model.ScheduleDetails)
                {
                    ScheduleDetailVM vmDetail = new ScheduleDetailVM()
                    {
                        StartTime = item.StartTime,
                        EndtTime = item.EndtTime,
                        WeatherWarning = item.WeatherWarning,
                        Pop = item.Pop,
                        Temperature = item.Temperature,
                        Uvi = item.Uvi,
                        Place = _context.Place.SingleOrDefault(e => e.PlaceId == item.PlaceId) ?? new Place(),
                    };
                    vmList.Add(vmDetail);
                }
            }
            

            ScheduleVM vm = new ScheduleVM()
            {
                ScheduleId=model.ScheduleId,
                UserId=model.UserId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                WeatherWarning=model.WeatherWarning,
                Details = vmList,
            };            
            return vm;
        }
    }
}
