using System;
using System.Collections.Generic;

namespace Audit.Service.Business.Entities
{
    public class AuditRecordDto
    {
        
        public string Id { get; set; }
        public string CommandName { get; set; }
        public string QueryString { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public List<AuditLogDto> Logs { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}