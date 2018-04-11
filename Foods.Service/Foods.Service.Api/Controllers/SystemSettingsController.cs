using System.Linq;
using System.Threading.Tasks;
using SystemSetting.Service.Business.Entities;
using AutoMapper;
using Common.Repository;
using Foods.Service.Repository.SystemSetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Foods.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Authorize]
    public class SystemSettingsController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IRepository<SystemSettings> _repository;

        public SystemSettingsController(
            IMapper mapper,
            IRepository<SystemSettings> repository
            )
        {
            _mapper = mapper;
            _repository = repository;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            async Task<SystemSettingsDto> GetSystemSettings()
            {
                var cookEntries = await _repository.GetAll(FilterDefinition<SystemSettings>.Empty);

                return cookEntries.Any() ? _mapper.Map<SystemSettingsDto>(cookEntries) : new SystemSettingsDto();
            }

            return await Execute(GetSystemSettings);
        }
    }
}
