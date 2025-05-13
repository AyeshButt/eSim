using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs.Email
{
    public class EmailDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; } = string.Empty;
        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; } = string.Empty;

    }
}
