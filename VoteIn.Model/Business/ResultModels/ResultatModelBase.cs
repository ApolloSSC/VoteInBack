using System.Collections.Generic;

namespace VoteIn.Model.Business.ResultModels
{
    public abstract class ResultatModelBase<TResultatIndividuel> : IResultatModel where TResultatIndividuel : ResultatIndividualModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultatModelBase{TResultatIndividuel}"/> class.
        /// </summary>
        public ResultatModelBase()
        {
            IndividualResults = new List<TResultatIndividuel>();
            Options = new List<OptionsModel>();
        }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
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
        /// <c>true</c> if this instance is valid result; otherwise, <c>false</c>.
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
        /// Gets or sets the individual results.
        /// </summary>
        /// <value>
        /// The individual results.
        /// </value>
        public List<TResultatIndividuel> IndividualResults { get; set; }
        /// <summary>
        /// Gets or sets the identifier new voting process.
        /// </summary>
        /// <value>
        /// The identifier new voting process.
        /// </value>
        public int? IdNewVotingProcess { get; set; }
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<OptionsModel> Options { get; set; }
    }
}