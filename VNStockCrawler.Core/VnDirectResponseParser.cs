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
                    Ticker = item.code,
                    Date = item.date,
                    Open = item.open,
                    Close = item.close,
                    High = item.high,
                    Low = item.low,
                    Average = item.average,
                    Volume = item.nmVolume + item.ptVolume
                };
            }
        }
    }
}
