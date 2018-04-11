using AutoMapper;
using Cooks.Service.Business.Entities.FoodBusiness;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Mappings.FoodBusiness
{
    public class FoodBusinessEstablishmentProfile : Profile
    {
        public FoodBusinessEstablishmentProfile()
        {
            CreateMap<FoodBusinessEstablishment, FoodBusinessEstablishmentDto>();
            CreateMap<FoodBusinessEstablishmentDto, FoodBusinessEstablishment>();
        }
    }
}
