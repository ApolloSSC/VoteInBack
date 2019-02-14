namespace VoteIn.Model.Business.ResultModels
{
    public class ResultatIndividualMajorityVotingProcessModel : ResultatIndividualModelBase
    {
        /// <summary>
        /// Gets or sets the votes.
        /// </summary>
        /// <value>
        /// The votes.
        /// </value>
        public int Votes { get; set; }
        /// <summary>
        /// Gets or sets the percentage.
        /// </summary>
        /// <value>
        /// The percentage.
        /// </value>
        public decimal Percentage { get; set; }
    }
}
