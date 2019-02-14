using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;
using VoteIn.BL.Exceptions;
using VoteIn.BL.Mapper;
using VoteIn.Model.Models;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Business;
using Newtonsoft.Json;

namespace VoteIn.BL.Tests.Mapper
{
    [TestClass]
    public class MapperServiceShould
    {
        private MapperService _mapper;

        [TestInitialize]
        public void Init()
        {
            _mapper = new MapperService();
        }

        [TestMethod]
        public void MapScrutinToScrutinModel()
        {
            var actes = new List<Act>
            {
                new Act
                {
                    IdSuffrage = 15,
                    IdVotingProcessOption = 3,
                    IdChoice = 20,
                    Value = 1
                }
            };
            var choice = new List<Choice>
            {
                new Choice
                {
                    Id = 20,
                    Name = "Choisi",
                    Value = 1
                }
            };
            var option = new Option
            {
                Id = 10,
                Name = "Candidat 1"
            };
            var scrutin = new VotingProcess
            {
                Id = 3,
                ClosingDate = new DateTime(2017, 11, 09),
                IdPreviousVotingProcess = 1,
                VotingProcessMode = new VotingProcessMode
                {
                    Choice = choice,
                    Code = "scrutin-majoritaire"
                },
                VotingProcessOption = new List<VotingProcessOption>()
                {
                    new VotingProcessOption()
                    {
                        Id = 3,
                        Option = option,
                        Act = actes
                    }
                }
            };

            var model = _mapper.MapScrutinToScrutinModel(scrutin);

            Check.That(model.Id).IsEqualTo(scrutin.Id);
            Check.That(model.ClosingDate).IsEqualTo(scrutin.ClosingDate);
            Check.That(model.IsLastRound).IsTrue();
            Check.That(model.IsBlankVoteTakenIntoAccount).IsFalse();

            //Choice
            Check.That(model.PossibleChoices).HasSize(choice.Count);
            var choiceModel = model.PossibleChoices.First();
            var choiceBdd = choice.First();
            Check.That(choiceModel.Id).IsEqualTo(choiceBdd.Id);
            Check.That(choiceModel.Name).IsEqualTo(choiceBdd.Name);
            Check.That(choiceModel.Value).IsEqualTo(choiceBdd.Value);

            //Option
            Check.That(model.Options).HasSize(1);
            var optionModel = model.Options.First();
            Check.That(optionModel.Id).IsEqualTo(option.Id);
            Check.That(optionModel.Name).IsEqualTo(option.Name);
            Check.That(optionModel.IsBlankVote).IsFalse();

            //Suffrage
            var suffragesModel = model.Options.SelectMany(o => o.Suffrages).ToList();
            Check.That(suffragesModel).HasSize(actes.Count);
            Check.That(suffragesModel.First().Value).IsEqualTo(actes.First().Value);
        }

        [TestMethod]
        public void MapScrutinToScrutinModelPasDernierTour()
        {
            var scrutin = new VotingProcess();

            var model = _mapper.MapScrutinToScrutinModel(scrutin);

            Check.That(model.IsLastRound).IsFalse();
        }

        [TestMethod]
        public void MapScrutinToScrutinModelActeSansValeur()
        {
            var actes = new List<Act>
            {
                new Act
                {
                    IdVotingProcessOption = 3,
                    IdChoice = 20,
                }
            };
            var choice = new List<Choice>
            {
                new Choice
                {
                    Id = 20,
                    Name = "Choisi",
                    Value = 1
                }
            };
            var option = new Option
            {
                Id = 10,
                Name = "Candidat 1"
            };
            var scrutin = new VotingProcess
            {
                ClosingDate = new DateTime(2017, 11, 09),
                IdPreviousVotingProcess = 1,
                VotingProcessMode = new VotingProcessMode
                {
                    Choice = choice
                },
                VotingProcessOption = new List<VotingProcessOption>()
                {
                    new VotingProcessOption()
                    {
                        Id = 3,
                        Option = option,
                        Act = actes
                    }
                }
            };

            var model = _mapper.MapScrutinToScrutinModel(scrutin);

            Check.That(model.PossibleChoices).HasSize(choice.Count);
            Check.That(model.Options.First().Suffrages.First().Value).IsEqualTo(choice.First().Value);
        }

        [TestMethod]
        public void MapScrutinToScrutinModelVodeBlanc()
        {
            var scrutin = new VotingProcess
            {
                VotingProcessOption = new List<VotingProcessOption>()
                {
                    new VotingProcessOption()
                    {
                        Id = 3,
                        Act = new List<Act>(),
                        Option = null
                    }
                }
            };

            var model = _mapper.MapScrutinToScrutinModel(scrutin);

            Check.That(model.Options.Single().IsBlankVote).IsTrue();
            Check.That(model.IsBlankVoteTakenIntoAccount).IsTrue();
        }

        [TestMethod]
        public void ThrowExceptionWhenScrutinIsNull()
        {
            Check.ThatCode(() => _mapper.MapScrutinToScrutinModel(null))
                .Throws<VoteInException>()
                .WithMessage("Aucun scrutin à mapper");
        }

        [TestMethod]
        public void ThrowExceptionOnMapResultatModelToResultatWhenParamIsNull()
        {
            Check.ThatCode(() => _mapper.MapResultatModelToResultat(null, default(ResultatMajorityVotingProcessModel)))
                .Throws<ArgumentNullException>()
                .WithProperty("ParamName", "scrutinModel");
            Check.ThatCode(() => _mapper.MapResultatModelToResultat(new VotingProcessModel(), default(ResultatMajorityVotingProcessModel)))
                .Throws<ArgumentNullException>()
                .WithProperty("ParamName", "resultatModel");
        }

        [TestMethod]
        public void MapScrutinMajoritaireResultatModelToResultat()
        {
            var scrutinModel = new VotingProcessModel { Id = 52, Mode = "scrutin-majoritaire" };
            var optionVainqueur = new OptionsModel
            {
                Id = 28,
                Name = "Winner"
            };
            var optionPerdant = new OptionsModel
            {
                Id = 32,
                Name = "Loser"
            };
            var resultatGagnant = new ResultatIndividualMajorityVotingProcessModel
            {
                Option = optionVainqueur,
                Votes = 3,
                Percentage = 75m
            };
            var resultatPerdant = new ResultatIndividualMajorityVotingProcessModel
            {
                Option = optionPerdant,
                Votes = 1,
                Percentage = 25m
            };
            var resultatModel = new ResultatMajorityVotingProcessModel
            {
                Id = 96,
                IsValidResult = true,
                Voters = 4,
                Winner = optionVainqueur,
                IndividualResults = { resultatGagnant, resultatPerdant }
            };

            var resultat = _mapper.MapResultatModelToResultat(scrutinModel, resultatModel);

            Check.That(resultat.IdVotingProcess).IsEqualTo(scrutinModel.Id);
            Check.That(resultat.IsValid).IsEqualTo(resultatModel.IsValidResult);
            Check.That(resultat.NbVoters).IsEqualTo(resultatModel.Voters);
            Check.That(resultat.IdWinningOption).IsEqualTo(resultatModel.Winner.Id);

            var scoreDetails = JsonConvert.DeserializeObject<List<ResultatIndividualMajorityVotingProcessModel>>(resultat.ScoreDetail);
            Check.That(scoreDetails[0].Votes).IsEqualTo(resultatGagnant.Votes);
            Check.That(scoreDetails[0].Percentage).IsEqualTo(resultatGagnant.Percentage);
            Check.That(scoreDetails[0].Option.Id).IsEqualTo(resultatGagnant.Option.Id);

            Check.That(scoreDetails[1].Votes).IsEqualTo(resultatPerdant.Votes);
            Check.That(scoreDetails[1].Percentage).IsEqualTo(resultatPerdant.Percentage);
            Check.That(scoreDetails[1].Option.Id).IsEqualTo(resultatPerdant.Option.Id);
        }

        [TestMethod]
        public void MapJugementMajoritaireResultatModelToResultat()
        {
            var scrutinModel = new VotingProcessModel { Id = 52 };
            var choice1 = new ChoiceModel { Id = 1, Value = 1 };
            var choice2 = new ChoiceModel { Id = 2, Value = 2 };
            var optionVainqueur = new OptionsModel
            {
                Id = 28,
                Name = "Winner"
            };
            var optionPerdant = new OptionsModel
            {
                Id = 32,
                Name = "Loser"
            };
            var resultatGagnant = new ResultatIndividualMajorityJudgmentModel
            {
                Option = optionVainqueur,
                Median = choice1,
                PercentageScoreInfMedian = 38m,
                PercentageScoreSupMedian = 42m
            };
            var resultatPerdant = new ResultatIndividualMajorityJudgmentModel
            {
                Option = optionPerdant,
                Median = choice2,
                PercentageScoreInfMedian = 40m,
                PercentageScoreSupMedian = 36m
            };
            var resultatModel = new ResultatMajoritaryJudgmentModel
            {
                Id = 96,
                IsValidResult = true,
                Voters = 4,
                Winner = optionVainqueur,
                IndividualResults = { resultatGagnant, resultatPerdant }
            };

            var resultat = _mapper.MapResultatModelToResultat(scrutinModel, resultatModel);

            Check.That(resultat.IdVotingProcess).IsEqualTo(scrutinModel.Id);
            Check.That(resultat.IsValid).IsEqualTo(resultatModel.IsValidResult);
            Check.That(resultat.NbVoters).IsEqualTo(resultatModel.Voters);
            Check.That(resultat.IdWinningOption).IsEqualTo(resultatModel.Winner.Id);

            var scoreDetails = JsonConvert.DeserializeObject<List<ResultatIndividualMajorityJudgmentModel>>(resultat.ScoreDetail);
            Check.That(scoreDetails[0].PercentageScoreInfMedian).IsEqualTo(resultatGagnant.PercentageScoreInfMedian);
            Check.That(scoreDetails[0].PercentageScoreSupMedian).IsEqualTo(resultatGagnant.PercentageScoreSupMedian);
            Check.That(scoreDetails[0].Median.Id).IsEqualTo(resultatGagnant.Median.Id);
            Check.That(scoreDetails[0].Option.Id).IsEqualTo(resultatGagnant.Option.Id);

            Check.That(scoreDetails[1].PercentageScoreInfMedian).IsEqualTo(resultatPerdant.PercentageScoreInfMedian);
            Check.That(scoreDetails[1].PercentageScoreSupMedian).IsEqualTo(resultatPerdant.PercentageScoreSupMedian);
            Check.That(scoreDetails[1].Median.Id).IsEqualTo(resultatPerdant.Median.Id);
            Check.That(scoreDetails[1].Option.Id).IsEqualTo(resultatPerdant.Option.Id);
        }

        [TestMethod]
        public void ThrowExceptionOnMapResultatToResultatScrutinMajoritaireModelWhenParamIsNull()
        {
            Check.ThatCode(() => _mapper.MapResultatToResultatModel(null, null))
                .Throws<ArgumentNullException>()
                .WithProperty("ParamName", "scrutin");
            Check.ThatCode(() => _mapper.MapResultatToResultatModel(new VotingProcess(), null))
                .Throws<ArgumentNullException>()
                .WithProperty("ParamName", "resultat");
        }

        [TestMethod]
        public void MapResultatToResultatScrutinMajoritaireModel()
        {
            var optionVainqueur = new OptionsModel
            {
                Id = 28,
                Name = "Winner"
            };
            var optionPerdant = new OptionsModel
            {
                Id = 32,
                Name = "Loser"
            };
            var resultatGagnant = new ResultatIndividualMajorityVotingProcessModel
            {
                Option = optionVainqueur,
                Votes = 3,
                Percentage = 75m
            };
            var resultatPerdant = new ResultatIndividualMajorityVotingProcessModel
            {
                Option = optionPerdant,
                Votes = 1,
                Percentage = 25m
            };

            var scrutin = new VotingProcess
            {
                VotingProcessOption = new List<VotingProcessOption>
                {
                    new VotingProcessOption { Option = new Option { Id = optionVainqueur.Id, Name = optionVainqueur.Name } },
                    new VotingProcessOption { Option = new Option { Id = optionPerdant.Id, Name = optionPerdant.Name } }
                },
                VotingProcessMode = new VotingProcessMode { Code = "scrutin-majoritaire" }
            };
            var resultat = new Result
            {
                Id = 3,
                IdWinningOption = optionVainqueur.Id,
                IsValid = true,
                NbVoters = 3,
                ScoreDetail = JsonConvert.SerializeObject(new List<ResultatIndividualMajorityVotingProcessModel> { resultatGagnant, resultatPerdant })
            };

            var resultatModel = _mapper.MapResultatToResultatModel(scrutin, resultat) as ResultatMajorityVotingProcessModel;

            Check.That(resultatModel.Id).IsEqualTo(resultat.Id);
            Check.That(resultatModel.IsValidResult).IsEqualTo(resultat.IsValid);
            Check.That(resultatModel.Voters).IsEqualTo(resultat.NbVoters);
            Check.That(resultatModel.Winner.Id).IsEqualTo(resultat.IdWinningOption);
            Check.That(resultatModel.Winner.Name).IsEqualTo(optionVainqueur.Name);

            var resultatsIndividuels = resultatModel.IndividualResults;
            Check.That(resultatsIndividuels[0].Votes).IsEqualTo(resultatGagnant.Votes);
            Check.That(resultatsIndividuels[0].Percentage).IsEqualTo(resultatGagnant.Percentage);
            Check.That(resultatsIndividuels[0].Option.Id).IsEqualTo(resultatGagnant.Option.Id);

            Check.That(resultatsIndividuels[1].Votes).IsEqualTo(resultatPerdant.Votes);
            Check.That(resultatsIndividuels[1].Percentage).IsEqualTo(resultatPerdant.Percentage);
            Check.That(resultatsIndividuels[1].Option.Id).IsEqualTo(resultatPerdant.Option.Id);
        }
    }
}
