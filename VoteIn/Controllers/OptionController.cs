using Microsoft.AspNetCore.Mvc;
using VoteIn.BL.Interfaces;
using VoteIn.Model.Models;

namespace VoteIn.Controllers
{
    [Route("api/Option")]
    public class OptionController : GenericController<Option>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public OptionController(IRepository<Option> repo)
        {
            repository = repo;
        }
    }
}