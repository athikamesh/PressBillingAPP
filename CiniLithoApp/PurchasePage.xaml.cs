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
    /// Interaction logic for PurchasePage.xaml
    /// </summary>
    public partial class PurchasePage : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        string paymentmode = "";string mode = "";
        public List<Purchase_List> dataToShow2 = new List<Purchase_List>();
        decimal Amt = 0, cgst = 0, sgst = 0, tamt = 0;
        public PurchasePage(string billno,string _mode)
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            Get_Detail(billno);
            mode = _mode;
            var pincodes = (from p in Cinidb.tbl_Supplier select p.CPName).Distinct().ToList();
            foreach (var pin in pincodes)
            {
                txt_search.AddItem(new AutoCompleteEntry(pin, null));
                txt_companyname.Items.Add(pin);
            }
            loaddata();
        }
        void Get_Detail(string bill_no)
        {
            try
            {
                var det = Cinidb.tbl_purchase.Where(b => b.Billno == bill_no).SingleOrDefault();
                if (det != null)
                {
                    txt_companyname.Text = det.CompanyName;
                    txt_nproductdetail.Text = det.ProductDetail;
                    txt_HNS.Text = det.HNSNo;
                    txt_Billno.Text = det.Billno;
                    txt_amount.Text = det.Amount.ToString();
                    txt_cgst.Text = det.CGST.ToString();
                    txt_sgst.Text = det.SGST.ToString();
                    double balance = double.Parse(txt_amount.Text) + double.Parse(txt_cgst.Text) + double.Parse(txt_sgst.Text);
                    txt_total.Text = balance.ToString();
                    txt_purchasedate.SelectedDate = det.Purchasedate;
                    if (det.PaymentMode == "CASH")
                    {
                        RBT_Cash.IsChecked = true;
                    }
                    if (det.PaymentMode == "SBI")
                    {
                        RBT_SBI.IsChecked = true;
                    }
                    if (det.PaymentMode == "TMB")
                    {
                        RBT_TMB.IsChecked = true;
                    }
                    if(det.PaymentMode=="")
                    {
                        RBT_Cash.IsChecked = false;
                        RBT_SBI.IsChecked = false;
                        RBT_TMB.IsChecked = false;
                    }
                    txt_paymentdate.SelectedDate = det.Paymentdate;
                    mode = "";
                }
            }catch { }

        }
        private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (mode == "SAVE")
                {
                    if (txt_companyname.Text != "" && txt_nproductdetail.Text != "" && txt_HNS.Text != "" && txt_Billno.Text != "" && txt_amount.Text != "" && txt_cgst.Text != "" && txt_sgst.Text != "" && txt_total.Text != "" && txt_purchasedate.Text != "")
                    {
                        tbl_purchase TP = new CiniLithoApp.tbl_purchase();
                        TP.CompanyName = txt_companyname.Text;
                        TP.ProductDetail = txt_nproductdetail.Text;
                        TP.HNSNo = txt_HNS.Text;
                        TP.Billno = txt_Billno.Text;
                        TP.Amount = decimal.Parse(txt_amount.Text);
                        TP.CGST = decimal.Parse(txt_cgst.Text);
                        TP.SGST = decimal.Parse(txt_sgst.Text);
                        TP.TotalAmount = decimal.Parse(txt_total.Text);
                        TP.Purchasedate = txt_purchasedate.SelectedDate;
                        TP.PaymentMode = paymentmode;
                        TP.Paymentdate = txt_paymentdate.SelectedDate;
                        Cinidb.tbl_purchase.Add(TP);
                        int i= Cinidb.SaveChanges();
                        if(i>0)
                        {
                            MessageBox.Show("Add Successfully", "Info");
                        }
                        clear();
                       
                    }
                }
                else
                {
                    if (txt_companyname.Text != "" && txt_nproductdetail.Text != "" && txt_HNS.Text != "" && txt_Billno.Text != "" && txt_amount.Text != "" && txt_cgst.Text != "" && txt_sgst.Text != "" && txt_total.Text != "" && txt_purchasedate.Text != "")
                    {
                        var tblpur = Cinidb.tbl_purchase.Where(b => b.Billno == txt_Billno.Text).SingleOrDefault();
                        tblpur.PaymentMode = paymentmode;
                        tblpur.Paymentdate = txt_paymentdate.DisplayDate;
                        Cinidb.SaveChanges();
                        clear();
                    }
                }
                loaddata();
            }
            catch { }
        }

        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                clear();
            }
            catch { }
        }
        void clear()
        {
            txt_companyname.Text = "";
            txt_nproductdetail.Text = "";
            txt_HNS.Text = "";
            txt_Billno.Text = "";
            txt_amount.Text = "";
            txt_cgst.Text = "";
            txt_sgst.Text = "";
            txt_total.Text = "";
            txt_purchasedate.Text = "";
            RBT_Cash.IsChecked = false;
            RBT_SBI.IsChecked = false;
            RBT_TMB.IsChecked = false;
            txt_paymentdate.Text = "";
            txt_companyname.Focus();
            mode = "SAVE";
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

        private void txt_amount_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_amount.Text != "" && txt_amount.Text != "000" && txt_cgst.Text != "" && txt_cgst.Text != "000" && txt_sgst.Text != "" && txt_sgst.Text != "000")
            {
                double balance = double.Parse(txt_amount.Text) + double.Parse(txt_cgst.Text)+ double.Parse(txt_sgst.Text);
                txt_total.Text = balance.ToString();
            }
        }

        private void txt_cgst_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_amount.Text != "" && txt_amount.Text != "000" && txt_cgst.Text != "" && txt_cgst.Text != "000" && txt_sgst.Text != "" && txt_sgst.Text != "000")
            {
                double balance = double.Parse(txt_amount.Text) + double.Parse(txt_cgst.Text) + double.Parse(txt_sgst.Text);
                txt_total.Text = balance.ToString();
            }
        }

        private void txt_sgst_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_amount.Text != "" && txt_amount.Text != "000" && txt_cgst.Text != "" && txt_cgst.Text != "000" && txt_sgst.Text != "" && txt_sgst.Text != "000")
            {
                double balance = double.Parse(txt_amount.Text) + double.Parse(txt_cgst.Text) + double.Parse(txt_sgst.Text);
                txt_total.Text = balance.ToString();
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

        private void RBT_Cash_Checked(object sender, RoutedEventArgs e)
        {
            paymentmode = "CASH";
          
        }

        private void RBT_SBI_Checked(object sender, RoutedEventArgs e)
        {
            paymentmode = "SBI";
        }

        private void RBT_TMB_Checked(object sender, RoutedEventArgs e)
        {
            paymentmode = "TMB";
            
        }

        private void txt_Billno_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Get_Detail(txt_Billno.Text);
                }
            }
            catch { }
        }

        private void btn_addsup_Click(object sender, RoutedEventArgs e)
        {
            Supplier_Frm SF = new CiniLithoApp.Supplier_Frm();
            SF.ShowDialog();
        }

        void loaddata()
        {
            dataToShow2 = new List<Purchase_List>();
            DateTime dt = DateTime.Now.Date;
            var data = Cinidb.tbl_purchase.Where(b => b.Purchasedate == dt).ToList();
            foreach (var d in data)
            {
                Purchase_List PL = new CiniLithoApp.Purchase_List();
                PL.sno = d.Sno;
                PL.Company_Name = d.CompanyName;
                PL.Product_Detail = d.ProductDetail;
                PL.HNS_No = d.HNSNo;
                PL.Bill_no = d.Billno;
                PL.amount = d.Amount.ToString();
                PL.cgst = d.CGST.ToString();
                PL.sgst = d.SGST.ToString();
                PL.Total_Amount = d.TotalAmount.ToString();
                PL.Purchase_date = (DateTime)d.Purchasedate;
                PL.PaymentMode = d.PaymentMode;
                if (d.Paymentdate != null)
                {
                    PL.Paymentdate = (DateTime)d.Paymentdate;
                }
                
                dataToShow2.Add(PL);
                Amt += (decimal)d.Amount;
                cgst += (decimal)d.CGST;
                sgst += (decimal)d.SGST;
                tamt += (decimal)d.TotalAmount;
            }
            Searchgrid.ItemsSource = dataToShow2;
            Searchgrid.Items.Refresh();
            amt.Text = Amt.ToString();
            cgsttax.Text = cgst.ToString();
            sgsttax.Text = sgst.ToString();
            totalamt.Text = tamt.ToString();
        }

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                dataToShow2 = new List<Purchase_List>();
                var data = Cinidb.tbl_purchase.Where(b => b.CompanyName == txt_search.Text).ToList();
                foreach (var d in data)
                {
                    Purchase_List PL = new CiniLithoApp.Purchase_List();
                    PL.sno = d.Sno;
                    PL.Company_Name = d.CompanyName;
                    PL.Product_Detail = d.ProductDetail;
                    PL.HNS_No = d.HNSNo;
                    PL.Bill_no = d.Billno;
                    PL.amount = d.Amount.ToString();
                    PL.cgst = d.CGST.ToString();
                    PL.sgst = d.SGST.ToString();
                    PL.Total_Amount = d.TotalAmount.ToString();
                    PL.Purchase_date = (DateTime)d.Purchasedate;
                    PL.PaymentMode = d.PaymentMode;
                    if (d.Paymentdate != null)
                    {
                        PL.Paymentdate = (DateTime)d.Paymentdate;
                    }
                    dataToShow2.Add(PL);
                    Amt += (decimal)d.Amount;
                    cgst += (decimal)d.CGST;
                    sgst += (decimal)d.SGST;
                    tamt += (decimal)d.TotalAmount;
                }
                Searchgrid.ItemsSource = dataToShow2;
                Searchgrid.Items.Refresh();
                amt.Text = Amt.ToString();
                cgsttax.Text = cgst.ToString();
                sgsttax.Text = sgst.ToString();
                totalamt.Text = tamt.ToString();
            }
        }

        private void fromdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (todate.Text != "" && fromdate.Text != "")
                {
                    dataToShow2.Clear();
                    var data = Cinidb.tbl_purchase.Where(b => b.Purchasedate >= fromdate.SelectedDate && b.Purchasedate <= todate.SelectedDate).ToList();
                    foreach (var d in data)
                    {
                        Purchase_List PL = new CiniLithoApp.Purchase_List();
                        PL.sno = d.Sno;
                        PL.Company_Name = d.CompanyName;
                        PL.Product_Detail = d.ProductDetail;
                        PL.HNS_No = d.HNSNo;
                        PL.Bill_no = d.Billno;
                        PL.amount = d.Amount.ToString();
                        PL.cgst = d.CGST.ToString();
                        PL.sgst = d.SGST.ToString();
                        PL.Total_Amount = d.TotalAmount.ToString();
                        PL.Purchase_date = (DateTime)d.Purchasedate;
                        PL.PaymentMode = d.PaymentMode;
                        if (d.Paymentdate != null)
                        {
                            PL.Paymentdate = (DateTime)d.Paymentdate;
                        }
                        dataToShow2.Add(PL);
                        Amt += (decimal)d.Amount;
                        cgst += (decimal)d.CGST;
                        sgst += (decimal)d.SGST;
                        tamt += (decimal)d.TotalAmount;
                    }
                    if (data.Count == 0)
                    {
                        MessageBox.Show("Data Not Found", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    Searchgrid.ItemsSource = dataToShow2;
                    Searchgrid.Items.Refresh();
                    amt.Text = Amt.ToString();
                    cgsttax.Text = cgst.ToString();
                    sgsttax.Text = sgst.ToString();
                    totalamt.Text = tamt.ToString();
                }
            }
            catch { }
        }

        private void todate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (todate.Text != "" && fromdate.Text != "")
                {
                    dataToShow2.Clear();
                    var data = Cinidb.tbl_purchase.Where(b => b.Purchasedate >= fromdate.SelectedDate && b.Purchasedate <= todate.SelectedDate).ToList();
                    foreach (var d in data)
                    {
                        Purchase_List PL = new CiniLithoApp.Purchase_List();
                        PL.sno = d.Sno;
                        PL.Company_Name = d.CompanyName;
                        PL.Product_Detail = d.ProductDetail;
                        PL.HNS_No = d.HNSNo;
                        PL.Bill_no = d.Billno;
                        PL.amount = d.Amount.ToString();
                        PL.cgst = d.CGST.ToString();
                        PL.sgst = d.SGST.ToString();
                        PL.Total_Amount = d.TotalAmount.ToString();
                        PL.Purchase_date = (DateTime)d.Purchasedate;
                        PL.PaymentMode = d.PaymentMode;
                        if (d.Paymentdate != null)
                        {
                            PL.Paymentdate = (DateTime)d.Paymentdate;
                        }
                        dataToShow2.Add(PL);
                        Amt += (decimal)d.Amount;
                        cgst += (decimal)d.CGST;
                        sgst += (decimal)d.SGST;
                        tamt += (decimal)d.TotalAmount;
                    }
                    if (data.Count == 0)
                    {
                        MessageBox.Show("Data Not Found", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    Searchgrid.ItemsSource = dataToShow2;
                    Searchgrid.Items.Refresh();
                    amt.Text = Amt.ToString();
                    cgsttax.Text = cgst.ToString();
                    sgsttax.Text = sgst.ToString();
                    totalamt.Text = tamt.ToString();
                }
            }
            catch { }

        }


        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            todate.Text = "";
            fromdate.Text = "";
            loaddata();
        }

        private void btn_print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txt_companyname_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string slttxt = (sender as ComboBox).SelectedItem as string;
                var det = Cinidb.tbl_Supplier.Where(b => b.CPName == slttxt).SingleOrDefault();
                if(det!=null)
                {
                   
                }
            }
            catch { }
        }

        private void SearchGridScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void Searchgrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object item = Searchgrid.SelectedItem;
            if (item != null)
            {
                string ID = (Searchgrid.SelectedCells[5].Column.GetCellContent(item) as TextBlock).Text;
               // double Balance = double.Parse((Searchgrid.SelectedCells[9].Column.GetCellContent(item) as TextBlock).Text);
                if (ID!=null && ID!="")
                {
                    Get_Detail(ID);
                }
                else
                {
                    MessageBox.Show("No Pending Payment..!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void defbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object citem = Searchgrid.SelectedItem;
                int ID = int.Parse((Searchgrid.SelectedCells[0].Column.GetCellContent(citem) as TextBlock).Text);
                MessageBoxResult msg = MessageBox.Show("Do you Want To Delete this Item !! ", "Alert Message", MessageBoxButton.YesNo, MessageBoxImage.Information);
                if (msg == MessageBoxResult.Yes)
                {
                    var result = Cinidb.tbl_purchase.Where(b => b.Sno == ID).FirstOrDefault();
                    Cinidb.tbl_purchase.Remove(result);
                    int s = Cinidb.SaveChanges();
                    loaddata();
                }
            }
            catch { }
        }
    }
}
