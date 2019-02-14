namespace VoteIn.Model.Business.ResultModels
{
    public class ResultatMajorityVotingProcessModel : ResultatModelBase<ResultatIndividualMajorityVotingProcessModel>
    {
        /// <summary>
        /// Gets or sets the previous round resultat.
        /// </summary>
        /// <value>
        /// The previous round resultat.
        /// </value>
        public ResultatMajorityVotingProcessModel PreviousRoundResultat { get; set; }
    }
}
