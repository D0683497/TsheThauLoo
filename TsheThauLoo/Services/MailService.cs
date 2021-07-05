using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Text;
using TsheThauLoo.Services.Interface;

namespace TsheThauLoo.Services
{
    public class MailService : IMailService
    {
        private readonly ILogger<MailService> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public MailService(ILogger<MailService> logger, IWebHostEnvironment environment, IConfiguration configuration)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
        }

        public async Task SendEmailConfirmAsync(string name, string email, string link, bool register)
        {
            var message = new MimeMessage {Importance = MessageImportance.High};
            message.From.Add(new MailboxAddress(_configuration["MailSettings:SenderName"], _configuration["MailSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress(name, email));

            #region 信件內容

            var path = register ? $"{_environment.WebRootPath}/email/register.html" : $"{_environment.WebRootPath}/email/email-confirm.html";
            var builder = new BodyBuilder();
            using (StreamReader streamReader = File.OpenText(path))
            {
                builder.HtmlBody = await streamReader.ReadToEndAsync();
            }

            var messageBody = string.Format(builder.HtmlBody, link);
            message.Subject = "【成就人才發展系統】用戶電子郵件驗證";
            message.Body = new TextPart(TextFormat.Html) { Text = messageBody };

            #endregion
            
            #region 寄信

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_configuration["MailSettings:Server"], Convert.ToInt32(_configuration["MailSettings:Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            #endregion
        }
        
        public async Task SendResetPasswordAsync(string name, string email, string link)
        {
            var message = new MimeMessage {Importance = MessageImportance.High};
            message.From.Add(new MailboxAddress(_configuration["MailSettings:SenderName"], _configuration["MailSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress(name, email));

            #region 信件內容

            var path = $"{_environment.WebRootPath}/email/reset-password.html";
            var builder = new BodyBuilder();
            using (StreamReader streamReader = File.OpenText(path))
            {
                builder.HtmlBody = await streamReader.ReadToEndAsync();
            }

            var messageBody = string.Format(builder.HtmlBody, link);
            message.Subject = "【成就人才發展系統】用戶重設密碼";
            message.Body = new TextPart(TextFormat.Html) { Text = messageBody };

            #endregion
            
            #region 寄信

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_configuration["MailSettings:Server"], Convert.ToInt32(_configuration["MailSettings:Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_configuration["MailSettings:UserName"], _configuration["MailSettings:Password"]);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

            #endregion
        }
    }
}