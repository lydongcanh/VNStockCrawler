using System;
using System.Collections.Generic;
using System.Text;

namespace VNStockCrawler.Core
{
    public class Stock
    {
        public string Code { get; set; }

        public DateTime Date { get; set; }

        public double OpenValue { get; set; }

        public double CloseValue { get; set; }

        public double HighValue { get; set; }

        public double LowValue { get; set; }

        public double AverageValue { get; set; }

        public double Volume { get; set; }
    }
}
