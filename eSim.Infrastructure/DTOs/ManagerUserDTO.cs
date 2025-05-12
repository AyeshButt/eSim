using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs
{
    public class ManagerUserDTO
    {
        
        public string? Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(dataType:DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [Required(ErrorMessage ="Role is required")]
        public string Role { get; set; }
    }
}
