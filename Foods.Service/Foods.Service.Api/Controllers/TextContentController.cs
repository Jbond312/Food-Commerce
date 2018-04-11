using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Enums;
using Common.Repository;
using Foods.Service.Repository.TextContent;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TextContent.Service.Business.Entities;

namespace Foods.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class TextContentController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IRepository<TextContents> _repository;

        public TextContentController(IRepository<TextContents> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{pageKey}/{language}")]
        public async Task<IActionResult> Get(string pageKey, Languages language)
        {
            async Task<TextContentDto> GetTextContent()
            {
                var languageFilter = new FilterDefinitionBuilder<TextContents>()
                    .Where(x => x.Language.ToLower() == language.ToString().ToLower()
                            && x.Key.ToLower() == pageKey.ToLower());
                var textContent = await _repository.GetAll(languageFilter);
                return _mapper.Map<TextContentDto>(textContent.FirstOrDefault());
            }

            return await Execute(GetTextContent);
        }
    }
}