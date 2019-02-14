using System.Collections.Generic;
using System.Linq;
using VoteIn.BL.Interfaces.Calculateurs;
using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Calculateurs
{
    public class CalculatorAlternativeVote : ICalculator
    {
        #region Public Methods
        /// <summary>
        /// Calculates the result.
        /// </summary>
        /// <param name="votingProcess">The voting process.</param>
        /// <returns></returns>
        public IResultatModel CalculateResult(VotingProcessModel votingProcess)
        {
            var resultat = new AlternativeVoteResultatModel
            {
                Stages = new List<AlternativeStageVote>()
            };

            var ballots = votingProcess.Ballots;

            var total = ballots.Count();
            resultat.Voters = total;
            var rejets = new List<int>();
            var victory = false;
            var conflict = false;

            while (!victory && !conflict && resultat.Stages.Count() < votingProcess.Options.Count())
            {
                var stage = new AlternativeStageVote { Scores = new List<AlternativeScoreVote>() };

                var preferences =
                    ballots
                    .Select(b => b.Acts.Where(a => rejets.All(r => r != a.IdOption)))
                    .Select(actes => actes.First(a => a.Value == actes.Min(acte => acte.Value)));
                foreach (var option in votingProcess.Options)
                {
                    var score = new AlternativeScoreVote
                    {
                        Option = option,
                        Votes = preferences.Count(a => a.IdOption == option.Id && a.Value != null)
                    };
                    stage.Scores.Add(score);

                    if (score.Votes > total / 2.0)
                    {
                        victory = true;
                        resultat.Winner = option;
                        resultat.IsValidResult = true;
                    }
                }

                if (!victory)
                {
                    var scoresNotRejected = stage.Scores.Where(s => rejets.All(r => r != s.Option.Id));
                    var reject = scoresNotRejected.First(s => s.Votes == scoresNotRejected.Min(sc => sc.Votes)).Option;

                    conflict = stage.Scores.Any(score => score.Votes == scoresNotRejected.Min(sc => sc.Votes) && score.Option.Id != reject.Id);

                    if (!conflict)
                    {
                        rejets.Add(reject.Id);
                        stage.RemovedOption = reject;
                    }
                }
                resultat.Stages.Add(stage);
            }

            return resultat;
        }
        #endregion
    }
}
