using System.Collections.Generic;
using System.Threading.Tasks;

namespace VNStockCrawler.Core
{
    public interface IExcelWriter
    {
        Task SaveToFileAsync(IEnumerable<Stock> stocks);

        string SaveFilePath { get; }
    }
}
