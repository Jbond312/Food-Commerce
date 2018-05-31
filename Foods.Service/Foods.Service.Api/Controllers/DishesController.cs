using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Foods.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Authorize]
    public class DishesController : BaseController
    {
        //private readonly IMapper _mapper;
        //private readonly IRepository<CookEntry> _cookEntryRepo;
        //private readonly IEntityValidator<CookEntryDto> _cookEntryValidator;

        public DishesController(
            //IJwtHelper jwtHelper,
            //IMapper mapper,
            //IRepository<CookEntry> cookEntryRepo,
            //IEntityValidator<CookEntryDto> cookEntryValidator
        )
        {
            //_mapper = mapper;
            //_cookEntryRepo = cookEntryRepo;
            //_cookEntryValidator = cookEntryValidator;
        }

        //[HttpGet]
        //[Route("")]
        //public async Task<IActionResult> Get()
        //{
        //    var cookId = JwtMetaData.CookId;

        //    var filter = new FilterDefinitionBuilder<CookEntry>().Eq(x => x.CookId, cookId);

        //    async Task<IEnumerable<CookEntryDto>> GetCookEntries()
        //    {
        //        var cookEntries = await _cookEntryRepo.GetAll(filter);

        //        return _mapper.Map<IEnumerable<CookEntryDto>>(cookEntries);
        //    }

        //    return await Execute(GetCookEntries);
        //}

        //[HttpGet]
        //[Route("{cookEntryId}")]

        //public async Task<IActionResult> Get(string cookEntryId)
        //{
        //    async Task<CookEntryDto> GetCookEntry()
        //    {
        //        var cookEntry = await _cookEntryRepo.Get(cookEntryId);

        //        return cookEntry.CookId != JwtMetaData.CookId ? null : _mapper.Map<CookEntryDto>(cookEntry);
        //    }

        //    return await Execute(GetCookEntry);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] CookEntryDto cookEntryDto)
        //{
        //    async Task<CookEntryDto> SaveCookEntry()
        //    {
        //        cookEntryDto.Id = null;
        //        cookEntryDto.CookId = JwtMetaData.CookId;
        //        await _cookEntryValidator.ValidateEntityAsync(cookEntryDto);

        //        var savedCookEntry = await _cookEntryRepo.Save(_mapper.Map<CookEntry>(cookEntryDto));
        //        return _mapper.Map<CookEntryDto>(savedCookEntry);
        //    }

        //    return await Execute(SaveCookEntry);
        //}

        //[HttpPut]
        //[Route("{cookEntryId}")]
        //public async Task<IActionResult> Put(string cookEntryId, [FromBody] CookEntryDto cookEntryDto)
        //{
        //    async Task<CookEntryDto> SaveCookEntry()
        //    {
        //        cookEntryDto.Id = cookEntryId;
        //        cookEntryDto.CookId = JwtMetaData.CookId;
        //        await _cookEntryValidator.ValidateEntityAsync(cookEntryDto);

        //        var savedCookEntry = await _cookEntryRepo.Save(_mapper.Map<CookEntry>(cookEntryDto));
        //        return _mapper.Map<CookEntryDto>(savedCookEntry);
        //    }

        //    return await Execute(SaveCookEntry);
        //}

        //[HttpDelete]
        //[Route("{cookEntryId}")]
        //public async Task<IActionResult> Delete(string cookEntryId)
        //{
        //    async Task DeleteCookEntry()
        //    {
        //        var cookEntry = await _cookEntryRepo.Get(cookEntryId);

        //        if (cookEntry != null && cookEntry.CookId != JwtMetaData.CookId)
        //        {
        //            throw new FoodsValidationException(nameof(cookEntryId), cookEntryId, $"The {cookEntryId} is either invalid or inaccessible");
        //        }

        //        await _cookEntryRepo.Delete(cookEntryId);
        //    }

        //    return await Execute(DeleteCookEntry);
        //}
    }
}
