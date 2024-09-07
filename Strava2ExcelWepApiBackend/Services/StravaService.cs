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
// we are using EPPlus nuget package



namespace Strava2ExcelWebApiBackend.Models
{
    public class StravaService : IStravaService
    {
        private readonly StravaDbContext _context;

        public StravaService(StravaDbContext context)
        {
            _context = context;
        }


        public async Task<List<StravaActivityData>> GetActivitiesFromStrava(string accessToken)
        {
            List<StravaActivityData> allActivities = new List<StravaActivityData>();
            int page = 1;
            int perPage = 200; // 200 is max we can get from strava

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                while (true)
                { 
                    Console.WriteLine("hits while");
                    string apiUrl = $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<StravaActivityData> activities = JsonConvert.DeserializeObject<List<StravaActivityData>>(responseBody);

                        if (activities.Count == 0)
                        {
                            // No more activities to fetch, break out of the loop
                            break;
                        }

                        // Format the activities before adding them to the list
                        List<StravaActivityData> formattedActivities = new List<StravaActivityData>();
                        foreach (var activity in activities)
                        {
                            StravaActivityData formattedActivity = new StravaActivityData();

                            formattedActivity.Name = activity.Name;
                            formattedActivity.Distance = activity.Distance;
                            formattedActivity.Type = activity.Type;
                            formattedActivity.StartDate = DateTime.ParseExact(activity.StartDateLocal.ToString(), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            formattedActivity.MovingTime = int.Parse(TimeSpan.FromSeconds(activity.MovingTime).ToString(@"hh\:mm\:ss").Split(':')[0]); // Assuming moving_time is in hours

                            if (activity.Type == "Ride")
                            {
                                // Format the pace for Ride activities
                                formattedActivity.Pace = (activity.AverageSpeed * 3.6).ToString("F2") + " km/h";
                            }
                            else if (activity.Type == "Run")
                            {
                                // Format the pace for Run activities
                                double minutesPerKm = 1.0 / (activity.AverageSpeed * 0.06);
                                formattedActivity.Pace = TimeSpan.FromMinutes(minutesPerKm).ToString(@"mm\:ss") + " / km";
                            }
                            else if (activity.Type == "Swim")
                            {
                                // Format the pace for Swim activities
                                double timePer100Meters = 100.0 / activity.AverageSpeed;
                                int min = (int)(timePer100Meters / 60);
                                int sec = (int)(timePer100Meters % 60);
                                formattedActivity.Pace = $"{min}:{sec} / 100 meters";
                            }

                            formattedActivity.AverageHeartrate = activity.AverageHeartrate;
                            formattedActivity.MaxHeartrate = activity.MaxHeartrate;
                            formattedActivity.TotalElevationGain = activity.TotalElevationGain;

                            formattedActivities.Add(formattedActivity);
                        }

                        // Add the formatted activities to the list
                        allActivities.AddRange(formattedActivities);

                        // Increment the page for the next request
                        page++;
                    }
                    else
                    {
                        throw new Exception($"Error GetActivitiesFromStrava: {response.StatusCode}");
                    }

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    string responseBody = await response.Content.ReadAsStringAsync();
                    //    List<Activities> ?activities = JsonConvert.DeserializeObject<List<Activities>>(responseBody);

                    //    if (activities.Count == 0)
                    //    {
                    //        // No more activities to fetch, break out of the loop
                    //        break;
                    //    }

                    //    allActivities.AddRange(activities);

                    //    // Increment the page for the next request
                    //    page++;
                    //}
                    //else
                    //{
                    //    throw new Exception($"Error GetActivitiesFromStrava: {response.StatusCode}");
                    //}
                }
            }

            // Only print in console for demonstration
            //foreach (var activity in allActivities)
            //{
            //    TimeSpan time = TimeSpan.FromSeconds(activity.moving_time);
            //    string formatActTime = time.ToString(@"hh\:mm\:ss");
            //    double kiloDistance = Math.Round(activity.distance / 1000, 2);
            //    Console.WriteLine($"ID: | {activity.id}, Type: | {activity.type} | Distance: | {kiloDistance}km, Time: | {formatActTime} | Elivation: | {activity.total_elevation_gain} | start: | {activity.start_date_local}");
            //    Console.WriteLine("\n");
            //}

            return allActivities;
        }

        private Activity MapToActivity(StravaActivityData stravaActivity)
        {
            var activity = new Activity
            {
                Name = stravaActivity.Name,
                Distance = stravaActivity.Distance,
                Type = stravaActivity.Type,
                MovingTime = stravaActivity.MovingTime,
                TotalElevationGain = stravaActivity.TotalElevationGain,
                AverageSpeed = stravaActivity.AverageSpeed,
                MaxSpeed = stravaActivity.MaxSpeed,
                AverageWatts = stravaActivity.AverageWatts,
                Kilojoules = stravaActivity.Kilojoules,
                AverageHeartrate = stravaActivity.AverageHeartrate,
                MaxHeartrate = stravaActivity.MaxHeartrate,
                UserId = stravaActivity.UserId,
                Timezone = stravaActivity.Timezone,
                UtcOffset = stravaActivity.UtcOffset,
                LocationCity = stravaActivity.LocationCity,
                LocationState = stravaActivity.LocationState,
                LocationCountry = stravaActivity.LocationCountry,
                AchievementCount = stravaActivity.AchievementCount,
                StartDate = ParseDate(stravaActivity.StartDate),
                StartDateLocal = ParseDate(stravaActivity.StartDateLocal)
            };

            // Calculate and format pace based on activity type
            activity.Pace = CalculatePace(stravaActivity.Type, stravaActivity.AverageSpeed);

            return activity;
        }

        private DateTime? ParseDate(DateTime? date)
        {
            if (date.HasValue && date != default(DateTime))
            {
                return date;
            }
            return null; // Return null if date is invalid or empty
        }

        private string? CalculatePace(string? type, double averageSpeed)
        {
            if (type == "Ride")
            {
                return $"{(averageSpeed * 3.6):F2} km/h";
            }
            else if (type == "Run")
            {
                double minutesPerKm = 1.0 / (averageSpeed * 0.06);
                return TimeSpan.FromMinutes(minutesPerKm).ToString(@"mm\:ss") + " / km";
            }
            else if (type == "Swim")
            {
                double timePer100Meters = 100.0 / averageSpeed;
                int min = (int)(timePer100Meters / 60);
                int sec = (int)(timePer100Meters % 60);
                return $"{min}:{sec:D2} / 100 meters";
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


        //public async Task<List<Activity>> FetchAndSaveActivities(string accessToken)
        //{
        //    var activities = await GetActivitiesFromStrava(accessToken);

        //    if (activities.Count > 0)
        //    {
        //        bool success = await SaveActivities(activities);
        //        if (!success)
        //        {
        //            throw new Exception("Failed to save activities to the database.");
        //        }
        //    }

        //    return activities;
        //}



        // method GetActivitiesFromStrava and then update database


    }
}
