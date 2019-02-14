using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VoteIn.BL.Interfaces;
using VoteIn.BL.Interfaces.Services;
using VoteIn.Model.Business.ResultModels;
using VoteIn.Model.Models;

namespace VoteIn.Controllers
{
    [Route("api/VotingProcess")]
    public class VotingProcessController : GenericController<VotingProcess>
    {
        /// <summary>
        /// The voting process service
        /// </summary>
        private IVotingProcessService votingProcessService;
        /// <summary>
        /// Gets the voting process repository.
        /// </summary>
        /// <value>
        /// The voting process repository.
        /// </value>
        private IVotingProcessRepository votingProcessRepository => (IVotingProcessRepository)repository;

        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="VotingProcessController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="votingProcessService">The voting process service.</param>
        public VotingProcessController(IVotingProcessRepository repo, IVotingProcessService votingProcessService)
        {
            repository = repo;
            includes = "VotingProcessMode.Choice,VotingProcessOption.Option,Voter,Envelope";
            this.votingProcessService = votingProcessService;
        }
        #endregion

        #region Public Methods        
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public override IEnumerable<VotingProcess> Get()
        {
            var userId = User.FindFirst("Id").Value;
            return votingProcessRepository.GetByUser(userId);
        }
        /// <summary>
        /// Gets the by token.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns></returns>
        [HttpGet("getByToken/{token}")]
        public VotingProcess GetByToken(string token)
        {
            return votingProcessRepository.GetByToken(token, includes);
        }
        /// <summary>
        /// Gets the by unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [HttpGet("getByGuid/{guid}")]
        public VotingProcess GetByGuid(string guid)
        {
            return votingProcessRepository.GetByGuid(new Guid(guid), includes);
        }
        /// <summary>
        /// Posts the specified SCR.
        /// </summary>
        /// <param name="scr">The SCR.</param>
        /// <returns></returns>
        public override VotingProcess Post([FromBody]VotingProcess scr)
        {
            scr.Guid = Guid.NewGuid();
            return base.Post(scr);
        }
        /// <summary>
        /// Puts the specified unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <param name="scrutin">The scrutin.</param>
        /// <returns></returns>
        [HttpPut("{guid:Guid}", Order = 1)]
        public VotingProcess Put(Guid guid, [FromBody] VotingProcess scrutin)
        {
            return votingProcessService.PutScrutin(Guid.Parse(guid.ToString()), scrutin, User);
        }
        /// <summary>
        /// Deletes the specified unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        [HttpDelete("{guid:Guid}", Order = 1)]
        public void Delete(Guid guid)
        {
            votingProcessRepository.DeleteByGuid(Guid.Parse(guid.ToString()));
        }
        /// <summary>
        /// Clores the specified unique identifier.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [HttpPost("{guid}/clore")]
        public IActionResult Clore(string guid)
        {
            try
            {
                votingProcessService.CloseVotingProcess(new Guid(guid), User);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Gets the resultat.
        /// </summary>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        [HttpGet("{guid}/resultat")]
        public IResultatModel GetResultat(string guid)
        {
            return votingProcessService.GetResultat(new Guid(guid));
        }

        /// <summary>
        /// Resultats the borda.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}/resultatBorda")]
        public ResultatBorda ResultatBorda(int id)
        {
            return votingProcessRepository.CalculateBorda(id);
        }
        #endregion
    }
}