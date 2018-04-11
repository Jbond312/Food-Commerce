using AutoMapper;
using Cooks.Service.Business.Entities.FoodBusiness;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Mappings.FoodBusiness
{
    public class ApplicantDetailsProfile : Profile
    {
        public ApplicantDetailsProfile()
        {
            CreateMap<ApplicantDetails, ApplicantDetailsDto>();
            CreateMap<ApplicantDetailsDto, ApplicantDetails>();
        }
    }
}