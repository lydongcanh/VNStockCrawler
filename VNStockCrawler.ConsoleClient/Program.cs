using System;
using System.Linq;
using System.Threading.Tasks;
using VNStockCrawler.Core;

namespace VNStockCrawler.ConsoleClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var stockCode = "MSN";
            var fromDate = new DateTime(2020, 1, 1);
            var toDate = new DateTime(2021, 04, 30);

            var parser = new VnDirectResponseParser();
            var client = new VnDirectHttpClient(parser);
            var excelWriter = new EPPlusExcelWriter();

            var stocks = await client.CrawlAsync(stockCode, fromDate, toDate);
            await excelWriter.SaveToFileAsync(stocks);

            Console.WriteLine($"The file have been saved in {excelWriter.SaveFilePath} successfully.");
        }
    }
}
