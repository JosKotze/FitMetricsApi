﻿using Microsoft.AspNetCore.Mvc;
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

namespace Strava2ExcelWepApiBackend.Controllers
{
    public class ActivitiesController(StravaDbContext context, IStravaService stravaService) : BaseApiController
    {
        //private readonly IStravaService _stravaService;

        //public ActivitiesController(IStravaService stravaService)
        //{
        //    _stravaService = stravaService;
        //}

        // GET: api/<ActivitiesController>
        //[HttpGet("getActivitiesFromStrava")]
        //public async Task<ActionResult<List<FitmetricModel.StravaActivityData>>> GetActivitiesFromStrava(string accessToken) // future: Will not need
        //{
        //    try
        //    {
        //        List<FitmetricModel.StravaActivityData> activities = await stravaService.GetActivitiesFromStrava(accessToken);

        //        return activities;
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"An error occurred: {ex.Message}");
        //    }
        //}

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

        //[HttpPost("saveActivitiesFromStrava")]
        //public async Task<ActionResult> SaveActivitiesFromStrava(string accessToken)
        //{
        //    try
        //    {
        //        // Fetch activities from Strava
        //        List<StravaActivityData> stravaActivities = await stravaService.GetActivitiesFromStrava(accessToken);

        //        // Map StravaActivityData to Activity
        //        var activitiesToSave = stravaActivities.Select(activity => new FitmetricModel.Activity
        //        {
        //            Name = activity.Name,
        //            Distance = activity.Distance,
        //            MovingTime = activity.MovingTime,
        //            TotalElevationGain = activity.TotalElevationGain,
        //            Type = activity.Type,
        //            StartDate = activity.StartDate,
        //            StartDateLocal = activity.StartDateLocal,
        //            Timezone = activity.Timezone,
        //            UtcOffset = activity.UtcOffset,
        //            LocationCity = activity.LocationCity,
        //            LocationState = activity.LocationState,
        //            LocationCountry = activity.LocationCountry,
        //            AchievementCount = activity.AchievementCount,
        //            AverageSpeed = activity.AverageSpeed,
        //            MaxSpeed = activity.MaxSpeed,
        //            AverageWatts = activity.AverageWatts,
        //            Kilojoules = activity.Kilojoules,
        //            DeviceWatts = activity.DeviceWatts,
        //            AverageHeartrate = activity.AverageHeartrate,
        //            MaxHeartrate = activity.MaxHeartrate,
        //            UserId = activity.UserId // Ensure this is properly mapped
        //        }).ToList();

        //        // Save mapped activities to the database
        //        await context.Activities.AddRangeAsync(activitiesToSave);
        //        await context.SaveChangesAsync();

        //        return Ok("Activities successfully saved to the database.");
        //    }
        //    catch (Exception ex)
        //    {
        //        var innerException = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
        //        return StatusCode(500, $"An error occurred: {ex.Message}. Inner exception: {innerException}");
        //    }
        //}





        [HttpPost("sync-strava-activities")]
        public async Task<IActionResult> SyncStravaActivities(string accessToken, int userId)
        {
            try
            {
                // Fetch activities from Strava
                var stravaActivities = await stravaService.GetActivitiesFromStravaAsync(accessToken, userId);

                if (stravaActivities == null || !stravaActivities.Any())
                {
                    return NotFound("No activities found from Strava.");
                }

                // Save activities to the database
                await stravaService.SaveStravaActivitiesAsync(stravaActivities, userId);

                return Ok("Strava activities have been synced successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while syncing Strava activities: {ex.Message}");
            }
        }


        //[HttpPost("sync-activities")]
        //public async Task<ActionResult> SyncActivitiesFromStrava(string accessToken, int userId)
        //{
        //    try
        //    {
        //        // Fetch activities from Strava
        //        List<StravaActivityData> stravaActivities = await stravaService.GetActivitiesFromStrava(accessToken);

        //        // Get the existing activities from the database
        //        var existingActivities = await context.Activities.ToListAsync();
        //        var existingActivitiesDict = existingActivities.ToDictionary(a => a.ActivityId);

        //        // Find new and updated activities
        //        foreach (var stravaActivity in stravaActivities)
        //        {
        //            var activity = stravaService.MapToActivity(stravaActivity, userId);

        //            if (!existingActivitiesDict.TryGetValue(stravaActivity.ActivityId, out var existingActivity))
        //            {
        //                // New activity, map and add to the database
        //                await context.Activities.AddAsync(activity);
        //            }
        //            else
        //            {
        //                // Update existing activity if it has changed
        //                existingActivity.Name = activity.Name;
        //                existingActivity.Distance = activity.Distance;
        //                existingActivity.MovingTime = activity.MovingTime;
        //                existingActivity.TotalElevationGain = activity.TotalElevationGain;
        //                existingActivity.Type = activity.Type;
        //                existingActivity.StartDate = activity.StartDate;
        //                existingActivity.StartDateLocal = activity.StartDateLocal;
        //                existingActivity.Timezone = activity.Timezone;
        //                existingActivity.UtcOffset = activity.UtcOffset;
        //                existingActivity.LocationCity = activity.LocationCity;
        //                existingActivity.LocationState = activity.LocationState;
        //                existingActivity.LocationCountry = activity.LocationCountry;
        //                existingActivity.AchievementCount = activity.AchievementCount;
        //                existingActivity.AverageSpeed = activity.AverageSpeed;
        //                existingActivity.MaxSpeed = activity.MaxSpeed;
        //                existingActivity.AverageWatts = activity.AverageWatts;
        //                existingActivity.Kilojoules = activity.Kilojoules;
        //                existingActivity.DeviceWatts = activity.DeviceWatts;
        //                existingActivity.AverageHeartrate = activity.AverageHeartrate;
        //                existingActivity.MaxHeartrate = activity.MaxHeartrate;
        //            }
        //        }

        //        await context.SaveChangesAsync();

        //        return Ok("Activities successfully synced with the database.");
        //    }
        //    catch (Exception ex)
        //    {
        //        var innerException = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
        //        return StatusCode(500, $"An error occurred: {ex.Message}. Inner exception: {innerException}");
        //    }
        //}








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
