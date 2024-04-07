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
        public async Task<ActionResult<List<Activities>>> Get()
        {
            try
            {
                // Assuming you have an access token already available
                string accessToken = "336b78f68633c9bc32eb8ee94c7318fd49a10dcb";

                // Call the static method from StravaService to get activities
                List<Activities> activities = await StravaService.GetActivitiesFromStrava(accessToken);

                // Return the list of activities
                return activities;
            }
            catch (Exception ex)
            {
                // If any error occurs, return a 500 Internal Server Error with the error message
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        // GET api/<ActivitiesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ActivitiesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ActivitiesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ActivitiesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
