using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoteIn.Model.Models
{
    [Table("SUFFRAGE")]
    public partial class Suffrage
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
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        [Column("CREATION_DATE")]
        public DateTime CreationDate { get; set; }
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
        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        /// <value>
        /// The act.
        /// </value>
        public virtual ICollection<Act> Act { get; set; }

    }
}
