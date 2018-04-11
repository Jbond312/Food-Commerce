using AutoMapper;
using Cooks.Service.Business.Entities.FoodBusiness;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Mappings.FoodBusiness
{
    public class SignatoryProfile : Profile
    {
        public SignatoryProfile()
        {
            CreateMap<Signatory, SignatoryDto>();
            CreateMap<SignatoryDto, Signatory>();
        }
    }
}