using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UtimeAPI.Data;
using UtimeAPI.Models.DTO;
using UtimeAPI.Models;
using UtimeAPI.Repository.IRepository;
using static Utime_utility.SD;
using System.Text.RegularExpressions;
using Utime_utility;
using Microsoft.AspNetCore.Authorization;
using DevExpress.Data.Browsing;
namespace UtimeAPI.Controllers
{
    [ApiController]
    [Route("api/ActivityAPI")]
    public class ActivityAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IActivityRepository _repos;
        private readonly ICategoryRepository _cat_repos;
        public Response api_response;
        private readonly ApplicationDBContext _db;
        public ActivityAPIController(IMapper map, IActivityRepository repos, ApplicationDBContext db,ICategoryRepository cat_repos)
        {
            _mapper = map;
            _db = db;
            _repos = repos;
            _cat_repos = cat_repos;
            api_response = new();
        }
        [HttpGet(Name = "GetAllActivities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Response>> GetAllActivties()
        {
            try
            {
                List<Activity> activities = await _repos.GetAllAsync(includeProperties:"Category");
                List<ActivityDTO> activities_dto=_mapper.Map<List<ActivityDTO>>(activities);

                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.OK;
                api_response.Contents = activities_dto;
                return Ok(api_response);
            }
            catch (Exception ex)
            {
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add(ex.ToString());
                return BadRequest(api_response);

            }
        }
        [HttpGet("{id:int}", Name = "GetActivity")]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<Response>> GetActivity(int id)
        {
            if (id == 0)
            {
                api_response.ErrorMessages = new();
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("An incorrect ID!");
                api_response.IsSuccess = false;
                return BadRequest(api_response);
            }

            try
            {
                Activity activity = await _repos.GetAsync(u => u.ID == id, includeProperties: "Category");
                if (activity == null)
                {
                    api_response.StatusCode = HttpStatusCode.NotFound;

                    api_response.IsSuccess = false;
                    return NotFound(api_response);
                }
                api_response.IsSuccess = true;
                
                api_response.Contents = _mapper.Map<ActivityDTO>(activity);
                api_response.StatusCode = HttpStatusCode.OK;
                return Ok(api_response);
            }
            catch (Exception ex)
            {
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add(ex.ToString());
                return BadRequest(api_response);
            }


        }
        [HttpPost(Name = "ActivityPost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Response>> CreateActivity([FromBody] ActivityCreateDTO activityDTO)
        {
            var ratify = activityDTO;
            if (activityDTO == null)
            {api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("Invalid input!");
                return BadRequest(api_response);
            }

            try
            { 

                string[] numbers = activityDTO.DeadLine.Split('/');
                DateTime date = new DateTime(int.Parse(numbers[2]), int.Parse(numbers[1]), int.Parse(numbers[0]));
                date += activityDTO.TimeFrame;
                if (DateTime.Compare(DateTime.Now, date) == 1)
                {api_response.ErrorMessages = new();
                    api_response.IsSuccess = false;
                    api_response.StatusCode = HttpStatusCode.BadRequest;
                    api_response.ErrorMessages.Add("Дедлайн неактуален!");
                    return BadRequest(api_response);

                }
                Activity activity=_mapper.Map<Activity>(activityDTO);
                Category category = await _cat_repos.GetAsync(u => u.ID == activity.CategoryID);

                category.Amount += 1;
                await _cat_repos.UpdateAsync(category);
                activity.DeadLine = date;
                await _repos.CreateAsync(activity);
                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetActivity", new { id = _db.Activities.FirstOrDefault(u => u.ActivityName == activityDTO.ActivityName).ID }, api_response);

            }
            catch (Exception ex)
            {
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add(ex.ToString());
                return BadRequest(api_response);
            }
        }
        [HttpDelete("{id:int}",Name = "DeleteActivity")]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Response>> DeleteActivity(int id)
        {
            if (id == 0)
            {
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("An incorrect ID!");
                api_response.IsSuccess = false;
                return BadRequest(api_response);
            }
            if (await _repos.GetAsync(u => u.ID == id, track: false) == null)
            {
                api_response.ErrorMessages = new();
                api_response.StatusCode = HttpStatusCode.NotFound;
                api_response.ErrorMessages.Add("The ID does not exist!");
                api_response.IsSuccess = false;
                return NotFound(api_response);
            }
            try
            {
                await _repos.DeleteAsync(await _repos.GetAsync(u => u.ID == id, track: false));
                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.NoContent;
                return api_response;

            }
            catch (Exception ex)
            {
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add(ex.ToString());
                return BadRequest(api_response);
            }
        }
        [HttpPut(Name = "UpdateActivity")]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<Response>> UpdateActivity([FromBody]ActivityUpdateDTO activityDTO)
        {
            if (activityDTO == null)
            {
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("An incorrect input");
                api_response.IsSuccess = false;
                return BadRequest(api_response);
            }
            if (await _repos.GetAsync(u => u.ID == activityDTO.ID, track: false) == null)
            {
                api_response.ErrorMessages = new();
                api_response.StatusCode = HttpStatusCode.NotFound;
                api_response.ErrorMessages.Add("The ID does not exist!");
                api_response.IsSuccess = false;

                return NotFound(api_response);
            }
            try
            {
                Activity activity = new();
                if (activityDTO.isCheck==false)
                {
                    string[] numbers = activityDTO.DeadLine.Split('/');
                    DateTime date = new DateTime(int.Parse(numbers[2]), int.Parse(numbers[1]), int.Parse(numbers[0]));
                    date += activityDTO.TimeFrame;
                    if (DateTime.Compare(DateTime.Now, date) == 1)
                    {
                        api_response.ErrorMessages = new();
                        api_response.IsSuccess = false;
                        api_response.StatusCode = HttpStatusCode.BadRequest;
                        api_response.ErrorMessages.Add("Дата неактуальна!");
                        return BadRequest(api_response);

                    }
                    activity = _mapper.Map<Activity>(activityDTO);
                    activity.DeadLine = date;
                }
                else
                {
                    //Category category = await _cat_repos.GetAsync(u => u.ID == activity.CategoryID);


                    activity = _mapper.Map<Activity>(activityDTO);
                    //if (activity.isExpired == false||(activity.isExpired==true&&activity.isDone==true))
                    //{
                    //    category.Amount -= 1;
                    //    await _cat_repos.UpdateAsync(category);
                    //}
                }

                await _repos.UpdateAsync(activity);
                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.OK;
                api_response.Contents = activity;
                return Ok(api_response);

            }
            catch (Exception ex)
            {
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add(ex.ToString());
                return BadRequest(api_response);
            }
        }
    }
}
