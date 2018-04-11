using System.Threading.Tasks;
using Foods.Service.Intercom.Postcode.Entities;
using Postcodes.Service.Business.Helper;

namespace Foods.Service.Intercom.Postcode
{
    public class PostcodeIntercom : IPostcodeIntercom
    {
        private readonly PostcodesClient _client;

        public PostcodeIntercom(PostcodesClient client)
        {
            _client = client;
        }

        public async Task<Location> GetLocation(string postcode)
        {
            var result = await _client.GetPostcode(postcode);
            return new Location
            {
                Postcode = result?.Result?.Postcode,
                IsValid = result.IsValid,
                ErrorMessage = result.ErrorMessage,
                Latitude = result.Result?.Latitude ?? 0.0D,
                Longitude = result.Result?.Longitude ?? 0.0D
            };
        }
    }
}