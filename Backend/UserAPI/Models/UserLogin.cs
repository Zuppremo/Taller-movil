namespace UserAPI.Models
{
    public class UserLogin
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string LoginToken { get; set; } = string.Empty;
    }
}
