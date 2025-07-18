using Microsoft.AspNetCore.Identity;

namespace eSim.Infrastructure.DTOs.AccessControl
{
    public class UserDTO
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? Email  { get; set; }
    }


    public class UserTempDTO
    {
        public UserTempDTO()
        {
                ParentKeys = new List<string>();    
            
        }
        public List<string> ParentKeys { get; set; }
        public IQueryable<ApplicationUserDTOTemporay> Users { get; set; }
        public int UserType { get; set; }
        public string UserId { get; set; }
    }

    public class ApplicationUserDTOTemporay
    {

        public string  Username { get; set; }
        public string  Email { get; set; }

        public string? UserId { get; set; }
        public string? UserRoleId { get; set; }
        public string? ParentId { get; set; }
        public int UserType { get; set; }

    }
}
