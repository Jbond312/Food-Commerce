using AutoMapper;
using Cooks.Service.Business.Entities.FoodBusiness;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Cooks.Service.Business.Mappings.FoodBusiness
{
    public class ApplicantBusinessProfile : Profile
    {
        public ApplicantBusinessProfile()
        {
            CreateMap<ApplicantBusiness, ApplicantBusinessDto>();
            CreateMap<ApplicantBusinessDto, ApplicantBusiness>();
        }
    }
}