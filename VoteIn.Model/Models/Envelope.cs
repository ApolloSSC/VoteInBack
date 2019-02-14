using System.ComponentModel.DataAnnotations.Schema;

namespace VoteIn.Model.Models
{
    [Table("ENVELOPE")]
    public class Envelope
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
        /// Gets or sets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        [Column("KEY")]
        public string Key { get; set; }
        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
        /// </value>
        [Column("CONTENT")]
        public string Content { get; set; }
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
