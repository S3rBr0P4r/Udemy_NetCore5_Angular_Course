using System.ComponentModel.DataAnnotations;

namespace Udemy.NetCore5.Angular.Logic.DTOs
{
    public class RegisterUserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4)]
        public string Password { get; set; }
    }
}