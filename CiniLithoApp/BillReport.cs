using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiniLithoApp
{
    class BillReport
    {
        public string billno { get; set; }
        public string customername { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string balance { get; set; }
        public string deliverydate { get; set; }
        public string orderdate { get; set; }
        public int No { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Copies { get; set; }
        public string Rate { get; set; }
        public string Tax { get; set; }
        public string Amount { get; set; }
        public string paidamount { get; set; }
        public string balanceamount { get; set; }
        public string totalamount { get; set; }
    }
}
