using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VoteIn.BL.Interfaces;
using VoteIn.DAL;
using VoteIn.Model.Models;
using VoteIn.Model.ViewModels;
using VoteIn.Utils;

namespace VoteIn.BL.Repositories
{
    public class SuffrageRepository : Repository<Suffrage>, ISuffrageRepository
    {
        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="SuffrageRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public SuffrageRepository(VoteInContext context) : base(context)
        {

        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Adds the vote alternatif.
        /// </summary>
        /// <param name="vote">The vote.</param>
        public void AddVoteAlternatif(VoteAlternative vote)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var scrutin = GetByIdWithOptions(vote.IdVotingProcess);

                var suffrage = CreateSuffrage(scrutin.Id);

                var preference = 0;
                foreach (var opt in vote.Ranking)
                {
                    var acte = new Act
                    {
                        IdVotingProcessOption = scrutin.VotingProcessOption.First(os => os.IdOption == opt.Id).Id,
                        Value = preference,
                        IdSuffrage = suffrage.Id
                    };

                    Context.Add(acte);
                    preference++;
                }

                Save();

                transaction.Commit();
            }
        }
        /// <summary>
        /// Adds the vote scrutin majoritaire.
        /// </summary>
        /// <param name="vote">The vote.</param>
        public void AddVoteScrutinMajoritaire(VoteMajoritaryVotingProcess vote)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var scrutin = GetByIdWithOptions(vote.IdVotingProcess);

                var suffrage = CreateSuffrage(scrutin.Id);

                foreach (var os in scrutin.VotingProcessOption)
                {
                    var newActe = new Act
                    {
                        IdSuffrage = suffrage.Id,
                        IdVotingProcessOption = os.Id,
                        IdChoice = os.IdOption == vote.IdOption
                            ? Context.Choice.First(c => c.Name == Constantes.VOTING_PROCESS_MAJ_CHOOSEN).Id
                            : Context.Choice.First(c => c.Name == Constantes.VOTING_PROCESS_MAJ_REJECTED).Id
                    };
                    Context.Add(newActe);
                }

                Save();
                transaction.Commit();
            }
        }
        /// <summary>
        /// Adds the vote jugement majoritaire.
        /// </summary>
        /// <param name="vote">The vote.</param>
        public void AddVoteJugementMajoritaire(VoteMajoritaryJudgment vote)
        {
            using (var transaction = Context.Database.BeginTransaction())
            {
                var scrutin = GetByIdWithOptions(vote.IdVotingProcess);

                var suffrage = CreateSuffrage(scrutin.Id);

                foreach (var oc in vote.OptionChoice)
                {
                    var acte = new Act
                    {
                        IdChoice = oc.IdChoice,
                        IdSuffrage = suffrage.Id,
                        IdVotingProcessOption = scrutin.VotingProcessOption.First(os => os.IdOption == oc.IdOption).Id
                    };
                    Context.Add(acte);
                }

                Save();

                transaction.Commit();
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the by identifier with options.
        /// </summary>
        /// <param name="idScrutin">The identifier scrutin.</param>
        /// <returns></returns>
        private VotingProcess GetByIdWithOptions(int idScrutin)
        {
            return Context.VotingProcess.Include(s => s.VotingProcessOption).First(sc => sc.Id == idScrutin);
        }
        /// <summary>
        /// Creates the suffrage.
        /// </summary>
        /// <param name="idScrutin">The identifier scrutin.</param>
        /// <returns></returns>
        private Suffrage CreateSuffrage(int idScrutin)
        {
            var suffrage = new Suffrage
            {
                IdVotingProcess = idScrutin,
                CreationDate = DateTime.Now
            };
            Context.Add(suffrage);
            Save();
            return suffrage;
        }
        #endregion
    }
}