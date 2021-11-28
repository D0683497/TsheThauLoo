using System.Collections.Generic;
using System.Threading.Tasks;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Services.Interface
{
    public interface IMailService
    {
        Task SendEmailConfirmAsync(string name, string email, string link, bool register);

        Task SendResetPasswordAsync(string name, string email, string link);

        Task SendActivityAttendeeAsync(string name, string email, string link, string title, AttendeeStatusType status);

        Task SendActivityDeleteAsync(string title, List<string> users);
    }
}