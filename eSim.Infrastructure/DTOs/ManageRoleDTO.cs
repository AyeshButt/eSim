using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs
{
    public class ManageRoleDTO
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Please provide unique role name")]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }
    }

    public class RoleDTO
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [DisplayName("Role Name")]
        public string? RoleName { get; set; }
        public List<List<ClaimItems>> Claims { get; set; } = new();
    }
    public class ClaimItems
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public bool isSelected { get; set; }
        public string? Title { get; set; }
    }




}
