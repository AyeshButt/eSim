using Microsoft.AspNetCore.Identity;

namespace eSim.EF.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? UserRoleId { get; set; }
        public string? ParentId { get; set; }

    }
}
