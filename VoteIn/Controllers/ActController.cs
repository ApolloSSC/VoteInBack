using Microsoft.AspNetCore.Mvc;
using VoteIn.BL.Interfaces;
using VoteIn.Model.Models;

namespace VoteIn.Controllers
{
    [Route("api/Act")]
    public class ActController : GenericController<Act>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public ActController(IRepository<Act> repo)
        {
            repository = repo;
        }
    }
}