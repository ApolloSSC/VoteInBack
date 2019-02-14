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
    public class CalculateurScrutinMajoritaireShould
    {
        private CalculatorMajoritaryVotingProcess _calculateur;

        [TestInitialize]
        public void Init()
        {
            _calculateur = new CalculatorMajoritaryVotingProcess();
        }

        [TestMethod]
        public void ThrowExceptionNullScrutin()
        {
            Check.ThatCode(() => _calculateur.CalculateResult(null))
                .Throws<VoteInException>()
                .WithMessage("Impossible de calculer les résultats : le scrutin est null");
        }

        [TestMethod]
        public void ReturnOrdereredResults()
        {
            var scrutinBusiness = new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 07),
                Options = new List<OptionsModel>
                {
                    new OptionsModel
                    {
                        Id = 23,
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>
                        {
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
                        Id = 12,
                        Name = "Candidat 2",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 45,
                        Name = "Candidat 3",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    }
                }
            };

            var resultat = _calculateur.CalculateResult(scrutinBusiness) as ResultatMajorityVotingProcessModel;

            Check.That(resultat.IndividualResults).HasSize(3);

            var firstOption = resultat.IndividualResults.First();
            Check.That(firstOption.Option.Id).IsEqualTo(23);
            Check.That(firstOption.Percentage).IsEqualTo(50);

            var secondOption = resultat.IndividualResults.ElementAt(1);
            Check.That(secondOption.Option.Id).IsEqualTo(45);
            Check.That(secondOption.Percentage).IsEqualTo(100 / 3m);

            var thirdOption = resultat.IndividualResults.Last();
            Check.That(thirdOption.Option.Id).IsEqualTo(12);
            Check.That(thirdOption.Percentage).IsEqualTo(100 / 6m);
        }

        [TestMethod]
        public void IdentifyWinnerOnLatestTurn()
        {
            var scrutinBusiness = new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 07),
                IsLastRound = true,
                Options = new List<OptionsModel>
                {
                    new OptionsModel
                    {
                        Id = 23,
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 12,
                        Name = "Candidat 2",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    }
                }
            };

            var resultat = _calculateur.CalculateResult(scrutinBusiness) as ResultatMajorityVotingProcessModel;

            var gagant = resultat.IndividualResults.First();
            var perdant = resultat.IndividualResults.Last();

            Check.That(gagant.Option.Id).IsEqualTo(12);
            Check.That(gagant.Option).IsEqualTo(resultat.Winner);
            Check.That(perdant.Option).Not.IsEqualTo(resultat.Winner);
        }

        [TestMethod]
        public void EndedVoteInCaseOfEquality()
        {
            var scrutinBusiness = new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 07),
                Options = new List<OptionsModel>
                {
                    new OptionsModel
                    {
                        Id = 23,
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 12,
                        Name = "Candidat 2",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    }
                }
            };

            var resultat = _calculateur.CalculateResult(scrutinBusiness);

            Check.That(resultat.IsValidResult).IsFalse();
        }

        [TestMethod]
        public void EndedVoteWhenBlankVoteWins()
        {
            var scrutinBusiness = new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 07),
                IsLastRound = true,
                IsBlankVoteTakenIntoAccount = true,
                Options = new List<OptionsModel>
                {
                    new OptionsModel
                    {
                        Id = 23,
                        Name = "Vote blanc",
                        IsBlankVote = true,
                        Suffrages = new List<SuffrageModel>
                        {
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
                        Id = 12,
                        Name = "Candidat 2",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    }
                }
            };

            var resultat = _calculateur.CalculateResult(scrutinBusiness);

            Check.That(resultat.IsValidResult).IsFalse();
        }

        [TestMethod]
        public void IdentifyWinnerWhenBlankVoteIsQualifiedAmongTwoCandidates()
        {
            var scrutinBusiness = new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 07),
                IsLastRound = false,
                IsBlankVoteTakenIntoAccount = true,
                Options = new List<OptionsModel>
                {
                    new OptionsModel
                    {
                        Id = 23,
                        Name = "Vote blanc",
                        IsBlankVote = true,
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 12,
                        Name = "Candidat 2",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            },
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    }
                }
            };

            var resultat = _calculateur.CalculateResult(scrutinBusiness);

            Check.That(resultat.IsValidResult).IsTrue();
            Check.That(resultat.Winner.Id).IsEqualTo(12);
        }

        [TestMethod]
        public void EndedVoteWhenBlankVoteIsQualified()
        {
            var scrutinBusiness = new VotingProcessModel
            {
                ClosingDate = new DateTime(2017, 11, 07),
                IsLastRound = false,
                IsBlankVoteTakenIntoAccount = true,
                Options = new List<OptionsModel>
                {
                    new OptionsModel
                    {
                        Id = 23,
                        Name = "Vote blanc",
                        IsBlankVote = true,
                        Suffrages = new List<SuffrageModel>
                        {
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
                        Id = 65,
                        Name = "Candidat 1",
                        Suffrages = new List<SuffrageModel>
                        {
                            new SuffrageModel
                            {
                                Value = 1
                            }
                        }
                    },
                    new OptionsModel
                    {
                        Id = 12,
                        Name = "Candidat 2",
                        Suffrages = new List<SuffrageModel>
                        {
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
                    }
                }
            };

            var resultat = _calculateur.CalculateResult(scrutinBusiness);
            Check.That(resultat.IsValidResult).IsFalse();
        }
    }
}
