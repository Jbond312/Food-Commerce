using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Attributes;
using Foods.Service.Api.Enums;
using Foods.Service.Intercom.Audit;
using Foods.Service.Repository.Audit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

namespace Foods.Service.Api.FilterAttributes.Audit
{
    public class AuditAttribute : IAsyncActionFilter
    {
        private readonly IAuditIntercom _auditIntercom;

        public AuditAttribute(IAuditIntercom auditIntercom)
        {
            _auditIntercom = auditIntercom;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var requests = context.ActionArguments.ToList();

            var requestString = GetRequestString(requests.Select(x => new FoodsRequest
            {
                Value = x.Value,
                Type = x.Value.GetType()
            }));
            var auditRecord = new AuditRecord
            {
                Request = requestString,
                QueryString = GetQueryString(context.HttpContext.Request),
                CreatedOn = DateTime.UtcNow,
                AuthToken = GetAuthToken(context.HttpContext),
                HttpVerb = context.HttpContext.Request.Method
            };
            await _auditIntercom.Create(auditRecord);

            context.HttpContext.Items.Add(ContextItems.AuditRecordId, auditRecord.Id);

            var executedResult = await next();
            auditRecord.Response = JsonConvert.SerializeObject(executedResult.Result);

            await _auditIntercom.Update(auditRecord);
        }

        private static StringValues GetAuthToken(HttpContext httpContext)
        {
            var fullAuthToken = httpContext.Request.Headers["Authorization"].ToString();
            return fullAuthToken.Replace("Bearer", "").Trim();
        }

        private string GetRequestString(IEnumerable<FoodsRequest> requestObjects)
        {
            var requestString = "";
            foreach (var requestObj in requestObjects.ToList())
            {
                var clone = requestObj.Clone();
                var cloneVal = clone.Type == typeof(string) 
                    ? clone.Value.ToString() 
                    : JsonConvert.DeserializeObject(clone.Value.ToString(), clone.Type);
                foreach (var property in clone.Type.GetProperties())
                {
                    if (property.CustomAttributes.Any(x => x.AttributeType == typeof(Sensitive)))
                    {
                        property.SetValue(cloneVal, "*");
                    }
                }
                requestString += JsonConvert.SerializeObject(cloneVal);
            }
            return requestString;
        }

        private static string GetQueryString(HttpRequest request)
        {
            return request.Path + request.QueryString.Value;
        }
    }
}