using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace IdentityServerApi.Controllers
{
    public class IdentityController : ControllerBase
    {
        [HttpGet, Route("JustAdmin")]
        [Authorize(Policy = "admin")]
        public IActionResult JustAdmin()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value, ruolo = "admin" });
        }

        [HttpGet, Route("JustUser")]
        [Authorize(Policy = "user")]
        public IActionResult JustUser()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value, ruolo = "user" });
        }

        [HttpGet, Route("Everybody")]
        [AllowAnonymous]
        public IActionResult Everybody()
        {
            return new JsonResult("Everybody can see this method!");
        }

    }
}
