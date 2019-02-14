using Microsoft.AspNetCore.Mvc;
using VoteIn.BL.Interfaces;
using VoteIn.Model.Models;

namespace VoteIn.Controllers
{
    [Route("api/VotingProcessMode")]
    public class VotingProcessModeController : GenericController<VotingProcessMode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VotingProcessModeController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public VotingProcessModeController(IRepository<VotingProcessMode> repo)
        {
            repository = repo;
        }
    }
}