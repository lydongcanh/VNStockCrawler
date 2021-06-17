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
            var fromDate = new DateTime(2009, 1, 1);
            var toDate = new DateTime(2009, 1, 31);

            var parser = new VnDirectResponseParser();
            var client = new VnDirectHttpClient(parser);
            var result = (await client.CrawlAsync("VCB", fromDate, toDate)).ToList();
            Console.WriteLine(result);
        }
    }
}
