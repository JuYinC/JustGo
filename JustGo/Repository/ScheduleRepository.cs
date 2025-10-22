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
    public class ScheduleRepository : IScheduleRepository
    {
        readonly IDbConnection _con;
        readonly TravelContext _context;

        public ScheduleRepository(TravelContext context, IDbConnection con)
        {
            _con = con;
            _context = context;            
        }

        public bool createScedule(ScheduleVM vm)
        {
            try
            {
                _context.Add(viewToModel(vm));
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Log database-specific errors
                Console.WriteLine($"Error creating schedule: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return false;
            }
            catch (Exception ex)
            {
                // Log unexpected errors
                Console.WriteLine($"Unexpected error creating schedule: {ex.Message}");
                return false;
            }
        }

        public bool copyScheduleByBlog(BlogVM vm)
        {            
            var blog = _context.Blog.Include(e => e.BlogDetails).SingleOrDefault(e => e.BlogId == vm.BlogId);
            Schedule model;
            if (blog != null)
            {
                int setDays = (vm.StartDate - blog.StartDate).Days;
                model = new Schedule()
                {
                    UserId = vm.UserId,
                    Title = "複製-" + blog.Title,
                    StartDate = blog.StartDate.AddDays(setDays),
                    EndDate = blog.EndDate.AddDays(setDays),
                    ScheduleDetails = new List<ScheduleDetails>()
                };
                foreach (var item in blog.BlogDetails)
                {
                    model.ScheduleDetails.Add(new ScheduleDetails()
                    {
                        StartTime = item.StartTime.AddDays(setDays).AddHours(-8),
                        EndtTime = item.EndtTime.AddDays(setDays).AddHours(-8),
                        PlaceId = item.PlaceId,
                        Town = ((_context.Place.SingleOrDefault(e=>e.PlaceId==item.PlaceId))??new Place()).Town,                        
                    });
                }
                _context.Add(model);
                _context.SaveChanges();
                return true;
            }            
            return false;
        }

        public bool deleteScedule(ScheduleVM vm)
        {
            var deleSchedule = _context.Schedule.Include(e=>e.ScheduleDetails).SingleOrDefault(e => e.ScheduleId== vm.ScheduleId && e.UserId==vm.UserId);
            if (deleSchedule != null)
            {
                _context.Remove(deleSchedule);
                _context.SaveChanges();
                return true;
            }
            return false;
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
            //return modelToView(_context.Schedule.Include(b => b.ScheduleDetails).SingleOrDefault(e => e.ScheduleId == SceduleId && e.UserId == UserId) ?? new Schedule());
            return modelToView(_context.Schedule.Include(b => b.ScheduleDetails).SingleOrDefault(e => e.ScheduleId == SceduleId) ?? new Schedule());
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
                            StartTime = item.StartTime.AddHours(8),
                            EndTime = item.EndtTime.AddHours(8),
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
