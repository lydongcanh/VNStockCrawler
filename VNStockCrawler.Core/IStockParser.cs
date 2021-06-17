using System.Collections.Generic;

namespace VNStockCrawler.Core
{
    public interface IStockParser
    {
        IEnumerable<Stock> Parse(string jsonString);
    }
}
