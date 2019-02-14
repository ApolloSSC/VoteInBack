using System.Linq;
using Microsoft.EntityFrameworkCore;
using VoteIn.BL.Interfaces;
using VoteIn.BL.Interfaces.Mapper;
using VoteIn.Model.Business;
using VoteIn.Model.Models;
using VoteIn.Model.Business.ResultModels;
using VoteIn.BL.Interfaces.Calculateurs;
using VoteIn.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using VoteIn.BL.Interfaces.Services;
using System.Threading;

namespace VoteIn.BL.Services
{
    public class ScrutinService : IVotingProcessService
    {
        /// <summary>
        /// The voting process repository
        /// </summary>
        protected readonly IVotingProcessRepository VotingProcessRepository;
        /// <summary>
        /// The envelope repository
        /// </summary>
        protected readonly IEnveloppeRepository EnvelopeRepository;
        /// <summary>
        /// The mapper service
        /// </summary>
        protected readonly IMapperService MapperService;
        /// <summary>
        /// The result repository
        /// </summary>
        private readonly IRepository<Result> ResultRepository;
        /// <summary>
        /// The calculator factory
        /// </summary>
        private readonly ICalculateurFactory CalculatorFactory;
        /// <summary>
        /// The file service
        /// </summary>
        private readonly IFileService FileService;
        /// <summary>
        /// The email sender service
        /// </summary>
        private readonly IEmailSenderService EmailSenderService;

        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="ScrutinService"/> class.
        /// </summary>
        /// <param name="votingProcessRepository">The voting process repository.</param>
        /// <param name="envelopeRepository">The envelope repository.</param>
        /// <param name="resultRepository">The result repository.</param>
        /// <param name="mapperService">The mapper service.</param>
        /// <param name="calculatorFactory">The calculator factory.</param>
        /// <param name="fileService">The file service.</param>
        /// <param name="emailSenderService">The email sender service.</param>
        public ScrutinService(IVotingProcessRepository votingProcessRepository, IEnveloppeRepository envelopeRepository, IRepository<Result> resultRepository,
            IMapperService mapperService, ICalculateurFactory calculatorFactory,
            IFileService fileService,
            IEmailSenderService emailSenderService)
        {
            EnvelopeRepository = envelopeRepository;
            VotingProcessRepository = votingProcessRepository;
            MapperService = mapperService;
            ResultRepository = resultRepository;
            CalculatorFactory = calculatorFactory;
            FileService = fileService;
            EmailSenderService = emailSenderService;
        }
        #endregion

        #region Public Methods        
        /// <summary>
        /// Close the voting process.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="user">The user.</param>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Impossible de cloturer, ce scrutin n'existe pas</exception>
        public void CloseVotingProcess(Guid guid, ClaimsPrincipal user)
        {
            var scrutin = GetScrutin(guid);
            if (scrutin.IdUser == null || scrutin.IdUser != null && user.FindFirst("Id").Value == scrutin.IdUser)
            {
                CloreScrutin(guid);
                if (scrutin.Voter == null) return;
                try
                {
                    var culture = Thread.CurrentThread.CurrentUICulture;
                    var mailContent = FileService.LoadFile(@"Mail\ResultLinkMail.html", culture);

                    if (mailContent != null)
                    {
                        foreach (var elec in scrutin.Voter)
                        {
                            var filledContent = mailContent.Replace("{0}", scrutin.Name)
                                .Replace("{1}", $"https://www.voteinapp.com/poll/{scrutin.Guid}/result")
                                .Replace("{2}", scrutin.Author);

                            EmailSenderService.SendEmailAsync(elec.Mail, $"VotingProcess cloturé - {scrutin.Name}", filledContent).Wait();
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
            else
            {
                throw new VoteInException("Impossible de cloturer, ce scrutin n'existe pas");
            }
        }
        /// <summary>
        /// Clores the scrutin.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Impossible de cloturer, ce scrutin n'a pas de suffrage exprimé</exception>
        public void CloreScrutin(Guid guid)
        {
            var scrutin = GetScrutin(guid);
            if (scrutin.NbVotes == 0)
            {
                throw new VoteInException("Impossible de cloturer, ce scrutin n'a pas de suffrage exprimé");
            }
            EnvelopeRepository.CountVote(scrutin);
            CalculerResultat(scrutin);
            CloreScrutin(scrutin);
        }
        /// <summary>
        /// Gets the resultat.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Le resultat n'existe pas</exception>
        public IResultatModel GetResultat(Guid guid)
        {
            var scrutin = GetScrutin(guid);
            var resultat = ResultRepository.GetAll().FirstOrDefault(r => r.IdVotingProcess == scrutin.Id);
            if (resultat == null)
                throw new VoteInException("Le resultat n'existe pas");
            var resultatModel = MapperService.MapResultatToResultatModel(scrutin, resultat);
            if (scrutin.IdPreviousVotingProcess != null)
            {
                ((ResultatMajorityVotingProcessModel)resultatModel).PreviousRoundResultat = (ResultatMajorityVotingProcessModel)GetResultat(scrutin.GuidPreviousVotingProcess);
            }
            return resultatModel;
        }
        /// <summary>
        /// Puts the scrutin.
        /// </summary>
        /// <param name="scrutinId">The scrutin identifier.</param>
        /// <param name="scrutin">The scrutin.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Impossible de cloturer, ce scrutin n'existe pas</exception>
        public VotingProcess PutScrutin(Guid scrutinId, VotingProcess scrutin, ClaimsPrincipal user)
        {
            var scrutinTuUpdte = GetScrutin(scrutinId);
            if (scrutin.IdUser == null || scrutin.IdUser != null && user.FindFirst("Id").Value == scrutin.IdUser)
            {
                VotingProcessRepository.Update(scrutin);
            }
            else
            {
                throw new VoteInException("Impossible de cloturer, ce scrutin n'existe pas");
            }
            return scrutinTuUpdte;
        }
        #endregion

        #region Private Mehtods        
        /// <summary>
        /// Calculers the resultat.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        private void CalculerResultat(VotingProcess scrutin)
        {
            var scrutinModel = GetScrutinModel(scrutin);
            var calculateur = CalculatorFactory.GetCalculator(scrutinModel.Mode);
            var resultatModel = calculateur.CalculateResult(scrutinModel);
            if (resultatModel is ResultatMajorityVotingProcessModel resultatScrutinMajoritaire)
            {
                if (resultatModel.IsValidResult
                    && !scrutinModel.IsLastRound
                    && resultatModel.Winner == null)
                {
                    resultatModel.IdNewVotingProcess = CreateTourSuivant(scrutinModel.Id, resultatScrutinMajoritaire);
                }
            }
            SaveResultat(scrutinModel, resultatModel);
        }
        /// <summary>
        /// Clores the scrutin.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Ce scrutin est déjà cloturé</exception>
        private void CloreScrutin(VotingProcess scrutin)
        {
            if (scrutin.ClosingDate.HasValue)
            {
                throw new VoteInException("Ce scrutin est déjà cloturé");
            }
            scrutin.ClosingDate = DateTime.Now;
            VotingProcessRepository.Update(scrutin);
        }
        /// <summary>
        /// Gets the scrutin.
        /// </summary>
        /// <param name="scrutinId">The scrutin identifier.</param>
        /// <returns></returns>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Le scrutin n'existe pas</exception>
        private VotingProcess GetScrutin(int scrutinId)
        {
            var scrutin = VotingProcessRepository.GetVotingProcess()
                .Include(s => s.VotingProcessMode.Choice)
                .Include(s => s.VotingProcessOption).ThenInclude(o => o.Option)
                .Include(s => s.VotingProcessOption).ThenInclude(o => o.Act)
                .Include(s => s.Suffrage)
                .Include(s => s.Envelope)
                .FirstOrDefault(s => s.Id == scrutinId);
            if (scrutin == null)
                throw new VoteInException("Le scrutin n'existe pas");
            return scrutin;
        }
        /// <summary>
        /// Gets the scrutin.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        /// <exception cref="VoteIn.BL.Exceptions.VoteInException">Le scrutin n'existe pas</exception>
        private VotingProcess GetScrutin(Guid guid)
        {
            var scrutin = VotingProcessRepository.GetVotingProcess()
                .Include(s => s.VotingProcessMode.Choice)
                .Include(s => s.VotingProcessOption).ThenInclude(o => o.Option)
                .Include(s => s.VotingProcessOption).ThenInclude(o => o.Act)
                .Include(s => s.Suffrage)
                .Include(s => s.Envelope)
                .FirstOrDefault(s => s.Guid.Equals(guid));
            if (scrutin == null)
                throw new VoteInException("Le scrutin n'existe pas");
            return scrutin;
        }
        /// <summary>
        /// Gets the scrutin model.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        private VotingProcessModel GetScrutinModel(VotingProcess scrutin)
        {
            return MapperService.MapScrutinToScrutinModel(scrutin);
        }
        /// <summary>
        /// Saves the resultat.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        /// <param name="resultatModel">The resultat model.</param>
        private void SaveResultat(VotingProcessModel scrutin, IResultatModel resultatModel)
        {
            var resultat = MapperService.MapResultatModelToResultat(scrutin, resultatModel);

            ResultRepository.Add(resultat);
        }
        /// <summary>
        /// Creates the tour suivant.
        /// </summary>
        /// <param name="idScutin">The identifier scutin.</param>
        /// <param name="resultatModel">The resultat model.</param>
        /// <returns></returns>
        private int CreateTourSuivant(int idScutin, ResultatMajorityVotingProcessModel resultatModel)
        {
            var ancienScrutin = GetScrutin(idScutin);
            var nouveauScrutin = ancienScrutin.CopyForNextTurn();
            nouveauScrutin.VotingProcessOption = GetOptionsQualifiees(resultatModel, ancienScrutin);
            VotingProcessRepository.Add(nouveauScrutin);
            return nouveauScrutin.Id;
        }
        /// <summary>
        /// Gets the options qualifiees.
        /// </summary>
        /// <param name="resultatModel">The resultat model.</param>
        /// <param name="ancienScrutin">The ancien scrutin.</param>
        /// <returns></returns>
        private List<VotingProcessOption> GetOptionsQualifiees(ResultatMajorityVotingProcessModel resultatModel, VotingProcess ancienScrutin)
        {
            Option GetOptionAncienScrutin(OptionsModel o) => ancienScrutin.VotingProcessOption.First(os => os.Option.Id == o.Id).Option;

            var resultatsQualifies = resultatModel.IndividualResults.Take(2);
            return resultatsQualifies.Select(o => new VotingProcessOption { Option = GetOptionAncienScrutin(o.Option) }).ToList();
        }
        #endregion
    }
}
