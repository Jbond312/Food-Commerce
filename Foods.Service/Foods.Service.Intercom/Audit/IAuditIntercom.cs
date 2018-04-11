using System.Threading.Tasks;
using Foods.Service.Repository.Audit;

namespace Foods.Service.Intercom.Audit
{
    public interface IAuditIntercom
    {
        Task<string> Create(AuditRecord auditRecord);
        Task Update(AuditRecord auditRecord);
        Task CreateLog(string auditRecordId, AuditLog auditLog);
    }
}