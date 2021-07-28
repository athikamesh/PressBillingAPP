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
    /// Interaction logic for PurchaseList.xaml
    /// </summary>
    public partial class PurchaseList : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        public List<Purchase_List> dataToShow2 = new List<Purchase_List>();
        decimal Amt = 0, cgst = 0, sgst = 0, tamt = 0;
        public PurchaseList()
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            loaddata();
            var pincodes = (from p in Cinidb.tbl_purchase select p.CompanyName).Distinct().ToList();
            foreach (var pin in pincodes)
            {
                txt_search.AddItem(new AutoCompleteEntry(pin, null));
            }
        }
        void loaddata()
        {
            dataToShow2 = new List<Purchase_List>();
            DateTime dt = DateTime.Now.Date;
            var data = Cinidb.tbl_purchase.Where(b=>b.Purchasedate==dt).ToList();
            foreach(var d in data)
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
                PL.Purchase_date =(DateTime)d.Purchasedate ;
                PL.PaymentMode = d.PaymentMode;
                PL.Paymentdate = (DateTime)d.Paymentdate;
                dataToShow2.Add(PL);
                Amt +=(decimal) d.Amount;
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

        private void btn_refresh_Click(object sender, RoutedEventArgs e)
        {
            todate.Text = "";
            fromdate.Text = "";
            loaddata();
        }

        private void btn_print_Click(object sender, RoutedEventArgs e)
        {

        }

        private void defbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                object citem = Searchgrid.SelectedItem;
                int ID =int.Parse( (Searchgrid.SelectedCells[0].Column.GetCellContent(citem) as TextBlock).Text);
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

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
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

        private void txt_search_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void Searchgrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void SearchGridScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void fromdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (todate.Text != "" && fromdate.Text != "")
                {
                    dataToShow2.Clear();
                    var data = Cinidb.tbl_purchase.Where(b=>b.Purchasedate>=fromdate.SelectedDate && b.Purchasedate<=todate.SelectedDate).ToList();
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
                    if(data.Count==0)
                    {
                        MessageBox.Show("Data Not Found", "Alert",MessageBoxButton.OK,MessageBoxImage.Warning);
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
    }
    public class Purchase_List
    {
        public int sno { get; set; }
        public DateTime Purchase_date { get; set; }
        public string Company_Name { get; set; }
        public string Product_Detail { get; set; }
        public string HNS_No { get; set; }
        public string Bill_no { get; set; }
        public string amount { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public DateTime Paymentdate { get; set; }
        public string PaymentMode { get; set; }
        public string Total_Amount { get; set; }
       
    }
}
