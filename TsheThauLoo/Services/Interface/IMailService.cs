using System.Threading.Tasks;
using MimeKit;

namespace TsheThauLoo.Services.Interface
{
    public interface IMailService
    {
        Task SendLinkEmailAsync(MessageImportance importance, string name, string email, string subject, string body1, string link, string buttonText, string body2);
    }
}