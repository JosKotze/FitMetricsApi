using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Strava2ExcelWebApiBackend.Controllers
{
    public class FallbackController : BaseApiController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            return PhysicalFile(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "index.html"), "text/HTML");
        }
    }
}
