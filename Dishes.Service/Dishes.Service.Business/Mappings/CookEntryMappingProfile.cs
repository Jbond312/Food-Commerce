using AutoMapper;
using Dishes.Service.Business.Entities;
using Foods.Service.Repository.Dishes;

namespace Dishes.Service.Business.Mappings
{
    public class CookEntryMappingProfile : Profile
    {
        public CookEntryMappingProfile()
        {
            CreateMap<CookEntryDto, CookEntry>().ReverseMap();
            CreateMap<CookEntryOptionHeader, CookEntryOptionHeaderDto>()
                .ForMember(x => x.OptionType, x => x.MapFrom(ot => (OptionTypes)ot.OptionType))
                .ReverseMap();
            CreateMap<CookEntryOption, CookEntryOptionDto>().ReverseMap();
        }
    }
}