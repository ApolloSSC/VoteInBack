using System.Collections.Generic;

namespace VoteIn.Model.Business.ResultModels
{
    public class AlternativeScoreVote
    {
        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        /// <value>
        /// The option.
        /// </value>
        public OptionsModel Option { get; set; }
        /// <summary>
        /// Gets or sets the votes.
        /// </summary>
        /// <value>
        /// The votes.
        /// </value>
        public int Votes { get; set; }
    }

    public class AlternativeStageVote
    {
        /// <summary>
        /// Gets or sets the removed option.
        /// </summary>
        /// <value>
        /// The removed option.
        /// </value>
        public OptionsModel RemovedOption { get; set; }
        /// <summary>
        /// Gets or sets the scores.
        /// </summary>
        /// <value>
        /// The scores.
        /// </value>
        public List<AlternativeScoreVote> Scores { get; set; }
    }

    public class AlternativeVoteResultatModel : IResultatModel
    {
        /// <summary>
        /// Gets or sets the stages.
        /// </summary>
        /// <value>
        /// The stages.
        /// </value>
        public List<AlternativeStageVote> Stages { get; set; }
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<OptionsModel> Options { get; set; }
        /// <summary>
        /// Gets or sets the voters.
        /// </summary>
        /// <value>
        /// The voters.
        /// </value>
        public int Voters { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid result.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid result; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidResult { get; set; }
        /// <summary>
        /// Gets or sets the winner.
        /// </summary>
        /// <value>
        /// The winner.
        /// </value>
        public OptionsModel Winner { get; set; }
        /// <summary>
        /// Gets or sets the identifier new voting process.
        /// </summary>
        /// <value>
        /// The identifier new voting process.
        /// </value>
        public int? IdNewVotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
    }
}
