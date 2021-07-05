using System.Threading.Tasks;

namespace TsheThauLoo.Services.Interface
{
    public interface IMailService
    {
        Task SendEmailConfirmAsync(string name, string email, string link, bool register);

        Task SendResetPasswordAsync(string name, string email, string link);
    }
}