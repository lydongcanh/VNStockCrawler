using System.Collections.Generic;
using System.Data;
using Newtonsoft.Json;

namespace VNStockCrawler.Core
{
    public class VnDirectResponseParser : IStockParser
    {
        public IEnumerable<Stock> Parse(string jsonString)
        {
            dynamic json = JsonConvert.DeserializeObject(jsonString);
            if (json == null)
                throw new NoNullAllowedException(nameof(json));

            var data = json.data;
            foreach (var item in data)
            {
                yield return new Stock
                {
                    Code = item.code,
                    Date = item.date,
                    OpenValue = item.open,
                    CloseValue = item.close,
                    HighValue = item.high,
                    LowValue = item.low,
                    AverageValue = item.average,
                    Volume = item.nmVolume + item.ptVolume
                };
            }
        }
    }
}
