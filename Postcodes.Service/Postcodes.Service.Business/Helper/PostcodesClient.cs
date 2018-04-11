using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Postcodes.Service.Business.Entities;

namespace Postcodes.Service.Business.Helper
{
    public class PostcodesClient
    {
        public async Task<ApiResult> GetPostcode(string postcode)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://api.postcodes.io/postcodes/" + postcode);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var httpResult = await client.SendAsync(request);
            var result = JsonConvert.DeserializeObject<ApiResult>(await httpResult.Content.ReadAsStringAsync());
            return result;
        }
    }
}