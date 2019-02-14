using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VoteIn.BL.Interfaces;
using VoteIn.Model.ViewModels;

namespace VoteIn.Controllers
{
    [Route("api/Envelope")]
    public class EnvelopeController
    {
        /// <summary>
        /// The envelope repository
        /// </summary>
        private IEnveloppeRepository _envelopeRepository;

        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="EnvelopeController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public EnvelopeController(IEnveloppeRepository repo)
        {
            _envelopeRepository = repo;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Voters the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        [HttpPost]
        [Route("Voter")]
        public void Voter([FromBody]EnvelopeViewModel obj)
        {
            _envelopeRepository.HasVoted(obj.IdVoter, obj.Token, obj.Envelope);
        }
        #endregion
    }
}