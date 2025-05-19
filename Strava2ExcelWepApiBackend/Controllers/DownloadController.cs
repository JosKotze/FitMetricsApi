using Microsoft.AspNetCore.Mvc;
using Strava2ExcelWebApiBackend.Interfaces;
using Strava2ExcelWebApiBackend.Services;

namespace Strava2ExcelWebApiBackend.Controllers
{
    public class DownloadController(IExcelService excelService) : BaseApiController
    {

        [HttpGet("download-excel")]
        public async Task<IActionResult> DownloadExcel(int userId)
        {
            try
            {
                var stream = await excelService.GenerateExcelAsync(userId);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "activities.xlsx");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
