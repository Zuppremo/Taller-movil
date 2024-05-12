using System.ComponentModel.DataAnnotations;

namespace UserAPI.Models
{
    public class RequestLogin
    {
        [Key]
        public int Id { get; set; }
        public string? Email { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
    }
}
