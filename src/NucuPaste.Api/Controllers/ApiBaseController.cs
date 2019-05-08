using Microsoft.AspNetCore.Mvc;

namespace NucuPaste.Api.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]", Name = "[controller]_[action]")]
    public class ApiBaseController : ControllerBase
    {
        
    }
}