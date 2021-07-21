using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStocksAssignment
{
    public class Stock
    {
        public List<Trade> trades;
        public StockSymbol stockSymbol { get; set; }
        public StockType stockType { get; set; }
        public decimal lastDividend { get; set; }
        public decimal fixedDividend { get; set; }
        public decimal parValue { get; set; }

        

        public Stock(StockSymbol stockSymbol, StockType stockType, decimal lastDividend, decimal fixedDividend, decimal parValue)
        {
            this.stockSymbol = stockSymbol;
            this.stockType = stockType;
            this.lastDividend = lastDividend;
            this.fixedDividend = fixedDividend;
            this.parValue = parValue;
            trades = new List<Trade>();
        }

        /// <summary>Calculates the dividend yield.</summary>
        /// <param name="price">The price.</param>
        /// <returns>The dividend yield</returns>
        public decimal CalculateDividendYield(decimal price)
        {
            if (price < 0)
                throw new StockErrorException(Resources.InvalidPrice);
            if (!Enum.IsDefined(typeof(StockType), stockType))
                throw new StockErrorException(Resources.InvalidStockType);

            return stockType == StockType.Common ? CalculateCommonDividendYield(price) : CalculatePreferredDividendYield(price);
        }

        /// <summary>Calculates the common dividend yield.</summary>
        /// <param name="price">The price.</param>
        /// <returns>common dividend yield</returns>
        private decimal CalculateCommonDividendYield(decimal price)
        {
            return decimal.Divide(lastDividend, price);
        }
        /// <summary>Calculates the preferred dividend yield.</summary>
        /// <param name="price">The price.</param>
        /// <returns>The preferred dividend yield</returns>
        private decimal CalculatePreferredDividendYield(decimal price)
        {
            return decimal.Divide(fixedDividend * parValue, price);
        }

        /// <summary>Calculates the  P/E ratio.</summary>
        /// <param name="price">The price.</param>
        /// <returns>The  P/E ratio.</returns>
        public decimal CalculatePERatio(decimal price)
        {
            if (price <= 0)
                throw new StockErrorException(Resources.InvalidPrice);

            decimal PER = 0;

            var dividend = CalculateDividendYield(price);
            if (dividend == 0)
                throw new StockErrorException(Resources.DividendZeroValue);
            PER = decimal.Divide(price, dividend);


            return PER;
        }

        /// <summary>Records the trade.</summary>
        /// <param name="stockSymbol">The stock symbol.</param>
        /// <param name="quantity">The quantity.</param>
        /// <param name="indicator">The indicator.</param>
        /// <param name="price">The price.</param>
        public void RecordTrade(StockSymbol stockSymbol, int quantity, Indicator indicator, decimal price)
        {
            trades.Add(new Trade(stockSymbol, DateTime.Now, quantity, indicator, price));
        }

        /// <summary>Calculates the volume weighted stock.</summary>
        /// <param name="stockSymbol">The stock symbol.</param>
        /// <returns>The volume weighted stock.</returns>
        public decimal CalculateVolumeWeightedStock(StockSymbol stockSymbol)
        {
            decimal volumeWeightedStock = 0;
            decimal totalTradedPrice = 0;
            int totalQuantity = 0;
            int numberOfMinutes = 5;

            var tradesOfStockSymbol = trades.Where(t => t.stockSymbol == stockSymbol).ToList();
            var timeFilteredTrades = Trade.FilterTradesByTime(numberOfMinutes, tradesOfStockSymbol);

            foreach (var trade in timeFilteredTrades)
            {
                // total price for a particular stock
                totalTradedPrice = totalTradedPrice + (trade.price * trade.quantity);

                totalQuantity += trade.quantity;
            }

            // check stock quantity 
            if (totalQuantity != 0)
                volumeWeightedStock = decimal.Divide(totalTradedPrice, totalQuantity);

            return volumeWeightedStock;
        }



    }
}
