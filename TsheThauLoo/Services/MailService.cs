using System;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
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

        public async Task SendLinkEmailAsync(MessageImportance importance, string name, string email, string subject, string body1, string link, string buttonText, string body2)
        {
            if (_environment.IsDevelopment())
            {
                _logger.LogInformation($"{name} - {email} - {link}");
                return;
            }
            
            var message = new MimeMessage();
            message.Importance = importance;
            message.From.Add(new MailboxAddress(_configuration["MailSettings:SenderName"], _configuration["MailSettings:SenderEmail"]));
            message.To.Add(new MailboxAddress(name, email));
            
            #region 信件內容

            var path = $"{_environment.WebRootPath}/email/email-template.html";
            var builder = new BodyBuilder();
            using (StreamReader streamReader = File.OpenText(path))
            {
                builder.HtmlBody = await streamReader.ReadToEndAsync();
            }

            if (_environment.IsDevelopment())
            {
                var messageBody = string.Format(builder.HtmlBody,
                    $"【開發測試信】{subject}", $"{body1}", $"{link}", $"{buttonText}", $"{body2}");
                message.Subject = $"【開發測試信】{subject}";
                message.Body = new TextPart(TextFormat.Html) { Text = messageBody };
            }
            else
            {
                var messageBody = string.Format(builder.HtmlBody, $"【成就人才發展系統】{subject}", $"{body1}", $"{link}", $"{buttonText}", $"{body2}");
                message.Subject = $"【成就人才發展系統】{subject}";
                message.Body = new TextPart(TextFormat.Html) { Text = messageBody };
            }

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