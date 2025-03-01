/// excel
// we are using EPPlus nuget package

namespace Strava2ExcelWebApiBackend.Models
{
    public partial class StravaService
    {
        public enum SaveActivityResult
        {
            Success,
            Failed,
            AlreadyExists
        }
    }
}
