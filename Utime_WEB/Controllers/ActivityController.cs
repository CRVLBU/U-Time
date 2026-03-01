
using AutoMapper;
using DevExpress.Data.Helpers;
using DevExpress.Utils.Filtering;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Web.Helpers;
using Utime_utility;
using Utime_WEB.Models;
using Utime_WEB.Models.DTO;
using Utime_WEB.Models.DTO.Static;
using Utime_WEB.Models.Static;
using Utime_WEB.Models.ViewModels;
using Utime_WEB.Services.IServices;



namespace Utime_WEB.Controllers
{
    public class ActivityController : Controller
    {
        
        private readonly IActivityService _service;
        private readonly ICategoryService _cat_service;
        private readonly IMapper _mapper;
        private readonly IHostEnvironment _env;


        public ActivityController( IMapper mapper, IActivityService service,ICategoryService cat_service,IHostEnvironment env)
        {
            
            _mapper = mapper;
            _service = service;
            _cat_service = cat_service;
            _env = env;
            
        


        }
        public async Task<IEnumerable<ActivityDTO>> Get_cur_activities(DateTime date)
        {
            var resp = await _service.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
            if(resp!=null && resp.IsSuccess == true)
            {
                List<ActivityDTO> activities = JsonConvert.DeserializeObject<List<ActivityDTO>>(resp.Contents.ToString());
                return activities.Where(u => DateTime.Compare(u.DeadLine, date) == 0);
            }
            return new List<ActivityDTO>() { };
        }
        public async Task<IActionResult> RemoveOutdated()
        {
            Response response = await _service.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
            List<ActivityDTO> activities = new List<ActivityDTO> { };
            if (response != null && response.IsSuccess == true)
            {
                activities = JsonConvert.DeserializeObject<List<ActivityDTO>>(response.Contents.ToString());
                IEnumerable<ActivityDTO> remove_activities = activities.Where(u => DateTime.Compare(u.DeadLine, new DateTime(2025,1,3)) == -1 );
                foreach (var entity in remove_activities)
                {
                    await _service.DeleteAsync<Response>(entity.ID, HttpContext.Session.GetString("JWT-Token"));
                }

            }
            return Ok();

        }
        public async Task<int> DefineOrder(string priority,string date,string timeFrame, int id)
        {
            var resp = await _service.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
            JwtSecurityTokenHandler handler = new();
            var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
            string username = token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value;
            //string username = "Maria";
            var activities = JsonConvert.DeserializeObject<List<ActivityDTO>>(resp.Contents.ToString()).Where(u =>u.isDone==false);
            activities = activities.Where(u => u.UserName == username);
            activities=activities.OrderBy(u => u.Priority).ThenBy(u => u.DeadLine).ThenBy(u => u.TimeFrame);
            //var year = date.Substring(6, 4);
            //var month = date.Substring(2, 2);
            //var day = date.Substring(0, 2);
            //var hour = timeFrame.Substring(0, 2);
            //var minute = timeFrame.Substring(3, 2);
    
            DateTime cur_date = new DateTime(int.Parse(date.Substring(6, 4)), int.Parse(date.Substring(3, 2)), int.Parse(date.Substring(0, 2)),int.Parse(timeFrame.Substring(0,2)), int.Parse(timeFrame.Substring(3, 2)),0);

            int ind = 1;
            foreach(var elem in activities){
                if (elem.ID == id)
                {
                    continue;
                }
                if (string.Compare(elem.Priority,priority)==0)
                {
                    if (DateTime.Compare(elem.DeadLine, cur_date) == 1)
                    {
                        return ind;
                    }
                }
                else if (string.Compare(elem.Priority, priority) == 1)
                {
                    return ind;
                }
                ind++;
            }
            return ind;
        }
        public async Task<List<List<string>>> Date_list(int increase,IEnumerable<ActivityDTO>? activities,string? opted_day)
        {

            if (increase == 1)
            {
                ActivityStatic.increment += 7;
            }
            else if (increase == -1)
            {
                ActivityStatic.increment -= 7;
            }
            if (opted_day == null)
            {
                opted_day = ActivityStatic.def_day.Day.ToString() + "/" + ActivityStatic.def_day.Month.ToString() + "/" + ActivityStatic.def_day.Year.ToString();
            }
            
            DateTime date = DateTime.Today;
            date = date.AddDays(ActivityStatic.increment);
            string cur_day = date.DayOfWeek.ToString();
            
            List<List<string>> output = new List<List<string>>{ };
            int i = SD.days.Keys.ToList().IndexOf(cur_day.Substring(0, 3).ToString());
            for (int n = 0; n < 7; n++)
            {

                var tier = date.AddDays(n - i).ToString("dd/MM/yyyy");
                string add_value = "0";
                if (DateTime.Compare(date.AddDays(n - i), DateTime.Today) > -1 && activities != null)
                {
                    if(activities.Where(u=>DateTime.Compare(u.DeadLine.Date, date.AddDays(n - i)) == 0&&u.isDone==false).Count() != 0)
                    {
                        add_value = "1";
                    }
                }
                output.Add(new List<string>() { SD.days[SD.days.Keys.ToList()[n]], date.AddDays(n - i).ToString("dd/MM/yyyy").Substring(0, 2), date.AddDays(n - i).ToString("dd/MM/yyyy").Substring(3, 2), date.AddDays(n - i).ToString("dd/MM/yyyy").Substring(6, 4) ,add_value});

                if (date.AddDays(n - i).ToString("dd/MM/yyyy") == opted_day)
                {
                    output[output.Count() - 1].Add("1");
                }
                else
                {
                    output[output.Count() - 1].Add("0");
                }

                var attorney = output[output.Count() - 1];


            }
           
            return output;

        }
        [Authorize]
        public async Task<ActionResult> IndexActivity(string? new_date,string? regime,string? sortBy)
        

        {
            var lull = _env;
            ActivityGetViewModel viewModel = new();
            var check = User.Identity.IsAuthenticated;
            if (regime == null)
            {
                regime = ActivityStatic.regime;
            }
            else
            {
                ActivityStatic.regime = regime;
            }

            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWT-Token")))
            //{
            //    return RedirectToAction("AccessDenied", "UserAuth", new {pagenum=2});
            //}
            DateTime date=new DateTime();

            if (regime == "0")
            {
                date = DateTime.Now;
                ActivityStatic.regime = "0";
            }
            else if(regime=="1")
            {
                if (!string.IsNullOrEmpty(new_date))
                {

                    date = new DateTime(int.Parse(new_date.Substring(6, 4)), int.Parse(new_date.Substring(3, 2)), int.Parse(new_date.Substring(0, 2)));
                    ActivityStatic.def_day = date;
                }
                else
                {
                    if (DateTime.Compare(ActivityStatic.def_day, new DateTime()) == 0)
                    {
                        date = DateTime.Now;
                    }
                    else
                    {
                        date = ActivityStatic.def_day;
                    }
                }
            }






            //await RemoveOutdated();
            Response response = await _service.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
            List<ActivityDTO> activities = new List<ActivityDTO>{};
            if (response != null && response.IsSuccess == true)
            {
                activities = JsonConvert.DeserializeObject<List<ActivityDTO>>(response.Contents.ToString());

                JwtSecurityTokenHandler handler = new();
                var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
                string username = token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value;
                //string username = "Maria";

                var Activities = activities.Where(u => u.UserName == username);
                var activities_for_round = activities.Where(u => u.UserName == username);
                if (regime != "2")
                {
                    Activities = Activities.Where(u => DateTime.Compare(u.DeadLine.Date, date.Date) == 0);
                }
                activities_for_round = activities.Where(u => DateTime.Compare(u.DeadLine.Date, DateTime.Today) > -1);


                if (regime != "2")
                {
                    foreach (var elem in Activities)
                    {
                        if (DateTime.Compare(new DateTime(elem.DeadLine.Date.Year, elem.DeadLine.Date.Month, elem.DeadLine.Date.Day, elem.TimeFrame.Hours, elem.TimeFrame.Minutes, 0), DateTime.Now) == -1)
                        {
                            var galvanize = new DateTime(elem.DeadLine.Date.Year, elem.DeadLine.Date.Month, elem.DeadLine.Date.Day, elem.TimeFrame.Hours, elem.TimeFrame.Minutes, 0);
                            var bail = DateTime.Now;
                            elem.percentage = 0;
                        }
                        else
                        {
                            elem.DeadLine.AddHours(elem.TimeFrame.Hours);
                            elem.DeadLine.AddMinutes(elem.TimeFrame.Minutes);
                            elem.percentage = Math.Max((int)Math.Ceiling(((elem.DeadLine - DateTime.Now) /(elem.DeadLine - elem.CreationDate)) * 100), 0);

                        }


                        if (elem.isDone == true)
                        {
                            elem.opacity = 50;
                        }
                        else
                        {
                            elem.opacity = 100;
                        }
                    }
                }
                if (regime != "2")
                {
                    viewModel.activities_done = Activities.Where(u => u.isDone == true);
                    viewModel.activities_undone = Activities.Where(u => u.isDone == false);
                }
                else
                {
                    viewModel.activities_undone = Activities.Where(u => u.isDone == false && u.isExpired == true);
                }
                if (sortBy != null)
                {
                    ActivityStatic.sorting = sortBy;
                }
                if (ActivityStatic.sorting == "Приоритет")
                {
                    viewModel.activities_undone = viewModel.activities_undone.OrderBy(u => u.Priority).ThenBy(u=>u.ActivityName);
                }
                else if (ActivityStatic.sorting == "Важность")
                {
                    viewModel.activities_undone = viewModel.activities_undone.OrderBy(u => u.Imp).ThenBy(u => u.ActivityName);
                }
                else if (ActivityStatic.sorting == "Срочность")
                {
                    viewModel.activities_undone = viewModel.activities_undone.OrderBy(u => u.Urgency).ThenBy(u => u.ActivityName);
                }
               

                
                /*for (int i = 0; i < 4; i++)
                {
                    var trauma = viewModel.activities;
                    viewModel.dates.Add(current_date);
                    
                    current_date=current_date.AddDays(1);
                }*/
                
                Response resp = await _cat_service.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
                
                viewModel.categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(resp.Contents.ToString()).Where(u=>u.UserName==username).Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.ID.ToString()
                }) ;
                if (regime == "1")
                    
                {
                    ActivityStatic.regime = "1";
                    var ratify = await Date_list(0, activities_for_round,date.Date.ToString("dd/MM/yyyy"));

                    viewModel.days = ratify;
                }
                else
                {
                    viewModel.days = new List<List<string>>();
                }
                viewModel.mode = regime;
                viewModel.start_ind = 0;
                ActivityStatic.VM = viewModel;
                return View(viewModel);
            }
            ModelState.AddModelError("IndexError", response.ErrorMessages.FirstOrDefault());
            return View();
            
            

        }

        public async Task<ArrayList> GetActivity(int id,string state)
        {
            ActivityDTO activity = JsonConvert.DeserializeObject<ActivityDTO>((await _service.GetAsync<Response>(id, HttpContext.Session.GetString("JWT-Token"))).Contents.ToString());


            if (state == "show")
            {
                return new ArrayList() { activity.ActivityName, activity.Category.CategoryName, activity.Priority, activity.CreationDate.ToString("dd/MM/yyyy HH:mm"), (activity.DeadLine).ToString("dd/MM/yyyy HH:mm"),activity.Description };
            }
            ArrayList mas= new ArrayList() { activity.ActivityName, activity.DeadLine.ToString("dd/MM/yyyy"), activity.TimeFrame.ToString("hh':'mm"), activity.CategoryID, activity.Imp, activity.Urgency,activity.Description };
            return mas;
            
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateActivity(ActivityGetViewModel activity)
        {
            JwtSecurityTokenHandler handler = new();
            var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
            string username = token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value;

            ActivityCreateDTO activityCreate= activity.activity_create;
            activityCreate.Priority = ActivityStatic.Priority;
            activityCreate.CreationDate = DateTime.Now;
            activityCreate.UserName = username;
            Response resp = await _service.CreateAsync<Response>(activityCreate, HttpContext.Session.GetString("JWT-Token"));
            if (resp != null && resp.IsSuccess == true)
            {
                return RedirectToAction(nameof(IndexActivity));
            }

            ActivityGetViewModel viewModel = ActivityStatic.VM;
            var taunt = resp.ErrorMessages.FirstOrDefault();
            ModelState.Clear();

            ModelState.AddModelError("CreateError", resp.ErrorMessages.FirstOrDefault().ToString());
            viewModel.activity_create = activity.activity_create;
            return View("IndexActivity",viewModel);
        }
        public List<string> SetPriority(string imp, string urg)
        {
            List<string> response = new List<string> { };
            if (imp == "High" && urg == "High")
            {
                response.AddRange(new string[]{"A", "#d13b5b" });
                ActivityStatic.Priority = "A";
                
            }
            else if ((imp == "High" && urg == "Medium") || (imp == "Medium" && urg == "High"))
            {
                response.AddRange(new string[] { "B", "#d1953b" });
                ActivityStatic.Priority = "B";
            }
            else if ((imp == "Medium" && urg == "Medium") || (imp == "Low" && urg == "High") ||(imp == "High" && urg == "Low"))
            {
                response.AddRange(new string[] { "C", "#d1d13b" });
                ActivityStatic.Priority = "C";
            }
            else if ((imp == "Low" && urg == "Low") || (imp == "Low" && urg == "Medium") || (imp == "Medium" && urg == "Low"))
            {
                response.AddRange(new string[] { "D", "#d1d13b" });
                ActivityStatic.Priority = "D";
            }
            return response;
        }
        public void CompleteActvity(int id)
        {
            ActivityStatic.id = id;
            ActivityStatic.isDone = true;
        }
        public void ExpireActivity(int id)
        {
            ActivityStatic.id = id;
            ActivityStatic.isExpired = true;
            ActivityStatic.Counter2++;
        }
        public void SetActivityId(int id)
        {

            ActivityStatic.id = id;
           
        }
        //[ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UpdateActivity(ActivityGetViewModel? ActivityModel=null)
        {
            JwtSecurityTokenHandler handler = new();
            
            var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
            string username = token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value;
            //string username = "Maria";
            var id = ActivityStatic.id;
            var response1 = await _service.GetAsync<Response>(ActivityStatic.id, HttpContext.Session.GetString("JWT-Token"));
            ActivityDTO activity = JsonConvert.DeserializeObject<ActivityDTO>(response1.Contents.ToString());
            if (ActivityStatic.isDone)
            {

                ActivityStatic.isDone = false;
                ActivityUpdateDTO activity_update = _mapper.Map<ActivityUpdateDTO>(activity);
                if (activity_update.isDone == true)
                {
                    activity_update.isDone = false;
                }
                else
                {
                    activity_update.isDone = true;
                    TempData["success"] = "Выполнено!";
                }
                activity_update.DeadLine = activity.DeadLine.Day.ToString() + "/" + activity.DeadLine.Month.ToString() + "/" + activity.DeadLine.Year.ToString();
                activity_update.ID = ActivityStatic.id;
                activity_update.isCheck = true;
                var resp = await _service.UpdateAsync<Response>(activity_update, HttpContext.Session.GetString("JWT-Token"));
               
                if (resp != null && resp.IsSuccess == true)
                {
      
                    return RedirectToAction(nameof(IndexActivity));
                }

            }
            else if (ActivityStatic.isExpired == true)
            {
                
                ActivityUpdateDTO activity_update = _mapper.Map<ActivityUpdateDTO>(activity);
               
                activity_update.DeadLine = activity.DeadLine.Day.ToString() + "/" + activity.DeadLine.Month.ToString() + "/" + activity.DeadLine.Year.ToString();
                activity_update.ID = ActivityStatic.id;
                activity_update.isCheck = true;
                activity_update.isExpired = true;
                var resp = await _service.UpdateAsync<Response>(activity_update, HttpContext.Session.GetString("JWT-Token"));
                ActivityStatic.Counter1++;
                if (resp != null && resp.IsSuccess == true&&ActivityStatic.Counter1==ActivityStatic.Counter2)
                {
                    ActivityStatic.Counter1 = 0;
                    ActivityStatic.Counter2 = 0;
                    ActivityStatic.isExpired = false;
                
                    return RedirectToAction(nameof(IndexActivity),new { regime=ActivityStatic.regime});
                }

            }
            

            ActivityUpdateDTO activityUpdate = ActivityModel.activity_update;
            activityUpdate.ID = ActivityStatic.id;
            activityUpdate.Priority = ActivityStatic.Priority;
            activityUpdate.isCheck = false;
            activityUpdate.UserName = username;
            activityUpdate.DeadLine = activityUpdate.DeadLine.Substring(0,2) + "/" + activityUpdate.DeadLine.Substring(3,2) + "/" + activityUpdate.DeadLine.Substring(6, 4);

            activityUpdate.CreationDate = activity.CreationDate;
            Response response = await _service.UpdateAsync<Response>(activityUpdate, HttpContext.Session.GetString("JWT-Token"));
            if (response != null && response.IsSuccess == true)
            {

                return RedirectToAction(nameof(IndexActivity));
            }
            ModelState.AddModelError("UpdateError", response.ErrorMessages.FirstOrDefault());
            return NoContent();
        }
        [Authorize]
        public async Task<ActionResult> DeleteActivity()
        {

            

                Response response = await _service.DeleteAsync<Response>(ActivityStatic.id, HttpContext.Session.GetString("JWT-Token"));


                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(IndexActivity));
                }
            ModelState.AddModelError("DeleteError", response.ErrorMessages.FirstOrDefault());
            return NoContent();
            }


        }



    }

