    using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GarageHub.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendInvoiceEmailAsync(string toEmail, string subject, string body);
    }
}
