using System.Collections.Generic;
using VoteIn.Model.Models;

namespace VoteIn.Model.Business.ResultModels
{
    public class ScoreBorda
    {
        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        /// <value>
        /// The option.
        /// </value>
        public Option Option { get; set; }
        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score { get; set; }
    }

    public class ResultatBorda
    {
        /// <summary>
        /// Gets or sets the scores.
        /// </summary>
        /// <value>
        /// The scores.
        /// </value>
        public List<ScoreBorda> Scores { get; set; }
        /// <summary>
        /// Gets or sets the votants.
        /// </summary>
        /// <value>
        /// The votants.
        /// </value>
        public int Votants { get; set; }
    }
}
