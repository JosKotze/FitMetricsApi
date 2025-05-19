namespace Strava2ExcelWebApiBackend.Interfaces
{
    public interface IExcelService
    {
        Task<MemoryStream> GenerateExcelAsync(int userId);
    }
}
