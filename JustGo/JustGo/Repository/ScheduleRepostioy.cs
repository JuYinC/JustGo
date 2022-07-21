using Dapper;
using JustGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
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

            _context.Remove(delDetails);
            schedule.ScheduleDetails=scheduleDetails;
            _context.Update(schedule);
            _context.SaveChanges();

            return true;
        }

        public ICollection<ScheduleDetails> selectSceduleDetails(int SceduleId)
        {
            return _context.ScheduleDetails.Where(e=>e.ScheduleId== SceduleId).ToList();
        }

        public ICollection<Schedule> selectUserSchedule(int UserId)
        {
            return _context.Schedule.Where(e=>e.UserId==UserId).ToList();
        }
    }
}
