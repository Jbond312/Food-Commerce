using System.Linq;
using System.Threading.Tasks;
using Common.Repository;
using Foods.Service.Repository.SystemSetting;
using MongoDB.Driver;

namespace Foods.Service.Intercom.SystemSetting
{
    public class SystemSettingIntercom : ISystemSettingIntercom
    {
        private readonly IRepository<SystemSettings> _repo;

        public SystemSettingIntercom(IRepository<SystemSettings> repo)
        {
            _repo = repo;
        }

        public async Task<SystemSettings> GetAllSystemSettings()
        {
            var systemSettings = (await _repo.GetAll(FilterDefinition<SystemSettings>.Empty)).ToList();
            return systemSettings.Any() ? systemSettings.First() : new SystemSettings();
        }
    }
}
