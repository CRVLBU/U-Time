using DevExpress.Data.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;


using Utime_utility;
using Utime_WEB.Models;
using Utime_WEB.Models.DTO;
using Utime_WEB.Models.DTO.Static;
using Utime_WEB.Models.Static;
using Utime_WEB.Models.ViewModels;
using Utime_WEB.Services.IServices;

using System.Web.Helpers;
using DevExpress.Data.ODataLinq.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Utime_WEB.Controllers
{
    public class AccountController:Controller
    {
        public Dictionary<long, long> table;
        public IActivityService _service;
        public AccountController(IActivityService service) {
            _service = service;
        }
        [Authorize]
        public ActionResult Index()
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWT-Token")))
            //{
            //    return RedirectToAction("AccessDenied", "UserAuth");
            //}
            return View();
        }
        public async Task<ArrayList> GetTasks()
        {
            
            var resp = await _service.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
            if (resp != null && resp.IsSuccess == true)
            {
                List<long> completed = new();
                List<long> failed = new();
                List<long> partial = new();
                List<string> days_week = new();
                List<string> dates_of_days = new();

                JwtSecurityTokenHandler handler = new();
                var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
                string username = token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value;

                var fit_days = JsonConvert.DeserializeObject<List<ActivityDTO>>(resp.Contents.ToString()).Where(u => DateTime.Compare(u.DeadLine, DateTime.Today.AddDays(-1)) < 1 && DateTime.Compare(u.DeadLine, DateTime.Today.AddDays(-8)) == 1&& u.UserName==username) ;
            
                
    

                long  append_days=1;
                
                for (var ind = DateTime.Now.AddDays(-1); ind > DateTime.Now.AddDays(-7);)
                {
                    long counter = 0;
                    long counter2=0;
                    DateTime cur_date = DateTime.Today.AddDays(append_days*-1);
                    ind = cur_date;
                    days_week.Insert(0, SD.days[cur_date.DayOfWeek.ToString().Substring(0,3)]);
                    string date_string = cur_date.Day.ToString() + "/" + cur_date.Month.ToString() + "/" + cur_date.Year.ToString();
                    append_days++;
                    dates_of_days.Insert(0, date_string);
                    var days = fit_days.Where(u => u.DeadLine.Day == cur_date.Day);
                    counter = days.Where(u => u.isDone == true&&u.isExpired==false).Count();
                    counter2 = days.Where(u => u.isDone == true && u.isExpired == true).Count();
                    completed.Insert(0,counter);
                    failed.Insert(0,days.Count()-counter-counter2);
                    partial.Insert(0, counter2);
                    
                }
                failed = new List<long>() { 2, 5, 3, 2, 4, 2, 3 };
                partial = new List<long>() { 2, 1, 3, 4, 6, 8, 4 };
                completed = new List<long>() { 3, 1, 4, 5, 2, 3, 1 };
  
                return new ArrayList() { failed, completed,days_week, dates_of_days,partial};

            }
            return new ArrayList() {};
        }
        public async Task<List<long>> GetPriorityPercent(string date)
        {
            string[] numbers = date.Split('/');
            DateTime new_date = new DateTime(int.Parse(numbers[2]), int.Parse(numbers[1]), int.Parse(numbers[0]));
            JwtSecurityTokenHandler handler = new();
            var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
            string username = token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value;
            
            var resp = await _service.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
            if (resp != null && resp.IsSuccess == true)
            {
                var fit_activities = JsonConvert.DeserializeObject<List<ActivityDTO>>(resp.Contents.ToString()).Where(u=>DateTime.Compare(u.DeadLine.Date,new_date)==0 && u.UserName == username);
               
                long countA = fit_activities.Where(u => u.Priority == "A").Count();
                long countB = fit_activities.Where(u => u.Priority == "B").Count();
                long countC = fit_activities.Where(u => u.Priority == "C").Count();
                long countD = fit_activities.Where(u => u.Priority == "D").Count();
                long DoneA= fit_activities.Where(u => u.Priority == "A"&&u.isDone==true&&u.isExpired==false).Count();
                long DoneB = fit_activities.Where(u => u.Priority == "B" && u.isDone == true && u.isExpired == false).Count();
                long DoneC = fit_activities.Where(u => u.Priority == "C" && u.isDone == true && u.isExpired == false).Count();
                long DoneD = fit_activities.Where(u => u.Priority == "D" && u.isDone == true && u.isExpired == false).Count();
                long DoneA1 = fit_activities.Where(u => u.Priority == "A" && u.isDone == true && u.isExpired == true).Count();
                long DoneB1 = fit_activities.Where(u => u.Priority == "B" && u.isDone == true && u.isExpired == true).Count();
                long DoneC1 = fit_activities.Where(u => u.Priority == "C" && u.isDone == true && u.isExpired == true).Count();
                long DoneD1 = fit_activities.Where(u => u.Priority == "D" && u.isDone == true && u.isExpired == true).Count();
                long failedA = countA - DoneA - DoneA1;
                long failedB = countB - DoneB - DoneB1;
                long failedC = countC - DoneC - DoneC1;
                long failedD = countC - DoneC - DoneC1;

                var return_list = new List<long>();
                return_list.Add(DoneA);
                return_list.Add(DoneA1);
                return_list.Add(countA - DoneA-DoneA1);
                return_list.Add(DoneB);
                return_list.Add(DoneB1);
                return_list.Add(countB - DoneB - DoneB1);

                return_list.Add(DoneC);
                return_list.Add(DoneC1);
                return_list.Add(countC - DoneC-DoneC1);
                return_list.Add(DoneD);
                return_list.Add(DoneD1);
                return_list.Add(countD - DoneD-DoneD1);
                return_list = new List<long>() { 2,3,4,6,4,3,5,3,2,5,8,2};
                return return_list;




            }
            return new List<long>() { };
        }
    }
}
