using AutoMapper;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using UtimeAPI.Data;
using UtimeAPI.Models.DTO;
using UtimeAPI.Repository.IRepository;

namespace UtimeAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDBContext _db;
        private IMapper _mapper;
        private UserManager<ApplicationUser> _user_manager;
        private RoleManager<IdentityRole> _role_manager;
        private string secret;
        public UserRepository(ApplicationDBContext db, UserManager<ApplicationUser> user_manager, IConfiguration config,IMapper mapper,RoleManager<IdentityRole> role_identity)
        {
            _db = db;
            _user_manager = user_manager;
            _role_manager = role_identity;
            _mapper = mapper;
            secret = config.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUnique(string name)
        {
            var response = _db.Users.FirstOrDefault(u => u.UserName == name);
            if (response == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO login_request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == login_request.UserName);
            bool check = await _user_manager.CheckPasswordAsync(user, login_request.Password);
            if (!check)
            {
                return new LoginResponseDTO
                {
                    user = new UserResponseDTO(),
                    Token = ""
                };
            }
            var roles = await _user_manager.GetRolesAsync(user);
        JwtSecurityTokenHandler token_handler = new JwtSecurityTokenHandler();
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new(ClaimTypes.Role, roles.FirstOrDefault())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), SecurityAlgorithms.HmacSha256)

            };
            var token = token_handler.CreateToken(descriptor);
            return new LoginResponseDTO()
            {
                Token = token_handler.WriteToken(token),
                user = _mapper.Map<UserResponseDTO>(user)
            };
        }

        public async Task<UserResponseDTO> RegisterAsync(RegisterRequestDTO register_request)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Name = register_request.UserName,
                UserName = register_request.UserName
            };
            try
            {
                var result = await _user_manager.CreateAsync(user, register_request.Password);
                if (result.Succeeded) {
                    if ( !_role_manager.RoleExistsAsync(register_request.Role).GetAwaiter().GetResult())
                    {
                        await _role_manager.CreateAsync(new IdentityRole(register_request.Role));
                     

                    }
                    await _user_manager.AddToRoleAsync(user, register_request.Role);
                    var created_user =  await _db.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
                    return _mapper.Map<UserResponseDTO>(created_user);
                }
            }
            catch(Exception ex)
            {
               
            }
            return new UserResponseDTO();
        }
        //public async Task<bool> IsAuthenticatedAsync(string username)
        //{
        //    var use = User;
        //    var check = User.Identity.IsAuthenticated;


        //}
    }
}
