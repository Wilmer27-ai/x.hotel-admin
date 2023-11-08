using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace x.hotel
{
    public class Payment
    {
        public string TransactionId { get; set; }
        public int TotalAmount { get; set; }
        public int AmountPaid { get; set; }
        public int Change { get; set; }
    }
}
