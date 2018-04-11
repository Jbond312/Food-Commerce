using System.Collections.Generic;
using System.Threading.Tasks;
using Foods.Service.Repository.Cooks;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Helpers
{
    public interface ILocationHelper
    {
        Task SetPostcodeLocation(Address address, string cookId);
        Task<Location> GetPostcodeLocation(string postcode);
        Task<IEnumerable<Cook>> GetNearbyCooks(double longitude, double latitude, double radiusInMiles);
    }
}
