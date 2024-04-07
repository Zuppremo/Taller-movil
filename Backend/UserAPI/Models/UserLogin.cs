namespace UserAPI.Models
{
    public class UserLogin : User
    {
        public string LoginToken { get; set; } = string.Empty;
    }
}
