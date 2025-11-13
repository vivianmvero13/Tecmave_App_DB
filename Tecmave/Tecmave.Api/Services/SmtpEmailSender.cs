using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System;

namespace Tecmave.Api.Services
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _cfg;
        public SmtpEmailSender(IConfiguration cfg) { _cfg = cfg; }

        public async Task SendAsync(string to, string subject, string bodyHtml)
        {
            var host = _cfg["Smtp:Host"];
            if (string.IsNullOrWhiteSpace(host))
                throw new InvalidOperationException("SMTP no configurado (Smtp:Host).");

            var port = int.TryParse(_cfg["Smtp:Port"], out var p) ? p : 587;
            var user = _cfg["Smtp:Username"];
            var pass = _cfg["Smtp:Password"];
            var from = _cfg["Smtp:From"] ?? user;
            var ssl  = bool.TryParse(_cfg["Smtp:UseSsl"], out var s) ? s : true;

            using var client = new SmtpClient(host, port)
            {
                EnableSsl = ssl,
                Credentials = string.IsNullOrWhiteSpace(user)
                    ? CredentialCache.DefaultNetworkCredentials
                    : new NetworkCredential(user, pass)
            };
            using var mail = new MailMessage(from!, to, subject, bodyHtml) { IsBodyHtml = true };
            await client.SendMailAsync(mail);
        }
    }
}
