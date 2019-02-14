using System;
using System.Collections.Generic;
using System.Linq;
using VoteIn.BL.Exceptions;
using VoteIn.BL.Interfaces.Calculateurs;
using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Calculateurs
{
    public class CalculatorMajoritaryJudgment : ICalculator
    {
        #region Public Method
        /// <summary>
        /// Calculates the result.
        /// </summary>
        /// <param name="votingProcess">The voting process.</param>
        /// <returns></returns>
        /// <exception cref="VoteInException">Impossible to calculate the results : The voting process is null</exception>
        public IResultatModel CalculateResult(VotingProcessModel votingProcess)
        {
            if (votingProcess == null)
            {
                throw new VoteInException("Impossible to calculate the results : The voting process is null");
            }

            var resultat = new ResultatMajoritaryJudgmentModel();

            var nbSuffrage = 0m;
            foreach (var option in votingProcess.Options)
            {
                var individualResults = new ResultatIndividualMajorityJudgmentModel
                {
                    Option = option
                };
                var suffrageOrderedByAscending = option.Suffrages.OrderBy(s => s.Value).ToList();

                nbSuffrage = option.Suffrages.Count;
                
                var medianValue = GetMediane(suffrageOrderedByAscending, nbSuffrage);
                individualResults.Median = votingProcess.PossibleChoices.First(c => c.Value == medianValue);

                var pourcentageScoreInfMediane = (suffrageOrderedByAscending.Count(s => s.Value < medianValue) / nbSuffrage) * 100;
                individualResults.PercentageScoreInfMedian = Math.Round(pourcentageScoreInfMediane, 2);

                var pourcentageScoreSupMediane = (suffrageOrderedByAscending.Count(s => s.Value > medianValue) / nbSuffrage) * 100;
                individualResults.PercentageScoreSupMedian = Math.Round(pourcentageScoreSupMediane, 2);

                foreach (var choice in votingProcess.PossibleChoices)
                {
                    decimal votes = suffrageOrderedByAscending.Count(s => s.Value == choice.Value);

                    var percentage = (votes / nbSuffrage) * 100;
                    var score = new ScoreModel
                    {
                        Votes = suffrageOrderedByAscending.Count(s => s.Value == choice.Value),
                        Choices = choice,
                        Percentage = Math.Round(percentage, 2)
                    };

                    individualResults.Scores.Add(score);
                }

                resultat.IndividualResults.Add(individualResults);
            }

            resultat.IndividualResults = resultat.IndividualResults.OrderByDescending(r => r.Median.Value)
                .ThenBy(r => r.PercentageScoreInfMedian)
                .ThenByDescending(r => r.PercentageScoreSupMedian).ToList();

            var winner = resultat.IndividualResults.First();
            var perfectEquality = resultat.IndividualResults.Any(r => r.Median.Value == winner.Median.Value &&
                                                                          r.PercentageScoreInfMedian ==
                                                                          winner.PercentageScoreInfMedian &&
                                                                          r.PercentageScoreSupMedian ==
                                                                          winner.PercentageScoreSupMedian &&
                                                                          r != winner);
            resultat.Voters = (int)nbSuffrage;
            if (!perfectEquality)
            {
                resultat.Winner = winner.Option;
                resultat.IsValidResult = true;
            }

            return resultat;
        }
        #endregion

        #region Private Mehtods
        /// <summary>
        /// Gets the mediane.
        /// </summary>
        /// <param name="suffrageOrderedByAscending">The suffrage ordered by ascending.</param>
        /// <param name="nbSuffrage">The nb suffrage.</param>
        /// <returns></returns>
        private static int GetMediane(IReadOnlyCollection<SuffrageModel> suffrageOrderedByAscending, decimal nbSuffrage)
        {
            if (nbSuffrage % 2 == 0) //pair
            {
                var positionMediane = nbSuffrage / 2;

                var suffragePositionMediane = suffrageOrderedByAscending.ElementAt((int) positionMediane - 1);
                var suffragePositionMedianeSup = suffrageOrderedByAscending.ElementAt((int) positionMediane);

                var medianBrutValue = (suffragePositionMediane.Value + suffragePositionMedianeSup.Value) / 2m;
                return (int)Math.Floor(medianBrutValue);
            }
            else
            {
                var positionMediane = Math.Floor(nbSuffrage / 2);
                return suffrageOrderedByAscending.ElementAt((int) positionMediane).Value;
            }
        }
        #endregion
    }
}