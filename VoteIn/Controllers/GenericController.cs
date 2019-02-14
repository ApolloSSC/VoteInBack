using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VoteIn.BL.Interfaces;
using VoteIn.Model.Models;

namespace VoteIn.Controllers
{
    [Produces("application/json")]
    public class GenericController<T> : Controller where T: class
    {
        /// <summary>
        /// The user manager
        /// </summary>
        protected UserManager<User> UserManager;
        /// <summary>
        /// The repository
        /// </summary>
        protected IRepository<T> repository;
        /// <summary>
        /// The includes
        /// </summary>
        protected string includes = string.Empty;

        #region Public Methods
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual IEnumerable<T> Get()
        {
            return repository.GetAll(includes);
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public virtual T Get(string id)
        {
            return repository.Get(int.Parse(id), includes);
        }

        /// <summary>
        /// Posts the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Registration fail, data are not valid</exception>
        [HttpPost]
        public virtual T Post([FromBody]T obj)
        {
            if (ModelState.IsValid)
            {
                repository.Add(obj);
                return obj;
            }
            else
            {
                throw new Exception("Registration fail, data are not valid");
            }
        }

        /// <summary>
        /// Puts the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Registration fail, data are not valid</exception>
        [HttpPut("{id:int}", Order = 0)]
        public virtual T Put(int id, [FromBody]T obj)
        {
            if (ModelState.IsValid)
            {
                repository.Update(obj);
                return obj;
            }
            else
            {
                throw new Exception("Registration fail, data are not valid");
            }
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        [HttpDelete("{id:int}", Order = 0)]
        public virtual void Delete(int id)
        {
            repository.Delete(id);
        }
        #endregion
    }
}
