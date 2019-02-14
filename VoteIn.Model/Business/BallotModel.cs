using System.Collections.Generic;

namespace VoteIn.Model.Business
{
    public class BallotModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BallotModel"/> class.
        /// </summary>
        public BallotModel()
        {
            Acts = new List<ActModel>();
        }
        /// <summary>
        /// Gets or sets the acts.
        /// </summary>
        /// <value>
        /// The acts.
        /// </value>
        public List<ActModel> Acts { get; set; }
    }
}