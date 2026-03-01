using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Utime_WEB.Models;
using Utime_WEB.Services.IServices;
using Utime_WEB.Models.DTO;
using Utime_WEB.Models.ViewModels;

namespace Utime_WEB.Controllers
{
    public class UserAuthController:Controller
    {
        private IUserAuth _service;
        public UserAuthController(IUserAuth service)
        {
            _service = service;
        }
        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO request = new();
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO request)
        {
            var response1 = await _service.LoginAsync<Response>(request);
            if (response1 != null && response1.IsSuccess == true)
            {
                LoginResponseDTO response = JsonConvert.DeserializeObject<LoginResponseDTO>(response1.Contents.ToString());
                ClaimsIdentity identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal = new();
                var token = handler.ReadJwtToken(response.Token);
                identity.AddClaim(new Claim(ClaimTypes.Name, token.Claims.FirstOrDefault(u => u.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, token.Claims.FirstOrDefault(u => u.Type == "role").Value));
                principal.AddIdentity(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                HttpContext.Session.SetString("JWT-Token", response.Token);

                return RedirectToAction("IndexActivity", "Activity", new {regime="0"});

            }
            else
            {
                ModelState.AddModelError("CustomError", response1.ErrorMessages.FirstOrDefault());
                return View(request);
            }

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO request)
        {
            request.Role = "user";
            var response = await _service.RegisterAsync<Response>(request);
            if (response != null && response.IsSuccess == true)
            {
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.Clear();
                var taunt = ModelState;
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                return View(request);
            }
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWT-Token", "");
            return RedirectToAction("Index", "Home", new {regime="0"});
        }
        public IActionResult AccessDenied(int pagenum)
        {
            var vm = new AccessDeniedVM();
            vm.number = pagenum;
            return View(vm);
        }
        
    }
}
