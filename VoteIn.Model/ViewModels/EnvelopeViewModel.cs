
using VoteIn.Model.Models;

namespace VoteIn.Model.ViewModels
{
    public class EnvelopeViewModel
    {
        /// <summary>
        /// Gets or sets the identifier voter.
        /// </summary>
        /// <value>
        /// The identifier voter.
        /// </value>
        public int IdVoter { get; set; }
        /// <summary>
        /// Converts to ken.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public string Token { get; set; }
        /// <summary>
        /// Gets or sets the envelope.
        /// </summary>
        /// <value>
        /// The envelope.
        /// </value>
        public Envelope Envelope { get; set; }
    }
}
