using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoteIn.Model.Models
{
    [Table("VOTING_PROCESS_OPTION")]
    public class VotingProcessOption
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
        /// Gets or sets the identifier option.
        /// </summary>
        /// <value>
        /// The identifier option.
        /// </value>
        [Column("ID_OPTION")]
        public int IdOption { get; set; }
        /// <summary>
        /// Gets or sets the voting process.
        /// </summary>
        /// <value>
        /// The voting process.
        /// </value>
        [ForeignKey("IdVotingProcess")]
        [JsonIgnore]
        public VotingProcess VotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        /// <value>
        /// The option.
        /// </value>
        [ForeignKey("IdOption")]
        public Option Option { get; set; }
        /// <summary>
        /// Gets or sets the act.
        /// </summary>
        /// <value>
        /// The act.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<Act> Act { get; set; }
    }
}
