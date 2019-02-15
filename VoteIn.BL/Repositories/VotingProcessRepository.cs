using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VoteIn.BL.Interfaces;
using VoteIn.BL.Interfaces.Services;
using VoteIn.DAL;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;
using static VoteIn.BL.Repositories.EnveloppeRepository;

namespace VoteIn.BL.Repositories
{
    public class VotingProcessRepository : Repository<VotingProcess>, IVotingProcessRepository
    {
        /// <summary>
        /// The file service
        /// </summary>
        private readonly IFileService _fileService;
        /// <summary>
        /// The email service
        /// </summary>
        private readonly IEmailSenderService _emailService;
        /// <summary>
        /// The configuration
        /// </summary>
        private IConfiguration _configuration;

        #region Public Methods
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileService"></param>
        /// <param name="emailService"></param>
        public VotingProcessRepository(VoteInContext context, IFileService fileService, IEmailSenderService emailService, IConfiguration config) : base(context)
        {
            _fileService = fileService;
            _emailService = emailService;
            _configuration = config;
        }

        /// <summary>
        /// Get voting process by token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public VotingProcess GetByToken(string token, string includes)
        {
            var voter = Context.Voter.FirstOrDefault(e => e.Token == token);
            if (voter.HasVoted)
            {
                throw new DejaVoteException();
            }
            var votingProcess = Get(voter.IdVotingProcess, includes);
            return votingProcess;
        }

        /// <summary>
        /// Get voting process by GUID
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public VotingProcess GetByGuid(Guid guid, string includes)
        {
            var votingProcess = DbSet.FirstOrDefault(s => s.Guid.Equals(guid));
            includeProperties(votingProcess, includes);
            return votingProcess;
        }

        /// <summary>
        /// Get voting process by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<VotingProcess> GetByUser(string userId)
        {
            var votingProcesses = DbSet.AsNoTracking().Where(s => s.IdUser == userId)
                .Include(s => s.VotingProcessMode).ThenInclude(ms => ms.Choice)
                .Include(s => s.VotingProcessOption).ThenInclude(os => os.Option)
                .Include(s => s.Voter)
                .Include(s => s.Envelope);
            return votingProcesses;
        }

        /// <summary>
        /// Delete voting process by GUID
        /// </summary>
        /// <param name="guid"></param>
        public void DeleteByGuid(Guid guid)
        {
            var votingProcess = DbSet.First(s => s.Guid == guid);
            DbSet.Remove(votingProcess);
            Save();
        }

        /// <summary>
        /// Get list of voting process
        /// </summary>
        /// <returns></returns>
        public IQueryable<VotingProcess> GetVotingProcess()
        {
            return Context.VotingProcess.AsQueryable();
        }

        /// <summary>
        /// Add new voting process
        /// </summary>
        /// <param name="votingProcess"></param>
        public override void Add(VotingProcess votingProcess)
        {
            votingProcess.VotingProcessMode = null;
            //Private key management, the privateKey field must not be exposed in the API
            votingProcess.PrivateKey = votingProcess.MyPrivateKey;
            var votingProcessOption = votingProcess.VotingProcessOption;
            votingProcess.VotingProcessOption = null;
            base.Add(votingProcess);
            foreach (var os in votingProcessOption)
            {
                if (Context.Option.FirstOrDefault(o => o.Id == os.Option.Id) != null)
                    Context.Update(os.Option);
                else
                    Context.Add(os.Option);
                Save();

                var opt = os.Option;
                os.Option = null;
                os.IdOption = opt.Id;
                os.IdVotingProcess = votingProcess.Id;

                Context.Add(os);
                Save();
            }

            try
            {
                var culture = Thread.CurrentThread.CurrentUICulture;
                var mailContent = _fileService.LoadFile(@"Mail\AdminLinkMail.html", culture);

                if (mailContent == null) return;

                string mailAuteur;
                string nameAuteur;

                if (votingProcess.User != null)
                {
                    mailAuteur = votingProcess.User.Email;
                    nameAuteur = votingProcess.User.UserName;
                }
                else
                {
                    mailAuteur = votingProcess.AuthorMail;
                    nameAuteur = votingProcess.Author;
                }

                var link = _configuration["AppPath"] + "/poll/" + votingProcess.Guid;
#if DEBUG
                link = _configuration["DebugAppPath"] + "/poll/" + votingProcess.Guid;
#endif
                var filledContent = mailContent.Replace("{0}", votingProcess.Name)
                    .Replace("{1}", link)
                    .Replace("{2}", nameAuteur);
                _emailService.SendEmailAsync(mailAuteur, $"Création du scrutin {votingProcess.Name}", filledContent, nameAuteur).Wait();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Update a voting process
        /// </summary>
        /// <param name="votingProcess"></param>
        public override void Update(VotingProcess votingProcess)
        {
            var votingProcessToUpdate = Context.VotingProcess.Include(s => s.VotingProcessOption).ThenInclude(os => os.Option).FirstOrDefault(s => s.Id == votingProcess.Id);

            if (votingProcess.VotingProcessOption != null)
            {
                //Update votingProcess option
                var votingProcessOptionToDelete = votingProcessToUpdate.VotingProcessOption.Where(os => votingProcess.VotingProcessOption.FirstOrDefault(opts => opts.IdOption == os.IdOption) == null);
                Context.RemoveRange(votingProcessOptionToDelete);

                foreach (var os in votingProcess.VotingProcessOption)
                {
                    var existingVotingProcessOption = votingProcessToUpdate.VotingProcessOption.FirstOrDefault(opts => opts.IdOption == os.IdOption);
                    if (existingVotingProcessOption == null)
                    {
                        Context.Add(os.Option);
                    }
                    else
                    {
                        existingVotingProcessOption.Option.Color = os.Option.Color;
                        existingVotingProcessOption.Option.Photo = os.Option.Photo;
                        existingVotingProcessOption.Option.Description = os.Option.Description;
                        existingVotingProcessOption.Option.Name = os.Option.Name;
                    }
                }
            }
            var votersToSendMail = new List<Voter>();
            if (votingProcess.Voter != null)
            {
                //Update voters
                var votersToDelete = Context.Voter.AsNoTracking().Where(el => el.IdVotingProcess == votingProcess.Id && votingProcess.Voter.FirstOrDefault(voter => voter.Mail.Equals(el.Mail)) == null);
                Context.RemoveRange(votersToDelete);

                foreach (var voter in votingProcess.Voter)
                {
                    if (Context.Voter.AsNoTracking().FirstOrDefault(el => el.IdVotingProcess == votingProcess.Id && el.Mail.Equals(voter.Mail)) == null)
                    {
                        Context.Add(voter);
                        votersToSendMail.Add(voter);
                    }
                }
            }

            if (votingProcessToUpdate != null)
            {
                votingProcessToUpdate.ClosingDate = votingProcess.ClosingDate;
                votingProcessToUpdate.Description = votingProcess.Description;
                votingProcessToUpdate.Name = votingProcess.Name;
                votingProcessToUpdate.IdPreviousVotingProcess = votingProcess.IdPreviousVotingProcess;
                votingProcessToUpdate.GuidPreviousVotingProcess = votingProcess.GuidPreviousVotingProcess;

                base.Update(votingProcessToUpdate);
            }

            try
            {
                var culture = Thread.CurrentThread.CurrentUICulture;
                var mailContent = _fileService.LoadFile(@"Mail\InvitLinkMail.html", culture);
                if (mailContent == null) return;

                foreach (var voter in votersToSendMail)
                {
                    var link = _configuration["AppPath"] + "/p/" + voter.Token;
#if DEBUG
                    link = _configuration["DebugAppPath"] + "/p/" + voter.Token;
#endif
                    var filledContent = mailContent.Replace("{0}", votingProcess.Name)
                        .Replace("{1}", link)
                        .Replace("{2}", votingProcess.Author);

                    _emailService.SendEmailAsync(voter.Mail, $"Votez - {votingProcess.Name}", filledContent).Wait();
                }
            }
            catch
            {
                // ignored
            }

        }

        /// <summary>
        /// Calculate with Bordas method
        /// </summary>
        /// <param name="idVotingProcess"></param>
        /// <returns></returns>
        public ResultatBorda CalculateBorda(int idVotingProcess)
        {
            var result = new ResultatBorda { Scores = new List<ScoreBorda>() };
            var votingProcess = Context.VotingProcess.Include("VotingProcessOption.Option").AsNoTracking().First(s => s.Id == idVotingProcess);

            var suffrage = Context.Suffrage.Include("Act.VotingProcessOption").Where(s => s.IdVotingProcess == idVotingProcess).ToList();

            var total = suffrage.Count();
            result.Votants = total;

            var nOptions = votingProcess.VotingProcessOption.Count;
            foreach (var votingProcessOption in votingProcess.VotingProcessOption)
            {
                var option = votingProcessOption.Option;
                var score = new ScoreBorda();
                option.VotingProcessOption = null;
                score.Option = option;
                var val = 0;
                var acts = suffrage.SelectMany(s => s.Act).Where(a => a.IdVotingProcessOption == votingProcessOption.Id);
                foreach (var act in acts)
                {
                    if (act.Value == null) continue;

                    var points = nOptions - act.Value;
                    val += (int)points;
                }
                score.Score = val;
                result.Scores.Add(score);
            }

            return result;
        }
        #endregion

        #region Private Methods
        #endregion
    }
}