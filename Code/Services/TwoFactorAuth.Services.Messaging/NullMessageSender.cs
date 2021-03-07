namespace TwoFactorAuth.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using TwoFactorAuth.Services.Messaging.Contracts;

    public class NullMessageSender : IAppEmailSender
    {
        public Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null)
        {
            return Task.CompletedTask;
        }
    }
}
