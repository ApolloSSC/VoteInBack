using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VoteIn.Model.Models;

namespace VoteIn.Controllers
{
    [Route("api/User")]
    public class UserController : GenericController<User>
    {
        #region Ctors.Dtors
        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="_userManager">The user manager.</param>
        public UserController(UserManager<User> _userManager)
        {
            UserManager = _userManager;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public override IEnumerable<User> Get()
        { 
            return UserManager.Users;
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public override User Get(string id)
        {
            User usr = UserManager.Users
                    .FirstOrDefault(u => u.Id.Equals(id));
            return usr;
        }

        /// <summary>
        /// Posts the specified user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Utilisez /Auth/Register pour enregistrer un nouvel utilisateur.</exception>
        [HttpPost]
        public override User Post([FromBody]User user)
        {
            throw new Exception("Utilisez /Auth/Register pour enregistrer un nouvel utilisateur.");
        }
        #endregion
    }
}