using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using VoteIn.BL.Calculateurs;
using VoteIn.BL.Exceptions;
using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;

namespace VoteIn.BL.Tests.Calculateurs
{
    [TestClass]
    public class CalculateurJugementMajoritaireShould
    {
        private CalculatorMajoritaryJudgment _calculateur;

        [TestInitialize]
        public void Init()
        {
            _calculateur = new CalculatorMajoritaryJudgment();
        }

        [TestMethod]
        public void ThrowExceptionNullScrutin()
        {
            Check.ThatCode(() => _calculateur.CalculateResult(null))
                .Throws<VoteInException>()
                .WithMessage("Impossible de calculer les résultats : le scrutin est null");
        }

        [TestMethod]
        public void VerifierScoresPourChaqueOption()
        {
            var scrutin = new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 8),
                PossibleChoices = GetChoice(),
                Options = new List<OptionsModel>()
                {
                    new OptionsModel
                    {
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 5
                            },
                            new SuffrageModel
                            {
                                Value = 5
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 3
                            },
                            new SuffrageModel
                            {
                                Value = 3
                            },
                            new SuffrageModel
                            {
                                Value = 3
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    }
                },
                IsLastRound = true
            };
            
            var result = _calculateur.CalculateResult(scrutin) as ResultatMajoritaryJudgmentModel;
            Check.That(result.IndividualResults).IsNotNull();
            Check.That(result.Voters).IsEqualTo(8);

            var gagnant = result.IndividualResults.First();
            var scoreGagnant = gagnant.Scores;

            Check.That(scoreGagnant.First(s => s.Choices.Id == 1).Votes).IsEqualTo(2);
            Check.That(scoreGagnant.First(s => s.Choices.Id == 1).Percentage).IsEqualTo(25m);
            Check.That(scoreGagnant.First(s => s.Choices.Id == 2).Votes).IsEqualTo(1);
            Check.That(scoreGagnant.First(s => s.Choices.Id == 3).Votes).IsEqualTo(3);
            Check.That(scoreGagnant.First(s => s.Choices.Id == 4).Votes).IsEqualTo(1);
            Check.That(scoreGagnant.First(s => s.Choices.Id == 5).Votes).IsEqualTo(0);
            Check.That(scoreGagnant.First(s => s.Choices.Id == 6).Votes).IsEqualTo(1);
            Check.That(scoreGagnant.First(s => s.Choices.Id == 7).Votes).IsEqualTo(0);

            Check.That(result.IsValidResult).IsTrue();
        }

        [TestMethod]
        public void VerifierMedianePourChaqueCandidat()
        {
            var scrutin = GetScrutin();

            var result = _calculateur.CalculateResult(scrutin) as ResultatMajoritaryJudgmentModel;
            Check.That(result.Voters).IsEqualTo(3);
            Check.That(result.IndividualResults).IsNotNull();
            Check.That(result.IndividualResults.First(r => r.Option.Id == 1).Median.Value).IsEqualTo(4);
            Check.That(result.IndividualResults.First(r => r.Option.Id == 3).Median.Value).IsEqualTo(3);
            Check.That(result.IndividualResults.First(r => r.Option.Id == 2).Median.Value).IsEqualTo(2);
            Check.That(result.IsValidResult).IsTrue();
        }

        [TestMethod]
        public void VerifierMedianePourChaqueCandidatAvecNombreSuffragePair()
        {
            var scrutin = GetScrutinAvecSuffragePair();

            var result = _calculateur.CalculateResult(scrutin) as ResultatMajoritaryJudgmentModel;
            Check.That(result.Voters).IsEqualTo(6);
            Check.That(result.IndividualResults).IsNotNull();
            Check.That(result.IndividualResults.First(r => r.Option.Id == 1).Median.Value).IsEqualTo(4);
            Check.That(result.IndividualResults.First(r => r.Option.Id == 3).Median.Value).IsEqualTo(4);
            Check.That(result.IndividualResults.First(r => r.Option.Id == 2).Median.Value).IsEqualTo(1);
            Check.That(result.IsValidResult).IsTrue();
        }

        [TestMethod]
        public void IdentifierCandidatVainqueur()
        {
            var scrutin = GetScrutin();
            
            var result = _calculateur.CalculateResult(scrutin) as ResultatMajoritaryJudgmentModel;
            Check.That(result.Voters).IsEqualTo(3);
            Check.That(result.IndividualResults).IsNotNull();
            Check.That(result.IsValidResult).IsTrue();
            Check.That(result.Winner.Name).Equals("Candidat 1");
        }

        [TestMethod]
        public void IdentifierCandidatVainqueurAvecEgalite()
        {
            var scrutin = GetScrutinAvecSuffragePair();

            var result = _calculateur.CalculateResult(scrutin) as ResultatMajoritaryJudgmentModel;
            Check.That(result.Voters).IsEqualTo(6);
            Check.That(result.IndividualResults).IsNotNull();
            
            var gagnant = result.IndividualResults.First(r => r.Option.Id == 1);
            Check.That(gagnant.Median.Value).IsEqualTo(4);
            Check.That(gagnant.Option.Name).IsEqualTo("Candidat 1");
            Check.That(gagnant.Option).IsEqualTo(result.Winner);
            Check.That(gagnant.PercentageScoreInfMedian).IsEqualTo(16.67m);
            Check.That(gagnant.PercentageScoreSupMedian).IsEqualTo(50m);

            var second = result.IndividualResults.First(r => r.Option.Id == 3);
            Check.That(second.Median.Value).IsEqualTo(4);
            Check.That(second.Option.Name).IsEqualTo("Candidat 3");
            Check.That(second.Option).Not.IsEqualTo(result.Winner);
            Check.That(second.PercentageScoreInfMedian).IsEqualTo(33.33m);
            Check.That(second.PercentageScoreSupMedian).IsEqualTo(33.33m);


            var dernier = result.IndividualResults.First(r => r.Option.Id == 2);
            Check.That(dernier.Median.Value).IsEqualTo(1);
            Check.That(dernier.Option.Name).IsEqualTo("Candidat 2");
            Check.That(dernier.Option).Not.IsEqualTo(result.Winner);
            Check.That(dernier.PercentageScoreInfMedian).IsEqualTo(0);
            Check.That(dernier.PercentageScoreSupMedian).IsEqualTo(50m);

            Check.That(result.IsValidResult).IsTrue();
        }

        [TestMethod]
        public void IdentifierCandidatVainqueurAvecEgalitePourcentageInf()
        {
            var scrutin = GetScrutinAvecSuffragePairAvecEgalitePourcentageInf();

            var result = _calculateur.CalculateResult(scrutin) as ResultatMajoritaryJudgmentModel;
            Check.That(result.Voters).IsEqualTo(6);
            Check.That(result.IndividualResults).IsNotNull();

            var gagnant = result.IndividualResults.First(r => r.Option.Id == 1);
            Check.That(gagnant.Median.Value).IsEqualTo(4);
            Check.That(gagnant.Option.Name).IsEqualTo("Candidat 1");
            Check.That(gagnant.Option).IsEqualTo(result.Winner);
            Check.That(gagnant.PercentageScoreInfMedian).IsEqualTo(16.67m);
            Check.That(gagnant.PercentageScoreSupMedian).IsEqualTo(50m);

            var second = result.IndividualResults.First(r => r.Option.Id == 3);
            Check.That(second.Median.Value).IsEqualTo(4);
            Check.That(second.Option.Name).IsEqualTo("Candidat 3");
            Check.That(second.Option).Not.IsEqualTo(result.Winner);
            Check.That(second.PercentageScoreInfMedian).IsEqualTo(16.67m);
            Check.That(second.PercentageScoreSupMedian).IsEqualTo(33.33m);
            
            Check.That(result.IsValidResult).IsTrue();
        }

        [TestMethod]
        public void NePasIdentifierCandidatVainqueurAvecEgaliteParfaite()
        {
            var scrutin = GetScrutinAvecSuffragePairAvecEgaliteParfaite();

            var result = _calculateur.CalculateResult(scrutin) as ResultatMajoritaryJudgmentModel;
            Check.That(result.Voters).IsEqualTo(6);
            Check.That(result.IndividualResults).IsNotNull();
            Check.That(result.Winner).IsNull();

            var gagnant = result.IndividualResults.First(r => r.Option.Id == 1);
            Check.That(gagnant.Median.Value).IsEqualTo(4);
            Check.That(gagnant.Option.Name).IsEqualTo("Candidat 1");
            Check.That(gagnant.PercentageScoreInfMedian).IsEqualTo(16.67m);
            Check.That(gagnant.PercentageScoreSupMedian).IsEqualTo(50m);

            var second = result.IndividualResults.First(r => r.Option.Id == 3);
            Check.That(second.Median.Value).IsEqualTo(4);
            Check.That(second.Option.Name).IsEqualTo("Candidat 3");
            Check.That(second.PercentageScoreInfMedian).IsEqualTo(16.67m);
            Check.That(second.PercentageScoreSupMedian).IsEqualTo(50m);

            Check.That(result.IsValidResult).IsFalse();
        }

        private static VotingProcessModel GetScrutin()
        {
            return new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 8),
                PossibleChoices = GetChoice(),
                Options = new List<OptionsModel>()
                {
                    new OptionsModel
                    {
                        Id = 1,
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 2,
                        Name = "Candidat 2",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 2
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 3,
                        Name = "Candidat 3",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 3
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    }
                },
                IsLastRound = true
            };
        }

        private static VotingProcessModel GetScrutinAvecSuffragePair()
        {
            return new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 8),
                PossibleChoices = GetChoice(),
                Options = new List<OptionsModel>()
                {
                    new OptionsModel
                    {
                        Id = 1,
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 5
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 2,
                        Name = "Candidat 2",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 3
                            },
                            new SuffrageModel
                            {
                                Value = 2
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 3,
                        Name = "Candidat 3",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 3
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 5
                            }
                        }
                    }
                },
                IsLastRound = true
            };
        }

        private static VotingProcessModel GetScrutinAvecSuffragePairAvecEgalitePourcentageInf()
        {
            return new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 8),
                PossibleChoices = GetChoice(),
                Options = new List<OptionsModel>()
                {
                    new OptionsModel
                    {
                        Id = 1,
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 5
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 3,
                        Name = "Candidat 3",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 5
                            }
                        }
                    }
                },
                IsLastRound = true
            };
        }

        private static VotingProcessModel GetScrutinAvecSuffragePairAvecEgaliteParfaite()
        {
            return new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 8),
                PossibleChoices = GetChoice(),
                Options = new List<OptionsModel>()
                {
                    new OptionsModel
                    {
                        Id = 1,
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 5
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 3,
                        Name = "Candidat 3",
                        Suffrages = new List<SuffrageModel>()
                        {
                            new SuffrageModel
                            {
                                Value = 5
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 4
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 6
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    }
                },
                IsLastRound = true
            };
        }

        private static List<ChoiceModel> GetChoice()
        {
            return new List<ChoiceModel>()
                {
                    new ChoiceModel
                    {
                        Id = 1,
                        Value = 5,
                        Name = "Très bien"
                    },
                    new ChoiceModel
                    {
                        Id = 2,
                        Value = 4,
                        Name = "Bien"
                    },
                    new ChoiceModel
                    {
                        Id = 3,
                        Value = 3,
                        Name = "Assez Bien"
                    },
                    new ChoiceModel
                    {
                        Id = 4,
                        Value = 1,
                        Name = "Passable"
                    },
                    new ChoiceModel
                    {
                        Id = 5,
                        Value = 0,
                        Name = "À rejeter"
                    },
                    new ChoiceModel
                    {
                        Id = 6,
                        Value = 6,
                        Name = "Excellent"
                    },
                    new ChoiceModel
                    {
                        Id = 7,
                        Value = 2,
                        Name = "Correct"
                    }
                };
        }
    }
}
