using CiniLithoApp.AutoComplete;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditBillPage.xaml
    /// </summary>
    public partial class EditBillPage : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        List<Billdetail> BD = new List<Billdetail>();
        int SNO = 0; public string paymentmode = "";
        public static int Gridvalue = 0;
        public EditBillPage()
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            var pincodes = (from p in Cinidb.tbl_customer select p.Cname).Distinct().ToList();
            foreach (var pin in pincodes)
            {
                txt_customername.AddItem(new AutoCompleteEntry(pin, null));
            }

            for (int i = 1; i <= 100; i++)
            {
                for (int j = 1; j <= 100; j++)
                {
                    string s = i + "-" + j;
                    txt_size.AddItem(new AutoCompleteEntry(s, null));
                }
            }
            txt_color.AddItem(new AutoCompleteEntry("MC", null));
            txt_color.AddItem(new AutoCompleteEntry("SC", null));
            txt_color.AddItem(new AutoCompleteEntry("DC", null));
            txt_color.AddItem(new AutoCompleteEntry("FLEX", null));
            txt_orderdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            addcity();
            billno.Focusable = true;
            billno.Focus();

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
        private void txt_copies_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txt_rate_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txt_tax_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Tab)
                {
                    if (txt_rate.Text != null && txt_size.Text != null && txt_color.Text != null && txt_copies.Text != null && txt_tax.Text != null && txt_amount.Text != null)
                    {
                        SNO++;
                        double trate = double.Parse(txt_rate.Text);
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
                        BTN_SAVE.IsEnabled = true;
                    }
                    double Total = 0; double sgst = 0;
                    foreach (var TBT in BD)
                    {
                        Total += double.Parse(TBT.Amount);
                        double trate = double.Parse(TBT.Rate);
                        double taxrate = (trate * double.Parse(TBT.Tax)) / 100;
                        sgst += taxrate / 2;
                        txt_total.Text = Total.ToString();
                        txt_sgst.Text = sgst.ToString();
                        txt_cgst.Text = sgst.ToString();
                        txt_remaining.Text = (double.Parse(txt_total.Text) - (double.Parse(txt_advance.Text) + double.Parse(txt_payment.Text))).ToString();
                    }
                    Searchgrid.ItemsSource = BD;
                    Searchgrid.Items.Refresh();
                    clear();
                    txt_size.Focusable = true;
                    txt_size.Focus();
                }
            }
            catch { }
        }

        private void txt_amount_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txt_tax_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_rate.Text != "" && txt_copies.Text != "" && txt_tax.Text != "")
                {
                    double trate = double.Parse(txt_rate.Text);
                    double taxrate = (trate * double.Parse(txt_tax.Text)) / 100;
                    txt_amount.Text = (trate + taxrate).ToString();
                }

            }
            catch { }
        }

        private void txt_color_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txt_size_KeyDown(object sender, KeyEventArgs e)
        {

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

        }

        private void RBT_Cash_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RBT_SBI_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RBT_TMB_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Searchgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = Searchgrid.SelectedItem;
                if (item != null)
                {
                    string ID = (Searchgrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
                    Gridvalue = int.Parse(ID);
                    Searchgrid.IsReadOnly = false;
                }
            }
            catch { }

        }

        private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
        {
            int s = 0;
            try
            {
                var DelBill = Cinidb.tbl_billdetails.Where(b => b.Billno == billno.Text).ToList();
                foreach (var k in DelBill)
                {
                    Cinidb.tbl_billdetails.Remove(k);
                    Cinidb.SaveChanges();
                }
                var USTot = (from S in Cinidb.tbl_newbill where S.Billno == billno.Text select S).Single();
                USTot.Name = txt_customername.Text;
                USTot.Sno = cstcode;
                USTot.Address = txt_address.Text;
                USTot.Mobile = txt_phone.Text;
                USTot.deliverydate = DateTime.Parse(txt_deliverdate.Text);
                s = Cinidb.SaveChanges();
                foreach (var bdt in BD)
                {
                    tbl_billdetails tdb = new CiniLithoApp.tbl_billdetails();
                    tdb.Billno = billno.Text;
                    tdb.Size = bdt.Size;
                    tdb.Color = bdt.Color;
                    tdb.Copies = decimal.Parse(bdt.Copies);
                    tdb.Rate = decimal.Parse(bdt.Rate);
                    tdb.M_state = 0;
                    tdb.SCST = decimal.Parse(bdt.Tax) / 2;
                    tdb.CGST = decimal.Parse(bdt.Tax) / 2;
                    tdb.GSTState = RBT_GST.IsChecked.ToString();
                    Cinidb.tbl_billdetails.Add(tdb);
                    s = Cinidb.SaveChanges();
                }
                var UPTot = (from S in Cinidb.tbl_totaldetails where S.Billno == billno.Text select S).Single();
                UPTot.Total = double.Parse(txt_total.Text);
                UPTot.Advance = double.Parse(txt_advance.Text);
                UPTot.Payment = double.Parse(txt_payment.Text);
                UPTot.Balance = double.Parse(txt_remaining.Text);
                UPTot.SGST = decimal.Parse(txt_sgst.Text);
                UPTot.CGST = decimal.Parse(txt_cgst.Text);
                UPTot.BankRef = txt_bankref.Text;
                s = Cinidb.SaveChanges();             

                MessageBoxResult dg = MessageBox.Show("Bill Updated Successfully", "Message Info", MessageBoxButton.YesNo);
                clear();
                clear1();
            }
            catch (Exception ex)
            {

            }
        }

        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            clear();
            clear1();
        }

        private void defbtn_Click(object sender, RoutedEventArgs e)
        {
            object citem = Searchgrid.SelectedItem;
            int ID = int.Parse((Searchgrid.SelectedCells[0].Column.GetCellContent(citem) as TextBlock).Text);
            MessageBoxResult msg = MessageBox.Show("Do you Want To Delete this Item !! ", "Alert Message", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (msg == MessageBoxResult.Yes && ID != 0)
            {
                var result = BD.Where(b => b.No == ID).FirstOrDefault();
                if (result != null)
                {
                    BD.Remove(result);
                    double Total = 0; double sgst = 0;
                    SNO--;
                    foreach (var TBT in BD)
                    {
                        Total += double.Parse(TBT.Amount);
                        double trate = double.Parse(TBT.Rate);
                        double taxrate = (trate * double.Parse(TBT.Tax)) / 100;
                        sgst += taxrate / 2;
                        txt_total.Text = Total.ToString();
                        txt_sgst.Text = sgst.ToString();
                        txt_cgst.Text = sgst.ToString();
                        txt_remaining.Text = (double.Parse(txt_total.Text) - (double.Parse(txt_advance.Text) + double.Parse(txt_payment.Text))).ToString();
                    }
                    BTN_SAVE.IsEnabled = true;
                    Searchgrid.ItemsSource = BD;
                    Searchgrid.Items.Refresh();
                }
            }
        }
        private List<Billdetail> LoadCollectionData()
        {
            List<Billdetail> authors = new List<Billdetail>();

            return authors;
        }

        private void billno_KeyDown(object sender, KeyEventArgs e)
        {
          try
            { 
                if (e.Key == Key.Enter)
                {
                    if(billno.Text.Contains("CL")!=true)
                    {
                        billno.Text = "CL"+ billno.Text;
                    }
                    var nbill = Cinidb.tbl_newbill.Where(b => b.Billno ==billno.Text).SingleOrDefault();                  
                    var customer = Cinidb.tbl_customer.Where(b => b.Sno == nbill.Sno).SingleOrDefault();
                    var billdetail = Cinidb.tbl_billdetails.Where(b => b.Billno ==  billno.Text).ToList();
                    var paydetail = Cinidb.tbl_totaldetails.Where(b => b.Billno == billno.Text).SingleOrDefault();
                   
                    if (nbill.M_Status == "True") { clear1(); MessageBox.Show("Bill Locked","Alert"); BTN_SAVE.IsEnabled = false; }
                    else { BTN_SAVE.IsEnabled = true; }
                  
                    if (nbill != null)
                    {
                        billno.Text = nbill.Billno;
                        txt_customername.Text = nbill.Name;
                        txt_address.Text = nbill.Address;
                        txt_phone.Text = nbill.Mobile;
                        if (customer != null)
                        {
                            txt_advanceamount.Text = customer.advance.ToString();
                        }
                        txt_orderdate.Text = nbill.orderdate.ToString("dd-MM-yyyy");
                        txt_deliverdate.Text = nbill.deliverydate.ToString();
                        if (nbill.Billno.Contains("CL"))
                        {
                            RBT_Normal.IsChecked = true;
                        }
                        else
                        {
                            RBT_GST.IsChecked = true;
                        }
                       
                    }
                    if (billdetail != null)
                    {
                       BD.Clear();SNO = 0;
                        foreach (var s in billdetail)
                        {
                            double trate = double.Parse(s.Rate.ToString()) ;
                            double tax = double.Parse(s.SCST.ToString()) + double.Parse(s.CGST.ToString());
                            double taxrate = (trate * tax) / 100;                           
                            SNO++;
                            Billdetail BLD = new CiniLithoApp.Billdetail();
                            BLD.No = SNO;
                            BLD.Size = s.Size;
                            BLD.Color = s.Color;
                            BLD.Copies = s.Copies.ToString();
                            BLD.Rate = s.Rate.ToString();
                            BLD.Tax = (s.SCST + s.CGST).ToString();
                            BLD.Amount = (trate + taxrate).ToString();
                            BD.Add(BLD);
                            
                        }
                        Searchgrid.ItemsSource = BD;
                        Searchgrid.Items.Refresh();
                        txt_size.Focus();
                       
                    }
                    if (paydetail != null)
                    {
                        txt_total.Text = paydetail.Total.ToString();
                        txt_advance.Text = paydetail.Advance.ToString();
                        txt_payment.Text = paydetail.Payment.ToString();
                        txt_remaining.Text = paydetail.Balance.ToString();
                        txt_sgst.Text = paydetail.SGST.ToString();
                        txt_cgst.Text = paydetail.CGST.ToString();
                        txt_bankref.Text = paydetail.BankRef;
                        if (paydetail.Pmode == "Cash")
                        {
                            RBT_Cash.IsChecked = true;
                        }
                        else if (paydetail.Pmode == "SBI")
                        {
                            RBT_SBI.IsChecked = true;
                        }
                        else
                        {
                            RBT_TMB.IsChecked = true;
                        }
                        txt_remaining.Text = (double.Parse(txt_total.Text) - (double.Parse(txt_advance.Text) + double.Parse(txt_payment.Text))).ToString();
                    
                    }
                }
          }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }
        void clear()
        {
            txt_size.Text = "";
            txt_color.Text = "";
            txt_copies.Text = "";
            txt_rate.Text = "";
            txt_tax.Text = "";
            txt_amount.Text = "";
        }
        void clear1()
        {
            txt_size.Text = "";
            txt_color.Text = "";
            txt_copies.Text = "";
            txt_rate.Text = "";
            txt_tax.Text = "";
            txt_amount.Text = "";

            billno.Text = "";
            txt_customername.Text = "";
            txt_address.Text = "";
            txt_phone.Text = "";
            txt_orderdate.Text = "";
            txt_deliverdate.Text = "";
            txt_advanceamount.Text = "";

            RBT_Cash.IsChecked = false;
            RBT_SBI.IsChecked = false;
            RBT_TMB.IsChecked = false;

            RBT_Normal.IsChecked = false;
           
            SNO = 0;
            BD.Clear();
            Searchgrid.ItemsSource = null;
            Searchgrid.Items.Refresh();

            txt_total.Text = "000";
            txt_advance.Text = "000";
            txt_payment.Text = "000";
            txt_remaining.Text = "000";
            txt_sgst.Text = "000";
            txt_cgst.Text = "000";
            txt_bankref.Text = "";
            BTN_SAVE.IsEnabled = false;
            txt_customername.Focus();
        }

        private void Searchgrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                double Total = 0; double sgst = 0;
                object item = Searchgrid.SelectedItem;
              
                if (item != null)
                {
                    string ID = (Searchgrid.SelectedCells[4].Column.GetCellContent(item) as TextBlock).Text;
                    string tax = (Searchgrid.SelectedCells[5].Column.GetCellContent(item) as TextBlock).Text;
                    double taxrate1 = (double.Parse(ID) * double.Parse(tax)) / 100;
                    string totamt = (taxrate1 + double.Parse(ID)).ToString();
                    string color = (Searchgrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                    string Copies = (Searchgrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                    string Size = (Searchgrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;

                    var D = BD.Where(b => b.No == Gridvalue).Select(b => { b.Color = color;b.Size = Size;b.Copies = Copies;b.Rate = ID;b.Tax = tax; b.Amount = totamt; return b; }).SingleOrDefault();
                    Searchgrid.ItemsSource = BD;
                    foreach (var TBT in BD)
                    {
                        Total += double.Parse(TBT.Amount);
                        double trate = double.Parse(TBT.Rate);
                        double taxrate = (trate * double.Parse(TBT.Tax)) / 100;
                        sgst += taxrate / 2;
                        txt_total.Text = Total.ToString();
                        txt_sgst.Text = sgst.ToString();
                        txt_cgst.Text = sgst.ToString();
                        txt_remaining.Text = (double.Parse(txt_total.Text) - (double.Parse(txt_advance.Text) + double.Parse(txt_payment.Text))).ToString();
                    }

                }

            }
            catch(Exception ex) {  }
        }

        private void txt_copies_TextChanged(object sender, TextChangedEventArgs e)
        {

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
                        txt_size.Focusable = true;
                        txt_size.Focus();
                    }

                }
            }
            catch { }
        }
    }
}
