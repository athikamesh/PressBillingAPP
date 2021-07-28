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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CiniLithoApp
{
    /// <summary>
    /// Interaction logic for Cashinhandpage.xaml
    /// </summary>
    public partial class Cashinhandpage : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        List<CashHand> CHL = new List<CashHand>();
        string paymode = "";double totamt = 0;
        ReportDocument RPT = new ReportDocument();
        public Cashinhandpage()
        {
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            InitializeComponent();     
            try
            {               
                string datim = DateTime.Now.ToString("dd-MM-yyyy");
                txt_datepicker.Text = datim;
                CASHLIST(datim, paymode);
                
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        void CASHLIST(string dat,string mode)
        {
            try { 
            totamt = 0;
            if (mode == "Cash" && dat!="")
            {
                   
                var det = Cinidb.tbl_logdetail.Where(b => b.Orderdet != "Advance Amount Updated" && b.Orderdet!="update Customer entry and amount" && b.datim == dat && b.mode == mode && b.paydet!="000").ToList();
                Searchgrid.ItemsSource = det;
                foreach(var s in det)
                {
                    if (s.paydet != "")
                    {
                        totamt += double.Parse(s.paydet);
                    }
                }
                    //  txt_totamt.Text = totamt.ToString();
                    var det1 = Cinidb.tbl_expence.Where(b => b.Exp_date == txt_datepicker.SelectedDate).ToList();
                    foreach (var s in det1)
                    {
                        if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                        {
                            totamt -= (double)s.Exp_Amount;
                        }
                        if (s.Exp_Type == "OTH")
                        {
                            totamt += (double)s.Exp_Amount;
                        }
                    }
                    txt_totamt.Text = totamt.ToString();
                }
                else
            {
                    totamt = 0;
                    var det = Cinidb.tbl_logdetail.Where(b => b.Orderdet != "Advance Amount Updated" && b.Orderdet != "update Customer entry and amount" && b.datim == dat && b.mode != "Cash" && b.paydet != "000").ToList();
                Searchgrid.ItemsSource = det;
                foreach (var s in det)
                {
                    if (s.paydet != "")
                    {
                        totamt += double.Parse(s.paydet);
                    }
                }
              txt_totamt.Text = totamt.ToString();
            }
               
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);  }
        }
        private void RBT_Cash_Checked(object sender, RoutedEventArgs e)
        {
            paymode = "Cash";
            if (txt_datepicker.Text != "")
            {
                string datim = txt_datepicker.Text;
                datim = Convert.ToDateTime(txt_datepicker.Text).ToString("dd-MM-yyyy");
                CASHLIST(datim, paymode);
            }
            else
            {
                string datim = DateTime.Now.ToString("dd-MM-yyyy");
                CASHLIST(datim, paymode);
            }
        }

        private void RBT_BANK_Checked(object sender, RoutedEventArgs e)
        {
            paymode = "Bank";
            if (txt_datepicker.Text != "")
            {
                string datim = txt_datepicker.Text;
                datim = Convert.ToDateTime(txt_datepicker.Text).ToString("dd-MM-yyyy");
                CASHLIST(datim, paymode);
            }
            else
            {
                string datim = DateTime.Now.ToString("dd-MM-yyyy");
                CASHLIST(datim, paymode);
            }
        }

        private void SearchGridScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void txt_datepicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            string datim = txt_datepicker.Text;
            datim = Convert.ToDateTime(txt_datepicker.Text).ToString("dd-MM-yyyy");
            CASHLIST(datim, paymode);
        }

        private void btn_exsubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                totamt=0;
                List<CashHand> CHS = new List<CiniLithoApp.CashHand>();
                string datim = Convert.ToDateTime(txt_datepicker.Text).ToString("dd-MM-yyyy");
                if (paymode == "Cash" && datim != "")
                {
                    var det = Cinidb.tbl_logdetail.Where(b => b.Orderdet != "Advance Amount Updated" && b.Orderdet != "update Customer entry and amount" && b.datim == datim && b.mode == paymode  && b.paydet != "000").ToList();
                    Searchgrid.ItemsSource = det;
                    foreach (var s in det)
                    {
                        CashHand CH = new CiniLithoApp.CashHand();
                        CH.Billno = s.Rno;                        
                        CH.CSTName = s.Rname;
                        CH.Mode = s.mode;
                        CH.BankRef = s.BankRef;
                        CH.CreditAmt = double.Parse(s.paydet);
                        CHS.Add(CH);
                        totamt += double.Parse(s.paydet);
                    }
                    var det1 = Cinidb.tbl_expence.Where(b => b.Exp_date == txt_datepicker.SelectedDate).ToList();
                    foreach (var s in det1)
                    {
                        if (s.Exp_Type == "EXP" || s.Exp_Type=="EMP")
                        {
                            CashHand CH = new CiniLithoApp.CashHand();
                            CH.Billno = s.Exp_No;
                            CH.CSTName = s.Exp_Name;
                            CH.Mode = "Cash";
                            CH.BankRef = s.Exp_Detail;
                            CH.DebitAmt = (double)s.Exp_Amount;
                            CHS.Add(CH);
                            totamt -= (double)s.Exp_Amount;
                        }
                        if(s.Exp_Type=="OTH")
                        {
                            CashHand CH = new CiniLithoApp.CashHand();
                            CH.Billno = s.Exp_No;
                            CH.CSTName = s.Exp_Name;
                            CH.Mode = "Cash";
                            CH.BankRef = s.Exp_Detail;
                            CH.CreditAmt = (double)s.Exp_Amount;
                            CHS.Add(CH);
                            totamt += (double)s.Exp_Amount;
                        }
                    }
                    txt_totamt.Text = totamt.ToString();
                    string s2 = Process.GetCurrentProcess().MainModule.FileName;
                    RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Cash_hand.rpt");
                    RPT.Database.Tables[0].SetDataSource(CHS);
                    ((TextObject)RPT.ReportDefinition.ReportObjects["Cashtot"]).Text = totamt.ToString();
                    ((TextObject)RPT.ReportDefinition.ReportObjects["Text4"]).Text = "Date: " + txt_datepicker.Text;
                    //CashPreview CRPV = new CiniLithoApp.CashPreview(RPT);
                    //CRPV.ShowDialog();
                    // RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "E:\\test" + DateTime.Now.ToString("ddMMyyyy hhmmss") + ".pdf");

                    System.Windows.Forms.PrintDialog printDialog1 = new System.Windows.Forms.PrintDialog();
                    System.Drawing.Printing.PrintDocument PD = new System.Drawing.Printing.PrintDocument();
                    //Open the PrintDialog

                    System.Windows.Forms.DialogResult dr = printDialog1.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        //Get the Copy times
                        int nCopy = PD.PrinterSettings.Copies;
                        //Get the number of Start Page
                        int sPage = PD.PrinterSettings.FromPage;
                        //Get the number of End Page
                        int ePage = PD.PrinterSettings.ToPage;
                        //Get the printer name
                        string PrinterName = PD.PrinterSettings.PrinterName;

                        try
                        {
                            //Set the printer name to print the report to. By default the sample
                            //report does not have a default printer specified. This will tell the
                            //engine to use the specified printer to print the report. Print out 
                            //a test page (from Printer properties) to get the correct value.

                            RPT.PrintOptions.PrinterName = PrinterName;

                            //Start the printing process. Provide details of the print job
                            //using the arguments.
                            RPT.PrintToPrinter(1, false, 0, 0);

                            //Let the user know that the print job is completed
                            //MessageBox.Show("Report finished printing!");
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.ToString());
                        }
                    }
                }
                else
                {
                    totamt = 0;
                    var det = Cinidb.tbl_logdetail.Where(b => b.Orderdet != "Advance Amount Updated" && b.Orderdet != "update Customer entry and amount" && b.datim == datim && b.mode != "Cash" && b.paydet != "000").ToList();
                    Searchgrid.ItemsSource = det;
                    foreach (var s in det)
                    {
                        CashHand CH = new CiniLithoApp.CashHand();
                        CH.Billno = s.Rno;
                        CH.CreditAmt = double.Parse(s.paydet);
                        CH.CSTName = s.Rname;
                        CH.Mode = s.mode;
                        CH.BankRef = s.BankRef;
                        CHS.Add(CH);
                        totamt += double.Parse(s.paydet);
                    }                   
                    txt_totamt.Text = totamt.ToString();
                    string s2 = Process.GetCurrentProcess().MainModule.FileName;
                    RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Cash_hand.rpt");
                    RPT.Database.Tables[0].SetDataSource(CHS);
                    ((TextObject)RPT.ReportDefinition.ReportObjects["Cashtot"]).Text = totamt.ToString();
                    ((TextObject)RPT.ReportDefinition.ReportObjects["Text4"]).Text = "Date: "+txt_datepicker.Text;
                    //RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "E:\\test"+DateTime.Now.ToString("ddMMyyyy hhmmss")+".pdf");
                    System.Windows.Forms.PrintDialog printDialog1 = new System.Windows.Forms.PrintDialog();
                    System.Drawing.Printing.PrintDocument PD = new System.Drawing.Printing.PrintDocument();
                    //Open the PrintDialog

                    System.Windows.Forms.DialogResult dr = printDialog1.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        //Get the Copy times
                        int nCopy = PD.PrinterSettings.Copies;
                        //Get the number of Start Page
                        int sPage = PD.PrinterSettings.FromPage;
                        //Get the number of End Page
                        int ePage = PD.PrinterSettings.ToPage;
                        //Get the printer name
                        string PrinterName = PD.PrinterSettings.PrinterName;

                        try
                        {
                            //Set the printer name to print the report to. By default the sample
                            //report does not have a default printer specified. This will tell the
                            //engine to use the specified printer to print the report. Print out 
                            //a test page (from Printer properties) to get the correct value.

                            RPT.PrintOptions.PrinterName = PrinterName;

                            //Start the printing process. Provide details of the print job
                            //using the arguments.
                            RPT.PrintToPrinter(1, false, 0, 0);

                            //Let the user know that the print job is completed
                           // MessageBox.Show("Report finished printing!");
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.ToString());
                        }
                    }
                    }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
  
}
