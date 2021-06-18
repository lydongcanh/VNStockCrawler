using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;
using Microsoft.Office.Interop.Excel;

namespace VNStockCrawler.Core
{
    public class EPPlusExcelWriter : IExcelWriter
    {
        public string SaveFilePath { get; protected set; } = "stock.xlsx";

        public EPPlusExcelWriter()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public async Task SaveToFileAsync(IEnumerable<Stock> stocks)
        {
            // Create new excel file.
            var stockArray = stocks.ToArray();
            var newFile = CreateExcelFile(stockArray);
            var fileName = Path.GetFileNameWithoutExtension(newFile.Name);
            using var excelPackage = new ExcelPackage(newFile);
            var worksheet = excelPackage.Workbook.Worksheets.Add(fileName);

            // Setup headers.
            worksheet.Cells["A1"].Value = "Date";
            worksheet.Cells["B1"].Value = "Open";
            worksheet.Cells["C1"].Value = "High";
            worksheet.Cells["D1"].Value = "Low";
            worksheet.Cells["E1"].Value = "Close";
            worksheet.Cells["F1"].Value = "Average";
            worksheet.Cells["G1"].Value = "Volume";

            // Populate values.
            var startIndex = 2;
            for (int i = startIndex; i < stockArray.Length + startIndex; i++)
            {
                var stock = stockArray[i - startIndex];
                worksheet.Cells[$"A{i}"].Value = stock.Date.ToString("dd/MM/yyyy");
                worksheet.Cells[$"B{i}"].Value = stock.Open;
                worksheet.Cells[$"C{i}"].Value = stock.High;
                worksheet.Cells[$"D{i}"].Value = stock.Low;
                worksheet.Cells[$"E{i}"].Value = stock.Close;
                worksheet.Cells[$"F{i}"].Value = stock.Average;
                worksheet.Cells[$"G{i}"].Value = stock.Volume;
            }

            // Save as xlsx.
            await excelPackage.SaveAsync();

            // Save as csv.
            Application app = new Application();
            Workbook workbook = app.Workbooks.Open($"{SaveFilePath}.xlsx");
            workbook.SaveAs($"{SaveFilePath}.csv", XlFileFormat.xlCSVWindows);
            workbook.Close(false);
            app.Quit();
        }

        private FileInfo CreateExcelFile(Stock[] stocks)
        {
            var fileName = BuildFileName(stocks);
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var folderPath = $"{desktopPath}/Stocks Data/{DateTime.Now:dd-MM-yyyy--HH-mm-ss}";
            SaveFilePath = $"{folderPath}/{fileName}".ToLower();

            Directory.CreateDirectory(folderPath);
            return new FileInfo($"{SaveFilePath}.xlsx");
        }

        private string BuildFileName(Stock[] stocks)
        {
            if (stocks == null || !stocks.Any())
                return $"empty_{DateTime.Now}";

            var code = stocks.First().Ticker;
            var fromYear = stocks.First().Date.Year;
            var toYear = stocks.Last().Date.Year;

            // Sample: VCB_2020_2021 or VCB_2021
            return $"{code}_{fromYear}{(toYear != fromYear ? $"_{toYear}" : string.Empty)}";
        }
    }
}
