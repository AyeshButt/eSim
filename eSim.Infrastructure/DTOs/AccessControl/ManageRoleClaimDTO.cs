using System.ComponentModel.DataAnnotations;

namespace eSim.Infrastructure.DTOs.AccessControl
{
    public class ManageRoleClaimDTO
    {
        public string Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public int ParentId { get; set; }
        public string? Title { get; set; }
    }

    public class RoleClaimDTO
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Please select a value")]
        public string ClaimType { get; set; }

        [Required(ErrorMessage = "Please select a value")]
        public string ParentType { get; set; }

        [Required(ErrorMessage = "Please enter a unique value")]
        public string ClaimValue { get; set; }

    }


    public class SideMenuDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ClaimType { get; set; }
        public int? ParentId { get; set; }
    }

}
