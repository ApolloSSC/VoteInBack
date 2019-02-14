using System.Linq;
using VoteIn.BL.Exceptions;
using VoteIn.BL.Interfaces.Calculateurs;
using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Calculateurs
{
    public class CalculatorMajoritaryVotingProcess : ICalculator
    {
        /// <summary>
        /// The choosen value
        /// </summary>
        private const int ChoosenValue = 1;

        #region Public Methods
        /// <summary>
        /// Calculates the result.
        /// </summary>
        /// <param name="votingProcess">The voting process.</param>
        /// <returns></returns>
        /// <exception cref="VoteInException">Impossible to calculate the results : The voting process is null</exception>
        public IResultatModel CalculateResult(VotingProcessModel votingProcess)
        {
            if (votingProcess == null)
                throw new VoteInException("Impossible to calculate the results : The voting process is null");

            decimal nbTotalVote = votingProcess.Options.SelectMany(o => o.Suffrages).Count(s => s.Value == ChoosenValue);

            var resultat = new ResultatMajorityVotingProcessModel
            {
                IndividualResults = votingProcess.Options
                .Select(o => CreateResultatIndividuel(nbTotalVote, o))
                .OrderByDescending(ri => ri.Votes)
                .ToList()
            };

            resultat.IsValidResult = IsResultatValide(resultat, votingProcess);

            var first = resultat.IndividualResults.FirstOrDefault();
            if (resultat.IsValidResult && first != null && (first.Percentage > 50m || votingProcess.IsLastRound))
            {
                resultat.Winner = first.Option;
            }

            return resultat;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the resultat individuel.
        /// </summary>
        /// <param name="nbTotalVote">The nb total vote.</param>
        /// <param name="option">The option.</param>
        /// <returns></returns>
        private static ResultatIndividualMajorityVotingProcessModel CreateResultatIndividuel(decimal nbTotalVote, OptionsModel option)
        {
            var nbVote = option.Suffrages.Count(s => s.Value == ChoosenValue);
            return new ResultatIndividualMajorityVotingProcessModel
            {
                Option = option,
                Votes = nbVote,
                Percentage = 100 * nbVote / nbTotalVote
            };
        }
        /// <summary>
        /// Determines whether [is resultat valide] [the specified resultat].
        /// </summary>
        /// <param name="resultat">The resultat.</param>
        /// <param name="votingProcess">The voting process.</param>
        /// <returns>
        ///   <c>true</c> if [is resultat valide] [the specified resultat]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsResultatValide(ResultatMajorityVotingProcessModel resultat, VotingProcessModel votingProcess)
        {
            var first = resultat.IndividualResults.FirstOrDefault();
            var second = resultat.IndividualResults.ElementAtOrDefault(1);
            var third = resultat.IndividualResults.ElementAtOrDefault(2);

            if (first == null || first.Option.IsBlankVote)
            {
                return false;
            }
            if ((resultat.IndividualResults.Count == 2 || votingProcess.IsLastRound) && first.Votes == second.Votes)
            {
                return false;
            }

            var isFirstRoundWithoutWinner = !votingProcess.IsLastRound && first.Percentage <= 50m;
            return !isFirstRoundWithoutWinner || (second?.Votes != third?.Votes && !second.Option.IsBlankVote);
        }
        #endregion
    }
}
