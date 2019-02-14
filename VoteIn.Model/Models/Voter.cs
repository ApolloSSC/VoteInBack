using System.ComponentModel.DataAnnotations.Schema;

namespace VoteIn.Model.Models
{
    [Table("VOTER")]
    public class Voter
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [Column("ID")]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the mail.
        /// </summary>
        /// <value>
        /// The mail.
        /// </value>
        [Column("MAIL")]
        public string Mail { get; set; }
        /// <summary>
        /// Converts to ken.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        [Column("TOKEN")]
        public string Token { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has voted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has voted; otherwise, <c>false</c>.
        /// </value>
        [Column("HAS_VOTED")]
        public bool HasVoted { get; set; }
        /// <summary>
        /// Gets or sets the identifier voting process.
        /// </summary>
        /// <value>
        /// The identifier voting process.
        /// </value>
        [Column("ID_VOTING_PROCESS")]
        public int IdVotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the voting process.
        /// </summary>
        /// <value>
        /// The voting process.
        /// </value>
        [ForeignKey("IdVotingProcess")]
        public VotingProcess VotingProcess { get; set; }

        
    }
}
