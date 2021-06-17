using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace VNStockCrawler.Core
{
    public class VnDirectHttpClient : ICrawlClient
    {
        private const string BaseUrl = "https://finfo-api.vndirect.com.vn/v4/stock_prices/";

        private readonly IStockParser _stockParser;

        public VnDirectHttpClient(IStockParser stockParser)
        {
            this._stockParser = stockParser;
        }

        public async Task<IEnumerable<Stock>> CrawlAsync(string stockCode, DateTime fromDate, DateTime toDate)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["sort"] = "date";
            query["size"] = $"{(toDate - fromDate).TotalDays + 1}";
            query["page"] = "1";
            query["q"] = $"code:{stockCode}~date:gte:{FormatDateTime(fromDate)}~date:lte:{FormatDateTime(toDate)}";

            var uriBuilder = new UriBuilder(BaseUrl) { Query = query.ToString() ?? string.Empty };

            using HttpClient client = new HttpClient();
            var response = await client.GetAsync(uriBuilder.Uri);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return _stockParser.Parse(responseContent);
        }

        private string FormatDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }
}
