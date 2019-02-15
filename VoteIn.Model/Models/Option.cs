using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace VoteIn.Model.Models
{
    [Table("OPTION")]
    public class Option
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
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        [Column("COLOR")]
        public string Color { get; set; }
        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        [Column("PHOTO", TypeName = "varchar(MAX)")]
        public string Photo { get; set; }
        /// <summary>
        /// Gets or sets the voting process option.
        /// </summary>
        /// <value>
        /// The voting process option.
        /// </value>
        public virtual ICollection<VotingProcessOption> VotingProcessOption { get; set; }
    }
}
