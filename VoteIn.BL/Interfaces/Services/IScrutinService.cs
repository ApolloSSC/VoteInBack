using System;
using System.Security.Claims;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;

namespace VoteIn.BL.Interfaces.Services
{
    public interface IVotingProcessService
    {
        /// <summary>
        /// Clores the scrutin.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        void CloreScrutin(Guid guid);
        /// <summary>
        /// Gets the resultat.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        IResultatModel GetResultat(Guid guid);
        /// <summary>
        /// Clores the scrutin.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="user">The user.</param>
        void CloseVotingProcess(Guid guid, ClaimsPrincipal user);
        /// <summary>
        /// Puts the scrutin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="scrutin">The scrutin.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        VotingProcess PutScrutin(Guid id, VotingProcess scrutin, ClaimsPrincipal user);
    }
}