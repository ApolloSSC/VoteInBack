using System;

namespace VoteIn.BL.Exceptions
{
    public class VoteInException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VoteInException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public VoteInException(string message) : base(message)
        {
        }
    }
}
