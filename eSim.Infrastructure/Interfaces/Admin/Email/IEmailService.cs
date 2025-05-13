
using eSim.Infrastructure.DTOs.Email;
using eSim.Infrastructure.DTOs.Global;

namespace eSim.Infrastructure.Interfaces.Admin.Email
{
    public interface IEmailService
    {
        public Task<Result<string>> SendEmail(EmailDTO input);
    }
}
