using System.Threading.Tasks;
using Common.Repository;
using Foods.Service.Repository.Users.Entities;

namespace Foods.Service.Repository.Users
{
    public interface IUserRepository<T> : IRepository<T> where T : User
    {
        Task<User> GetByEmail(string email);
        Task<User> GetByIdentityId(string identityProviderId);
    }
}
