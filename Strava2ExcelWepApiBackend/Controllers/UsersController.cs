using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strava2ExcelWebApiBackend.Data;
using Strava2ExcelWebApiBackend.Models;

namespace Strava2ExcelWebApiBackend.Controllers
{
    public class UsersController(StravaDbContext context) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await context.Athletes.ToListAsync();
            return users;
        }
    }
}
