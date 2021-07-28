using CiniLithoApp.AutoComplete;
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
    /// Interaction logic for Billpage.xaml
    /// </summary>
    public partial class Billpage : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        List<Billdetail> BD = new List<Billdetail>();
         public static string Y_lable = "";
        public static int Recount = 9000;
        int SNO = 0;public string paymentmode = "";
        ReportDocument RPT = new ReportDocument();
        public Billpage()
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            //Printbill("CL1005");
            Searchgrid.ItemsSource = LoadCollectionData();
            var pincodes = (from p in Cinidb.tbl_customer select p.Cname).Distinct().ToList();
            foreach (var pin in pincodes)
            {
                txt_customername.AddItem(new AutoCompleteEntry(pin, null));
            }

            //for (int i=1;i<=100;i++)
            //{
            //    for(int j=1;j<=100;j++)
            //    {
            //        string s = i + "-" + j;
            //        txt_size.AddItem(new AutoCompleteEntry(s, null));
            //    }
            //}

            txt_size.AddItem(new AutoCompleteEntry("Bit 1", null));
            txt_size.AddItem(new AutoCompleteEntry("Bit 2", null));
            txt_size.AddItem(new AutoCompleteEntry("Bit 3", null));
            txt_size.AddItem(new AutoCompleteEntry("Bit 4", null));
            txt_size.AddItem(new AutoCompleteEntry("Bit 6", null));
            txt_size.AddItem(new AutoCompleteEntry("Bit 8", null));
            txt_size.AddItem(new AutoCompleteEntry("Bit 10", null));           

            txt_orderdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            addcity();
            RBT_Normal.IsChecked = true;
           // billno.Text = GetCustCode("False");
            txt_customername.Focus();
            txt_deliverdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
          
        }

        void addcity()
        {
            txt_address.AddItem(new AutoCompleteEntry("Ariyalur", null));
            txt_address.AddItem(new AutoCompleteEntry("Chennai", null));
            txt_address.AddItem(new AutoCompleteEntry("Coimbatore", null));
            txt_address.AddItem(new AutoCompleteEntry("Cuddalore", null));
            txt_address.AddItem(new AutoCompleteEntry("Dharmapuri", null));
            txt_address.AddItem(new AutoCompleteEntry("Dindigul", null));
            txt_address.AddItem(new AutoCompleteEntry("Erode", null));
            txt_address.AddItem(new AutoCompleteEntry("Kallakurichi", null));
            txt_address.AddItem(new AutoCompleteEntry("Kanchipuram", null));
            txt_address.AddItem(new AutoCompleteEntry("Kanniyakumari", null));
            txt_address.AddItem(new AutoCompleteEntry("Karur", null));
            txt_address.AddItem(new AutoCompleteEntry("Krishnagiri", null));
            txt_address.AddItem(new AutoCompleteEntry("Madurai", null));
            txt_address.AddItem(new AutoCompleteEntry("Nagapattinam", null));
            txt_address.AddItem(new AutoCompleteEntry("Namakkal", null));
            txt_address.AddItem(new AutoCompleteEntry("Nilgiris", null));
            txt_address.AddItem(new AutoCompleteEntry("Perambalur", null));
            txt_address.AddItem(new AutoCompleteEntry("Pudukkottai", null));
            txt_address.AddItem(new AutoCompleteEntry("Ramanathapuram", null));
            txt_address.AddItem(new AutoCompleteEntry("Salem", null));
            txt_address.AddItem(new AutoCompleteEntry("Sivaganga", null));
            txt_address.AddItem(new AutoCompleteEntry("Thanjavur", null));
            txt_address.AddItem(new AutoCompleteEntry("Theni", null));
            txt_address.AddItem(new AutoCompleteEntry("Thoothukudi", null));
            txt_address.AddItem(new AutoCompleteEntry("Tiruchirappalli", null));
            txt_address.AddItem(new AutoCompleteEntry("Tirunelveli", null));
            txt_address.AddItem(new AutoCompleteEntry("Tiruppur", null));
            txt_address.AddItem(new AutoCompleteEntry("Tiruvallur", null));
            txt_address.AddItem(new AutoCompleteEntry("Tiruvannamalai", null));
            txt_address.AddItem(new AutoCompleteEntry("Tiruvarur", null));
            txt_address.AddItem(new AutoCompleteEntry("Vellore", null));
            txt_address.AddItem(new AutoCompleteEntry("Viluppuram", null));
            txt_address.AddItem(new AutoCompleteEntry("Virudhunagar", null));

        }

        String GetCustCode(string type)
        {
            List<String> BN = new List<string>();
            long i = 0; string s = "";
           
            BN = (from categorytypes in Cinidb.tbl_newbill where categorytypes.GSTSTAT.Equals(type) && categorytypes.YLabel==Y_lable select categorytypes.Billno).ToList();
            if (BN.Count > 0 && BN.Count < Recount)
            {
                s = BN[BN.Count - 1];

            }
            else 
            {
                string[] arr = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
                string target = Y_lable;
                int results = Array.FindIndex(arr, s1 => s1.Equals(target));
                if (results == -1) { results = 0; } 
                var data = arr[results]; Y_lable = data.ToString();
                var det = Cinidb.tbl_alfserial.SingleOrDefault();
                if (det != null)
                {                 
                    det.SERALB = data.ToString();               
                    Cinidb.SaveChanges();
                }
                else
                {
                    tbl_alfserial tbl_Alfserial = new tbl_alfserial();
                    tbl_Alfserial.SERNO = results;
                    tbl_Alfserial.SERALB = Y_lable;
                    Cinidb.tbl_alfserial.Add(tbl_Alfserial);
                    Cinidb.SaveChanges();
                }
                s = ""; 
            }      


            if (type == "True")
            {
                if (s != "" && s != null)
                {
                    string TR = s.Remove(0, 3);
                    int BNO = Convert.ToInt32(TR) + 1;
                    s = "CLG" + Y_lable  + BNO + "/" + DateTime.Now.Year.ToString();
                }
                else
                {
                    s = "CLG1001" + Y_lable + "/" + DateTime.Now.Year.ToString();
                }
            }
            if (type == "False")
            {
                if (s != "" && s != null)
                {
                    string TR = s.Remove(0, 3);
                    int BNO = Convert.ToInt32(TR) + 1;
                    s = "CL"+Y_lable + BNO+"/"+DateTime.Now.Year.ToString();
                }
                else
                {
                    s = "CL"+ Y_lable + "1001" + "/" + DateTime.Now.Year.ToString();
                }
            }

            return s;
        }

        private void SearchGridScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void Searchgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
          
            

        }

        private List<Billdetail> LoadCollectionData()
        {
            List<Billdetail> authors = new List<Billdetail>();
           
            return authors;
        }
        int cstcode = 0;
        private void txt_customername_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                var billname = txt_customername.Text.Trim();
                if (billname != "")
                {
                    var customer = Cinidb.tbl_customer.Where(b => b.Cname.Equals(billname)).SingleOrDefault();
                    if (customer != null)
                    {
                        txt_address.Text = customer.address;
                        txt_phone.Text = customer.cphone;
                        txt_advanceamount.Text = customer.advance.ToString();
                        cstcode = customer.Sno;
                        customerno.Text = cstcode.ToString();
                        txt_size.Focusable = true;
                        txt_size.Focus();
                    }
                   
                }
            }
            catch { }
     }

        private void Searchgrid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
               int j = Searchgrid.CurrentColumn.DisplayIndex;
                if(j==6)
                {
                    DataGrid dataGrid = sender as DataGrid;
                    DataGridRow row = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(dataGrid.SelectedIndex);
                    DataGridCell RowColumn_7 = dataGrid.Columns[6].GetCellContent(row).Parent as DataGridCell;
                    DataGridCell RowColumn_6 = dataGrid.Columns[5].GetCellContent(row).Parent as DataGridCell;
                    DataGridCell RowColumn_5 = dataGrid.Columns[4].GetCellContent(row).Parent as DataGridCell;
                    DataGridCell RowColumn_4 = dataGrid.Columns[3].GetCellContent(row).Parent as DataGridCell;
                    DataGridCell RowColumn_3 = dataGrid.Columns[2].GetCellContent(row).Parent as DataGridCell;
                    DataGridCell RowColumn_2 = dataGrid.Columns[1].GetCellContent(row).Parent as DataGridCell;
                    DataGridCell RowColumn_1 = dataGrid.Columns[0].GetCellContent(row).Parent as DataGridCell;
                    int datagridcount = dataGrid.Items.Count-1;
                   
                    row.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                }
            }
            
        }

        private void txt_size_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                txt_color.Focus();
            }
        }

        private void txt_color_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_copies.Focus();
            }
        }

        private void txt_copies_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_rate.Focus();
            }
        }

        private void txt_rate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                txt_tax.Focus();
            }
        }
       
        private void txt_tax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {             
                if(txt_rate.Text!=null && txt_rate.Text != "" && txt_size.Text!=null && txt_color.Text!=null && txt_copies.Text!=null && txt_tax.Text!=null && txt_amount.Text!=null)
                {
                    SNO++;
                    double trate = double.Parse(txt_rate.Text) ;
                    double taxrate = (trate * double.Parse(txt_tax.Text)) / 100;
                    txt_amount.Text = (trate + taxrate).ToString();
                    Billdetail BDT = new CiniLithoApp.Billdetail();
                    BDT.No = SNO;
                    BDT.Size = txt_size.Text;
                    BDT.Color = txt_color.Text;
                    BDT.Copies = txt_copies.Text;
                    BDT.Rate = txt_rate.Text;
                    BDT.Tax = txt_tax.Text;
                    BDT.Amount = txt_amount.Text;
                    BD.Add(BDT);
                  
                }
                double Total = 0; double sgst = 0;
                foreach (var TBT in BD)
                {
                    Total += double.Parse(TBT.Amount);
                    double trate = double.Parse(TBT.Rate) ;
                    double taxrate = (trate * double.Parse(TBT.Tax)) / 100;
                    sgst += taxrate / 2;
                    txt_total.Text = Total.ToString();
                    txt_sgst.Text = sgst.ToString();
                    txt_cgst.Text = sgst.ToString();
                    txt_remaining.Text = Total.ToString();
                }
                Searchgrid.ItemsSource = BD;
                Searchgrid.Items.Refresh();
                clear();
                txt_size.Focusable = true;
                txt_size.Focus();
                
            }
        }
        void clear()
        {
            txt_size.Text = "";
            txt_color.Text = "";
            txt_copies.Text = "";
            txt_rate.Text = "";
            txt_tax.Text = "0";
            txt_amount.Text = "";
        }
        void clear1()
        {
           txt_size.Text="";
           txt_color.Text = "";
           txt_copies.Text = "";
           txt_rate.Text = "";
           txt_tax.Text = "0";
           txt_amount.Text = "";

            billno.Text = "";
            txt_customername.Text="";
            cstcode = 0;
            txt_address.Text = "";
            txt_phone.Text = "";
            txt_orderdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txt_deliverdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txt_advanceamount.Text = "0";

            RBT_Cash.IsChecked = false;
            RBT_Normal.IsChecked = true;
            billno.Text = GetCustCode("False");

            SNO = 0;
            BD.Clear();
            Searchgrid.ItemsSource = null;
            Searchgrid.Items.Refresh();
            txt_total.Text = "000";
            txt_sgst.Text = "000";
            txt_cgst.Text = "000";
            txt_advance.Text = "000";
            txt_remaining.Text = "000";
            txt_payment.Text = "000";
          
            txt_customername.Focus();
        }
        private void txt_amount_KeyDown(object sender, KeyEventArgs e)
        {
            txt_size.Focusable = true;
            txt_size.Focus();

        }

        private void txt_customername_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Tab)
            {
                txt_copies.Focusable = true;
                txt_copies.Focus();
            }
        }

        private void defbtn_Click(object sender, RoutedEventArgs e)
        {
            object citem = Searchgrid.SelectedItem;
            int ID =int.Parse((Searchgrid.SelectedCells[0].Column.GetCellContent(citem) as TextBlock).Text);
            MessageBoxResult msg = MessageBox.Show("Do you Want To Delete this Item !! ", "Alert Message", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (msg == MessageBoxResult.Yes && ID != 0)
            {
                var result = BD.Where(b => b.No == ID).FirstOrDefault();
                if(result!=null)
                {
                    BD.Remove(result);
                    double Total = 0; double sgst = 0;
                    foreach (var TBT in BD)
                    {
                        Total += double.Parse(TBT.Amount);
                        double trate = double.Parse(TBT.Rate) * double.Parse(TBT.Copies);
                        double taxrate = (trate * double.Parse(TBT.Tax)) / 100;
                        sgst += taxrate / 2;
                        txt_total.Text = Total.ToString();
                        txt_sgst.Text = sgst.ToString();
                        txt_cgst.Text = sgst.ToString();
                    }
                    if(BD==null)
                    {
                        txt_total.Text = "000";
                        txt_sgst.Text = "000";
                        txt_cgst.Text = "000";
                        txt_advance.Text = "000";
                        txt_remaining.Text = "000";
                        txt_payment.Text = "000";
                    }
                    Searchgrid.ItemsSource = BD;
                    Searchgrid.Items.Refresh();                   
                }
            }
        }

        private void txt_tax_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (txt_rate.Text != "" && txt_copies.Text != "" && txt_tax.Text != "")
            {
                double trate = double.Parse(txt_rate.Text) ;
                double taxrate = (trate * double.Parse(txt_tax.Text)) / 100;
                txt_amount.Text = (trate + taxrate).ToString();
                
            }
        }

        private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                if (billno.Text == "") { MessageBox.Show("Bill No is Empty...!"); return; }
                if (txt_customername.Text == "") { MessageBox.Show("Customer Name is Empty...!");return; }
                if (txt_address.Text == "") { MessageBox.Show("Address is Empty..!"); return; }
                if (txt_phone.Text == "") { MessageBox.Show("Phone No is Empty...!"); return; }
                if (txt_deliverdate.Text == "") { MessageBox.Show("Delivery Date is not selected...!"); return; }
                if (txt_orderdate.Text == "") { MessageBox.Show("Order Date is Empty !"); return; }
               
                if (paymentmode == "") { MessageBox.Show("Please selecte any one payment mode...!"); return; }
                if (BD.Count == 0) { MessageBox.Show("No matrials list available...!"); return; }
                //if (paymentmode == "SBI" || paymentmode == "TMB")
                //{
                //    if (txt_bankref.Text == "")
                //    {
                //        MessageBox.Show("Please add Bank Reference", "Alert");
                //        return;
                //    }
                //}
                tbl_newbill tnb = new CiniLithoApp.tbl_newbill();
                tnb.Billno = billno.Text;
                tnb.Name = txt_customername.Text;
                tnb.Address = txt_address.Text;
                tnb.Mobile = txt_phone.Text;
                tnb.orderdate = DateTime.Parse(DateTime.Now.ToString());
                if (txt_deliverdate.Text != "")
                {
                    tnb.deliverydate = DateTime.Parse(txt_deliverdate.Text);
                }
                
                tnb.rmark = "";                
                tnb.Sno = cstcode;
                
                tnb.GSTSTAT = RBT_GST.IsChecked.ToString();
                tnb.YLabel = Y_lable;
                Cinidb.tbl_newbill.Add(tnb);
                int n= Cinidb.SaveChanges();
                int s = 0;int f = 0;
                if(n>0)
                {
                    foreach(var bdt in BD)
                    {
                        tbl_billdetails tdb = new CiniLithoApp.tbl_billdetails();
                        tdb.Billno = billno.Text;
                        tdb.Size = bdt.Size;
                        tdb.Color = bdt.Color;
                        tdb.Copies =decimal.Parse(bdt.Copies);
                        tdb.Rate = decimal.Parse(bdt.Rate);
                        tdb.M_state = 0;
                        tdb.SCST = decimal.Parse(bdt.Tax)/2;
                        tdb.CGST = decimal.Parse(bdt.Tax) / 2;
                        tdb.GSTState = RBT_GST.IsChecked.ToString();
                        Cinidb.tbl_billdetails.Add(tdb);
                        s=Cinidb.SaveChanges();
                    }
                    if(s>0)
                    {
                        tbl_totaldetails TTD = new CiniLithoApp.tbl_totaldetails();
                        TTD.Billno = billno.Text;
                        TTD.Total =double.Parse(txt_total.Text);
                        TTD.Advance = double.Parse(txt_advance.Text);
                        TTD.Payment = double.Parse(txt_payment.Text); 
                        TTD.Balance = double.Parse(txt_remaining.Text); 
                        TTD.Discount = "0";
                        TTD.PDate = DateTime.Now.ToString("dd-MM-yyyy");
                        TTD.Adate = DateTime.Now.ToString("dd-MM-yyyy");
                        TTD.Pmode = paymentmode;
                        TTD.SGST = decimal.Parse(txt_sgst.Text);
                        TTD.CGST = decimal.Parse(txt_cgst.Text);
                        TTD.BankRef = txt_bankref.Text;
                        Cinidb.tbl_totaldetails.Add(TTD);
                        f = Cinidb.SaveChanges();
                        if(f==1)
                        {
                            if (cstcode != 0)
                            {
                                var USTot = (from S in Cinidb.tbl_customer where S.Sno == cstcode select S).Single();
                                if (USTot.advance > decimal.Parse(txt_advance.Text))
                                { USTot.advance = USTot.advance - decimal.Parse(txt_advance.Text); }
                                else
                                {
                                    USTot.advance = decimal.Parse(txt_advance.Text) - USTot.advance;
                                }
                                int K = Cinidb.SaveChanges();
                            }

                            tbl_logdetail tld = new CiniLithoApp.tbl_logdetail();
                            tld.Rno = billno.Text;
                            tld.Rname = txt_customername.Text;
                            tld.Rmobile = txt_phone.Text;
                            tld.Orderdet =BD.Count+ " items for new entry to Advance";
                            tld.paydet = txt_advance.Text;
                            tld.mode = paymentmode;
                            tld.datim = DateTime.Now.ToString("dd-MM-yyyy");
                            tld.BankRef = txt_bankref.Text;
                            Cinidb.tbl_logdetail.Add(tld);
                            Cinidb.SaveChanges();

                            MessageBoxResult dg=MessageBox.Show("Do you want Print bill","Message Info",MessageBoxButton.YesNo);
                            if(dg==MessageBoxResult.Yes)
                            {

                                
                                Printbill(billno.Text);
                                //BillPrint Bp = new BillPrint(billno.Text);
                                //Bp.ShowDialog();
                            }
                            clear1();
                        }
                    }
                }
            }
            catch(Exception EX) { MessageBox.Show(EX.Message); }
        }
        
        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clear1();
            }
            catch (Exception EX) { }
        }

        private void RBT_GST_Checked(object sender, RoutedEventArgs e)
        {
            string code = GetCustCode("True");
            billno.Text = code;
        }

        private void RBT_Normal_Checked(object sender, RoutedEventArgs e)
        {
            string code = GetCustCode("False");
            billno.Text = code;
        }

        private void RBT_Cash_Checked(object sender, RoutedEventArgs e)
        {
            paymentmode = "CASH";
            txt_bankref.IsEnabled = false;
            txt_bankref.Text = "";
        }

        private void RBT_SBI_Checked(object sender, RoutedEventArgs e)
        {
            paymentmode = "SBI";
            txt_bankref.IsEnabled = true;
           
        }

        private void RBT_TMB_Checked(object sender, RoutedEventArgs e)
        {
            paymentmode = "TMB";
            txt_bankref.IsEnabled = true;
        }

        private void allowinteger(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Char newChar = e.Text.ToString()[0];
                e.Handled = !(Char.IsNumber(newChar) || (newChar == '.'));
            }
            catch (Exception ex)
            {             
                 MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
             }
        }

        private void txt_advance_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            { 
            if(txt_advance.Text!="" && txt_advance.Text!="000")
            {
                double balance = double.Parse(txt_total.Text) - double.Parse(txt_advance.Text);
                txt_remaining.Text = balance.ToString();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        void Printbill(string BillNo)
        {
            List<BillReportClass> BRC = new List<BillReportClass>();
            if(BillNo!="")
            {
                int BCount = 0;
                var bill = Cinidb.BillPrint1(BillNo).ToList();
                foreach(var s in bill)
                {
                    BCount++;
                    BillReportClass BC = new CiniLithoApp.BillReportClass();
                    BC.sno = BCount;
                    BC.billno = s.Billno;
                    BC.name = s.Name;
                    BC.Phone = s.Mobile;
                    BC.Address = s.address;
                    BC.TotalAmount = s.Total;
                    BC.Amount = double.Parse(s.Rate.ToString());
                    BC.SGST =(double) s.SGST;
                    BC.CGST = (double) s.CGST;
                    BC.color = s.Color;
                    BC.rate = (double)s.Rate;
                    BC.size = s.Size;
                    BC.copies = (int) s.Copies;
                    BC.orderdate = s.orderdate;
                    BC.deliverdate = s.deliverydate;
                    BC.Taxper = double.Parse(s.PCgst.ToString()) + double.Parse(s.PSgct.ToString());
                    BRC.Add(BC);
                }
                string s2 = Process.GetCurrentProcess().MainModule.FileName;
                if (RBT_GST.IsChecked == true)
                {
                    RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Bill_RPT.rpt");
                }
                else
                {
                    RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\NBill_RPT.rpt");
                }
                RPT.Database.Tables[0].SetDataSource(BRC);
                // RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, "D:\\Bill-" + DateTime.Now.ToString("ddMMyyyy hhmmss") + ".pdf");
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
                        MessageBox.Show("Report finished printing!");
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.ToString());
                    }
                }
            }
        }

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.F4 )
            {
                txt_advance.Focusable = true;
                txt_advance.Focus();
                txt_advance.SelectAll();
            }
            if(e.Key==Key.F5)
            {
                BTN_SAVE_Click(null, null);
            }
        }

        private void txt_color_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txt_color.Text == "M") { txt_color.Text = "MC"; txt_color.SelectAll(); }
                if (txt_color.Text == "D") { txt_color.Text = "DC"; txt_color.SelectAll(); }
                if (txt_color.Text == "S") { txt_color.Text = "SC"; txt_color.SelectAll(); }
            }
            catch { }
        }

        private void txt_payment_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txt_advance_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Tab)
            {
                RBT_Cash.Focusable = true;
                RBT_Cash.Focus();
                
            }
        }

        private void RBT_Cash_GotFocus(object sender, RoutedEventArgs e)
        {
            RBT_Cash.IsChecked = true;
        }

        private void RBT_SBI_GotFocus(object sender, RoutedEventArgs e)
        {
            RBT_SBI.IsChecked = true;
        }

        private void RBT_TMB_GotFocus(object sender, RoutedEventArgs e)
        {
            RBT_TMB.IsChecked = true;
        }

       
        public static void Send(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, key)
                    {
                        RoutedEvent = Keyboard.KeyDownEvent
                    };
                    InputManager.Current.ProcessInput(e);

                    // Note: Based on your requirements you may also need to fire events for:
                    // RoutedEvent = Keyboard.PreviewKeyDownEvent
                    // RoutedEvent = Keyboard.KeyUpEvent
                    // RoutedEvent = Keyboard.PreviewKeyUpEvent
                }
            }
        }

       
    }
    public class Billdetail
    {
        public int No { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Copies { get; set; }
        public string Rate { get; set; }
        public string Tax { get; set; }
        public string Amount { get; set; }
    }
}
