using System.Linq;
using System.Threading.Tasks;
using Common.Repository;
using Foods.Service.Repository.Users.Entities;
using MongoDB.Driver;

namespace Foods.Service.Repository.Users
{
    public class UserRepository : Repository<User>, IUserRepository<User>
    {
        public UserRepository(string connectionString, string databaseName, string collection) 
            : base(connectionString, databaseName, collection)
        {
        }

        public async Task<User> GetByEmail(string email)
        {
            var filterBuilder = new FilterDefinitionBuilder<User>();
            var filter = filterBuilder.Where(x => x.Email.Equals(email));
            var existingUser = await GetAll(filter);
            return existingUser.FirstOrDefault();
        }

        public async Task<User> GetByIdentityId(string identityProviderId)
        {
            var filterBuilder = new FilterDefinitionBuilder<User>();
            var filter = filterBuilder.Where(x => x.IdentityProviderId.Equals(identityProviderId));
            var existingUser = await GetAll(filter);
            return existingUser.FirstOrDefault();
        }
    }
}
