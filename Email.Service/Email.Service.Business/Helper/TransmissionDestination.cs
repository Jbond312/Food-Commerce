using System.Collections.Generic;

namespace Email.Service.Business.Helper
{
    public class TransmissionDestination
    {
        public string DestinationEmail { get; set; }
        public Dictionary<string, object> SubstitutionData { get; set; }
    }
}