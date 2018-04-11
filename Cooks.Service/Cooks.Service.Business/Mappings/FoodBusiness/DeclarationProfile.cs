using AutoMapper;
using Cooks.Service.Business.Entities.FoodBusiness;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Mappings.FoodBusiness
{
    public class DeclarationProfile : Profile
    {
        public DeclarationProfile()
        {
            CreateMap<Declaration, DeclarationDto>();
            CreateMap<DeclarationDto, Declaration>();
        }
    }
}
