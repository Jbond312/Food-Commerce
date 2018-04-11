using System.Threading.Tasks;
using Foods.Service.Repository.SystemSetting;

namespace Foods.Service.Intercom.SystemSetting
{
    public interface ISystemSettingIntercom
    {
        Task<SystemSettings> GetAllSystemSettings();
    }
}
