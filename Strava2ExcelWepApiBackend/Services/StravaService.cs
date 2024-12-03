using Microsoft.AspNetCore.Mvc;
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
// we are using EPPlus nuget package



namespace Strava2ExcelWebApiBackend.Models
{
    public class StravaService : IStravaService
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

        public async Task SyncActivitiesWithDatabaseAsync2(string accessToken, int userId)
        {
            // Step 1: Sync basic activities
            //var activities = await GetStravaActivitiesAsync(); // Assume this fetches data from Strava
            //var activityModels = activities.Select(data => MapToActivity(data)).ToList();

            //// Step 2: Save basic activities to the database
            //foreach (var activity in activityModels)
            //{
            //    await SaveActivityToDatabaseAsync(activity); // Assume this saves Activity model to the database
            //}
            await SyncActivitiesWithDatabaseAsync(accessToken, userId);

            List<long> activityIds = await GetActivityIdsByUserIdAsync(userId);

            // Step 3: Fetch additional details for each activity
            foreach (var id in activityIds)
            {
                //var detailedData = await GetActivityByIdAsync(activity.ActivityId, true); // Fetch detailed data from Strava
                var detailedData = await GetActivityByIdAsync(userId, true);
                var activityDetails = MapToActivityDetails(detailedData, userId); // Map to ActivityDetails model

                // Step 4: Save activity details to the database
                await SaveActivityDetailsToDatabaseAsync(activityDetails);
            }
        }

        public async Task<List<long>> GetActivityIdsByUserIdAsync(int userId)
        {
            // Using LINQ to query the database
            var activityIds = await context.Activities
                .Where(a => a.UserId == userId) // Filter by UserId
                .Select(a => a.ActivityId)     // Select only the ActivityId column
                .ToListAsync();                // Execute the query and convert to a list

            return activityIds;
        }


        // //Method to fetch activities from Strava
        //private async Task<List<StravaActivityData>> GetStravaActivitiesAsync()
        //{
        //    // Your logic to call Strava API and fetch basic activities
        //    // Example:
        //    return await GetActivitiesAsync();
        //}

        // //Method to fetch activity details from Strava
        //private async Task<StravaActivityData> GetActivityByIdAsync(long activityId)
        //{
        //    // Your logic to call Strava API to get additional details for a specific activity
        //    // Example:
        //    return await GetActivityDetailsByIdAsync(activityId);
        //}

        // Mapping method to convert StravaActivityData to Activity
        private Activity MapToActivity(StravaActivityData data, int userId)
        {
            return new Activity
            {
                ActivityId = data.id,
                UserId = userId,
                //Pace = data.pace,
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
                //Map = data.map != null ? JsonConvert.SerializeObject(data.map) : null
            };
        }

        // Mapping method to convert StravaActivityData to ActivityDetails
        private ActivityDetails MapToActivityDetails(StravaActivityData data, int userId)
        {
            var activity = MapToActivity(data, userId);
            return new ActivityDetails
            {
                // Inherit base properties
                Id = activity.Id,
                ActivityId = activity.ActivityId,
                //UserId = activity.UserId,
                //Pace = activity.Pace,
                //Name = activity.Name,
                //Distance = activity.Distance,
                //MovingTime = activity.MovingTime,
                //AverageHeartrate = activity.AverageHeartrate,
                //AverageSpeed = activity.AverageSpeed,
                //TotalElevationGain = activity.TotalElevationGain,
                //Type = activity.Type,
                //StartDate = activity.StartDate,
                //StartDateLocal = activity.StartDateLocal,
                //Timezone = activity.Timezone,
                //MaxHeartrate = activity.MaxHeartrate,
                //Description = data.description,
                //Calories = data.calories,
                Trainer = data.trainer,
                AverageWatts = data.average_watts,
                MaxWatts = data.max_watts,
                WeightedAverageWatts = data.weighted_average_watts,
                Kilojoules = data.kilojoules,
                DeviceWatts = data.device_watts,
                ElevHigh = data.elev_high,
                ElevLow = data.elev_low,
                //DeviceName = data.device_name,
                //EmbedToken = data.embed_token,
                SportType = data.sport_type,
                StartLatlng = data.start_latlng,
                EndLatlng = data.end_latlng,
                //SegmentEfforts = data.segment_efforts,
                //SplitMetric = data.split_metric,
                //Laps = data.laps,
            };
        }

        // Save basic activity to the database
        private async Task SaveActivityToDatabaseAsync(Activity activity)
        {
            await context.Activities.AddAsync(activity);
            await context.SaveChangesAsync();
        }

        // Save activity details to the database
        private async Task SaveActivityDetailsToDatabaseAsync(ActivityDetails activityDetails)
        {
            await context.ActivityDetails.AddAsync(activityDetails);
            await context.SaveChangesAsync();
        }



        // ----------------------------------------------------------------------------------------------

        public async Task<StravaActivityData> GetActivityByIdAsync(long activityId, bool includeAllEfforts)
        {
            string token = "7619e8c58f0968a81db8bd471290e4316f9ad386"; // Replace securely in production.
            var baseUrl = "https://www.strava.com/api/v3";
            string requestUrl = $"{baseUrl}/activities/{activityId}?include_all_efforts={includeAllEfforts.ToString().ToLower()}";

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using var response = await httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                var activity = JsonConvert.DeserializeObject<StravaActivityData>(content);

                return activity;
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
