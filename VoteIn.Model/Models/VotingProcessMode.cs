using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VoteIn.Model.Models
{
    [Table("VOTING_PROCESS_MODE")]
    public class VotingProcessMode
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
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        [Column("CODE")]
        public string Code { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="VotingProcessMode"/> is numeric.
        /// </summary>
        /// <value>
        ///   <c>true</c> if numeric; otherwise, <c>false</c>.
        /// </value>
        [Column("NUMERIC")]
        public bool Numeric { get; set; }
        /// <summary>
        /// Gets or sets the voting process.
        /// </summary>
        /// <value>
        /// The voting process.
        /// </value>
        [JsonIgnore]
        public virtual ICollection<VotingProcess> VotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the choice.
        /// </summary>
        /// <value>
        /// The choice.
        /// </value>
        public virtual ICollection<Choice> Choice { get; set; }
    }
}
