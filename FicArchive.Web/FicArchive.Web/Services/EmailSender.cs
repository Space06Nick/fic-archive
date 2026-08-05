using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace FicArchive.Web.Services
{
    public class EmailSender : IEmailSender, IEmailSender<IdentityUser>
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        private async Task SendAsync(string to, string subject, string html)
        {
            var login = _config["Email:Login"];
            var pass = _config["Email:AppPassword"];

            Console.WriteLine($"[EmailSender] from={login}, pass length={pass?.Length}, to={to}");

            try
            {
                using var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(login, pass),
                    Timeout = 15000
                };

                var message = new MailMessage
                {
                    From = new MailAddress(login!),
                    Subject = subject,
                    Body = html,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
                Console.WriteLine("[EmailSender] OK: SMTP принял письмо без ошибок");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailSender] FAIL: {ex.GetType().Name}: {ex.Message}");
                throw;
            }
        }

        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
            => SendAsync(email, subject, htmlMessage);

        public Task SendConfirmationLinkAsync(IdentityUser user, string email, string confirmationLink)
            => SendAsync(email, "Confirm your FicArchive account",
                $"Welcome to FicArchive! Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

        public Task SendPasswordResetLinkAsync(IdentityUser user, string email, string resetLink)
            => SendAsync(email, "Reset your FicArchive password",
                $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

        public Task SendPasswordResetCodeAsync(IdentityUser user, string email, string resetCode)
            => SendAsync(email, "Your FicArchive reset code",
                $"Your password reset code is: <b>{resetCode}</b>");
    }
}