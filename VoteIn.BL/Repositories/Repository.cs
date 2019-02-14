using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using VoteIn.BL.Interfaces;
using VoteIn.DAL;

namespace VoteIn.BL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly VoteInContext Context;
        protected DbSet<T> DbSet;

        #region Public Methods        
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(VoteInContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }
        /// <summary>
        /// Adds the specified voting process.
        /// </summary>
        /// <param name="votingProcess">The voting process.</param>
        public virtual void Add(T votingProcess)
        {
            DbSet.Add(votingProcess);

            Save();
        }
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void Delete(T entity)
        {
            AttachIfDetached(entity);
            DbSet.Remove(entity);
            Save();
        }
        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="id">The identifier.</param>
        public void Delete<TKey>(TKey id)
        {
            var entity = DbSet.Find(id);
            DbSet.Remove(entity);
            Save();
        }
        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="includes">The includes.</param>
        /// <returns></returns>
        public virtual T Get<TKey>(TKey id, string includes = "")
        {
            T entity = DbSet.Find(id);
            includeProperties(entity, includes);
            return entity;
        }
        /// <summary>
        /// Includes the properties.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="includes">The includes.</param>
        protected void includeProperties(object entity, string includes)
        {
            if (string.IsNullOrEmpty(includes)) return;

            foreach (var str in includes.Split(','))
            {
                var entry = Context.Entry(entity);
                var pieces = str.Split(new[] { '.' }, 2);
                var navig = entry.Navigation(pieces[0]);
                navig.Load();

                if (pieces.Length == 2)
                {
                    var newEnt = navig.CurrentValue;
                    if (newEnt.GetType().GetInterfaces().Contains(typeof(IEnumerable)))
                    {
                        var collec = (IEnumerable)newEnt;
                        foreach (var ent in collec)
                        {
                            includeProperties(ent, pieces[1]);
                        }
                    }
                    else
                    {
                        includeProperties(newEnt, pieces[1]);
                    }
                }
            }
        }
        /// <summary>
        /// Gets all.
        /// </summary>
        /// <param name="includes">The includes.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public IQueryable<T> GetAll(string includes = "", Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return string.IsNullOrEmpty(includes) ? query : includes.Split(',').Aggregate(query, (current, str) => current.Include(str));
        }
        /// <summary>
        /// Updates the specified voting process.
        /// </summary>
        /// <param name="votingProcess">The voting process.</param>
        public virtual void Update(T votingProcess)
        {
            AttachIfDetached(votingProcess);
            Context.Entry(votingProcess).State = EntityState.Modified;
            Save();
        }
        /// <summary>
        /// Saves this instance.
        /// </summary>
        protected void Save()
        {
            Context.SaveChanges();
        }
        /// <summary>
        /// Attaches if detached.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected void AttachIfDetached(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
        }
        #endregion
    }
}