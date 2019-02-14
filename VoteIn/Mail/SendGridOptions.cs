using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VoteIn.Mail
{
    public class SendGridOptions
    {
        /// <summary>
        /// Gets or sets the send grid user.
        /// </summary>
        /// <value>
        /// The send grid user.
        /// </value>
        public string SendGridUser { get; set; }
        /// <summary>
        /// Gets or sets the send grid key.
        /// </summary>
        /// <value>
        /// The send grid key.
        /// </value>
        public string SendGridKey { get; set; }
        /// <summary>
        /// Gets or sets the no reply mail.
        /// </summary>
        /// <value>
        /// The no reply mail.
        /// </value>
        public string NoReplyMail { get; set; }
        /// <summary>
        /// Gets or sets the name of the no reply.
        /// </summary>
        /// <value>
        /// The name of the no reply.
        /// </value>
        public string NoReplyName { get; set; }
    }
}
