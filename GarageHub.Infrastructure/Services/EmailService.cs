using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using GarageHub.Application.Interfaces;

namespace GarageHub.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendInvoiceEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("smartsikchya.noreply@gmail.com", "kgxsabghpcnnfdxo"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("smartisckhya.noreply@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(toEmail);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
