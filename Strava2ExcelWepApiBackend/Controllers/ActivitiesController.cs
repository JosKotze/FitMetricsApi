using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Strava2ExcelWebApiBackend.Data;
using FitmetricModel = Strava2ExcelWebApiBackend.Models;
using System;
using System.Threading.Tasks;
using Strava2ExcelWebApiBackend.Models;
using Strava2ExcelWebApiBackend.Interfaces;
using Newtonsoft.Json;
using Strava2ExcelWebApiBackend.Controllers;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Azure.Core;
using Strava2ExcelWebApiBackend.Helpers;
using Strava2ExcelWebApiBackend.Extensions;
using static Strava2ExcelWebApiBackend.Models.StravaService;
using Microsoft.AspNetCore.Authorization;

namespace Strava2ExcelWepApiBackend.Controllers
{
    public class ActivitiesController(StravaDbContext context, IStravaService stravaService) : BaseApiController
    {
        [HttpGet("getActivityById")]
        public async Task<ActionResult<FitmetricModel.Activity>> GetActivityById(int userId, long activityId)
        {
            try
            {
                var activity = await context.Activities.FirstOrDefaultAsync(x => x.ActivityId == activityId && x.UserId == userId);

                return Ok(activity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("saveActivityMap")]
        public async Task<ActionResult> SaveActivityMap(string accessToken, int userId, long activityId)
        {
            try
            {
                var result = await stravaService.SaveActivityAsync(accessToken, userId, activityId);

                if (result == SaveActivityResult.AlreadyExists)
                {
                    return Conflict("Activity map already exists.");
                }

                if (result == SaveActivityResult.Failed)
                {
                    return BadRequest("Failed to save activity details.");
                }

                return Ok("Activity details saved successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("getActivityMap")]
        public async Task<ActionResult<Map>> getActivityMap([FromBody] GetActivityMapRequest request)
        {
            try
            {
                var map = await context.Maps
                    .Where(x => x.ActivityId == request.ActivityId && x.UserId == request.UserId)
                    .FirstOrDefaultAsync();
                if (map == null)
                {
                    //var accessToken = request.AccessToken?.Trim('"');

                    var result = await stravaService.SaveActivityAsync(request.AccessToken, request.UserId, request.ActivityId);

                    if (result == SaveActivityResult.Success)
                    {
                        var mapRetry = await context.Maps
                            .Where(x => x.ActivityId == request.ActivityId && x.UserId == request.UserId)
                            .FirstOrDefaultAsync();

                        return Ok(mapRetry);
                    }
                }

                return Ok(map);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"internal server error: {ex.Message}");
            }
        }

        public class GetActivityMapRequest
        {
            public string AccessToken { get; set; }
            public int UserId { get; set; }
            public long ActivityId { get; set; }
        }

        [HttpGet("syncActivities")]
        public async Task<ActionResult> SyncActivities(int userId, string accessToken)
        {
            try
            {
                await stravaService.SyncActivitiesWithDatabaseAsync(accessToken, userId);
                return Ok("Sync completed successfully.");
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

        [HttpGet("getActivityByTypeByYear")]
        public async Task<ActionResult<List<FitmetricModel.Activity>>> GetActivityByTypeByYear(int year, string type, int userId)
        {
            try
            {
                var activities = await context.Activities.
                    Where(x => x.StartDate.Value.Year == year && x.Type == type && x.UserId == userId).OrderBy(x => x.StartDate).ToListAsync();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getPagedActivities")]
        public async Task<ActionResult<IEnumerable<FitmetricModel.Activity>>> GetPagedActivities([FromQuery] ActivityParams activityParams)
        {
            // Modify the query to use IQueryable and order by StartDate
            var query = context.Activities
                                .OrderByDescending(a => a.StartDate) // Order by StartDate in ascending order
                                .AsQueryable();

            // Apply pagination
            var pagedActivities = await PagedList<FitmetricModel.Activity>.CreateAsync(query, activityParams.pageNumber, activityParams.PageSize);

            // Add pagination header
            Response.AddPaginationHeader(pagedActivities);

            // Return the paginated result
            return Ok(pagedActivities);
        }


        //[HttpGet("getPagedActivities")]
        //public async Task<ActionResult<IEnumerable<FitmetricModel.Activity>>> GetPagedActivities([FromQuery] ActivityParams activityParams)
        //{
        //    // Modify the query to use IQueryable instead of fetching all data immediately
        //    var query = context.Activities.AsQueryable();

        //    // Apply pagination
        //    var pagedActivities = await PagedList<FitmetricModel.Activity>.CreateAsync(query, activityParams.pageNumber, activityParams.PageSize);

        //    // Add pagination header
        //    Response.AddPaginationHeader(pagedActivities);

        //    // Return the paginated result
        //    return Ok(pagedActivities);
        //}

        [HttpGet("getSavedActivities")]
        public async Task<ActionResult<List<FitmetricModel.Activity>>> GetSavedActivities()
        {
            try
            {
                var activities = await context.Activities.ToListAsync();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getActivitiesByType")]
        public async Task<ActionResult<IEnumerable<FitmetricModel.Activity>>> getActivitiesByType(string type, int userId)
        {
            try
            {
                var activities = await context.Activities.Where(x => x.Type == type && x.UserId == userId).ToListAsync();
                return Ok(activities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("getLatestActivity")]
        public async Task<ActionResult<IEnumerable<FitmetricModel.Activity>>> GetLatestActivity(int userId)
        {
            try
            {
                var activity = await context.Activities.OrderByDescending(x => x.StartDate)
                                                        .Where(x => x.UserId == userId)
                                                        .FirstOrDefaultAsync();
                return Ok(activity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }

}
