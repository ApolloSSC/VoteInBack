using System.ComponentModel.DataAnnotations.Schema;

namespace VoteIn.Model.Models
{
    [Table("RESULT")]
    public class Result
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
        /// Gets or sets the identifier voting process.
        /// </summary>
        /// <value>
        /// The identifier voting process.
        /// </value>
        [Column("ID_VOTING_PROCESS")]
        public int IdVotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the nb voters.
        /// </summary>
        /// <value>
        /// The nb voters.
        /// </value>
        [Column("NUMBER_VOTERS")]
        public int NbVoters { get; set; }
        /// <summary>
        /// Returns true if ... is valid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid; otherwise, <c>false</c>.
        /// </value>
        [Column("VALID")]
        public bool IsValid { get; set; }
        /// <summary>
        /// Gets or sets the identifier winning option.
        /// </summary>
        /// <value>
        /// The identifier winning option.
        /// </value>
        [Column("ID_WINNING_OPTION")]
        public int? IdWinningOption { get; set; }
        /// <summary>
        /// Gets or sets the score detail.
        /// </summary>
        /// <value>
        /// The score detail.
        /// </value>
        [Column("SCORES_DETAIL")]
        public string ScoreDetail { get; set; }
        /// <summary>
        /// Gets or sets the voting process.
        /// </summary>
        /// <value>
        /// The voting process.
        /// </value>
        [ForeignKey("IdVotingProcess")]
        public VotingProcess VotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the winning option.
        /// </summary>
        /// <value>
        /// The winning option.
        /// </value>
        [ForeignKey("IdWinningOption")]
        public Option WinningOption { get; set; }
    }
}
