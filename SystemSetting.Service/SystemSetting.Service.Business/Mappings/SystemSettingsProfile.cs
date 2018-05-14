using SystemSetting.Service.Business.Entities;
using AutoMapper;
using Foods.Service.Repository.SystemSetting;

namespace SystemSetting.Service.Business.Mappings
{
    public class SystemSettingsProfile : Profile
    {
        public SystemSettingsProfile()
        {
            CreateMap<SystemSettings, SystemSettingsDto>().ReverseMap();
            CreateMap<DietaryPreference, DietaryPreferenceDto>().ReverseMap();
        }
    }
}