using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStocksAssignment
{
    public class StockErrorException : Exception
    {
        public StockErrorException()
        {
        }

        public StockErrorException(string message)
            : base(message)
        {
        }

        public StockErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
