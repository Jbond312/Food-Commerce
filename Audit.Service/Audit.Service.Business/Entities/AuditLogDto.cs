using System;

namespace Audit.Service.Business.Entities
{
    public class AuditLogDto
    {
        public string Data { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}