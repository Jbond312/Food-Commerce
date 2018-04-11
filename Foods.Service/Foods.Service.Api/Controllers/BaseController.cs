using System;
using System.Threading.Tasks;
using Common.Exceptions;
using Foods.Service.Api.Enums;
using Foods.Service.Api.FilterAttributes.JwtInterceptor;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace Foods.Service.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        private JwtMetaData _jwtMetaData;
        private string _auditRecord;

        protected string AuditRecordId => _auditRecord ?? (_auditRecord = HttpContext.Items[ContextItems.AuditRecordId].ToString());
        protected JwtMetaData JwtMetaData => _jwtMetaData ?? (_jwtMetaData = (JwtMetaData) HttpContext.Items[ContextItems.JwtMetaData]);

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
}