using System.Threading.Tasks;
using TsheThauLoo.Dtos.Account.Login;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Services.Interface
{
    public interface IOAuthService
    {
        Task<NIDUserInfoDto> GetNIDUserInfoAsync(string userCode);

        Task<ApplicationUser> HandleNIDLoginAsync(NIDUserInfoDto info);
    }
}