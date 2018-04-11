using Audit.Service.Business.Entities;

namespace Audit.Service.Business.Validators.Models
{
    public class AuditLogValidatorModel
    {
        public string AuditRecordId { get; set; }
        public AuditLogDto LogDto { get; set; }
    }
    
}
