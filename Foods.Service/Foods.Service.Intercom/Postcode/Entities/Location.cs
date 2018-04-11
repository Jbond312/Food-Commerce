using Common.Exceptions;

namespace Foods.Service.Intercom.Postcode.Entities
{
    public class Location : ExternalApiError
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Postcode { get; set; }
    }
}