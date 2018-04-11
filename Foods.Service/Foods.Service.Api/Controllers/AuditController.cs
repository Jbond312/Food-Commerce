using System.Threading.Tasks;
using Audit.Service.Business.Entities;
using AutoMapper;
using Common.Repository;
using Foods.Service.Repository.Audit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foods.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Authorize]
    public class AuditController : BaseController
    {
        private readonly IRepository<AuditRecord> _repo;
        private readonly IMapper _mapper;

        public AuditController(IRepository<AuditRecord> repo, IMapper mapper
        ) 
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{auditRecordId}")]
        public async Task<IActionResult> Get(string auditRecordId)
        {
            async Task<AuditRecordDto> GetAuditRecord()
            {
                var auditRecordDto = await _repo.Get(auditRecordId);
                return _mapper.Map<AuditRecordDto>(auditRecordDto);
            }

            return await Execute(GetAuditRecord);
        }
    }
}