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
// we are using EPPlus nuget package



namespace Strava2ExcelWebApiBackend.Models
{
    public class StravaService
    {
        public static async Task<List<Activities>> GetActivitiesFromStrava(string accessToken)
        {
            Console.WriteLine("Token from GetActivitiesFromStrava: " + accessToken);
            List<Activities> allActivities = new List<Activities>();
            int page = 1;
            int perPage = 200; // 200 is max we can get from strava

            using (HttpClient client = new HttpClient())
            {
                Console.WriteLine("hit using");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                while (true)
                { 
                    Console.WriteLine("hits while");
                    string apiUrl = $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        List<Activities> activities = JsonConvert.DeserializeObject<List<Activities>>(responseBody);

                        if (activities.Count == 0)
                        {
                            // No more activities to fetch, break out of the loop
                            break;
                        }

                        allActivities.AddRange(activities);

                        // Increment the page for the next request
                        page++;
                    }
                    else
                    {
                        throw new Exception($"Error GetActivitiesFromStrava: {response.StatusCode}");
                    }
                }
            }

            // Only print in console for demonstration
            foreach (var activity in allActivities)
            {
                TimeSpan time = TimeSpan.FromSeconds(activity.moving_time);
                string formatActTime = time.ToString(@"hh\:mm\:ss");
                double kiloDistance = Math.Round(activity.distance / 1000, 2);
                Console.WriteLine($"ID: | {activity.id}, Type: | {activity.type} | Distance: | {kiloDistance}km, Time: | {formatActTime} | Elivation: | {activity.total_elevation_gain} | start: | {activity.start_date_local}");
                Console.WriteLine("\n");
            }

            return allActivities;
        }

    }
}
