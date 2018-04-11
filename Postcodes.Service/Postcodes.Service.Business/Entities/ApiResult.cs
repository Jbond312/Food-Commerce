using Common.Exceptions;
using Newtonsoft.Json;

namespace Postcodes.Service.Business.Entities
{
    public class ApiResult : ExternalApiError
    {
        public Location Result { get; set; }
        public new bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        [JsonProperty("error")]
        public new string ErrorMessage { get; set; }
    }
}