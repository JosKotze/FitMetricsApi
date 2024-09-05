using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strava2ExcelWebApiBackend.Data;
using FitmetricModel = Strava2ExcelWebApiBackend.Models;
using System;
using System.Threading.Tasks;
using Strava2ExcelWebApiBackend.Models;
using Strava2ExcelWebApiBackend.Interfaces;
using Newtonsoft.Json;

namespace Strava2ExcelWepApiBackend.Controllers
{
    public class ActivitiesController(StravaDbContext context, IStravaService stravaService) : ControllerBase
    {
        //private readonly IStravaService _stravaService;

        //public ActivitiesController(IStravaService stravaService)
        //{
        //    _stravaService = stravaService;
        //}

        // GET: api/<ActivitiesController>
        [HttpGet("getActivitiesFromStrava")]
        public async Task<ActionResult<List<FitmetricModel.Activity>>> GetActivitiesFromStrava(string accessToken) // future: Will not need
        {
            try
            {
                List<FitmetricModel.Activity> activities = await stravaService.GetActivitiesFromStrava(accessToken);

                return activities;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getActivitiesFromFitMetrics")]
        public async Task<ActionResult<List<FitmetricModel.Activity>>> GetActivitiesFromFitMetrics() // future: Will not need
        {
            try
            {
                var activities = await context.Activities.ToListAsync();
                //List<FitmetricModel.Activity> activities = await _stravaService.GetActivitiesFromStrava(accessToken);

                return activities;
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("saveActivities")]
        public async Task<ActionResult<Activity>> SaveActivities([FromBody] List<Activity> activities)
        {
            if (activities == null)
            {
                return BadRequest("No activity provided.");
            }

            try
            {
                foreach (var activity in activities)
                {
                    await context.Activities.AddAsync(activity);
                }
                await context.SaveChangesAsync();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                return StatusCode(500, $"An error occurred: {ex.Message}. Inner exception: {innerException}");
            }
        }







        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] Activity activity)
        //{
        //    if (activity == null)
        //    {
        //        return BadRequest("No activity provided.");
        //    }

        //    try
        //    {
        //        bool success = await _stravaService.SaveActivity(activity);
        //        if (success)
        //        {
        //            return Ok("Activity saved successfully.");
        //        }
        //        else
        //        {
        //            return StatusCode(500, "Failed to save activity.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}



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
