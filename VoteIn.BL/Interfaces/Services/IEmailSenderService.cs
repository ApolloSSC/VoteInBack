using System.Threading.Tasks;

namespace VoteIn.BL.Interfaces.Services
{
    public interface IEmailSenderService
    {
        /// <summary>
        /// Sends the email asynchronous.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="message">The message.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message, string name = null);
    }
}
