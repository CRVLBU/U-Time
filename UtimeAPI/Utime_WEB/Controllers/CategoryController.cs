using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using Utime_WEB.Models;
using Utime_WEB.Models.DTO;
using Utime_WEB.Models.Static;
using Utime_WEB.Models.ViewModels;
using Utime_WEB.Services.IServices;

namespace Utime_WEB.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _service;
        private readonly IActivityService _service_activity;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryService service, IMapper mapper, IActivityService service_activity)
        {
            _service = service;
            _mapper = mapper;
            _service_activity = service_activity;


        }
        [Authorize]
        public async Task<ActionResult> IndexCategory()
        {
            //if (string.IsNullOrEmpty(HttpContext.Session.GetString("JWT-Token")))
            //{
            //    return RedirectToAction("AccessDenied", "UserAuth", new {pagenum=1});
            //}
            List<CategoryDTO> categories = new List<CategoryDTO> { };
            Response response = await _service.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
            Category1ViewModel categories_view = new Category1ViewModel();
            if (response != null && response.IsSuccess == true)
            {
                categories = JsonConvert.DeserializeObject<List<CategoryDTO>>(response.Contents.ToString());
                JwtSecurityTokenHandler handler = new();
                var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
                string username = token.Claims.FirstOrDefault(u => u.Type == "name").Value;

                categories_view.Categories = categories.Where(u => u.UserName == username).ToList();

            }
            //ModelState.AddModelError("IndexError", response.ErrorMessages.FirstOrDefault());
            return View(categories_view);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> CreateCategory(Category1ViewModel categoryVM)
        {
            JwtSecurityTokenHandler handler = new();
            var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
            string username = token.Claims.FirstOrDefault(u => u.Type == "name").Value;
            categoryVM.CreateCategory.UserName = username;
            Response resp = await _service.CreateAsync<Response>(categoryVM.CreateCategory, HttpContext.Session.GetString("JWT-Token"));
            if (resp != null && resp.IsSuccess == true)
            {
                return RedirectToAction(nameof(IndexCategory));
            }
            ModelState.AddModelError("CreateError", resp.ErrorMessages.FirstOrDefault());
            return NoContent();
        }

        public void SetIDCategory(int id)
        {

            CategoryCreatePartialDTO.ID = id;


        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult> UpdateCategory(Category1ViewModel categoryVM)
        {
            var mingle = CategoryCreatePartialDTO.ID;
            CategoryUpdateDTO updateCategory = _mapper.Map<CategoryUpdateDTO>(categoryVM.CreateCategory);
            JwtSecurityTokenHandler handler = new();
            var token = handler.ReadJwtToken(HttpContext.Session.GetString("JWT-Token"));
            string username = token.Claims.FirstOrDefault(u => u.Type == "name").Value;
            updateCategory.ID = CategoryCreatePartialDTO.ID;
            updateCategory.UserName = username;
            Response response = await _service.UpdateAsync<Response>(updateCategory, HttpContext.Session.GetString("JWT-Token"));
            if (response != null && response.IsSuccess == true)
            {
                return RedirectToAction(nameof(IndexCategory));
            }
            ModelState.AddModelError("UpdateError", response.ErrorMessages.FirstOrDefault());
            return NoContent();
        }
        [Authorize]
        public async Task<ActionResult> DeleteCategory()
        {
            Response resp = await _service_activity.GetAllAsync<Response>(HttpContext.Session.GetString("JWT-Token"));
            if (resp != null && resp.IsSuccess == true)
            {
                IEnumerable<ActivityDTO> activities = JsonConvert.DeserializeObject<List<ActivityDTO>>(resp.Contents.ToString()).Where(u => u.CategoryID == CategoryCreatePartialDTO.ID);
                foreach (var item in activities)
                {
                    await _service_activity.DeleteAsync<Response>(item.ID, HttpContext.Session.GetString("JWT-Token"));
                }
                Response response = await _service.DeleteAsync<Response>(CategoryCreatePartialDTO.ID, HttpContext.Session.GetString("JWT-Token"));


                if (response != null && response.IsSuccess == true)
                {
                    return RedirectToAction(nameof(IndexCategory));
                }
            }
            ModelState.AddModelError("DeleteError", resp.ErrorMessages.FirstOrDefault());
            return NoContent();
        }



    }
}
