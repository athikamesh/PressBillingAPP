//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CiniLithoApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_totaldetails
    {
        public string Billno { get; set; }
        public double Total { get; set; }
        public double Advance { get; set; }
        public double Payment { get; set; }
        public double Balance { get; set; }
        public string Discount { get; set; }
        public string PDate { get; set; }
        public string Adate { get; set; }
        public string Pmode { get; set; }
        public Nullable<decimal> SGST { get; set; }
        public Nullable<decimal> CGST { get; set; }
        public string BankRef { get; set; }
    }
}
