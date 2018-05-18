using Microsoft.AspNetCore.Mvc;

namespace Foods.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    //[Authorize]
    public class SystemSettingsController : BaseController
    {
        

        public SystemSettingsController(
           
            )
        {
            
        }

    }
}
