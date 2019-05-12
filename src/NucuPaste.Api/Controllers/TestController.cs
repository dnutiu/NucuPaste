using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NucuPaste.Api.Controllers
{
    public class TestController : ApiBaseController
    {

        [Authorize]
        [HttpGet]
        IActionResult TestAuthorization()
        {
            return Ok("You're authorized!");
        }
    }
}