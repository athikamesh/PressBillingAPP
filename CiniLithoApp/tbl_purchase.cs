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
    
    public partial class tbl_purchase
    {
        public int Sno { get; set; }
        public string CompanyName { get; set; }
        public string ProductDetail { get; set; }
        public string HNSNo { get; set; }
        public string Billno { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<decimal> CGST { get; set; }
        public Nullable<decimal> SGST { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public Nullable<System.DateTime> Purchasedate { get; set; }
        public string PaymentMode { get; set; }
        public Nullable<System.DateTime> Paymentdate { get; set; }
    }
}
