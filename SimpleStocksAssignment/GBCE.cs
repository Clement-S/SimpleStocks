using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStocksAssignment
{
    public class GBCE
    {

        public GBCE() { }

        /// <summary>Calculates the GBCE All Share Index for all the stocks.</summary>
        /// <returns>The GBCE share index.</returns>
        public static decimal CalculateGBCEAllShareIndex(List<Stock> stocks)
        {
            if (stocks.Count == 0)
                throw new GBCEException(Resources.NoStocks);

            var stockPrices = new List<decimal>();
            foreach(var stock in stocks)
            {
                var stockPrice = stock.CalculateVolumeWeightedStock(stock.stockSymbol);
                if (stockPrice != 0)
                    stockPrices.Add(stockPrice);
            }

            var GBCEIndex = CalculateGeometricMean(stockPrices);
            return (decimal)GBCEIndex;
        }

        /// <summary>Calculates the geometric mean.</summary>
        /// <param name="stockPrices">The stock prices.</param>
        /// <returns>The Geometric mean</returns>
        private static double CalculateGeometricMean(List<decimal> stockPrices)
        {
            double geometricMean = 0;
            double sum = 0;

            for(var i = 0; i < stockPrices.Count; i++)
                // Log works with double not decimal
                sum = sum + Math.Log((double)stockPrices[i]);

            sum = sum / stockPrices.Count;
            geometricMean = Math.Exp(sum);

            return geometricMean;
        }
    }
}
