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
// we are using EPPlus nuget package



namespace Strava2ExcelWebApiBackend.Models
{
    public class StravaService : IStravaService
    {
        private readonly StravaDbContext context;
        private readonly IHttpClientFactory httpClientFactory;

        public StravaService(StravaDbContext context, IHttpClientFactory httpClientFactory)
        {
            this.context = context;
            this.httpClientFactory = httpClientFactory;
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
                            .FirstOrDefaultAsync(a => a.Id == mappedActivity.Id);

                        if (existingActivity == null)
                        {
                            // New activity, so add it to the database
                            context.Activities.Add(mappedActivity);
                        }
                        else
                        {
                            // Optionally update the existing activity, if necessary
                            existingActivity.Name = mappedActivity.Name;
                            existingActivity.Distance = mappedActivity.Distance;
                            existingActivity.MovingTime = mappedActivity.MovingTime;
                            existingActivity.TotalElevationGain = mappedActivity.TotalElevationGain;
                            existingActivity.Pace = mappedActivity.Pace;

                            context.Activities.Update(existingActivity);
                        }
                    }

                    page++;  // Move to the next page of activities
                }
                else
                {
                    throw new Exception($"Failed to fetch activities from Strava: {response.StatusCode}");
                }
            }

            // Save all changes to the database after processing all activities
            await context.SaveChangesAsync();
        }

        public async Task<DateTime?> GetLastActivityDateFromFitMetricsByUserId(int userId)
        {
            var latestStartDate = context.Activities
                                    .Where(x => x.UserId == userId)
                                    .OrderBy(x => x.StartDate)
                                    .Select(x => x.StartDate)
                                    .FirstOrDefault();

            return latestStartDate;
        }

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

        public Activity MapToActivity(StravaActivityData stravaActivity, int userId)
        {
            return new Activity
            {
                Pace = CalculatePace(stravaActivity.type, stravaActivity.average_speed),
                Name = stravaActivity.name,
                Distance = stravaActivity.distance,
                MovingTime = stravaActivity.moving_time,
                ElapsedTime = stravaActivity.elapsed_time,
                TotalElevationGain = stravaActivity.total_elevation_gain,
                Type = stravaActivity.type,
                StartDate = stravaActivity.start_date,
                StartDateLocal = stravaActivity.start_date_local,
                Timezone = stravaActivity.timezone,
                UtcOffset = stravaActivity.utc_offset,
                LocationCity = stravaActivity.location_city,
                LocationState = stravaActivity.location_state,
                LocationCountry = stravaActivity.location_country,
                AchievementCount = stravaActivity.achievement_count,
                KudosCount = stravaActivity.kudos_count,
                Commentount = stravaActivity.comment_count,
                AthleteCount = stravaActivity.athlete_count,
                PhotoCount = stravaActivity.photo_count,
                Trainer = stravaActivity.trainer,
                Commute = stravaActivity.commute,
                Manual = stravaActivity.manual,
                Private = stravaActivity.Private,
                Visibility = stravaActivity.visibility,
                Flagged = stravaActivity.flagged,
                GearId = stravaActivity.gear_id,
                StartLatlng = stravaActivity.start_latlng,
                EndLatlng = stravaActivity.end_latlng,
                AverageSpeed = stravaActivity.average_speed,
                MaxSpeed = stravaActivity.max_speed,
                AverageWatts = stravaActivity.average_watts,
                MaxWatts = stravaActivity.max_watts,
                WeightedAverageWatts = stravaActivity.weighted_average_watts,
                Kilojoules = stravaActivity.kilojoules,
                DeviceWatts = stravaActivity.device_watts,
                HasHeartrate = stravaActivity.has_heartrate,
                AverageHeartrate = stravaActivity.average_heartrate,
                MaxHeartrate = stravaActivity.max_heartrate,
                ElevHigh = stravaActivity.elev_high,
                ElevLow = stravaActivity.elev_low,
                PrCount = stravaActivity.pr_count,
                UploadId = stravaActivity.upload_id,
                ExternalId = stravaActivity.external_id,
                TotalPhotoCount = stravaActivity.total_photo_count,
                HasKudoed = stravaActivity.has_kudoed,
                UserId = userId
            };
        }

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
