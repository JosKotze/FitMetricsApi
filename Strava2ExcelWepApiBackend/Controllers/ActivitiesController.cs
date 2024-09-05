using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strava2ExcelWebApiBackend.Data;
using FitmetricModel = Strava2ExcelWebApiBackend.Models;
using System;
using System.Threading.Tasks;
using Strava2ExcelWebApiBackend.Models;
using Strava2ExcelWebApiBackend.Interfaces;

namespace Strava2ExcelWepApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly IStravaService _stravaService;

        public ActivitiesController(IStravaService stravaService)
        {
            _stravaService = stravaService;
        }

        // GET: api/<ActivitiesController>
        [HttpGet]
        public async Task<ActionResult<List<FitmetricModel.Activity>>> GetFromStrava(string accessToken) // future: Will not need
        {
            try
            {
                List<FitmetricModel.Activity> activities = await _stravaService.GetActivitiesFromStrava(accessToken);

                return activities;
            }
            catch (Exception ex)
            {
                // If any error occurs, return a 500 Internal Server Error with the error message
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Activity activity)
        {
            if (activity == null)
            {
                return BadRequest("No activity provided.");
            }

            try
            {
                bool success = await _stravaService.SaveActivity(activity);
                if (success)
                {
                    return Ok("Activity saved successfully.");
                }
                else
                {
                    return StatusCode(500, "Failed to save activity.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



        // POST: api/Activities
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] FitmetricModel.Activity activity)
        //{
        //        //List<FitmetricModel.Activity> activities = await 

        //        //return true;
        //        // If any error occurs, return a 500 Internal Server Error with the error message
        //}
    }

}
