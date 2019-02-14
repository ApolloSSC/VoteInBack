using System.Collections.Generic;
using VoteIn.Model.Models;

namespace VoteIn.Model.Business.ResultModels
{
    public class DuelCondorcet
    {
        /// <summary>
        /// Gets or sets the option1.
        /// </summary>
        /// <value>
        /// The option1.
        /// </value>
        public OptionsModel Option1 { get; set; }
        /// <summary>
        /// Gets or sets the option2.
        /// </summary>
        /// <value>
        /// The option2.
        /// </value>
        public OptionsModel Option2 { get; set; }
        /// <summary>
        /// Gets or sets the preferences option1.
        /// </summary>
        /// <value>
        /// The preferences option1.
        /// </value>
        public int PreferencesOption1 { get; set; }
        /// <summary>
        /// Gets or sets the preferences option2.
        /// </summary>
        /// <value>
        /// The preferences option2.
        /// </value>
        public int PreferencesOption2 { get; set; }
        /// <summary>
        /// Gets or sets the winner.
        /// </summary>
        /// <value>
        /// The winner.
        /// </value>
        public OptionsModel Winner { get; set; }
    }

    public class ScoreCondorcet
    {
        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        /// <value>
        /// The option.
        /// </value>
        public OptionsModel Option { get; set; }
        /// <summary>
        /// Gets or sets the score.
        /// </summary>
        /// <value>
        /// The score.
        /// </value>
        public int Score { get; set; }
    }

    public class ResultatCondorcet
    {
        /// <summary>
        /// Gets or sets the duels.
        /// </summary>
        /// <value>
        /// The duels.
        /// </value>
        public List<DuelCondorcet> Duels { get; set; }
        /// <summary>
        /// Gets or sets the scores.
        /// </summary>
        /// <value>
        /// The scores.
        /// </value>
        public List<ScoreCondorcet> Scores { get; set; }
        /// <summary>
        /// Gets or sets the winner.
        /// </summary>
        /// <value>
        /// The winner.
        /// </value>
        public OptionsModel Winner { get; set; }
        /// <summary>
        /// Gets or sets the voters.
        /// </summary>
        /// <value>
        /// The voters.
        /// </value>
        public int Voters { get; set; }
    }
}
