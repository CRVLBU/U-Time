using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UtimeAPI.Repository.IRepository;
using UtimeAPI.Models.DTO;
using UtimeAPI.Models;
using System.Net;
using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using UtimeAPI.Data;
using Microsoft.AspNetCore.Authorization;

namespace UtimeAPI.Controllers
{
    [ApiController]
    [Route("api/CategoryAPI")]
    public class CategoryAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _repos;
        public Response api_response;
        private readonly ApplicationDBContext _db;
        public CategoryAPIController(IMapper map, ICategoryRepository repos,ApplicationDBContext db)
        {
            _mapper = map;
            _db = db;
            _repos = repos;
            api_response = new();
        }
        [HttpGet(Name = "GetAllCategories")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Response>> GetAllCategories()
        {
            api_response = new();
            try
            {
                List<Category> categories = await _repos.GetAllAsync();
                _mapper.Map<List<CategoryDTO>>(categories);
                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.OK;
                api_response.Contents = categories;
                return Ok(api_response);
            }
            catch (Exception ex)
            {api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add(ex.ToString());
                return BadRequest(api_response);

            }
        }
        [HttpGet("{id:int}", Name = "GetCategory")]
        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<Response>> GetCategory(int id)
        {
            api_response = new();
            if (id == 0)
            {
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("An incorrect ID!");
                api_response.IsSuccess = false;
                return BadRequest(api_response);
            }
            
            try
            {
                Category category = await _repos.GetAsync(u => u.ID == id);
                if (category == null)
                {
                    api_response.StatusCode = HttpStatusCode.NotFound;

                    api_response.IsSuccess = false;
                    return NotFound(api_response);
                }
                api_response.IsSuccess = true;
                api_response.Contents = _mapper.Map<CategoryDTO>(category);
                api_response.StatusCode = HttpStatusCode.OK;
                return Ok(api_response);
            }
            catch(Exception ex)
            {api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add(ex.ToString());
                return BadRequest(api_response);
            }


        }
        [HttpPost(Name ="CategoryPost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Response>> CreateCategory([FromBody] CategoryCreateDTO categoryDTO)
        {
            api_response = new();
            if (categoryDTO == null)
            {
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("Invalid input!");
                return BadRequest(api_response);
            }
            if (_db.Categories.FirstOrDefault(u => u.CategoryName == categoryDTO.CategoryName&&u.UserName==categoryDTO.UserName) != null)
            {
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("The indicated name is not a a unique one");
                return BadRequest(api_response);
            }
            try
            {
                await _repos.CreateAsync(_mapper.Map<Category>(categoryDTO));
                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetCategory", new { id = _db.Categories.FirstOrDefault(u => u.CategoryName == categoryDTO.CategoryName).ID }, api_response);
                
            }
            catch(Exception ex)
            {
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add(ex.ToString());
                return BadRequest(api_response);
            }
        }
        [HttpDelete("{id:int}",Name ="DeleteCategory")]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Response>> DeleteCategory(int id)
        {
            api_response = new();
            if (id == 0)
            {
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("An incorrect ID!");
                api_response.IsSuccess = false;
                return BadRequest(api_response);
            }
            if (await _repos.GetAsync(u => u.ID == id, track: false)==null)
            {
                api_response.StatusCode = HttpStatusCode.NotFound;
                api_response.ErrorMessages.Add("The ID does not exist!");
                api_response.IsSuccess = false;
                return NotFound(api_response);
            }
            try
            {
                await _repos.DeleteAsync(await _repos.GetAsync(u=>u.ID==id, track: false));
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
        [HttpPut(Name = "UpdateCategory")]

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize]
        public async Task<ActionResult<Response>> UpdateCategory(CategoryUpdateDTO categoryDTO)
        {
            api_response = new();
            if (categoryDTO == null)
            {
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("An incorrect input");
                api_response.IsSuccess = false;
                return BadRequest(api_response);
            }
            if (await _repos.GetAsync(u => u.ID == categoryDTO.ID,track:false) == null)
            {
                api_response.StatusCode = HttpStatusCode.NotFound;
                api_response.ErrorMessages.Add("The ID does not exist!");
                api_response.IsSuccess = false;

                return NotFound(api_response);
            }
            try
            {
                Category category = _mapper.Map<Category>(categoryDTO);
                await _repos.UpdateAsync(category);
                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.OK;
                api_response.Contents = category;
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
