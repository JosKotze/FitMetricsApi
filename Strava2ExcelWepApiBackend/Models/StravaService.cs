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
// we are using EPPlus nuget package



namespace Strava2ExcelWebApiBackend.Models
{
    public class StravaService
    {
        public static async Task<List<Activity>> GetActivitiesFromStrava(string accessToken)
        {
            List<Activity> allActivities = new List<Activity>();
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
                        List<Activity> activities = JsonConvert.DeserializeObject<List<Activity>>(responseBody);

                        if (activities.Count == 0)
                        {
                            // No more activities to fetch, break out of the loop
                            break;
                        }

                        // Format the activities before adding them to the list
                        List<Activity> formattedActivities = new List<Activity>();
                        foreach (var activity in activities)
                        {
                            Activity formattedActivity = new Activity();

                            formattedActivity.name = activity.name;
                            formattedActivity.distance = activity.distance;
                            formattedActivity.type = activity.type;
                            formattedActivity.start_date = DateTime.ParseExact(activity.start_date_local.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                            formattedActivity.moving_time = int.Parse(TimeSpan.FromSeconds(activity.moving_time).ToString(@"hh\:mm\:ss").Split(':')[0]); // Assuming moving_time is in hours

                            if (activity.type == "Ride")
                            {
                                // Format the pace for Ride activities
                                formattedActivity.pace = (activity.average_speed * 3.6).ToString("F2") + " km/h";
                            }
                            else if (activity.type == "Run")
                            {
                                // Format the pace for Run activities
                                double minutesPerKm = 1.0 / (activity.average_speed * 0.06);
                                formattedActivity.pace = TimeSpan.FromMinutes(minutesPerKm).ToString(@"mm\:ss") + " / km";
                            }
                            else if (activity.type == "Swim")
                            {
                                // Format the pace for Swim activities
                                double timePer100Meters = 100.0 / activity.average_speed;
                                int min = (int)(timePer100Meters / 60);
                                int sec = (int)(timePer100Meters % 60);
                                formattedActivity.pace = $"{min}:{sec} / 100 meters";
                            }

                            formattedActivity.average_heartrate = activity.average_heartrate;
                            formattedActivity.max_heartrate = activity.max_heartrate;
                            formattedActivity.total_elevation_gain = activity.total_elevation_gain;

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

    }
}
