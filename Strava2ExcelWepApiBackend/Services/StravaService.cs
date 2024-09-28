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
        //public async Task<List<StravaActivityData>> GetActivitiesFromStrava(string accessToken)
        //{
        //    List<StravaActivityData> allActivities = new List<StravaActivityData>();
        //    int page = 1;
        //    int perPage = 200; // 200 is max we can get from Strava

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        while (true)
        //        {
        //            string apiUrl = $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}";

        //            HttpResponseMessage response = await client.GetAsync(apiUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                List<StravaActivityData> activities = JsonConvert.DeserializeObject<List<StravaActivityData>>(responseBody);

        //                if (activities == null || activities.Count == 0)
        //                {
        //                    // No more activities to fetch, break out of the loop
        //                    break;
        //                }

        //                allActivities.AddRange(activities);

        //                // Increment the page for the next request
        //                page++;
        //            }
        //            else
        //            {
        //                throw new Exception($"Error GetActivitiesFromStrava: {response.StatusCode}");
        //            }
        //        }
        //    }

        //    return allActivities;
        //}

        public async Task<List<StravaActivityData>> GetActivitiesFromStravaAsync(string accessToken, int userId)
        {
            List<StravaActivityData> allActivities = new List<StravaActivityData>();
            int page = 1;
            int perPage = 200; // Maximum number of activities per page allowed by Strava

            var httpClient = httpClientFactory.CreateClient();

            while (true)
            {
                // Set the Strava API request URL with pagination parameters
                var requestUrl = $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}";
                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Send the request to Strava
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON response
                    var stravaJsonResponse = await response.Content.ReadAsStringAsync();

                    // Deserialize the response into a list of StravaActivityData objects
                    var stravaActivities = JsonConvert.DeserializeObject<List<StravaActivityData>>(stravaJsonResponse);

                    if (stravaActivities.Count == 0)
                    {
                        // No more activities to fetch, break out of the loop
                        break;
                    }

                    // Add the fetched activities to the list
                    allActivities.AddRange(stravaActivities);

                    // Move to the next page
                    page++;
                }
                else
                {
                    // Handle errors (e.g., log the error, throw an exception, etc.)
                    throw new Exception($"Failed to fetch activities from Strava: {response.StatusCode}");
                }
            }

            // Return the list of all activities
            return allActivities;
        }

        //public async Task<List<StravaActivityData>> GetActivitiesFromStravaAsync(string accessToken, int userId)
        //{
        //    // Set the Strava API request URL (replace with your desired endpoint)
        //    var httpClient = httpClientFactory.CreateClient();

        //    var requestUrl = "https://www.strava.com/api/v3/athlete/activities";
        //    var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //    // Send the request to Strava
        //    var response = await httpClient.SendAsync(request);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        // Read the JSON response
        //        var stravaJsonResponse = await response.Content.ReadAsStringAsync();

        //        // Use JsonConvert from Newtonsoft.Json to deserialize the response into a list of StravaActivity objects
        //        var stravaActivities = JsonConvert.DeserializeObject<List<StravaActivityData>>(stravaJsonResponse);

        //        // Map and save activities to the database
        //        await SaveStravaActivitiesAsync(stravaActivities, userId);

        //        // Return the list of activities
        //        return stravaActivities;
        //    }
        //    else
        //    {
        //        // Handle errors (e.g., log the error, throw an exception, etc.)
        //        throw new Exception("Failed to fetch activities from Strava.");
        //    }

        //    // Ensure the method always returns a value, even if something goes wrong
        //    return new List<StravaActivityData>();
        //}


        public async Task SaveStravaActivitiesAsync(List<StravaActivityData> stravaActivities, int userId)
        {
            if (stravaActivities != null)
            {
                foreach (var stravaActivity in stravaActivities)
                {
                    // Map to your Activity model
                    var activity = MapToActivity(stravaActivity, userId);

                    // Add to the database context
                    context.Add(activity);
                }

                // Save changes to the database
                await context.SaveChangesAsync();
            }
        }

        public Activity MapToActivity(StravaActivityData stravaActivity, int userId)
        {
            return new Activity
            {
                Pace = CalculatePace(stravaActivity.type, stravaActivity.average_speed),
                Name = stravaActivity.name, // Use `name` from `StravaActivityData`
                Distance = stravaActivity.distance,
                MovingTime = stravaActivity.moving_time,
                ElapsedTime = stravaActivity.elapsed_time, // Added mapping for ElapsedTime
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
                Commentount = stravaActivity.comment_count, // Corrected to match `Activity` property name
                AthleteCount = stravaActivity.athlete_count,
                PhotoCount = stravaActivity.photo_count,
                Trainer = stravaActivity.trainer,
                Commute = stravaActivity.commute,
                Manual = stravaActivity.manual,
                Private = stravaActivity.Private, // Corrected casing
                Visibility = stravaActivity.visibility,
                Flagged = stravaActivity.flagged,
                GearId = stravaActivity.gear_id, // Corrected casing
                StartLatlng = stravaActivity.start_latlng, // Directly map the list
                EndLatlng = stravaActivity.end_latlng, // Directly map the list
                AverageSpeed = stravaActivity.average_speed,
                MaxSpeed = stravaActivity.max_speed,
                AverageWatts = stravaActivity.average_watts,
                MaxWatts = stravaActivity.max_watts,
                WeightedAverageWatts = stravaActivity.weighted_average_watts, // Added mapping for WeightedAverageWatts
                Kilojoules = stravaActivity.kilojoules,
                DeviceWatts = stravaActivity.device_watts,
                HasHeartrate = stravaActivity.has_heartrate, // Corrected casing
                AverageHeartrate = stravaActivity.average_heartrate,
                MaxHeartrate = stravaActivity.max_heartrate,
                ElevHigh = stravaActivity.elev_high, // Corrected casing
                ElevLow = stravaActivity.elev_low, // Corrected casing
                PrCount = stravaActivity.pr_count,
                UploadId = stravaActivity.upload_id,
                ExternalId = stravaActivity.external_id,
                TotalPhotoCount = stravaActivity.total_photo_count,
                HasKudoed = stravaActivity.has_kudoed
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

        //public async Task<bool> SaveActivity(StravaActivityData activity)
        //{
        //    // Validate user and save activity logic here
        //    // Example:
        //    //var user = await _context.Athletes.FindAsync(activity.UserId);
        //    //if (user == null)
        //    //{
        //    //    throw new Exception("User not found.");
        //    //}

        //    _context.Activities.Add(activity);
        //    await _context.SaveChangesAsync();
        //    return true;
        //}

        //public async Task<List<StravaActivityData>> GetActivitiesFromStrava(string accessToken)
        //{
        //    List<StravaActivityData> allActivities = new List<StravaActivityData>();
        //    int page = 1;
        //    int perPage = 200; // 200 is max we can get from Strava

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        while (true)
        //        {
        //            string apiUrl = $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}";

        //            HttpResponseMessage response = await client.GetAsync(apiUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                List<StravaActivityData> activities = JsonConvert.DeserializeObject<List<StravaActivityData>>(responseBody);

        //                if (activities == null || activities.Count == 0)
        //                {
        //                    // No more activities to fetch, break out of the loop
        //                    break;
        //                }

        //                allActivities.AddRange(activities);

        //                // Increment the page for the next request
        //                page++;
        //            }
        //            else
        //            {
        //                throw new Exception($"Error GetActivitiesFromStrava: {response.StatusCode}");
        //            }
        //        }
        //    }

        //    return allActivities;
        //}

        //public Activity MapToActivity(StravaActivityData stravaActivity, int userId)
        //{
        //    return new Activity
        //    {
        //        Name = stravaActivity.Name,
        //        Distance = stravaActivity.Distance,
        //        MovingTime = stravaActivity.MovingTime,
        //        TotalElevationGain = stravaActivity.TotalElevationGain,
        //        Type = stravaActivity.Type,
        //        StartDate = stravaActivity.StartDate,
        //        StartDateLocal = stravaActivity.StartDateLocal,
        //        Timezone = stravaActivity.Timezone,
        //        UtcOffset = stravaActivity.UtcOffset,
        //        LocationCity = stravaActivity.LocationCity,
        //        LocationState = stravaActivity.LocationState,
        //        LocationCountry = stravaActivity.LocationCountry,
        //        AchievementCount = stravaActivity.AchievementCount,
        //        AverageSpeed = stravaActivity.AverageSpeed,
        //        MaxSpeed = stravaActivity.MaxSpeed,
        //        AverageWatts = stravaActivity.AverageWatts,
        //        Kilojoules = stravaActivity.Kilojoules,
        //        DeviceWatts = stravaActivity.DeviceWatts,
        //        AverageHeartrate = stravaActivity.AverageHeartrate,
        //        MaxHeartrate = stravaActivity.MaxHeartrate,
        //        UserId = userId,
        //        ActivityId = stravaActivity.ActivityId // This should be set for reference
        //    };
        //}

        //private string? CalculatePace(string? type, double averageSpeed)
        //{
        //    try
        //    {
        //        if (type == "Ride")
        //        {
        //            return $"{(averageSpeed * 3.6):F2} km/h";
        //        }
        //        else if (type == "Run")
        //        {
        //            // Calculate minutes per kilometer
        //            double minutesPerKm = 1.0 / (averageSpeed * 0.06);

        //            // Ensure minutesPerKm is not causing an overflow
        //            if (minutesPerKm <= 0 || minutesPerKm > 60)
        //            {
        //                return "Invalid pace";
        //            }

        //            TimeSpan pace = TimeSpan.FromMinutes(minutesPerKm);
        //            return pace.ToString(@"mm\:ss") + " / km";
        //        }
        //        else if (type == "Swim")
        //        {
        //            // Ensure average speed is valid
        //            if (averageSpeed <= 0)
        //            {
        //                return "Invalid speed";
        //            }

        //            // Calculate time per 100 meters
        //            double timePer100Meters = 100.0 / averageSpeed;

        //            // Ensure timePer100Meters is not causing an overflow
        //            if (timePer100Meters <= 0 || timePer100Meters > 3600)
        //            {
        //                return "Invalid pace";
        //            }

        //            int min = (int)(timePer100Meters / 60);
        //            int sec = (int)(timePer100Meters % 60);
        //            return $"{min}:{sec:D2} / 100 meters";
        //        }
        //        return null; // Return null for unsupported activity types
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception or handle it as needed
        //        return "Error calculating pace";
        //    }
        //}



        //public async Task<List<StravaActivityData>> GetActivitiesFromStrava(string accessToken)
        //{
        //    List<StravaActivityData> allActivities = new List<StravaActivityData>();
        //    int page = 1;
        //    int perPage = 200; // 200 is max we can get from strava

        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        //        while (true)
        //        {
        //            Console.WriteLine("hits while");
        //            string apiUrl = $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}";

        //            HttpResponseMessage response = await client.GetAsync(apiUrl);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                string responseBody = await response.Content.ReadAsStringAsync();
        //                List<StravaActivityData> activities = JsonConvert.DeserializeObject<List<StravaActivityData>>(responseBody);

        //                if (activities.Count == 0)
        //                {
        //                    // No more activities to fetch, break out of the loop
        //                    break;
        //                }

        //                // Format the activities before adding them to the list
        //                List<StravaActivityData> formattedActivities = new List<StravaActivityData>();
        //                foreach (var activity in activities)
        //                {
        //                    StravaActivityData formattedActivity = new StravaActivityData();

        //                    formattedActivity.Name = activity.Name;
        //                    formattedActivity.Distance = activity.Distance;
        //                    formattedActivity.Type = activity.Type;
        //                    //formattedActivity.StartDate = DateTime.ParseExact(activity.StartDateLocal.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
        //                    formattedActivity.StartDate = activity.StartDate.Value;
        //                    formattedActivity.MovingTime = int.Parse(TimeSpan.FromSeconds(activity.MovingTime).ToString(@"hh\:mm\:ss").Split(':')[0]); // Assuming moving_time is in hours

        //                    if (activity.Type == "Ride")
        //                    {
        //                        // Format the pace for Ride activities
        //                        formattedActivity.Pace = (activity.AverageSpeed * 3.6).ToString("F2") + " km/h";
        //                    }
        //                    else if (activity.Type == "Run")
        //                    {
        //                        // Format the pace for Run activities
        //                        double minutesPerKm = 1.0 / (activity.AverageSpeed * 0.06);
        //                        formattedActivity.Pace = TimeSpan.FromMinutes(minutesPerKm).ToString(@"mm\:ss") + " / km";
        //                    }
        //                    else if (activity.Type == "Swim")
        //                    {
        //                        // Format the pace for Swim activities
        //                        double timePer100Meters = 100.0 / activity.AverageSpeed;
        //                        int min = (int)(timePer100Meters / 60);
        //                        int sec = (int)(timePer100Meters % 60);
        //                        formattedActivity.Pace = $"{min}:{sec} / 100 meters";
        //                    }

        //                    formattedActivity.AverageHeartrate = activity.AverageHeartrate;
        //                    formattedActivity.MaxHeartrate = activity.MaxHeartrate;
        //                    formattedActivity.TotalElevationGain = activity.TotalElevationGain;

        //                    formattedActivities.Add(formattedActivity);
        //                }

        //                // Add the formatted activities to the list
        //                allActivities.AddRange(formattedActivities);

        //                // Increment the page for the next request
        //                page++;
        //            }
        //            else
        //            {
        //                throw new Exception($"Error GetActivitiesFromStrava: {response.StatusCode}");
        //            }

        //            //if (response.IsSuccessStatusCode)
        //            //{
        //            //    string responseBody = await response.Content.ReadAsStringAsync();
        //            //    List<Activities> ?activities = JsonConvert.DeserializeObject<List<Activities>>(responseBody);

        //            //    if (activities.Count == 0)
        //            //    {
        //            //        // No more activities to fetch, break out of the loop
        //            //        break;
        //            //    }

        //            //    allActivities.AddRange(activities);

        //            //    // Increment the page for the next request
        //            //    page++;
        //            //}
        //            //else
        //            //{
        //            //    throw new Exception($"Error GetActivitiesFromStrava: {response.StatusCode}");
        //            //}
        //        }
        //    }

        //    // Only print in console for demonstration
        //    //foreach (var activity in allActivities)
        //    //{
        //    //    TimeSpan time = TimeSpan.FromSeconds(activity.moving_time);
        //    //    string formatActTime = time.ToString(@"hh\:mm\:ss");
        //    //    double kiloDistance = Math.Round(activity.distance / 1000, 2);
        //    //    Console.WriteLine($"ID: | {activity.id}, Type: | {activity.type} | Distance: | {kiloDistance}km, Time: | {formatActTime} | Elivation: | {activity.total_elevation_gain} | start: | {activity.start_date_local}");
        //    //    Console.WriteLine("\n");
        //    //}

        //    return allActivities;
        //}

        //private Activity MapToActivity(StravaActivityData stravaActivity)
        //{
        //    var activity = new Activity
        //    {
        //        Name = stravaActivity.Name,
        //        Distance = stravaActivity.Distance,
        //        Type = stravaActivity.Type,
        //        MovingTime = stravaActivity.MovingTime,
        //        TotalElevationGain = stravaActivity.TotalElevationGain,
        //        AverageSpeed = stravaActivity.AverageSpeed,
        //        MaxSpeed = stravaActivity.MaxSpeed,
        //        AverageWatts = stravaActivity.AverageWatts,
        //        Kilojoules = stravaActivity.Kilojoules,
        //        AverageHeartrate = stravaActivity.AverageHeartrate,
        //        MaxHeartrate = stravaActivity.MaxHeartrate,
        //        UserId = stravaActivity.UserId,
        //        Timezone = stravaActivity.Timezone,
        //        UtcOffset = stravaActivity.UtcOffset,
        //        LocationCity = stravaActivity.LocationCity,
        //        LocationState = stravaActivity.LocationState,
        //        LocationCountry = stravaActivity.LocationCountry,
        //        AchievementCount = stravaActivity.AchievementCount,
        //        StartDate = ParseDate(stravaActivity.StartDate),
        //        StartDateLocal = ParseDate(stravaActivity.StartDateLocal)
        //    };

        //    // Calculate and format pace based on activity type
        //    activity.Pace = CalculatePace(stravaActivity.Type, stravaActivity.AverageSpeed);

        //    return activity;
        //}

        //private DateTime? ParseDate(DateTime? date)
        //{
        //    if (date.HasValue && date != default(DateTime))
        //    {
        //        return date;
        //    }
        //    return null; // Return null if date is invalid or empty
        //}




        ////public async Task<List<Activity>> FetchAndSaveActivities(string accessToken)
        ////{
        ////    var activities = await GetActivitiesFromStrava(accessToken);

        ////    if (activities.Count > 0)
        ////    {
        ////        bool success = await SaveActivities(activities);
        ////        if (!success)
        ////        {
        ////            throw new Exception("Failed to save activities to the database.");
        ////        }
        ////    }

        ////    return activities;
        ////}





    }
}
