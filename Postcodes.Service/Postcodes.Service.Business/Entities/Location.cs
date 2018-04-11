using Common.Exceptions;
using Newtonsoft.Json;

namespace Postcodes.Service.Business.Entities
{
    public class Location 
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string Postcode { get; set; }
    }
}