using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml;

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
            using var excelPackage = new ExcelPackage(newFile);
            var worksheet = excelPackage.Workbook.Worksheets.Add(newFile.Name);

            // Setup headers.
            worksheet.Cells["A1"].Value = "DATE";
            worksheet.Cells["B1"].Value = "CLOSE";
            worksheet.Cells["C1"].Value = "TICKER";
            worksheet.Cells["D1"].Value = "OPEN";
            worksheet.Cells["E1"].Value = "HIGH";
            worksheet.Cells["F1"].Value = "LOW";
            worksheet.Cells["G1"].Value = "VOLUME";
            worksheet.Cells["H1"].Value = "HELPER";

            // Populate values.
            var startIndex = 2;
            var length = stockArray.Length + startIndex;
            for (int i = startIndex; i < length; i++)
            {
                var stock = stockArray[i - startIndex];
                worksheet.Cells[$"A{i}"].Value = stock.Date.ToString("dd/MM/yyyy");
                worksheet.Cells[$"B{i}"].Value = stock.Close;
                worksheet.Cells[$"C{i}"].Value = stock.Code;
                worksheet.Cells[$"D{i}"].Value = stock.Open;
                worksheet.Cells[$"E{i}"].Value = stock.High;
                worksheet.Cells[$"F{i}"].Value = stock.Low;
                worksheet.Cells[$"G{i}"].Value = stock.Volume;
                worksheet.Cells[$"H{i}"].Value = length - i;
            }

            await excelPackage.SaveAsync();
        }

        private FileInfo CreateExcelFile(Stock[] stocks)
        {
            var fileName = BuildFileName(stocks);
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var folderPath = $"{desktopPath}/Stocks Data/{DateTime.Now:dd-MM-yyyy--HH-mm-ss}";
            SaveFilePath = $"{folderPath}/{fileName}.xlsx";

            Directory.CreateDirectory(folderPath);
            return new FileInfo(SaveFilePath);
        }

        private string BuildFileName(Stock[] stocks)
        {
            if (stocks == null || !stocks.Any())
                return $"empty_{DateTime.Now}";

            var code = stocks.First().Code;
            var fromYear = stocks.First().Date.Year;
            var toYear = stocks.Last().Date.Year;

            // Sample: VCB_2020_2021 or VCB_2021
            return $"{code}_{fromYear}{(toYear != fromYear ? $"_{toYear}" : string.Empty)}";
        }
    }
}
