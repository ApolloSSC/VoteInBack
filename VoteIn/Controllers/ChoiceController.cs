using Microsoft.AspNetCore.Mvc;
using VoteIn.BL.Interfaces;
using VoteIn.Model.Models;

namespace VoteIn.Controllers
{
    [Route("api/Choice")]
    public class ChoiceController : GenericController<Choice>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChoiceController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public ChoiceController(IRepository<Choice> repo)
        {
            repository = repo;
        }
    }
}