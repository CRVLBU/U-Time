namespace Utime_WEB.Models.DTO
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public UserResponseDTO user { get; set; }
    }
}
