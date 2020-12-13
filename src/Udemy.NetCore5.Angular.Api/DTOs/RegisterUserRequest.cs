using System.ComponentModel.DataAnnotations;

namespace Udemy.NetCore5.Angular.Api.DTOs
{
    public class RegisterUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}