using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStocksAssignment
{
    public class Trade
    {
        public StockSymbol stockSymbol { get; set; } 
        public DateTime timeStamp { get; set; }
        public int quantity { get; set; }
        public Indicator indicator { get; set; }
        public decimal price { get; set; }

        public Trade(StockSymbol stockSymbol, DateTime timeOfTrade, int quantity, Indicator indicator, decimal price)
        {
            this.stockSymbol = stockSymbol;
            this.timeStamp = timeOfTrade;
            this.quantity = quantity;
            this.indicator = indicator;
            this.price = price;
        }

        /// <summary>Gets a list of trades that occured in the last number of specified minutes.</summary>
        /// <param name="minutes">Specify the number of minutes to filter the Trades</param>
        /// <returns>A list of trades that occured in the last specified minutes.</returns>
        public static List<Trade> FilterTradesByTime(int minutes, List<Trade> allTrades)
        {
            var tradesInLastSpecifiedMinutes = allTrades.Where(t => t.timeStamp >= DateTime.UtcNow.Add(new TimeSpan(0, -minutes, 0))).ToList();
            
            return tradesInLastSpecifiedMinutes;
        }
    }
}
