using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiniLithoApp
{
    class BillReportClass
    {
        public int sno { get; set; }
        public DateTime orderdate { get; set; }
        public DateTime deliverdate { get; set; }
        public string name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string billno { get; set; }
        public string detailcolor { get; set; }
        public int copies { get; set; }
        public double rate { get; set; }
        public double advance { get; set; }
        public double payment { get; set; }
        public double balance { get; set; }
        public double Amount { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public double Taxper { get; set; }
        public double SGST { get; set; }
        public double CGST { get; set; }
        public double TotalAmount { get; set; }

    }
}
