using System;
using System.Threading.Tasks;
using Common.Enums;
using Common.Exceptions;
using Common.Repository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using TextContent.Service.Business.Entities;
using System.Linq;
using AutoMapper;
using Foods.Service.Repository.TextContent;

namespace TextContent.Service.Api.Controllers
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