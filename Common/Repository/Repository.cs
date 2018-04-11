using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Common.Repository
{
    public class Repository<TEntity>  : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly IMongoCollection<TEntity> _collection;

        public Repository(string connectionString, string databaseName, string collection)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _collection = database.GetCollection<TEntity>(collection);
        }

        public async Task<IEnumerable<TEntity>> GetAll(FilterDefinition<TEntity> filters, ProjectionDefinition<TEntity> projection = null, int? limitResults = null)
        {
            var result = _collection.Find(filters);

            if (limitResults != null)
            {
                result.Limit(limitResults);
            }

            if (projection != null)
            {
                result.Project(projection);
            }

            return await result.ToListAsync();
        }

        public async Task<TEntity> Get(string id)
        {
            var dbResult = await _collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            return dbResult;
        }

        public async Task Delete(string id)
        {
            await _collection.DeleteOneAsync(x => x.Id.Equals(id));
        }

        public async Task Delete(TEntity entity)
        {
            await _collection.DeleteOneAsync(x => x.Id.Equals(entity.Id));
        }

        public async Task<TEntity> Save(TEntity entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id.Equals(entity.Id), entity, new UpdateOptions
            {
                IsUpsert = true
            });
            return entity;
        }

        public async Task Update(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update)
        {
            await _collection.UpdateOneAsync(filter, update, new UpdateOptions
            {
                IsUpsert = false
            });
        }

        public async Task<bool> Exists(string id)
        {
            var entity = await _collection.Find(x => x.Id.Equals(id)).FirstOrDefaultAsync();
            return entity != null;
        }
    }
}
