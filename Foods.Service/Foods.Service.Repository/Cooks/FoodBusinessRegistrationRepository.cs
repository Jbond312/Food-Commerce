using System.Linq;
using System.Threading.Tasks;
using Common.Repository;
using MongoDB.Driver;

namespace Foods.Service.Repository.Cooks
{
    public class FoodBusinessRegistrationRepository : Repository<FoodBusinessRegistration>, IFoodBusinessRegistrationRepository<FoodBusinessRegistration>
    {
        public FoodBusinessRegistrationRepository(string connectionString, string databaseName, string collection) : base(connectionString, databaseName, collection)
        {
        }

        public async Task<FoodBusinessRegistration> GetByCookId(string cookId)
        {
            var filter = new FilterDefinitionBuilder<FoodBusinessRegistration>().Eq(x => x.CookId, cookId);
            var result = await GetAll(filter);
            return result.FirstOrDefault();
        }
    }
}
