using eSim.EF.Context;
using eSim.Infrastructure.DTOs.AccessControl;
using eSim.Infrastructure.Interfaces.SystemClaimRepo;
using Microsoft.EntityFrameworkCore;


namespace eSim.Implementations.Services.SystemClaimRepo
{
    public class SystemClaimService : ISystemClaimService
    {

        private readonly ApplicationDbContext _db;

        public SystemClaimService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> AddClaimAsync(RoleClaimDTO input)
        {
            try
            {
                _db.SystemClaims.Add(new EF.Entities.SystemClaims
                {
                    Id = Guid.NewGuid().ToString(),
                    ParentId = Convert.ToInt32(input.ParentType),
                    Type = input.ClaimType,
                    Value = input.ClaimValue.ToLower(),

                });
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public Task<bool> ClaimExistsAsync(RoleClaimDTO input)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> EditClaimAsync(RoleClaimDTO input)
        {
            try
            {
                var claim = _db.SystemClaims.Find(input.Id);
                if (claim is null)
                {
                    return false;
                }
                claim.Value = input.ClaimValue.ToLower();
                claim.Type = input.ClaimType;
                claim.ParentId = Convert.ToInt32(input.ParentType);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                return false;
            }
            return true;
        }

        public Task<RoleClaimDTO> GetClaimAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<RoleClaimDTO?> GetClaimByIdAsync(string Id)
        {
            var claim = await _db.SystemClaims.FirstOrDefaultAsync(x => x.Id == Id);

            if (claim == null)
            {
                return null;
            }

            RoleClaimDTO output = new RoleClaimDTO()
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                ParentType = claim.ParentId.ToString(),
                Id = claim.Id
            };

            return output;

        }

        public IQueryable<ManageRoleClaimDTO> GetClaims()
        {
            var model = (from s1 in _db.SystemClaims join s2 in _db.SideMenu on s1.ParentId equals s2.Id
                         select new ManageRoleClaimDTO()
                         {
                             Id = s1.Id,
                             ParentId = s1.ParentId,
                             ClaimValue = s1.Value,
                             ClaimType = s1.Type,
                             Title = s2.Title
                         });

            //return _db.SystemClaims.Select(a => new ManageRoleClaimDTO { Id = a.Id, ClaimType = a.Type, ClaimValue = a.Value, ParentId = a.ParentId });
            return model;
        }

        public IQueryable<SideMenuDTO> GetSideMenus()
        {
            var output = _db.SideMenu.Select(a => new SideMenuDTO { Id = a.Id, Title = a.Title,ParentId = a.ParentId,ClaimType = a.ClaimType });

            return output;
        }
        public IQueryable<SideMenuDTO> GetSubSideMenus(int sideMenuId)
        {
            var output = _db.SideMenu.Where(u=>u.ParentId == sideMenuId).Select(a => new SideMenuDTO { Id = a.Id, Title = a.Title, ParentId = a.ParentId,ClaimType = a.ClaimType });

            return output;
        }
    }

}