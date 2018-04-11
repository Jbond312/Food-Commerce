using System.Collections.Generic;
using SparkPost;

namespace Email.Service.Business.Helper
{
    public class TransmissionPayload
    {
        public IEnumerable<TransmissionDestination> Destinations{ get; set; }
        public IEnumerable<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}