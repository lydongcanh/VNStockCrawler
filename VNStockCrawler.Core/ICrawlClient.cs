using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VNStockCrawler.Core
{
    public interface ICrawlClient
    {
        Task<IEnumerable<Stock>> CrawlAsync(string stockCode, DateTime fromDate, DateTime toDate);
    }
}
