using System.ComponentModel.DataAnnotations.Schema;

namespace VoteIn.Model.Models
{
    [Table("ACT")]
    public partial class Act
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
        /// Gets or sets the identifier choice.
        /// </summary>
        /// <value>
        /// The identifier choice.
        /// </value>
        [Column("ID_CHOICE")]
        public int? IdChoice { get; set; }
        /// <summary>
        /// Gets or sets the identifier voting process option.
        /// </summary>
        /// <value>
        /// The identifier voting process option.
        /// </value>
        [Column("ID_VOTING_PROCESS_OPTION")]
        public int? IdVotingProcessOption { get; set; }
        /// <summary>
        /// Gets or sets the identifier suffrage.
        /// </summary>
        /// <value>
        /// The identifier suffrage.
        /// </value>
        [Column("ID_SUFFRAGE")]
        public int IdSuffrage { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Column("VALUE")]
        public int? Value { get; set; }
        /// <summary>
        /// Gets or sets the choice.
        /// </summary>
        /// <value>
        /// The choice.
        /// </value>
        [ForeignKey("IdChoice")]
        public Choice Choice { get; set; }
        /// <summary>
        /// Gets or sets the voting process option.
        /// </summary>
        /// <value>
        /// The voting process option.
        /// </value>
        [ForeignKey("IdVotingProcessOption")]
        public VotingProcessOption VotingProcessOption { get; set; }
        /// <summary>
        /// Gets or sets the suffrage.
        /// </summary>
        /// <value>
        /// The suffrage.
        /// </value>
        [ForeignKey("IdSuffrage")]
        public Suffrage Suffrage { get; set; }
    }
}
