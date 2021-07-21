using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStocksAssignment
{
    public class GBCEException : Exception
    {
        public GBCEException()
        {
        }

        public GBCEException(string message)
            : base(message)
        {
        }

        public GBCEException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
