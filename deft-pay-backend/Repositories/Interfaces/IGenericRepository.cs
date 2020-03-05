using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace deft_pay_backend.Repositories.Interfaces
{
    /// <summary>
    /// Interface for all repositories used to request data from the DB
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IGenericRepository<TEntity>
    {

        /// <summary>
        /// Get all enitities that match 'filter', ordered by 'orderBy', and includes 'includeProperties'
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Get an entity using it's primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        TEntity GetByID(object id);

        /// <summary>
        /// Insert an object into the DB
        /// </summary>
        /// <param name="entity"></param>
        void Insert(TEntity entity);

        /// <summary>
        /// Delete an entity using it's primary key
        /// </summary>
        /// <param name="id"></param>
        void Delete(object id);

        /// <summary>
        /// Delete a single entity
        /// </summary>
        /// <param name="entityToDelete"></param>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Delete all the passed entities
        /// </summary>
        /// <param name="entitiesToDelete"></param>
        void DeleteRange(IEnumerable<TEntity> entitiesToDelete);

        /// <summary>
        /// Update the specified entity
        /// </summary>
        /// <param name="entityToUpdate"></param>
        void Update(TEntity entityToUpdate);

        /// <summary>
        /// Save the cahnges made to the DB
        /// </summary>
        void Save();
    }
}
