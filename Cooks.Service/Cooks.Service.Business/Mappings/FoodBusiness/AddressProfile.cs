using AutoMapper;
using Cooks.Service.Business.Entities.FoodBusiness;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Mappings.FoodBusiness
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressDto>();
            CreateMap<AddressDto, Address>();
        }
    }
}