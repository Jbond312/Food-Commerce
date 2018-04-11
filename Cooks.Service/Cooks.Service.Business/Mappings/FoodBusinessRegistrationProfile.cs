using AutoMapper;
using Cooks.Service.Business.Entities;
using Foods.Service.Repository.Cooks;

namespace Cooks.Service.Business.Mappings
{
    public class FoodBusinessRegistrationProfile : Profile
    {
        public FoodBusinessRegistrationProfile()
        {
            CreateMap<FoodBusinessRegistration, FoodBusinessRegistrationDto>();
            CreateMap<FoodBusinessRegistrationDto, FoodBusinessRegistration>();
        }
    }
}