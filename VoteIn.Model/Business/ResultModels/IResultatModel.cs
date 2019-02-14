using System.Collections.Generic;

namespace VoteIn.Model.Business.ResultModels
{
    public interface IResultatModel
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid result.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is valid result; otherwise, <c>false</c>.
        /// </value>
        bool IsValidResult { get; set; }
        /// <summary>
        /// Gets or sets the winner.
        /// </summary>
        /// <value>
        /// The winner.
        /// </value>
        OptionsModel Winner { get; set; }
        /// <summary>
        /// Gets or sets the voters.
        /// </summary>
        /// <value>
        /// The voters.
        /// </value>
        int Voters { get; set; }
        /// <summary>
        /// Gets or sets the identifier new voting process.
        /// </summary>
        /// <value>
        /// The identifier new voting process.
        /// </value>
        int? IdNewVotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        int Id { get; set; }
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        List<OptionsModel> Options { get; set; }
    }
}