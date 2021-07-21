using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleStocksAssignment;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Diagnostics;

namespace SimpleStocksTests
{
    [TestClass]
    public class SimpleStocksTests
    {
        List<Stock> stocks;

        public SimpleStocksTests()
        {
            stocks = new List<Stock>();
            SetupStocks();
        }

        [TestMethod]
        public void GivenAValidPriceShouldCalcuateDividendYield()
        {
            // Arrange
            var commonStock = stocks[0];
            var preferredStock = stocks[3];
            decimal price = 25.5m;
            decimal expectedDividendYieldCommon= 0m;
            decimal expectedDividendYieldPreferred = 7.84m;
            
            // Act
            var calculatedDividendYieldCommon = commonStock.CalculateDividendYield(price);
            var calculatedDividendYieldPreferred = Math.Round(preferredStock.CalculateDividendYield(price), 2);

            // Assert
            Assert.IsTrue(expectedDividendYieldCommon == calculatedDividendYieldCommon);
            Assert.IsTrue(expectedDividendYieldPreferred == calculatedDividendYieldPreferred);
        }

        [TestMethod]
        [ExpectedException(typeof(StockErrorException))]
        public void GivenAnInValidPriceCalcuateDividendYieldShouldFail()
        {
            // Arrange
            var stock = stocks[0];
            decimal price = -25.5m;           

            // Act
            var calculatedDividendYield = stock.CalculateDividendYield(price);           
        }

        [TestMethod]
        public void GivenAValidPriceShouldCalcuatePER()
        {
            // Arrange
            var commonStock = stocks[1];
            decimal price = 25m;
            decimal expectedPER = 78.125m;

            // Act
            var calculatedPER = commonStock.CalculatePERatio(price);

            // Assert
            Assert.IsTrue(expectedPER == calculatedPER);
        }

        [TestMethod]
        public void GivenAnExistingStockShouldRecordTradesOnIt()
        {
            // Arrange
            var commonStock = stocks[2];
            decimal price = 25m;
            var expectedNumberOfTrades = 2;

            // Act
            commonStock.RecordTrade(commonStock.stockSymbol, 10, Indicator.BUY, price);
            commonStock.RecordTrade(commonStock.stockSymbol, 30, Indicator.SELL, price);

            // Assert
            Assert.IsTrue(commonStock.trades.Count == expectedNumberOfTrades);
        }

        [TestMethod]
        public void GivenAListofTradesShouldReturnTradesWithinLastSpecifiedMinutes()
        {
            // Arrange
            var commonStock = stocks[2];
            decimal price = 25m;
            var expectedNumberOfTrades = 3;

            commonStock.RecordTrade(commonStock.stockSymbol, 10, Indicator.BUY, price);
            commonStock.RecordTrade(commonStock.stockSymbol, 30, Indicator.SELL, price);
            // Add 9 seconds delay before another trade
            Thread.Sleep(9000);
            commonStock.RecordTrade(commonStock.stockSymbol, 70, Indicator.SELL, price);

            // Act
            var tradesInPastFiveMinutes = Trade.FilterTradesByTime(5, commonStock.trades);

            // Assert
            Assert.IsTrue(tradesInPastFiveMinutes.Count == expectedNumberOfTrades);
        }

        [TestMethod]
        public void GivenAStockCalculateVolumeWeightedStockPrice()
        {
            // Arrange
            var commonStock = stocks[1];
            decimal price = 25m;
            decimal expectedVWSP = 25;

            commonStock.RecordTrade(commonStock.stockSymbol, 10, Indicator.BUY, price);
            commonStock.RecordTrade(commonStock.stockSymbol, 30, Indicator.SELL, price);
            commonStock.RecordTrade(commonStock.stockSymbol, 50, Indicator.SELL, price);

            // Act
            var vwsp = commonStock.CalculateVolumeWeightedStock(commonStock.stockSymbol);

            // Assert
            Assert.AreEqual(vwsp, expectedVWSP);
        }

        [TestMethod]
        public void GivenAListWithNoStocksCalculateGBCEAllShareIndexShouldFail()
        {
            // Arrange
            decimal expectedGBCEShareIndex = 20.16m;
            // add some trades on the stocks
            stocks[0].RecordTrade(stocks[0].stockSymbol, 10, Indicator.BUY, 25m);
            stocks[0].RecordTrade(stocks[0].stockSymbol, 300, Indicator.SELL, 20m);
            stocks[1].RecordTrade(stocks[0].stockSymbol, 150, Indicator.BUY, 25m);
            stocks[2].RecordTrade(stocks[0].stockSymbol, 110, Indicator.BUY, 25m);
            stocks[3].RecordTrade(stocks[0].stockSymbol, 210, Indicator.BUY, 20m);
            stocks[3].RecordTrade(stocks[0].stockSymbol, 10, Indicator.BUY, 30m);

            // Act
            var GBCEShareIndex = Math.Round(GBCE.CalculateGBCEAllShareIndex(stocks),2);

            // Assert
            Assert.AreEqual(GBCEShareIndex, expectedGBCEShareIndex);
        }

        [TestMethod]
        [ExpectedException(typeof(GBCEException))]
        public void GivenAListOfStocksCalculateGBCEAllShareIndex()
        {
            // Arrange
            // Act
            var GBCEShareIndex = Math.Round(GBCE.CalculateGBCEAllShareIndex(new List<Stock>()), 2);
        }

        public void SetupStocks()
        {
            var stock1 = new Stock(StockSymbol.TEA, StockType.Common, 0, 0, 100);
            var stock2 = new Stock(StockSymbol.POP, StockType.Common, 8, 0, 100);
            var stock3 = new Stock(StockSymbol.ALE, StockType.Common, 23, 0, 60);
            var stock4 = new Stock(StockSymbol.GIN, StockType.Preferred, 8, 2, 100);

            stocks.Add(stock1);
            stocks.Add(stock2);
            stocks.Add(stock3);
            stocks.Add(stock4);
        }
    }
}
