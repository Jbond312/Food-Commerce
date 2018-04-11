using AutoMapper;
using Cooks.Service.Business.Entities;
using Foods.Service.Repository.Cooks;

namespace Cooks.Service.Business.Mappings
{
    public class CookMappingProfile : Profile
    {
        public CookMappingProfile()
        {
            CreateMap<Cook, CookDto>();
            CreateMap<CookDto, Cook>();
        }
    }
}