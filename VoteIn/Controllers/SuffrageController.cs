using Microsoft.AspNetCore.Mvc;
using VoteIn.BL.Interfaces;
using VoteIn.Model.Models;

namespace VoteIn.Controllers
{
    [Route("api/Suffrage")]
    public class SuffrageController : GenericController<Suffrage>
    {
        /// <summary>
        /// Gets the suffrage repository.
        /// </summary>
        /// <value>
        /// The suffrage repository.
        /// </value>
        private ISuffrageRepository suffrageRepository => (ISuffrageRepository)repository;

        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="SuffrageController"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public SuffrageController(ISuffrageRepository repo)
        {
            repository = repo;
        }
        #endregion
    }
}