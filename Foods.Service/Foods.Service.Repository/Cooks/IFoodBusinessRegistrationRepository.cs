using System.Threading.Tasks;
using Common.Repository;

namespace Foods.Service.Repository.Cooks
{
    public interface IFoodBusinessRegistrationRepository<T> : IRepository<T> where T : FoodBusinessRegistration
    {
        Task<FoodBusinessRegistration> GetByCookId(string cookId);
    }
}
