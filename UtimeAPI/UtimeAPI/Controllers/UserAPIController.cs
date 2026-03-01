using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UtimeAPI.Repository.IRepository;
using UtimeAPI.Models.DTO;
using UtimeAPI.Models;
using System.Net;
using System.Runtime.CompilerServices;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using UtimeAPI.Data;

namespace UtimeAPI.Controllers
{
    [ApiController]
    [Route("api/UserAuthAPI")]
    public class UserAPIController:Controller
    {
        public IUserRepository _repos;
        public Response api_response;
        public UserAPIController(IUserRepository repos) {
            _repos = repos;
            }
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            api_response = new();
            if (request == null)
            {
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(api_response);
            }
            var output = await _repos.LoginAsync(request);
            if (output.user == null || string.IsNullOrEmpty(output.Token))
            {
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("Incorrect login or password");
                return BadRequest(api_response);
            }
            else
            {
                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.OK;
                api_response.Contents = output;
                return Ok(api_response);
            }
        }
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            bool unique = _repos.IsUnique(request.UserName);
            api_response = new();
            if (!unique)
            {
                
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("Имя занято");

                return BadRequest(api_response);
            }
    
            var response = await _repos.RegisterAsync(request);
            api_response = new();
            if (response == null)
            {
     
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
            

                return BadRequest(api_response);
            }
            else if (response.UserName == null)
            {
                api_response.ErrorMessages = new();
                api_response.IsSuccess = false;
                api_response.StatusCode = HttpStatusCode.BadRequest;
                api_response.ErrorMessages.Add("Пароль должен содержать буквы, цифры и спец. символы!");

                return BadRequest(api_response);
            }
            else
            {

                api_response.IsSuccess = true;
                api_response.StatusCode = HttpStatusCode.OK;
                api_response.Contents = response;


                return Ok(api_response);
            }
        }
        //public async Task<IActionResult> IsAuthenticated(string username)
        //{
        //    bool res = await _repos.IsAuthenticatedAsync(username);
        //}
        
    }
}
