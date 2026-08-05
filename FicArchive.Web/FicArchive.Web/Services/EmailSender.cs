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

        private Task SendAsync(string to, string subject, string html)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_config["Email:Login"], _config["Email:AppPassword"])
            };
            return client.SendMailAsync(_config["Email:Login"], to, subject, html);
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