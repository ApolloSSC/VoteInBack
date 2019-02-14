using System;
using System.Collections.Generic;

namespace VoteIn.Model.Business
{
    public class VotingProcessModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VotingProcessModel"/> class.
        /// </summary>
        public VotingProcessModel()
        {
            Options = new List<OptionsModel>();
            PossibleChoices = new List<ChoiceModel>();
            Ballots = new List<BallotModel>();
        }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the closing date.
        /// </summary>
        /// <value>
        /// The closing date.
        /// </value>
        public DateTime? ClosingDate { get; set; }
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<OptionsModel> Options { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is last round.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is last round; otherwise, <c>false</c>.
        /// </value>
        public bool IsLastRound { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is blank vote taken into account.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is blank vote taken into account; otherwise, <c>false</c>.
        /// </value>
        public bool IsBlankVoteTakenIntoAccount { get; set; }
        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>
        /// The mode.
        /// </value>
        public string Mode { get; set; }
        /// <summary>
        /// Gets or sets the possible choices.
        /// </summary>
        /// <value>
        /// The possible choices.
        /// </value>
        public List<ChoiceModel> PossibleChoices { get; set; }
        /// <summary>
        /// Gets or sets the ballots.
        /// </summary>
        /// <value>
        /// The ballots.
        /// </value>
        public List<BallotModel> Ballots { get; set; }
    }
}