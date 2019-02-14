using VoteIn.Model.Models;

namespace VoteIn.BL.Interfaces
{
    public interface IEnveloppeRepository : IRepository<Envelope>
    {
        /// <summary>
        /// Counts the vote.
        /// </summary>
        /// <param name="scrutin">The scrutin.</param>
        void CountVote(VotingProcess scrutin);
        /// <summary>
        /// Determines whether the specified identifier electeur has voted.
        /// </summary>
        /// <param name="idElecteur">The identifier electeur.</param>
        /// <param name="token">The token.</param>
        /// <param name="enveloppe">The enveloppe.</param>
        void HasVoted(int idElecteur, string token, Envelope enveloppe);
    }
}