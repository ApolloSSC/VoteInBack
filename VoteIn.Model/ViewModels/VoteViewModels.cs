using System.Collections.Generic;
using VoteIn.Model.Models;

namespace VoteIn.Model.ViewModels
{
    public class Vote
    {
        /// <summary>
        /// Gets or sets the identifier voting process.
        /// </summary>
        /// <value>
        /// The identifier voting process.
        /// </value>
        public int IdVotingProcess { get; set; }
    }

    public class VoteMajoritaryVotingProcess : Vote
    {
        /// <summary>
        /// Gets or sets the identifier option.
        /// </summary>
        /// <value>
        /// The identifier option.
        /// </value>
        public int IdOption { get; set; }
    }

    public class OptionChoice
    {
        /// <summary>
        /// Gets or sets the identifier option.
        /// </summary>
        /// <value>
        /// The identifier option.
        /// </value>
        public int IdOption { get; set; }
        /// <summary>
        /// Gets or sets the identifier choice.
        /// </summary>
        /// <value>
        /// The identifier choice.
        /// </value>
        public int IdChoice { get; set; }
        /// <summary>
        /// Gets or sets the choice.
        /// </summary>
        /// <value>
        /// The choice.
        /// </value>
        public Choice Choice { get; set; }
    }

    public class VoteMajoritaryJudgment : Vote
    {
        /// <summary>
        /// Gets or sets the option choice.
        /// </summary>
        /// <value>
        /// The option choice.
        /// </value>
        public ICollection<OptionChoice> OptionChoice { get; set; }
    }

    public class VoteAlternative : Vote
    {
        /// <summary>
        /// Gets or sets the ranking.
        /// </summary>
        /// <value>
        /// The ranking.
        /// </value>
        public ICollection<Option> Ranking { get; set; }
    }
}
