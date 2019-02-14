using System;
using System.Linq;
using System.Linq.Expressions;

namespace VoteIn.BL.Interfaces
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        T Get<TKey>(TKey id, string includes = "");
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        IQueryable<T> GetAll(string includes = "", Expression<Func<T, bool>> filter = null);
        /// <summary>
        /// Adds the specified voting process.
        /// </summary>
        /// <param name="votingProcess">The voting process.</param>
        void Add(T votingProcess);
        /// <summary>
        /// Updates the specified voting process.
        /// </summary>
        /// <param name="votingProcess">The voting process.</param>
        void Update(T votingProcess);
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Delete(T entity);
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="id">The identifier.</param>
        void Delete<TKey>(TKey id);
    }
}
