using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Common.Repository;
using Common.Validation;
using Cooks.Service.Business.Entities;
using Cooks.Service.Business.Helpers;
using Cooks.Service.Business.Validators;
using Cooks.Service.Business.Validators.Models;
using Foods.Service.Repository.Cooks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Cooks.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Authorize]
    public class CooksController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Cook> _cookRepo;
        private readonly IFoodBusinessRegistrationRepository<FoodBusinessRegistration> _foodBusRepo;
        private readonly IEntityValidator<CookDto> _cookValidator;
        private readonly IEntityValidator<FoodBusinessRegistrationValidatorModel> _foodBusValidator;
        private readonly ILocationHelper _locationHelper;

        public CooksController(
            IMapper mapper,
            IRepository<Cook> cookRepo,
            IFoodBusinessRegistrationRepository<FoodBusinessRegistration> foodBusRepo,
            IEntityValidator<CookDto> cookValidator,
            IEntityValidator<FoodBusinessRegistrationValidatorModel> foodBusValidator,
            ILocationHelper locationHelper
            )

        {
            _mapper = mapper;
            _cookRepo = cookRepo;
            _foodBusRepo = foodBusRepo;
            _cookValidator = cookValidator;
            _foodBusValidator = foodBusValidator;
            _locationHelper = locationHelper;
        }

        [HttpPost]
        [Route("Registrations")]
        public async Task<IActionResult> RegisterCook([FromBody]FoodBusinessRegistrationDto foodBusinessRegistrationDto)
        {
            var cookId = JwtMetaData.CookId;

            async Task<FoodBusinessRegistrationDto> RegisterCook()
            {
                foodBusinessRegistrationDto.Id = null;
                foodBusinessRegistrationDto.CookId = cookId;

                var foodBusRegValidModel = new FoodBusinessRegistrationValidatorModel
                {
                    FoodBusinessRegistration = foodBusinessRegistrationDto
                };

                await _foodBusValidator.ValidateEntityAsync(foodBusRegValidModel, foodBusRegValidModel.RuleSet);

                var foodBusinessReg = _mapper.Map<FoodBusinessRegistration>(foodBusinessRegistrationDto);
                var savedFoodBusinessReg = await _foodBusRepo.Save(foodBusinessReg);

                return _mapper.Map<FoodBusinessRegistrationDto>(savedFoodBusinessReg);
            }

            return await Execute(RegisterCook);


        }

        [HttpPut]
        [Route("Registrations/{foodRegistrationId}")]
        public async Task<IActionResult> UpdateCookRegistration(string foodRegistrationId, [FromBody]FoodBusinessRegistrationDto foodBusinessRegDto)
        {
            var cookId = JwtMetaData.CookId;

            async Task<FoodBusinessRegistrationDto> UpdateCookRegistration()
            {
                foodBusinessRegDto.Id = foodRegistrationId;
                foodBusinessRegDto.CookId = cookId;

                var foodBusRegValidModel = new FoodBusinessRegistrationValidatorModel
                {
                    FoodBusinessRegistration = foodBusinessRegDto,
                };

                await _foodBusValidator.ValidateEntityAsync(foodBusRegValidModel, foodBusRegValidModel.RuleSet);

                var foodBusinessReg = _mapper.Map<FoodBusinessRegistration>(foodBusinessRegDto);
                return _mapper.Map<FoodBusinessRegistrationDto>(await _foodBusRepo.Save(foodBusinessReg));
            }

            return await Execute(UpdateCookRegistration);
        }

        [HttpGet]
        [Route("Registrations/{foodRegistrationId}")]
        public async Task<IActionResult> GetCookRegistration(string foodRegistrationId)
        {
            var cookId = JwtMetaData.CookId;

            async Task<FoodBusinessRegistrationDto> GetCookRegistration()
            {
                var foodBusReg = await _foodBusRepo.Get(foodRegistrationId);

                if (foodBusReg == null || foodBusReg.CookId != cookId)
                {
                    return null;
                }

                return _mapper.Map<FoodBusinessRegistrationDto>(foodBusReg);
            }

            return await Execute(GetCookRegistration);
        }

        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update([FromBody]CookDto cookDto)
        {
            var cookId = JwtMetaData.CookId;
            async Task<CookDto> UpdateCook()
            {
                cookDto.Id = cookId;

                await _cookValidator.ValidateEntityAsync(cookDto, RuleSets.Cooks.ToString());

                var cook = _mapper.Map<Cook>(cookDto);

                await _locationHelper.SetPostcodeLocation(cook.Address, cookDto.Id);

                var savedCook = _mapper.Map<CookDto>(await _cookRepo.Save(cook));

                if (savedCook != null)
                {
                    var foodBusReg = await _foodBusRepo.GetByCookId(cookId);
                    savedCook.FoodBusinessRegistrationId = foodBusReg?.Id;
                }

                return savedCook;
            }

            return await Execute(UpdateCook);
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get()
        {
            var cookId = JwtMetaData.CookId;

            async Task<CookDto> GetCook()
            {
                var cook = await _cookRepo.Get(cookId);

                var cookDto = _mapper.Map<CookDto>(cook);

                if (cookDto != null)
                {
                    var foodBusReg = await _foodBusRepo.GetByCookId(cookId);
                    cookDto.FoodBusinessRegistrationId = foodBusReg?.Id;
                }

                return cookDto;
            }

            return await Execute(GetCook);
        }

        [HttpGet]
        [Route("Nearby")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNearbyCooks(string postCode, double? searchRadiusInMiles)
        {
            async Task<IEnumerable<dynamic>> GetCook()
            {
                if (searchRadiusInMiles == null || searchRadiusInMiles < 0)
                {
                    searchRadiusInMiles = 2;
                }

                var location = await _locationHelper.GetPostcodeLocation(postCode);

                var coords = GetCoords(location.Coordinates.ToList());

                var nearbyCooks = await _locationHelper.GetNearbyCooks(coords.longitude, coords.latitude, (double)searchRadiusInMiles);

                var cooks = _mapper.Map<IEnumerable<CookDto>>(nearbyCooks);

                return cooks.Select(x => new
                {
                    x.Id,
                    x.DisplayName,
                    x.Categories
                });

            }

            (double longitude, double latitude) GetCoords(IReadOnlyList<double> coords)
            {
                return (longitude: coords[0], latitude: coords[1]);
            }

            return await Execute(GetCook);
        }

        [HttpDelete]
        [Route("")]
        public async Task<IActionResult> Delete()
        {
            var cookId = JwtMetaData.CookId;

            async Task DeleteCook()
            {
                var foodBusReg = await _foodBusRepo.GetByCookId(cookId);

                if (foodBusReg != null)
                {
                    await _foodBusRepo.Delete(foodBusReg.Id);
                }

                await _cookRepo.Delete(cookId);
            }

            return await Execute(DeleteCook);
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
