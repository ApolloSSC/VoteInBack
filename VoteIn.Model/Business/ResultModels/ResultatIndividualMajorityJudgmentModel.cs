using System.Collections.Generic;

namespace VoteIn.Model.Business.ResultModels
{
    public class ResultatIndividualMajorityJudgmentModel : ResultatIndividualModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResultatIndividualMajorityJudgmentModel"/> class.
        /// </summary>
        public ResultatIndividualMajorityJudgmentModel()
        {
            Scores = new List<ScoreModel>();
        }
        /// <summary>
        /// Gets or sets the scores.
        /// </summary>
        /// <value>
        /// The scores.
        /// </value>
        public List<ScoreModel> Scores { get; set; }
        /// <summary>
        /// Gets or sets the percentage score sup median.
        /// </summary>
        /// <value>
        /// The percentage score sup median.
        /// </value>
        public decimal PercentageScoreSupMedian { get; set; }
        /// <summary>
        /// Gets or sets the percentage score inf median.
        /// </summary>
        /// <value>
        /// The percentage score inf median.
        /// </value>
        public decimal PercentageScoreInfMedian { get; set; }
        /// <summary>
        /// Gets or sets the median.
        /// </summary>
        /// <value>
        /// The median.
        /// </value>
        public ChoiceModel Median { get; set; }
    }
}