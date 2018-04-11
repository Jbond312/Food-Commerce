using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Common.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        /// <summary>
        /// Returns the requested value
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="projection"></param>
        /// <param name="limitResults"></param>
        /// <returns>A collection of requested items that match search criteria</returns>
        Task<IEnumerable<TEntity>> GetAll(FilterDefinition<TEntity> filters, ProjectionDefinition<TEntity> projection = null, int? limitResults = null);

        /// <summary>
        /// Returns a single entity based on its unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> Get(string id);

        /// <summary>
        /// Deletes a single entity based on its unique identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(string id);

        /// <summary>
        /// Deletes a single entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Delete(TEntity entity);

        /// <summary>
        /// Saves or updates an entity in the database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> Save(TEntity entity);

        Task Update(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update);

        Task<bool> Exists(string id);
    }
}
