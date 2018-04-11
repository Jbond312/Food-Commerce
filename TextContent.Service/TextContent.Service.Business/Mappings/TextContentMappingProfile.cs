using AutoMapper;
using Foods.Service.Repository.TextContent;
using TextContent.Service.Business.Entities;

namespace TextContent.Service.Business.Mappings
{
    public class TextContentMappingProfile : Profile
    {
        public TextContentMappingProfile()
        {
            CreateMap<TextContents, TextContentDto>();
            CreateMap<TranslationDto, Translation>().ReverseMap();
        }
    }
}