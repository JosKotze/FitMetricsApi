using Strava2ExcelWebApiBackend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Strava2ExcelWebApiBackend.Interfaces
{
    public interface IStravaService
    {
        Task<List<Activity>> GetActivitiesFromStrava(string accessToken);
        Task<bool> SaveActivity(Activity activities);
        //Task<List<Activity>> FetchAndSaveActivities(string accessToken);
    }
}
