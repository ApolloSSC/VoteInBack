using System.Collections.Generic;
using Newtonsoft.Json;

namespace VoteIn.Model.Business
{
    public class OptionsModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsModel"/> class.
        /// </summary>
        public OptionsModel()
        {
            Suffrages = new List<SuffrageModel>();
        }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the suffrages.
        /// </summary>
        /// <value>
        /// The suffrages.
        /// </value>
        [JsonIgnore]
        public List<SuffrageModel> Suffrages { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is blank vote.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is blank vote; otherwise, <c>false</c>.
        /// </value>
        public bool IsBlankVote { get; set; }
    }
}