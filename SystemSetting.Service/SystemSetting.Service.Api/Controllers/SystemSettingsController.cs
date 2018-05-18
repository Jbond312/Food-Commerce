using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemSetting.Service.Business.Entities;
using AutoMapper;
using Common.Exceptions;
using Common.Repository;
using Foods.Service.Repository.SystemSetting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SystemSetting.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
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
        [SwaggerResponse(200, typeof(IEnumerable<SystemSettingsDto>))]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            async Task<IEnumerable<SystemSettingsDto>> GetSystemSettings()
            {
                var cookEntries = await _repository.GetAll(FilterDefinition<SystemSettings>.Empty);

                return cookEntries.Any() ? _mapper.Map<IEnumerable<SystemSettingsDto>>(cookEntries) : new List<SystemSettingsDto>();
            }

            return await Execute(GetSystemSettings);
        }
    }

    public abstract class BaseController : Controller
    {
        private JwtMetaData _jwtMetaData;
        private string _auditRecord;

        protected string AuditRecordId => _auditRecord ?? (_auditRecord = HttpContext.Items[0].ToString()); // Context Item = AuditRecordId
        protected JwtMetaData JwtMetaData => _jwtMetaData ?? (_jwtMetaData = (JwtMetaData)HttpContext.Items[1]); // Context Item = JwtMetaData

        protected async Task<IActionResult> Execute<T>(
            Func<Task<T>> expression)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return new BadRequestResult();
                }
                var result = await expression();
                return Ok(result);
            }
            catch (MongoException exception)
            {
                return StatusCode(500, exception);
            }
            catch (FoodsValidationException exception)
            {
                return BadRequest(exception.Errors);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception);
            }
        }
        protected async Task<IActionResult> Execute(Func<Task> expression)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                await expression();
                return Ok();
            }
            catch (MongoException exception)
            {
                HttpContext.Response.StatusCode = 500;
                return StatusCode(500, exception);
            }
            catch (FoodsValidationException exception)
            {
                return BadRequest(exception.Errors);
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception);
            }
        }
    }

    public class JwtMetaData
    {
        public string CookId { get; set; }
        public string UserId { get; set; }
        public string IdentityUserId { get; set; }
    }
}
