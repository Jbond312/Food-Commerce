using System.Threading.Tasks;
using Foods.Service.Intercom.Postcode.Entities;

namespace Foods.Service.Intercom.Postcode
{
    public interface IPostcodeIntercom
    {
        Task<Location> GetLocation(string postcode);
    }
}