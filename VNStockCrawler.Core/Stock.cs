using System;
using System.Collections.Generic;
using System.Text;

namespace VNStockCrawler.Core
{
    public class Stock
    {
        public string Code { get; set; }

        public DateTime Date { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double AverageValue { get; set; }

        public double Volume { get; set; }
    }
}
