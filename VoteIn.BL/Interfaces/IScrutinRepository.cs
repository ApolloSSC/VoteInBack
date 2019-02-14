using System;
using System.Collections.Generic;
using System.Linq;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;

namespace VoteIn.BL.Interfaces
{
    public interface IVotingProcessRepository : IRepository<VotingProcess>
    {
        /// <summary>
        /// Calculates the borda.
        /// </summary>
        /// <param name="idVotingProcess">The identifier voting process.</param>
        /// <returns></returns>
        ResultatBorda CalculateBorda(int idVotingProcess);
        /// <summary>
        /// Gets the voting process.
        /// </summary>
        /// <returns></returns>
        IQueryable<VotingProcess> GetVotingProcess();
        /// <summary>
        /// Gets the by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        VotingProcess GetByToken(string token, string includes);
        /// <summary>
        /// Gets the by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        VotingProcess GetByGuid(Guid guid, string includes);
        /// <summary>
        /// Gets the by user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        IEnumerable<VotingProcess> GetByUser(string userId);
        /// <summary>
        /// Deletes the by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        void DeleteByGuid(Guid guid);
    }
}