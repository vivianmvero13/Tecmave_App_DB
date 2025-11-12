using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Tecmave.Api.Services
{
    public class EmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _fromEmail = "oyo.setiembre2004@gmail.com";
        private readonly string _fromPassword = "watxxefkwbletqgk";

        public async Task EnviarCorreo(string destino, string asunto, string cuerpo)
        {
            using (var cliente = new SmtpClient(_smtpServer, _smtpPort))
            {
                cliente.Credentials = new NetworkCredential(_fromEmail, _fromPassword);
                cliente.EnableSsl = true;

                var mail = new MailMessage(_fromEmail, destino, asunto, cuerpo)
                {
                    IsBodyHtml = true
                };

                await cliente.SendMailAsync(mail);
            }
        } 
    }
}
