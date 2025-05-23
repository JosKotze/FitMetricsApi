﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;


/// excel
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Globalization;
using System.Diagnostics;
using Strava2ExcelWebApiBackend.Data;
using Strava2ExcelWebApiBackend.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Azure.Core;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Newtonsoft.Json.Linq;
using Azure;
// we are using EPPlus nuget package

//using com.strava.api.v3.Api;
//using com.strava.api.v3.Client;
//using com.strava.api.v3.Model;

namespace Strava2ExcelWebApiBackend.Models
{
    public partial class StravaService : IStravaService
    {
        private readonly StravaDbContext context;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly HttpClient httpClient;
        public StravaService(StravaDbContext context, IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            this.context = context;
            this.httpClientFactory = httpClientFactory;
            this.httpClient = httpClient;
        }


        public async Task<SaveActivityResult> SaveActivityAsync(string accessToken, int userId, long activityId)
        {
            // Check if the activity already exists
            var existingActivity = await context.Maps
                .FirstOrDefaultAsync(x => x.ActivityId == activityId);

            if (existingActivity != null)
            {
                return SaveActivityResult.AlreadyExists;
            }

            // Fetch and save activity details
            //var activityDetails = await SaveActivityMapAndDetails(accessToken, userId, activityId);
            //var existingActivity = await context.Maps
            //     .Where(x => x.ActivityId == activityId && x.UserId == userId)
            //     .FirstOrDefaultAsync();

            //if (existingActivity == null)
            //{
            //    return SaveActivityResult.Failed;
            //}

            // Save the activity
            //var newActivity = new Map
            //{
            //    ActivityId = activityId,
            //    UserId = userId,
            //    MapData = activityDetails.MapData,
            //};


            await SaveActivityMapAndDetails(accessToken, userId, activityId);

            return SaveActivityResult.Success;
        }

        public async Task SaveActivityMapAndDetails(string accessToken, int userId, long activityId)
        {
            var detailedData = await GetActivityByIdAsync(activityId, true, accessToken);

            // Map to Map entity
            var mapDetails = new Map
            {
                ActivityId = detailedData.id,
                UserId = userId,
                Polyline = detailedData.map?.Polyline, // Use lowercase 'polyline' as defined in Map class
                StartLatlng = detailedData.start_latlng,
                EndLatlng = detailedData.end_latlng
            };

            // Map to ActivityDetails entity
            var activityDetails = new ActivityDetails
            {
                ActivityId = detailedData.id,
                UserId = userId,
                ExternalId = detailedData.external_id,
                UploadId = detailedData.upload_id,
                AthleteId = 0, // You might need to fetch this separately or adjust based on your data model
                Name = detailedData.name,
                Distance = detailedData.distance,
                MovingTime = detailedData.moving_time,
                ElapsedTime = detailedData.elapsed_time,
                TotalElevationGain = detailedData.total_elevation_gain,
                Type = detailedData.type,
                SportType = detailedData.sport_type,
                StartDate = detailedData.start_date,
                StartDateLocal = detailedData.start_date_local,
                Timezone = detailedData.timezone,
                UtcOffset = detailedData.utc_offset,
                StartLatlng = detailedData.start_latlng,
                EndLatlng = detailedData.end_latlng,
                AchievementCount = (int)detailedData.achievement_count,
                KudosCount = (int)detailedData.kudos_count,
                CommentCount = (int)detailedData.comment_count,
                AthleteCount = (int)detailedData.athlete_count,
                PhotoCount = (int)detailedData.photo_count,
                TotalPhotoCount = detailedData.total_photo_count,
                MaxHeartrate = detailedData.max_heartrate,
                AverageHeartrate = detailedData.average_heartrate,
                MapId = detailedData.map?.MapId,
                //Polyline = detailedData.map?.polyline,
                //SummaryPolyline = detailedData.map?.summary_polyline,
                Trainer = detailedData.trainer ?? false,
                Commute = detailedData.commute ?? false,
                Manual = detailedData.manual ?? false,
                Private = detailedData.Private ?? false,
                Flagged = detailedData.flagged ?? false,
                Visibility = detailedData.visibility,
                GearId = detailedData.gear_id,
                //lockdownDeviceName = detailedData.device_name,
                EmbedToken = detailedData.embed_token,
                AverageSpeed = detailedData.average_speed,
                MaxSpeed = detailedData.max_speed,
                AverageCadence = detailedData.average_cadence,
                AverageWatts = detailedData.average_watts,
                WeightedAverageWatts = (int?)detailedData.weighted_average_watts,
                Kilojoules = detailedData.kilojoules,
                DeviceWatts = detailedData.device_watts,
                MaxWatts = (int?)detailedData.max_watts,
                ElevHigh = detailedData.elev_high,
                ElevLow = detailedData.elev_low,
                PrCount = detailedData.pr_count,
                Calories = detailedData.calories,
                HasKudoed = detailedData.has_kudoed,
                WorkoutType = detailedData.workout_type,
                Description = detailedData.description,
                ResourceState = detailedData.resource_state,
                SegmentEffortsJson = detailedData.segment_efforts != null
                    ? JsonConvert.SerializeObject(detailedData.segment_efforts)
                    : null,
                SplitsMetricJson = detailedData.splits_metric != null
                    ? JsonConvert.SerializeObject(detailedData.splits_metric)
                    : null,
                LapsJson = detailedData.laps != null
                    ? JsonConvert.SerializeObject(detailedData.laps)
                    : null
            };

            // Save to database (assuming EF Core)
            await SaveActivityDetailsAsync(activityDetails);
            await SaveActivityMapAsync(mapDetails);
        }

        //// new One with saving MAP.
        //public async Task SyncActivitiesWithDatabaseAsync2(string accessToken, int userId)
        //{
        //    await SyncActivitiesWithDatabaseAsync(accessToken, userId);

        //    List<long> activityIds = await GetActivityIdsByUserIdAsync(userId);

        //    // Step 3: Fetch additional details for each activity
        //    foreach (var id in activityIds)
        //    {
        //        //var detailedData = await GetActivityByIdAsync(activity.ActivityId, true); // Fetch detailed data from Strava
        //        var detailedData = await GetActivityByIdAsync(id, true, accessToken);
        //        var activityDetails = MapToActivityDetails(detailedData, userId); // Map to ActivityDetails model

        //        var mapDetails = MapToMap(id, userId, detailedData.map.Polyline, detailedData.start_latlng, detailedData.end_latlng);
        //        // Step 4: Save activity details to the database

        //        await SaveActivityDetailsAsync(activityDetails);
        //        await SaveActivityMapAsync(mapDetails);
        //    }
        //}

        public async Task<List<long>> GetActivityIdsByUserIdAsync(int userId)
        {
            var activityIds = await context.Activities
                .Where(a => a.UserId == userId) 
                .Select(a => a.ActivityId)   
                .ToListAsync();               

            return activityIds;
        }

        private Activity MapToActivity(StravaActivityData data, int userId)
        {
            return new Activity
            {
                ActivityId = data.id,
                UserId = userId,
                Pace = data.pace,
                Name = data.name,
                Distance = data.distance,
                MovingTime = data.moving_time,
                AverageHeartrate = data.average_heartrate,
                AverageSpeed = data.average_speed,
                TotalElevationGain = data.total_elevation_gain,
                Type = data.type,
                StartDate = data.start_date,
                StartDateLocal = data.start_date_local,
                Timezone = data.timezone,
                MaxHeartrate = data.max_heartrate,
            };
        }

        private static Map MapToMap(long activityId, int userId, string polyLine, List<double>? startLatlng, List<double>? endLatlng)
        {
            return new Map
            {
                ActivityId = activityId,
                UserId = userId,
                Polyline = polyLine,
                StartLatlng = startLatlng,
                EndLatlng = endLatlng,
            };
        }

        //private ActivityDetails MapToActivityDetails(StravaActivityData data, int userId)
        //{
        //    var activity = MapToActivity(data, userId);
        //    return new ActivityDetails
        //    {
        //        // Inherit base properties
        //        Id = activity.Id,
        //        ActivityId = activity.ActivityId,
        //        //UserId = activity.UserId,
        //        //Pace = activity.Pace,
        //        //Name = activity.Name,
        //        //Distance = activity.Distance,
        //        //MovingTime = activity.MovingTime,
        //        //AverageHeartrate = activity.AverageHeartrate,
        //        //AverageSpeed = activity.AverageSpeed,
        //        //TotalElevationGain = activity.TotalElevationGain,
        //        //Type = activity.Type,
        //        //StartDate = activity.StartDate,
        //        //StartDateLocal = activity.StartDateLocal,
        //        //Timezone = activity.Timezone,
        //        //MaxHeartrate = activity.MaxHeartrate,
        //        Description = data.description,
        //        Calories = data.calories,
        //        Trainer = data.trainer,
        //        AverageWatts = data.average_watts,
        //        MaxWatts = data.max_watts,
        //        WeightedAverageWatts = data.weighted_average_watts,
        //        Kilojoules = data.kilojoules,
        //        DeviceWatts = data.device_watts,
        //        ElevHigh = data.elev_high,
        //        ElevLow = data.elev_low,
        //        DeviceName = data.device_name,
        //        EmbedToken = data.embed_token,
        //        //SegmentEffort = data.segment_efforts,
        //        //SplitMetric = data.split_metric,
        //        Laps = data.laps,
        //        KudosCount = data.kudos_count,
        //        CommentCount = data.comment_count,
        //        AchievementCount = data.achievement_count,
        //        AthleteCount = data.athlete_count,
        //        SportType = data.sport_type,
        //        StartLatlng = data.start_latlng,
        //        EndLatlng = data.end_latlng,
                
        //    };
        //}

        // Save basic activity to the database
        private async Task SaveActivityAsync(Activity activity)
        {
            await context.Activities.AddAsync(activity);
            await context.SaveChangesAsync();
        }

        // Save activity details to the database
        private async Task SaveActivityDetailsAsync(ActivityDetails activityDetails)
        {
            await context.ActivityDetails.AddAsync(activityDetails);
            await context.SaveChangesAsync();
        }

        private async Task SaveActivityMapAsync(Map map)
        {
            await context.Maps.AddAsync(map);
            await context.SaveChangesAsync();
        }



        // ----------------------------------------------------------------------------------------------

        public async Task<StravaActivityData> GetActivityByIdAsync(long activityId, bool includeAllEfforts, string token)
        {
            var baseUrl = "https://www.strava.com/api/v3";
            string requestUrl = $"{baseUrl}/activities/{activityId}?include_all_efforts={includeAllEfforts.ToString().ToLower()}";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                return new StravaActivityData
                {
                    resource_state = json["resource_state"]?.Value<int>() ?? 0,
                    name = json["name"]?.Value<string>(),
                    distance = json["distance"]?.Value<double>() ?? 0,
                    moving_time = json["moving_time"]?.Value<int>() ?? 0,
                    elapsed_time = json["elapsed_time"]?.Value<int>() ?? 0,
                    total_elevation_gain = json["total_elevation_gain"]?.Value<double>() ?? 0,
                    type = json["type"]?.Value<string>(),
                    sport_type = json["sport_type"]?.Value<string>(),
                    id = json["id"]?.Value<long>() ?? 0,
                    start_date = json["start_date"]?.Value<DateTime?>(),
                    start_date_local = json["start_date_local"]?.Value<DateTime?>(),
                    timezone = json["timezone"]?.Value<string>(),
                    utc_offset = json["utc_offset"]?.Value<double?>(),
                    achievement_count = json["achievement_count"]?.Value<int>() ?? 0,
                    kudos_count = json["kudos_count"]?.Value<int>() ?? 0,
                    comment_count = json["comment_count"]?.Value<int>() ?? 0,
                    athlete_count = json["athlete_count"]?.Value<int>() ?? 0,
                    photo_count = json["photo_count"]?.Value<int>() ?? 0,
                    map = json["map"]?.ToObject<Map>(),
                    trainer = json["trainer"]?.Value<bool?>(),
                    commute = json["commute"]?.Value<bool?>(),
                    manual = json["manual"]?.Value<bool?>(),
                    Private = json["private"]?.Value<bool?>(),
                    visibility = json["visibility"]?.Value<string>(),
                    flagged = json["flagged"]?.Value<bool?>(),
                    gear_id = json["gear_id"]?.Value<string>(),
                    start_latlng = json["start_latlng"]?.ToObject<List<double>>(),
                    end_latlng = json["end_latlng"]?.ToObject<List<double>>(),
                    average_speed = json["average_speed"]?.Value<double>() ?? 0,
                    max_speed = json["max_speed"]?.Value<double>() ?? 0,
                    average_cadence = json["average_cadence"]?.Value<double?>(),
                    has_heartrate = json["has_heartrate"]?.Value<bool>() ?? false,
                    elev_high = json["elev_high"]?.Value<double?>(),
                    elev_low = json["elev_low"]?.Value<double?>(),
                    upload_id = json["upload_id"]?.Value<long?>(),
                    external_id = json["external_id"]?.Value<string>(),
                    total_photo_count = json["total_photo_count"]?.Value<int>() ?? 0,
                    has_kudoed = json["has_kudoed"]?.Value<bool>() ?? false,
                    description = json["description"]?.Value<string>(),
                    calories = (double)(json["calories"]?.Value<double?>()),
                    device_name = json["device_name"]?.Value<string>(),
                    embed_token = json["embed_token"]?.Value<string>(),
                    pr_count = json["pr_count"]?.Value<int>() ?? 0,
                    segment_efforts = json["segment_efforts"]?.ToObject<List<SegmentEffort>>(),
                    splits_metric = json["splits_metric"]?.ToObject<List<SplitsMetric>>(),
                    laps = json["laps"]?.ToObject<List<Lap>>()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching activity: {ex.Message}");
                throw;
            }
        }



        public async Task SyncActivitiesWithDatabaseAsync(string accessToken, int userId)
        {
            List<StravaActivityData> allActivities = new List<StravaActivityData>();
            int page = 1;
            int perPage = 200;

            var httpClient = httpClientFactory.CreateClient();

            while (true)
            {
                var requestUrl = $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var stravaJsonResponse = await response.Content.ReadAsStringAsync();
                    var stravaActivities = JsonConvert.DeserializeObject<List<StravaActivityData>>(stravaJsonResponse);

                    // Exit loop if there are no more activities
                    if (stravaActivities.Count == 0)
                    {
                        break;
                    }

                    foreach (var stravaActivity in stravaActivities)
                    {
                        // Map the Strava activity to your Activity model
                        var mappedActivity = MapToActivity(stravaActivity, userId);

                        // Check if the activity already exists in the database
                        var existingActivity = await context.Activities
                            .FirstOrDefaultAsync(a => a.ActivityId == mappedActivity.ActivityId && a.UserId == userId);

                        if (existingActivity == null)
                        {
                            // New activity, so add it to the database
                            context.Activities.Add(mappedActivity);
                        }
                    }

                    page++;
                }
                else
                {
                    throw new Exception($"Failed to fetch activities from Strava: {response.StatusCode}");
                }
            }

            // Save all changes to the database after processing all activities
            await context.SaveChangesAsync();
        }

        //public async Task<DateTime?> GetLastActivityDateFromFitMetricsByUserId(int userId)
        //{
        //    var latestStartDate = context.Activities
        //                            .Where(x => x.UserId == userId)
        //                            .OrderBy(x => x.StartDate)
        //                            .Select(x => x.StartDate)
        //                            .FirstOrDefault();

        //    return latestStartDate;
        //}

        public async Task<List<StravaActivityData>> GetActivitiesFromStravaAsync(string accessToken, int userId)
        {
            List<StravaActivityData> allActivities = new List<StravaActivityData>();
            int page = 1;
            int perPage = 200;

            var httpClient = httpClientFactory.CreateClient();

            while (true)
            {
                var requestUrl = $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var stravaJsonResponse = await response.Content.ReadAsStringAsync();

                    var stravaActivities = JsonConvert.DeserializeObject<List<StravaActivityData>>(stravaJsonResponse);

                    if (stravaActivities.Count == 0)
                    {
                        break;
                    }

                    allActivities.AddRange(stravaActivities);

                    page++;
                }
                else
                {
                    throw new Exception($"Failed to fetch activities from Strava: {response.StatusCode}");
                }
            }

            return allActivities;
        }

        public async Task SaveStravaActivitiesAsync(List<StravaActivityData> stravaActivities, int userId)
        {
            if (stravaActivities != null)
            {
                foreach (var stravaActivity in stravaActivities)
                {
                    var activity = MapToActivity(stravaActivity, userId);

                    context.Add(activity);
                }

                await context.SaveChangesAsync();
            }
        }




        //public Activity MapToActivity(StravaActivityData data, int userId)
        //{
        //    return new Activity
        //    {
        //        ActivityId = data.id,
        //        UserId = userId,
        //        Pace = data.pace,
        //        Name = data.name,
        //        Distance = data.distance,
        //        MovingTime = data.moving_time,
        //        AverageHeartrate = data.average_heartrate,
        //        AverageSpeed = data.average_speed,
        //        TotalElevationGain = data.total_elevation_gain,
        //        Type = data.type,
        //        StartDate = data.start_date,
        //        StartDateLocal = data.start_date_local,
        //        Timezone = data.timezone,
        //        MaxHeartrate = data.max_heartrate,
        //        Map = data.map
        //    };
        //}

        //public ActivityDetails MapToActivityDetails(StravaActivityData data, int userId)
        //{
        //    var activity = MapToActivity(data, userId);
        //    return new ActivityDetails
        //    {
        //        // Inherit base properties
        //        Id = activity.Id,
        //        ActivityId = activity.ActivityId,
        //        UserId = activity.UserId,
        //        Pace = activity.Pace,
        //        Name = activity.Name,
        //        Distance = activity.Distance,
        //        MovingTime = activity.MovingTime,
        //        AverageHeartrate = activity.AverageHeartrate,
        //        AverageSpeed = activity.AverageSpeed,
        //        TotalElevationGain = activity.TotalElevationGain,
        //        Type = activity.Type,
        //        StartDate = activity.StartDate,
        //        StartDateLocal = activity.StartDateLocal,
        //        Timezone = activity.Timezone,
        //        MaxHeartrate = activity.MaxHeartrate,
        //        // Add extended details
        //        Description = data.description,
        //        Calories = data.calories,
        //        Trainer = data.trainer,
        //        AverageWatts = data.average_watts,
        //        MaxWatts = data.max_watts,
        //        WeightedAverageWatts = data.weighted_average_watts,
        //        Kilojoules = data.kilojoules,
        //        DeviceWatts = data.device_watts,
        //        ElevHigh = data.elev_high,
        //        ElevLow = data.elev_low,
        //        DeviceName = data.device_name,
        //        EmbedToken = data.embed_token,
        //        SportType = data.sport_type,
        //        StartLatlng = data.start_latlng,
        //        EndLatlng = data.end_latlng,
        //        SegmentEfforts = data.segment_efforts,
        //        SplitMetric = data.split_metric,
        //        Laps = data.laps,
        //    };
        //}




        //public Activity MapToActivity(StravaActivityData stravaActivity, int userId)
        //{
        //    return new Activity
        //    {
        //        Pace = CalculatePace(stravaActivity.type, stravaActivity.average_speed),
        //        ActivityId = stravaActivity.id,
        //        Name = stravaActivity.name,
        //        Distance = stravaActivity.distance,
        //        MovingTime = stravaActivity.moving_time,
        //        ElapsedTime = stravaActivity.elapsed_time,
        //        TotalElevationGain = stravaActivity.total_elevation_gain,
        //        Type = stravaActivity.type,
        //        StartDate = stravaActivity.start_date,
        //        StartDateLocal = stravaActivity.start_date_local,
        //        Timezone = stravaActivity.timezone,
        //        UtcOffset = stravaActivity.utc_offset,
        //        LocationCity = stravaActivity.location_city,
        //        LocationState = stravaActivity.location_state,
        //        LocationCountry = stravaActivity.location_country,
        //        AchievementCount = stravaActivity.achievement_count,
        //        KudosCount = stravaActivity.kudos_count,
        //        Visibility = stravaActivity.visibility,
        //        AverageSpeed = stravaActivity.average_speed,
        //        AverageHeartrate = stravaActivity.average_heartrate,
        //        MaxHeartrate = stravaActivity.max_heartrate,
        //        ExternalId = stravaActivity.external_id,
        //        TotalPhotoCount = stravaActivity.total_photo_count,
        //        UserId = userId,
        //    };
        //}



        //public Activity MapToActivity(StravaActivityData stravaActivity, int userId)
        //{
        //    return new Activity
        //    {
        //        Pace = CalculatePace(stravaActivity.type, stravaActivity.average_speed),
        //        ActivityId = stravaActivity.id,
        //        Name = stravaActivity.name,
        //        Distance = stravaActivity.distance,
        //        MovingTime = stravaActivity.moving_time,
        //        ElapsedTime = stravaActivity.elapsed_time,
        //        TotalElevationGain = stravaActivity.total_elevation_gain,
        //        Type = stravaActivity.type,
        //        StartDate = stravaActivity.start_date,
        //        StartDateLocal = stravaActivity.start_date_local,
        //        Timezone = stravaActivity.timezone,
        //        UtcOffset = stravaActivity.utc_offset,
        //        LocationCity = stravaActivity.location_city,
        //        LocationState = stravaActivity.location_state,
        //        LocationCountry = stravaActivity.location_country,
        //        AchievementCount = stravaActivity.achievement_count,
        //        KudosCount = stravaActivity.kudos_count,
        //        Commentount = stravaActivity.comment_count,
        //        AthleteCount = stravaActivity.athlete_count,
        //        PhotoCount = stravaActivity.photo_count,
        //        Trainer = stravaActivity.trainer,
        //        Commute = stravaActivity.commute,
        //        Manual = stravaActivity.manual,
        //        Private = stravaActivity.Private,
        //        Visibility = stravaActivity.visibility,
        //        Flagged = stravaActivity.flagged,
        //        GearId = stravaActivity.gear_id,
        //        StartLatlng = stravaActivity.start_latlng,
        //        EndLatlng = stravaActivity.end_latlng,
        //        AverageSpeed = stravaActivity.average_speed,
        //        MaxSpeed = stravaActivity.max_speed,
        //        AverageWatts = stravaActivity.average_watts,
        //        MaxWatts = stravaActivity.max_watts,
        //        WeightedAverageWatts = stravaActivity.weighted_average_watts,
        //        Kilojoules = stravaActivity.kilojoules,
        //        DeviceWatts = stravaActivity.device_watts,
        //        HasHeartrate = stravaActivity.has_heartrate,
        //        AverageHeartrate = stravaActivity.average_heartrate,
        //        MaxHeartrate = stravaActivity.max_heartrate,
        //        ElevHigh = stravaActivity.elev_high,
        //        ElevLow = stravaActivity.elev_low,
        //        PrCount = stravaActivity.pr_count,
        //        UploadId = stravaActivity.upload_id,
        //        ExternalId = stravaActivity.external_id,
        //        TotalPhotoCount = stravaActivity.total_photo_count,
        //        HasKudoed = stravaActivity.has_kudoed,
        //        Map = stravaActivity.map,
        //        UserId = userId,
        //    };
        //}

        private string? CalculatePace(string? type, double averageSpeed)
        {
            if (type == "Ride")
            {
                return $"{Math.Round((averageSpeed * 3.6), 2)} km/h";
            }
            else if (type == "Run" || type == "Hike" || type == "Walk")
            {
                double minutesPerKm = 1.0 / (averageSpeed * 0.06);
                return TimeSpan.FromMinutes(minutesPerKm).ToString(@"mm\:ss") + " / km";
            }
            else if (type == "Swim")
            {
                double timePer100Meters = 100.0 / averageSpeed;
                int min = (int)(timePer100Meters / 60);
                int sec = (int)(timePer100Meters % 60);
                return $"{min}:{sec} / 100 meters";
            }
            return null; // Return null for unsupported activity types
        }
    }
}
