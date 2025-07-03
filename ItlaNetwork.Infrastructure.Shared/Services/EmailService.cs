using ItlaNetwork.Core.Application.DTOs.Email;
using ItlaNetwork.Core.Application.Interfaces.Services;
using ItlaNetwork.Core.Domain.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace ItlaNetwork.Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        // Inyectamos la configuración de MailSettings usando IOptions
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
            // --- VERIFICACIÓN DE NULOS AÑADIDA ---
            if (string.IsNullOrEmpty(request.To) || string.IsNullOrEmpty(_mailSettings.EmailFrom))
            {
                // Esto previene el error "Value cannot be null"
                throw new System.ArgumentNullException("La dirección de correo del destinatario o del remitente no puede ser nula.");
            }

            try
            {
                var email = new MimeMessage();
                email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.EmailFrom);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;

                var builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (System.Exception ex)
            {
                // En un futuro, aquí podrías registrar el error en un log.
                // Por ahora, lo relanzamos para que se vea en la consola de depuración.
                throw;
            }
        }
    }
}