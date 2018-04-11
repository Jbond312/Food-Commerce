using AutoMapper;
using Cooks.Service.Business.Entities.FoodBusiness;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Mappings.FoodBusiness
{
    public class BusinessOperationProfile : Profile
    {
        public BusinessOperationProfile()
        {
            CreateMap<BusinessOperation, BusinessOperationDto>();
            CreateMap<BusinessOperationDto, BusinessOperation>();
        }
    }
}