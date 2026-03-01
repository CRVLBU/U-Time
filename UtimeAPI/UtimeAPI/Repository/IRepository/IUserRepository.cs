
using UtimeAPI.Models.DTO;

namespace UtimeAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        public bool IsUnique(string name);
        public Task<LoginResponseDTO> LoginAsync(LoginRequestDTO login_request);
        public Task<UserResponseDTO> RegisterAsync(RegisterRequestDTO register_request);
        //public Task<bool> IsAuthenticatedAsync(string username);
    }
}
