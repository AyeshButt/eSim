
using eSim.Infrastructure.DTOs;

namespace eSim.Infrastructure.Interfaces.SystemClaimRepo
{
    public interface ISystemClaimService
    {
        Task<bool> ClaimExistsAsync(RoleClaimDTO input);
        Task<bool> AddClaimAsync(RoleClaimDTO input);
        Task<RoleClaimDTO?> GetClaimByIdAsync(string Id);
        Task<bool> EditClaimAsync(RoleClaimDTO input);
        Task<RoleClaimDTO> GetClaimAsync();
        IQueryable<ManageRoleClaimDTO> GetClaims();
        IQueryable<SideMenuDTO> GetSideMenus();
        IQueryable<SideMenuDTO> GetSubSideMenus(int id);
    }
}
