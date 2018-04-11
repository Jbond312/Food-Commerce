using System.Collections.Generic;

namespace Cooks.Service.Business.Entities
{
    public class LocationDto
    {
        public string Type { get; set; }
        public IEnumerable<double> Coordinates { get; set; }
    }
}
