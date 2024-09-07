using Strava2ExcelWebApiBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strava2ExcelWebApiBackend.Interfaces
{
    public interface IStravaService
    {
        Task<List<StravaActivityData>> GetActivitiesFromStrava(string accessToken);
        //Task<bool> SaveActivity(StravaActivityData activities);
        //Task<List<Activity>> FetchAndSaveActivities(string accessToken);
    }
}
