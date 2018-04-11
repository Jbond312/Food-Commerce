using System.Linq;
using System.Threading.Tasks;
using Foods.Service.Api.Enums;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Foods.Service.Api.FilterAttributes.JwtInterceptor
{
    public class JwtInterceptorAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var filters = context.ActionDescriptor.FilterDescriptors.ToList().Select(x => x.Filter);
            if (filters.All(x => x.GetType() != typeof(AllowAnonymousFilter)))
            {
                var cookId = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == "cid")?.Value;
                var userId = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
                var identityUserId = context.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
                var metaData = new JwtMetaData
                {
                    CookId = cookId,
                    UserId = userId,
                    IdentityUserId = identityUserId
                };

                context.HttpContext.Items.Add(ContextItems.JwtMetaData, metaData);
            }
            

            await next();
        }
    }
}