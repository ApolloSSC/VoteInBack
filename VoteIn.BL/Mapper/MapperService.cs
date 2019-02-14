using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using VoteIn.BL.Exceptions;
using VoteIn.BL.Interfaces.Mapper;
using VoteIn.Model.Business;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;
using VoteIn.Utils;

namespace VoteIn.BL.Mapper
{
    public class MapperService : IMapperService
    {
        #region Public Methods
        /// <summary>
        /// Maps the scrutin to scrutin model.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Aucun scrutin à mapper</exception>
        public VotingProcessModel MapScrutinToScrutinModel(VotingProcess scrutin)
        {
            if (scrutin == null)
            {
                throw new VoteInException("Aucun scrutin à mapper");
            }

            var model = new VotingProcessModel
            {
                Id = scrutin.Id,
                ClosingDate = scrutin.ClosingDate,
                IsLastRound = scrutin.IdPreviousVotingProcess.HasValue,
                Mode = scrutin.VotingProcessMode?.Code
            };

            if (scrutin.VotingProcessMode != null)
            {
                var choiceModels = MapChoiceToChoiceModel(scrutin.VotingProcessMode.Choice.ToList());
                model.PossibleChoices.AddRange(choiceModels);
            }

            if (scrutin.VotingProcessOption != null)
            {
                var optionsModels = MapOptionToOptionsModels(scrutin.VotingProcessOption.ToList(), model.PossibleChoices);
                model.Options.AddRange(optionsModels);
            }

            if (scrutin.Suffrage != null)
            {
                var bulletinModels = MapSuffrageToBulletin(scrutin.Suffrage.ToList());
                model.Ballots.AddRange(bulletinModels);
            }

            model.IsBlankVoteTakenIntoAccount = model.Options.Any(o => o.IsBlankVote);

            return model;
        }

        /// <summary>
        /// Maps the resultat model to resultat.
        /// </summary>
        /// <param name="scrutinModel">The scrutin model.</param>
        /// <param name="resultatModel">The resultat model.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// scrutinModel
        /// or
        /// resultatModel
        /// </exception>
        public Result MapResultatModelToResultat(VotingProcessModel scrutinModel, IResultatModel resultatModel)
        {
            if (scrutinModel is null)
                throw new ArgumentNullException(nameof(scrutinModel));

            if (resultatModel is null)
                throw new ArgumentNullException(nameof(resultatModel));

            return new Result
            {
                IdVotingProcess = scrutinModel.Id,
                IsValid = resultatModel.IsValidResult,
                NbVoters = resultatModel.Voters,
                IdWinningOption = resultatModel.Winner?.Id,
                ScoreDetail = JsonConvert.SerializeObject(GetScoreDetail(resultatModel))
            };
        }

        /// <summary>
        /// Maps the resultat to resultat model.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <param name="resultat">The resultat.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">
        /// scrutin
        /// or
        /// resultat
        /// </exception>
        public IResultatModel MapResultatToResultatModel(VotingProcess scrutin, Result resultat)
        {
            if (scrutin is null)
                throw new ArgumentNullException(nameof(scrutin));
            if (resultat is null)
                throw new ArgumentNullException(nameof(resultat));

            var resultatModel = CreateResultatModel(resultat, scrutin);
            resultatModel.Id = resultat.Id;
            resultatModel.IsValidResult = resultat.IsValid;
            resultatModel.Voters = resultat.NbVoters;
            resultatModel.Winner = scrutin.VotingProcessOption.Select(MapOptionScrutinToOptionModel).FirstOrDefault(o => o.Id == resultat.IdWinningOption);
            resultatModel.Options = scrutin.VotingProcessOption.Select(MapOptionScrutinToOptionModel).ToList();

            return resultatModel;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Maps the suffrage to bulletin.
        /// </summary>
        /// <param name="suffrages">The suffrages.</param>
        /// <returns></returns>
        private static IEnumerable<BallotModel> MapSuffrageToBulletin(List<Suffrage> suffrages)
        {
            var bulletins = new List<BallotModel>();

            foreach (var suffrage in suffrages)
            {
                var bulletin = new BallotModel();
                foreach (var acte in suffrage.Act)
                {
                    bulletin.Acts.Add(new ActModel
                    {
                        IdOption = acte.VotingProcessOption.IdOption,
                        Value = acte.Value
                    });
                }
                bulletins.Add(bulletin);
            }

            return bulletins;
        }
        /// <summary>
        /// Maps the option to options models.
        /// </summary>
        /// <param name="optionsScrutin">The options scrutin.</param>
        /// <param name="choiceModels">The choice models.</param>
        /// <returns></returns>
        private static IEnumerable<OptionsModel> MapOptionToOptionsModels(List<VotingProcessOption> optionsScrutin, List<ChoiceModel> choiceModels)
        {
            var optionsModels = new List<OptionsModel>();

            foreach (var optionScrutin in optionsScrutin)
            {
                var optionModel = MapOptionScrutinToOptionModel(optionScrutin);

                foreach (var acte in optionScrutin.Act)
                {
                    var suffrageModel = new SuffrageModel();
                    if (acte.Value.HasValue)
                    {
                        suffrageModel.Value = acte.Value.Value;
                    }
                    else
                    {
                        var choice = choiceModels.FirstOrDefault(c => c.Id == acte.IdChoice);
                        if (choice != null)
                        {
                            suffrageModel.Value = choice.Value.GetValueOrDefault();
                        }
                    }

                    optionModel.Suffrages.Add(suffrageModel);
                }

                optionsModels.Add(optionModel);
            }

            return optionsModels;
        }
        /// <summary>
        /// Maps the option scrutin to option model.
        /// </summary>
        /// <param name="optionScrutin">The option scrutin.</param>
        /// <returns></returns>
        private static OptionsModel MapOptionScrutinToOptionModel(VotingProcessOption optionScrutin)
        {
            if (optionScrutin.Option == null) //vote blanc
            {
                return new OptionsModel
                {
                    IsBlankVote = true,
                    Name = Constantes.BLANK_VOTE
                };
            }
            else
            {
                return new OptionsModel
                {
                    Id = optionScrutin.Option.Id,
                    Name = optionScrutin.Option.Name,
                };
            }
        }
        /// <summary>
        /// Maps the choice to choice model.
        /// </summary>
        /// <param name="choiceList">The choice list.</param>
        /// <returns></returns>
        private static IEnumerable<ChoiceModel> MapChoiceToChoiceModel(List<Choice> choiceList)
        {
            var models = new List<ChoiceModel>();
            if (choiceList != null && choiceList.Any())
            {
                foreach (var choice in choiceList)
                {
                    var choiceModel = new ChoiceModel
                    {
                        Id = choice.Id,
                        Name = choice.Name,
                        Value = choice.Value
                    };
                    models.Add(choiceModel);
                }
            }

            return models;
        }

        /// <summary>
        /// Gets the score detail.
        /// </summary>
        /// <param name="resultatModel">The resultat model.</param>
        /// <returns></returns>
        private static object GetScoreDetail(IResultatModel resultatModel)
        {
            switch (resultatModel)
            {
                case ResultatMajorityVotingProcessModel scrutinMajoritaireModel:
                    return scrutinMajoritaireModel.IndividualResults;
                case ResultatMajoritaryJudgmentModel jugementMajoritaireModel:
                    return jugementMajoritaireModel.IndividualResults;
                case AlternativeVoteResultatModel voteAlternatifModel:
                    return voteAlternatifModel.Stages;
                case ResultatCondorcetRandomiseModel condorcetModel:
                    return condorcetModel.WinningLottery;
                default:
                    return null;
            }
        }

        /// <summary>
        /// Creates the resultat model.
        /// </summary>
        /// <param name="resultat">The resultat.</param>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Type de vote inconnu</exception>
        private IResultatModel CreateResultatModel(Result resultat, VotingProcess scrutin)
        {
            switch (scrutin.VotingProcessMode.Code)
            {
                case Constantes.JUG_MAJ:
                    return new ResultatMajoritaryJudgmentModel
                    {
                        IndividualResults = JsonConvert.DeserializeObject<List<ResultatIndividualMajorityJudgmentModel>>(resultat.ScoreDetail)
                    };
                case Constantes.VOTING_PROCESS_MAJ:
                    return new ResultatMajorityVotingProcessModel
                    {
                        IndividualResults = JsonConvert.DeserializeObject<List<ResultatIndividualMajorityVotingProcessModel>>(resultat.ScoreDetail)
                    };
                case Constantes.VOTE_ALTER:
                    return new AlternativeVoteResultatModel
                    {
                        Stages = JsonConvert.DeserializeObject<List<AlternativeStageVote>>(resultat.ScoreDetail)
                    };
                default:
                    throw new VoteInException("Unknown vote type.");
            }
        }
        #endregion
    }
}
