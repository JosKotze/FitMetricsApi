using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Strava2ExcelWebApiBackend.Data;
using Strava2ExcelWebApiBackend.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Strava2ExcelWebApiBackend.Interfaces;
using System.ComponentModel;

namespace Strava2ExcelWebApiBackend.Services
{
    public class ExcelService : IExcelService
    {
        private readonly StravaDbContext context;
        public ExcelService(StravaDbContext context) 
        {
            this.context = context;
        }

        public async Task<MemoryStream> GenerateExcelAsync(int userId)
        {
            var data = await FetchDataFromDatabase(userId);

            ExcelPackage.License.SetNonCommercialPersonal("JPKOTZE");

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Activities");

                worksheet.Cells[1, 1].Value = "Id";
                worksheet.Cells[1, 2].Value = "ActivityId";
                worksheet.Cells[1, 3].Value = "UserId";
                worksheet.Cells[1, 4].Value = "Pace";
                worksheet.Cells[1, 5].Value = "Name";
                worksheet.Cells[1, 6].Value = "Distance";
                worksheet.Cells[1, 7].Value = "MovingTime";
                worksheet.Cells[1, 8].Value = "AverageHeartrate";
                worksheet.Cells[1, 9].Value = "AverageSpeed";
                worksheet.Cells[1, 10].Value = "TotalElevationGain";
                worksheet.Cells[1, 11].Value = "Type";
                worksheet.Cells[1, 12].Value = "StartDate";
                worksheet.Cells[1, 13].Value = "StartDateLocal";
                worksheet.Cells[1, 14].Value = "Timezone";
                worksheet.Cells[1, 15].Value = "MaxHeartrate";

                for (int i = 0; i < data.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = data[i].Id;
                    worksheet.Cells[i + 2, 2].Value = data[i].ActivityId;
                    worksheet.Cells[i + 2, 3].Value = data[i].UserId;
                    worksheet.Cells[i + 2, 4].Value = data[i].Pace;
                    worksheet.Cells[i + 2, 5].Value = data[i].Name;
                    worksheet.Cells[i + 2, 6].Value = data[i].Distance;
                    worksheet.Cells[i + 2, 7].Value = data[i].MovingTime;
                    worksheet.Cells[i + 2, 8].Value = data[i].AverageHeartrate;
                    worksheet.Cells[i + 2, 9].Value = data[i].AverageSpeed;
                    worksheet.Cells[i + 2, 10].Value = data[i].TotalElevationGain;
                    worksheet.Cells[i + 2, 11].Value = data[i].Type;
                    worksheet.Cells[i + 2, 12].Value = data[i].StartDate?.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[i + 2, 13].Value = data[i].StartDateLocal?.ToString("yyyy-MM-dd HH:mm:ss");
                    worksheet.Cells[i + 2, 14].Value = data[i].Timezone;
                    worksheet.Cells[i + 2, 15].Value = data[i].MaxHeartrate;
                }

                worksheet.Cells.AutoFitColumns();

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);
                stream.Position = 0;

                return stream;
            }
            
        }
        private async Task<List<Activity>> FetchDataFromDatabase(int userId)
        {
            return await context.Activities
            .Where(x => x.UserId == userId)
            .ToListAsync();
        }
    }
}
