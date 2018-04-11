using AutoMapper;
using Cooks.Service.Business.Entities.FoodBusiness;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Mappings.FoodBusiness
{
    public class FoodBusinessOperatorProfile : Profile
    {
        public FoodBusinessOperatorProfile()
        {
            CreateMap<FoodBusinessOperator, FoodBusinessOperatorDto>();
            CreateMap<FoodBusinessOperatorDto, FoodBusinessOperator>();
        }
    }
}