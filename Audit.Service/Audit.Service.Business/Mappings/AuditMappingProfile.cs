using Audit.Service.Business.Entities;
using AutoMapper;
using Foods.Service.Repository.Audit;

namespace Audit.Service.Business.Mappings
{
    public class AuditMappingProfile : Profile
    {
        public AuditMappingProfile()
        {
            CreateMap<AuditRecordDto, AuditRecord>().ReverseMap();
            CreateMap<AuditLogDto, AuditLog>().ReverseMap();
        }
    }
}