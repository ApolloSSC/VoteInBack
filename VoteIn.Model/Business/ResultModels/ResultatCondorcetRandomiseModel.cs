using System.Collections.Generic;

namespace VoteIn.Model.Business.ResultModels
{
    public class ResultatCondorcetRandomiseModel : IResultatModel
    {
        /// <summary>
        /// Gets or sets the resultat condorcet.
        /// </summary>
        /// <value>
        /// The resultat condorcet.
        /// </value>
        public ResultatCondorcet ResultatCondorcet { get; set; }
        /// <summary>
        /// Gets or sets the winning lottery.
        /// </summary>
        /// <value>
        /// The winning lottery.
        /// </value>
        public Loterie WinningLottery { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is valid result.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is valid result; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidResult { get ; set; }
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
        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public List<OptionsModel> Options { get; set; }
    }

    public class Loterie
    {
        /// <summary>
        /// Gets or sets the probabilites.
        /// </summary>
        /// <value>
        /// The probabilites.
        /// </value>
        public List<Probabilite> Probabilites { get; set; }
    }

    public class Probabilite
    {
        /// <summary>
        /// Gets or sets the option.
        /// </summary>
        /// <value>
        /// The option.
        /// </value>
        public OptionsModel Option { get; set; }
        /// <summary>
        /// Gets or sets the valeur.
        /// </summary>
        /// <value>
        /// The valeur.
        /// </value>
        public double Valeur { get; set; }
    }
}
