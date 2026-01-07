using Application.Common.Interfaces;
using ClosedXML.Excel;
using Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ComplaintExportService : IComplaintExportService
    {
        private readonly IWebHostEnvironment _env;

        public ComplaintExportService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> ExportToExcelAsync(IEnumerable<Complaint> complaints)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Complaints");

            // Header
            worksheet.Cell(1, 1).Value = "Reference Number";
            worksheet.Cell(1, 2).Value = "Title";
            worksheet.Cell(1, 3).Value = "Status";
            worksheet.Cell(1, 4).Value = "Type";
            worksheet.Cell(1, 5).Value = "Severity";
            worksheet.Cell(1, 6).Value = "Created At";
            worksheet.Cell(1, 7).Value = "Description";

            // Formatting headers
            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.FromHtml("#4E73DF");
            headerRow.Style.Font.FontColor = XLColor.White;

            int row = 2;
            foreach (var complaint in complaints)
            {
                worksheet.Cell(row, 1).Value = complaint.ReferenceNumber;
                worksheet.Cell(row, 2).Value = complaint.Title;
                worksheet.Cell(row, 3).Value = complaint.Status.ToString();
                worksheet.Cell(row, 4).Value = complaint.Type.ToString();
                worksheet.Cell(row, 5).Value = complaint.Severity;
                worksheet.Cell(row, 6).Value = complaint.CreatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
                worksheet.Cell(row, 7).Value = complaint.Description;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            var webRootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
            var folderPath = Path.Combine(webRootPath, "exports");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fileName = $"Complaints_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var filePath = Path.Combine(folderPath, fileName);

            workbook.SaveAs(filePath);

            return $"/exports/{fileName}";
        }
    }
}
