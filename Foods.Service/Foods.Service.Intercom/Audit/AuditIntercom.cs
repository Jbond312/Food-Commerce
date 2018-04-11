using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Repository;
using Foods.Service.Repository.Audit;

namespace Foods.Service.Intercom.Audit
{
    public class AuditIntercom : IAuditIntercom
    {
        private readonly IRepository<AuditRecord> _repository;

        public AuditIntercom(IRepository<AuditRecord> repository)
        {
            _repository = repository;
        }

        public async Task<string> Create(AuditRecord auditRecord)
        {
            auditRecord.Id = null;
            var result = await _repository.Save(auditRecord);

            return result.Id;
        }

        public async Task Update(AuditRecord auditRecord)
        {
            var existingAuditRecord = await _repository.Get(auditRecord.Id);
            auditRecord.Logs = existingAuditRecord.Logs;
            await _repository.Save(auditRecord);
        }

        public async Task CreateLog(string auditRecordId, AuditLog auditLog)
        {
            var auditRecord = await _repository.Get(auditRecordId); 

            if (auditRecord.Logs == null || !auditRecord.Logs.Any())
            {
                auditRecord.Logs = new List<AuditLog>
                {
                    auditLog
                };
            }
            else
            {
                auditRecord.Logs.Add(auditLog);
            }

            await _repository.Save(auditRecord);
        }
    }
}