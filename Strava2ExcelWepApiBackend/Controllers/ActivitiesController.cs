using Microsoft.AspNetCore.Mvc;
using Strava2ExcelWebApiBackend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strava2ExcelWepApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        // GET: api/<ActivitiesController>
        [HttpGet]
        public async Task<ActionResult<List<Activity>>> Get()
        {
            try
            {
                // Assuming you have an access token already available
                string accessToken = "d910eda2ac40b7efbf08ec2d6081e10abc660fc5";

                // Call the static method from StravaService to get activities
                List<Activity> activities = await StravaService.GetActivitiesFromStrava(accessToken);

                // Return the list of activities
                return activities;
            }
            catch (Exception ex)
            {
                // If any error occurs, return a 500 Internal Server Error with the error message
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
