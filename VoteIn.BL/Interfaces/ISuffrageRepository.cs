using VoteIn.Model.Models;
using VoteIn.Model.ViewModels;

namespace VoteIn.BL.Interfaces
{
    public interface ISuffrageRepository : IRepository<Suffrage>
    {
        /// <summary>
        /// Adds the vote alternatif.
        /// </summary>
        /// <param name="vote">The vote.</param>
        void AddVoteAlternatif(VoteAlternative vote);
        /// <summary>
        /// Adds the vote scrutin majoritaire.
        /// </summary>
        /// <param name="vote">The vote.</param>
        void AddVoteScrutinMajoritaire(VoteMajoritaryVotingProcess vote);
        /// <summary>
        /// Adds the vote jugement majoritaire.
        /// </summary>
        /// <param name="vote">The vote.</param>
        void AddVoteJugementMajoritaire(VoteMajoritaryJudgment vote);
    }
}