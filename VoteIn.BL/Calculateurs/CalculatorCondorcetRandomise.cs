using Google.OrTools.LinearSolver;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using VoteIn.BL.Interfaces.Calculateurs;
using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Calculateurs
{
    public class CalculatorCondorcetRandomise : ICalculator
    {
        #region Public Methods
        /// <summary>
        /// Calculates the result.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        public IResultatModel CalculateResult(VotingProcessModel scrutin)
        {

            var resultat = new ResultatCondorcetRandomiseModel();
            var resultatCondorcet = CalculCondorcet(scrutin);
            if (resultatCondorcet.Winner == null)
            {
                resultat.WinningLottery = linearSolverLoterie(resultatCondorcet.Duels, scrutin);
            }
            else
            {
                resultat.ResultatCondorcet = resultatCondorcet;
            }
            return resultat;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Calculs the condorcet.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        private ResultatCondorcet CalculCondorcet(VotingProcessModel scrutin)
        {
            var resultat = new ResultatCondorcet
            {
                Duels = new List<DuelCondorcet>()
            };

            var bulletins = scrutin.Ballots;

            var total = bulletins.Count();
            resultat.Voters = total;

            var n = scrutin.Options.Count;
            for (var i = 0; i < n; ++i)
            {
                var option1 = scrutin.Options.ElementAt(i);
                for (var j = 0; j < n; ++j)
                {
                    var option2 = scrutin.Options.ElementAt(j);
                    var duel = new DuelCondorcet
                    {
                        Option1 = option1,
                        Option2 = option2,
                        PreferencesOption1 = bulletins.Count(b => b.Acts.First(a => a.IdOption == option1.Id).Value < b.Acts.First(a => a.IdOption == option2.Id).Value)
                    };
                    duel.PreferencesOption2 = total - duel.PreferencesOption1;
                    if (duel.PreferencesOption1 > duel.PreferencesOption2)
                    {
                        duel.Winner = option1;
                    }
                    else if (duel.PreferencesOption2 > duel.PreferencesOption1)
                    {
                        duel.Winner = option2;
                    }
                    resultat.Duels.Add(duel);
                }
            }
            resultat.Scores = new List<ScoreCondorcet>();
            foreach (var option in scrutin.Options)
            {
                var score = new ScoreCondorcet
                {
                    Option = option,
                    Score = resultat.Duels.Count(d => d.Option1.Id == option.Id && d.Winner != null && d.Winner.Id == option.Id)
                };
                resultat.Scores.Add(score);
            }

            var max = scrutin.Options.Count;
            if (resultat.Scores.Count(s => s.Score == max) == 1)
            {
                resultat.Winner = resultat.Scores.First(s => s.Score == max).Option;
            }
            return resultat;
        }
        /// <summary>
        /// Linears the solver loterie.
        /// </summary>
        /// <param name="duels">The duels.</param>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        private Loterie linearSolverLoterie(IReadOnlyCollection<DuelCondorcet> duels, VotingProcessModel scrutin)
        {
            var solver = Solver.CreateSolver("LoterieSolver", "GLOP_LINEAR_PROGRAMMING");

            var probas = new Dictionary<int, Variable>();
            foreach (var option in scrutin.Options)
            {
                probas.Add(option.Id, solver.MakeNumVar(0, double.PositiveInfinity, option.Name));
            }

            var normalDistrib = new Normal(0, 1);
            var objective = solver.Objective();
            foreach (var proba in probas)
            {
                objective.SetCoefficient(proba.Value, normalDistrib.Sample());
            }

            var sommeP = solver.MakeConstraint(1, 1, "Sum = 1");
            foreach (var proba in probas)
            {
                sommeP.SetCoefficient(proba.Value, 1);
                var positif = solver.MakeConstraint(0, double.PositiveInfinity, proba.Value.Name() + " positive");

                foreach (var proba2 in probas)
                {
                    positif.SetCoefficient(proba2.Value, proba.Key == proba2.Key ? 1 : 0);
                }
            }

            foreach (var option in scrutin.Options)
            {
                var c = solver.MakeConstraint(0, double.PositiveInfinity);
                c.SetCoefficient(probas[option.Id], 0);
                foreach (var option2 in scrutin.Options)
                {
                    if (option.Id != option2.Id)
                    {
                        var duel = duels.First(d => d.Option1.Id == option.Id && d.Option2.Id == option2.Id);
                        var coeff = duel.PreferencesOption1 - duel.PreferencesOption2;
                        if (coeff != 0)
                        {
                            coeff /= Math.Abs(coeff);
                        }
                        c.SetCoefficient(probas[option2.Id], coeff);
                    }
                }
            }
            objective.SetMaximization();

            solver.Solve();

            var loterie = new Loterie { Probabilites = new List<Probabilite>() };
            foreach (var proba in probas)
            {
                loterie.Probabilites.Add(new Probabilite() { Option = scrutin.Options.First(o => o.Id == proba.Key), Valeur = proba.Value.SolutionValue() });
            }

            Console.Out.Write(solver.ExportModelAsLpFormat(false));

            return loterie;
        }
        #endregion
    }
}
