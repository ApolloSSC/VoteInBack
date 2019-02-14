using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VoteIn.BL.Interfaces.Services;

namespace VoteIn.Mail
{
    public class EmailSenderService : IEmailSenderService
    {
        /// <summary>
        /// Gets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public SendGridOptions Options { get; } //set only via Secret Manager
        /// <summary>
        /// The client
        /// </summary>
        private SendGridClient _client;

        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailSenderService"/> class.
        /// </summary>
        /// <param name="optionsAccessor">The options accessor.</param>
        public EmailSenderService(IOptions<SendGridOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
            _client = new SendGridClient(Options.SendGridKey);
        }
        #endregion

        #region Public Methods
        public Task SendEmailAsync(string email, string subject, string message, string name = null)
        {
            return Execute(subject, message, email, name);
        }
        #endregion

        #region Private Methods
        private Task Execute(string subject, string message, string email, string name = null)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(Options.NoReplyMail, Options.NoReplyName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, name));
            return _client.SendEmailAsync(msg);
        }
        #endregion
    }
}
