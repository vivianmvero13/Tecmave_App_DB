using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Tecmave.Api.Services
{
    public class EmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _fromPassword;
        private readonly bool _useSsl;

        public EmailService(IConfiguration configuration)
        {
            _smtpServer = configuration["Smtp:Host"] ?? "smtp.gmail.com";
            _smtpPort = int.TryParse(configuration["Smtp:Port"], out var port) ? port : 587;
            _fromEmail = configuration["Smtp:From"] ?? configuration["Smtp:Username"];
            _fromPassword = configuration["Smtp:Password"] ?? string.Empty;
            _useSsl = bool.TryParse(configuration["Smtp:UseSsl"], out var ssl) ? ssl : true;
        }

        public async Task EnviarCorreo(string destino, string asunto, string cuerpo)
        {
            using (var cliente = new SmtpClient(_smtpServer, _smtpPort))
            {
                cliente.Credentials = new NetworkCredential(_fromEmail, _fromPassword);
                cliente.EnableSsl = _useSsl;

                var mail = new MailMessage(_fromEmail!, destino, asunto, cuerpo)
                {
                    IsBodyHtml = true
                };

                await cliente.SendMailAsync(mail);
            }
        }
    }
}
