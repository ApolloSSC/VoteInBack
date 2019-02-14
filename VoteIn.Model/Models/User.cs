using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace VoteIn.Model.Models
{
    public class User : IdentityUser
    {
        /// <summary>
        /// Gets or sets the voting process.
        /// </summary>
        /// <value>
        /// The voting process.
        /// </value>
        public virtual ICollection<VotingProcess> VotingProcess { get; set; }
    }
}
