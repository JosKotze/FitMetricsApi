using Microsoft.AspNetCore.Mvc;
using Strava2ExcelWebApiBackend.DTOs;
using Strava2ExcelWebApiBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Strava2ExcelWebApiBackend.Models.StravaService;

namespace Strava2ExcelWebApiBackend.Interfaces
{
    public interface IStravaService
    {
        Task<List<StravaActivityData>> GetActivitiesFromStravaAsync(string accessToken, int userId);
        //Activity MapToActivity(StravaActivityData activity, int userId);
        Task SaveStravaActivitiesAsync(List<StravaActivityData> stravaActivities, int userId);
        Task SyncActivitiesWithDatabaseAsync(string accessToken, int userId);
        //Task SyncActivitiesWithDatabaseAsync2(string accessToken, int userId);
        Task<StravaActivityData> GetActivityByIdAsync(long activityId, bool includeAllEfforts, string token);
        Task SaveActivityMapAndDetails(string accessToken, int userId, long activityId);
        Task<SaveActivityResult> SaveActivityAsync(string accessToken, int userId, long activityId);
    }

    //public interface IStravaService
    //{
    //    Task<List<StravaActivityData>> GetActivitiesFromStrava(string accessToken);
    //    Activity MapToActivity(StravaActivityData activity, int userId);
    //    Task SaveStravaActivitiesAsync(string stravaJsonResponse, int userId);
    //    //Task<bool> SaveActivity(StravaActivityData activities);
    //    //Task<List<Activity>> FetchAndSaveActivities(string accessToken);
    //}
}
