using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoteIn.Model.Models
{
    [Table("CHOICE")]
    public class Choice
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Column("NAME")]
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Column("VALUE")]
        public int? Value { get; set; }
        /// <summary>
        /// Gets or sets the identifier voting process mode.
        /// </summary>
        /// <value>
        /// The identifier voting process mode.
        /// </value>
        [Column("ID_VOTING_PROCESS_MODE")]
        public int IdVotingProcessMode { get; set; }
        /// <summary>
        /// Gets or sets the voting process mode.
        /// </summary>
        /// <value>
        /// The voting process mode.
        /// </value>
        [ForeignKey("IdVotingProcessMode")]
        public VotingProcessMode VotingProcessMode { get; set; }
        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        /// <value>
        /// The act.
        /// </value>
        public virtual ICollection<Act> Act { get; set; }
    }
}
