using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiniLithoApp
{
    class BalanceReport
    {
        public int sno { get; set; }
        public DateTime orderdate { get; set; }
        public string name { get; set; }
        public string Mobile { get; set; }
        public string billno { get; set; }        
        public double rate { get; set; }
        public double advance { get; set; }
        public double payment { get; set; }
        public double balance { get; set; }
        public double total { get; set; }      
       
    }
    class PartyReport
    {
        public int sno { get; set; }
        public DateTime orderdate { get; set; }
        public string name { get; set; }
        public string Mobile { get; set; }
        public string billno { get; set; }
        public double rate { get; set; }
        public double advance { get; set; }
        public double payment { get; set; }
        public double balance { get; set; }
        public double total { get; set; }

    }
    class StatementReport
    {
        public int sno { get; set; }
        public DateTime orderdate { get; set; }
        public string name { get; set; }
        public string Mobile { get; set; }
        public string billno { get; set; }
        public double rate { get; set; }
        public double advance { get; set; }
        public double payment { get; set; }
        public double balance { get; set; }
        public double total { get; set; }

    }
    class PaymentInfo
    {
        public int sno { get; set; }        
        public string Rno { get; set; }
        public string Rname { get; set; }
        public string Rmobile { get; set; }
        public string paydet { get; set; }
        public string datim { get; set; }
    }
    class NewPartReport
    {
        public string Code { get; set; }
        public string Date { get; set; }
        public string name { get; set; }
        public string Mobile { get; set; }
        public string Detail { get; set; }       
        public double Credit { get; set; }
        public double Debit { get; set; }
    }
    public class balancelistclass
    {
        public int sno { get; set; }
        public DateTime orderdate { get; set; }
        public string name { get; set; }
        public string billno { get; set; }
        public string detailcolor { get; set; }
        public string copies { get; set; }
        public string rate { get; set; }
        public string advance { get; set; }
        public string payment { get; set; }
        public string balance { get; set; }
        public string total { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public String Status { get; set; }
        public string Mobile { get; set; }
    }
}
