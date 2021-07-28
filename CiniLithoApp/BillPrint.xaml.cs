using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CiniLithoApp
{
    /// <summary>
    /// Interaction logic for BillPrint.xaml
    /// </summary>
    public partial class BillPrint : Window
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        ReportDocument RPT = new ReportDocument();
        public BillPrint(string code)
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            Printbill(code);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        void Printbill(string BillNo)
        {
            List<BillReportClass> BRC = new List<BillReportClass>();
            if (BillNo != "")
            {
                int BCount = 0;
                var bill = Cinidb.BillPrint1(BillNo).ToList();
                foreach (var s in bill)
                {
                    BCount++;
                    BillReportClass BC = new CiniLithoApp.BillReportClass();
                    BC.sno = BCount;
                    BC.billno = s.Billno;
                    BC.name = s.Name;
                    BC.Phone = s.Mobile;
                    BC.Address = s.address;
                    BC.TotalAmount = s.Total;
                    BC.Amount = double.Parse(s.Rate.ToString()) * double.Parse(s.Copies.ToString());
                    BC.SGST = (double)s.SGST;
                    BC.CGST = (double)s.CGST;
                    BC.color = s.Color;
                    BC.rate = (double)s.Rate;
                    BC.size = s.Size;
                    BC.copies = (int)s.Copies;
                    BC.orderdate = s.orderdate;
                    BC.deliverdate = s.deliverydate;
                    BC.Taxper = double.Parse(s.PCgst.ToString()) + double.Parse(s.PSgct.ToString());
                    BRC.Add(BC);
                }
                string s2 = Process.GetCurrentProcess().MainModule.FileName;
                RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Bill_RPT.rpt");
                RPT.Database.Tables[0].SetDataSource(BRC);
                CRPT.ViewerCore.ReportSource = RPT;
                //CRPT.ViewerCore.RefreshReport();
            }
        }
    }
}
