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
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
       
        public List<String> filterPattern = new List<string>();
        public List<customerlistclass> dataToShow2 = new List<customerlistclass>();
        public List<customerlistclass> dataToShow = new List<customerlistclass>();
        string paymode = "";

        public CustomerPage()
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            Pagination.Balance.Visibility = Visibility.Collapsed;
            Pagination.TotalBalance.Visibility = Visibility.Collapsed;
            Pagination.lbl_Total.Visibility = Visibility.Collapsed;
            Pagination.Totalamount.Visibility = Visibility.Collapsed;
            GridFilter.atof.Click += atof_Click;
            GridFilter.gtol.Click += gtol_Click;
            GridFilter.mtos.Click += mtos_Click;
            GridFilter.ttoz.Click += ttoz_Click;
            GridFilter.atoz.Click += atoz_Click;
            loaddata();
            Pagination.First.Click += First_Click;
            Pagination.Previous.Click += Previous_Click;
            Pagination.Next.Click += Next_Click;
            Pagination.Last.Click += Last_Click;
            Pagination.ShowRecords.SelectionChanged += ShowRecords_SelectionChanged;
            Pagination.GoTo.Click += GoTo_Click;
            Pagination.Reload.Click += Reload_Click;
            //  Pagination.close.Click +=close_Click;
            Pagination.close.Visibility = Visibility.Hidden;

            Pagination.FirstIndex = 1;
            Pagination.CurrentIndex = 1;
            Pagination.PreviousIndex = 0;
            Pagination.NextIndex = 2;
            int totpage = dataToShow.Count / Pagination.RecordsPerPage;
            int remd = dataToShow.Count % Pagination.RecordsPerPage;
            if (remd > 0)
            {
                Pagination.LastIndex = totpage + 1;
            }
            else
            {
                Pagination.LastIndex = totpage;
            }

            Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
            Pagination.ShowLastIndex.Content = Pagination.LastIndex.ToString();
            int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
            int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
            if (end > dataToShow.Count)
                end = dataToShow.Count;

            EDButtons();LoadPartialGrid(start, end);
            Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            var item = Searchgrid.Items.CurrentItem;

            var pincodes = (from p in Cinidb.tbl_customer select p.Cname).Distinct().ToList();
            foreach (var pin in pincodes)
            {
                txt_excustomername.AddItem(new AutoCompleteEntry(pin, null));
            }
            if(MainWindow.user_detail.ToUpper()=="ADMIN")
            {
                txt_exbalanceamount.IsEnabled = true;
            }
        }
        void loaddata()
        {
            try
            {
                dataToShow2 = new List<customerlistclass>();
                dataToShow = new List<customerlistclass>();               
                var shiftdetail = (from l in Cinidb.tbl_customer select new { l.Sno,l.Cname,l.address,l.advance,l.cphone,l.mode,l.cdate }).ToList();
                foreach (var se in shiftdetail)
                {
                    customerlistclass CLS = new customerlistclass();
                    CLS.Sno = se.Sno;
                    CLS.Cname = se.Cname;
                    CLS.address = se.address;
                    CLS.cphone = se.cphone;
                    CLS.advance = se.advance.ToString();
                    CLS.mode = se.mode;
                    CLS.cdate = se.cdate;
                    dataToShow2.Add(CLS);
                }
                dataToShow = dataToShow2.OrderBy(b => b.Cname).ToList();
            }
            catch (Exception ex)
            { }
        }
        public void LoadPartialGrid(int start, int end)
        {
            try
            {
                List<customerlistclass> temp = new List<customerlistclass>();
                for (int i = start; i < end; i++)
                {
                    temp.Add(dataToShow[i]);
                }
                Searchgrid.ItemsSource = temp.OrderBy(b => b.Cname).ToList();
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                // ErrorLog.WriterLog(ex, "EmployeeSearchlist", "LoadPartialGrid()");
            }
        }
        public void EDButtons()
        {
            try
            {
                if (Pagination.CurrentIndex > 2)
                    Pagination.First.IsEnabled = true;
                else
                    Pagination.First.IsEnabled = false;
                if (Pagination.CurrentIndex > 1)
                    Pagination.Previous.IsEnabled = true;
                else
                    Pagination.Previous.IsEnabled = false;
                if (Pagination.CurrentIndex > Pagination.LastIndex - 2)
                    Pagination.Last.IsEnabled = false;
                else
                    Pagination.Last.IsEnabled = true;
                if (Pagination.CurrentIndex > Pagination.LastIndex - 1)
                    Pagination.Next.IsEnabled = false;
                else
                    Pagination.Next.IsEnabled = true;

                if (dataToShow.Count == 0)
                    Pagination.ShowRecords.IsEnabled = false;
                else
                    Pagination.ShowRecords.IsEnabled = true;
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                // ErrorLog.WriterLog(ex, "CustomerSearchlist", "EDButtons()");
            }
        }
        private void RBT_Cash_Checked(object sender, RoutedEventArgs e)
        {
            paymode = "Cash";
        }

        private void RBT_SBI_Checked(object sender, RoutedEventArgs e)
        {
            paymode = "SBI";
        }

        private void RBT_TMB_Checked(object sender, RoutedEventArgs e)
        {
            paymode = "TMB";
        }

        private void SearchGridScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        void GoTo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var gotoIndex = Pagination.GotoTxt.Text;
                if (gotoIndex != "")
                {
                    int gi = int.Parse(gotoIndex);
                    if (gi >= 1 && gi <= Pagination.LastIndex)
                    {
                        Pagination.PreviousIndex = gi - 1;
                        Pagination.CurrentIndex = gi;
                        Pagination.NextIndex = gi + 1;
                        Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                        int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                        int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                        if (end > dataToShow.Count)
                            end = dataToShow.Count;

                        EDButtons();LoadPartialGrid(start, end);
                    }
                }
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "EmployeeSearchlist", "GoTo_Click()");
            }
        }

        void ShowRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int tempStart = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                var val = Pagination.ShowRecords.SelectedItem;
                if (val != null)
                {
                    Pagination.RecordsPerPage = int.Parse(((ComboBoxItem)val).Tag.ToString());
                    Pagination.FirstIndex = 1;
                    int totpage = dataToShow.Count / Pagination.RecordsPerPage;
                    int remd = dataToShow.Count % Pagination.RecordsPerPage;
                    if (remd > 0)
                    {
                        Pagination.LastIndex = totpage + 1;
                    }
                    else
                    {
                        Pagination.LastIndex = totpage;
                    }
                    int newCI = tempStart / Pagination.RecordsPerPage;
                    if (newCI == 0)
                    {
                        newCI = 1;
                    }
                    if (newCI > Pagination.LastIndex)
                    {
                        newCI = Pagination.LastIndex;
                    }
                    Pagination.CurrentIndex = newCI;
                    Pagination.PreviousIndex = newCI - 1;
                    Pagination.NextIndex = newCI + 1;
                    Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                    Pagination.ShowLastIndex.Content = Pagination.LastIndex.ToString();
                    int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                    int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                    if (end > dataToShow.Count)
                        end = dataToShow.Count;

                    EDButtons();LoadPartialGrid(start, end);
                }
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "EmployeeSearchlist", "ShowRecords_SelectionChanged()");
            }
        }

        void Last_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Pagination.PreviousIndex = Pagination.LastIndex - 1;
                Pagination.CurrentIndex = Pagination.LastIndex;
                Pagination.NextIndex = Pagination.LastIndex + 1;
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
            }
            catch (Exception ex)
            {
                // new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                // ErrorLog.WriterLog(ex, "EmployeeSearchlist", "Last_Click()");
            }
        }

        void Previous_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Pagination.PreviousIndex -= 1;
                Pagination.CurrentIndex -= 1;
                Pagination.NextIndex -= 1;
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
            }
            catch (Exception ex)
            {
                // new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                // ErrorLog.WriterLog(ex, "EmployeeSearchlist", "Previous_Click()");
            }
        }

        void First_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Pagination.PreviousIndex = 0;
                Pagination.CurrentIndex = 1;
                Pagination.NextIndex = 2;
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
            }
            catch (Exception ex)
            {
                // new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "EmployeeSearchlist", "First_Click()");
            }
        }

        void Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Pagination.PreviousIndex += 1;
                Pagination.CurrentIndex += 1;
                Pagination.NextIndex += 1;
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
            }
            catch (Exception ex)
            {
                // new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "EmployeeSearchlist", "Next_Click()");
            }
        }
        private void Reload_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                loaddata();
                Pagination.FirstIndex = 1;
                Pagination.CurrentIndex = 1;
                Pagination.PreviousIndex = 0;
                Pagination.NextIndex = 2;
                int totpage = dataToShow.Count / Pagination.RecordsPerPage;
                int remd = dataToShow.Count % Pagination.RecordsPerPage;
                if (remd > 0)
                {
                    Pagination.LastIndex = totpage + 1;
                }
                else
                {
                    Pagination.LastIndex = totpage;
                }
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                Pagination.ShowLastIndex.Content = Pagination.LastIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
                Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "Reload_Click", "Reload_Click()");
            }
        }

        private void chk_Status_Click(object sender, MouseButtonEventArgs e)
        {

        }
        void atoz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filterPattern = new List<string>() { };
                loaddata();
                Pagination.FirstIndex = 1;
                Pagination.CurrentIndex = 1;
                Pagination.PreviousIndex = 0;
                Pagination.NextIndex = 2;
                int totpage = dataToShow.Count / Pagination.RecordsPerPage;
                int remd = dataToShow.Count % Pagination.RecordsPerPage;
                if (remd > 0)
                {
                    Pagination.LastIndex = totpage + 1;
                }
                else
                {
                    Pagination.LastIndex = totpage;
                }
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                Pagination.ShowLastIndex.Content = Pagination.LastIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
                Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //  ErrorLog.WriterLog(ex, "City_Searchlist", "ttoz_Click()");
            }
        }

        void ttoz_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filterPattern = new List<string>() { "T", "U", "V", "W", "X", "Y", "Z", "t", "u", "v", "w", "x", "y", "z" };
                FilterData(filterPattern);
                Pagination.FirstIndex = 1;
                Pagination.CurrentIndex = 1;
                Pagination.PreviousIndex = 0;
                Pagination.NextIndex = 2;
                int totpage = dataToShow.Count / Pagination.RecordsPerPage;
                int remd = dataToShow.Count % Pagination.RecordsPerPage;
                if (remd > 0)
                {
                    Pagination.LastIndex = totpage + 1;
                }
                else
                {
                    Pagination.LastIndex = totpage;
                }
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                Pagination.ShowLastIndex.Content = Pagination.LastIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
                Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            }
            catch (Exception ex)
            {
                // new NBox().Show(Loginfrm.exception, MessageType.error);
                // MessageBox.Show(Loginfrm.exception, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                // ErrorLog.WriterLog(ex, "ProductSearchlist", "ttoz_Click()");
            }
        }

        void mtos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filterPattern = new List<string>() { "M", "N", "O", "P", "Q", "R", "S", "m", "n", "o", "p", "q", "r", "s" };
                FilterData(filterPattern);
                Pagination.FirstIndex = 1;
                Pagination.CurrentIndex = 1;
                Pagination.PreviousIndex = 0;
                Pagination.NextIndex = 2;
                int totpage = dataToShow.Count / Pagination.RecordsPerPage;
                int remd = dataToShow.Count % Pagination.RecordsPerPage;
                if (remd > 0)
                {
                    Pagination.LastIndex = totpage + 1;
                }
                else
                {
                    Pagination.LastIndex = totpage;
                }
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                Pagination.ShowLastIndex.Content = Pagination.LastIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
                Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            }
            catch (Exception ex)
            {
                // new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                // ErrorLog.WriterLog(ex, "ProductSearchlist", "mtos_Click()");
            }
        }

        void gtol_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filterPattern = new List<string>() { "G", "H", "I", "J", "K", "L", "g", "h", "i", "j", "k", "l" };
                FilterData(filterPattern);
                Pagination.FirstIndex = 1;
                Pagination.CurrentIndex = 1;
                Pagination.PreviousIndex = 0;
                Pagination.NextIndex = 2;
                int totpage = dataToShow.Count / Pagination.RecordsPerPage;
                int remd = dataToShow.Count % Pagination.RecordsPerPage;
                if (remd > 0)
                {
                    Pagination.LastIndex = totpage + 1;
                }
                else
                {
                    Pagination.LastIndex = totpage;
                }
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                Pagination.ShowLastIndex.Content = Pagination.LastIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
                Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "ProductSearchlist", "gtol_Click()");
            }
        }

        void atof_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                filterPattern = new List<string>() { "A", "B", "C", "D", "E", "F", "a", "b", "c", "d", "e", "f" };
                FilterData(filterPattern);
                Pagination.FirstIndex = 1;
                Pagination.CurrentIndex = 1;
                Pagination.PreviousIndex = 0;
                Pagination.NextIndex = 2;
                int totpage = dataToShow.Count / Pagination.RecordsPerPage;
                int remd = dataToShow.Count % Pagination.RecordsPerPage;
                if (remd > 0)
                {
                    Pagination.LastIndex = totpage + 1;
                }
                else
                {
                    Pagination.LastIndex = totpage;
                }
                Pagination.ShowIndex.Content = Pagination.CurrentIndex.ToString();
                Pagination.ShowLastIndex.Content = Pagination.LastIndex.ToString();
                int start = Pagination.RecordsPerPage * Pagination.PreviousIndex;
                int end = Pagination.RecordsPerPage * Pagination.CurrentIndex;
                if (end > dataToShow.Count)
                    end = dataToShow.Count;

                EDButtons();LoadPartialGrid(start, end);
                Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                // ErrorLog.WriterLog(ex, "ProductSearchlist", "atof_Click()");
            }
        }
        public void FilterData(List<string> pattern)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                dataToShow = dataToShow2.Where(item => pattern.Any(l => item.Cname.StartsWith(l))).OrderBy(o => o.Cname).ToList();
                //dataToShow = entity.ProductMaster.Where(item => pattern.Any(l => item.Product_Name.StartsWith(l))).OrderBy(o => o.Code).ToList();
                this.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                // new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //  ErrorLog.WriterLog(ex, "ProductSearchlist", "FilterData()");
            }
        }

        private void Searchgrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object item = Searchgrid.SelectedItem;
            if (item != null)
            {
                string ID = (Searchgrid.SelectedCells[2].Column.GetCellContent(item) as TextBlock).Text;
                var CSTDETAIL = Cinidb.tbl_logdetail.Where(b => b.Rmobile == ID).ToList();
                Searchgrid1.ItemsSource = CSTDETAIL;
                Tabcntrl1.SelectedIndex = 2;
              
            }
        }
        private void Searchgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                object item = Searchgrid.SelectedItem;
                if (item != null)
                {
                    int ID = int.Parse((Searchgrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                    var CSTDETAIL = Cinidb.tbl_customer.Where(b => b.Sno == ID).SingleOrDefault();
                    if (CSTDETAIL != null)
                    {
                        txt_excustomername.Text = CSTDETAIL.Cname;
                        txt_excustomername.Tag = CSTDETAIL.Sno;
                        txt_exaddress.Text = CSTDETAIL.address;
                        txt_exphone.Text = CSTDETAIL.cphone;
                        txt_exbalanceamount.Text = CSTDETAIL.advance.ToString();
                        txt_exadvanceamount.Text = "0";
                        if (CSTDETAIL.mode == "Cash")
                        {
                            exRBT_Cash.IsChecked = true;
                        }
                        if (CSTDETAIL.mode == "SBI")
                        {
                            exRBT_SBI.IsChecked = true;
                        }
                        if (CSTDETAIL.mode == "TMB")
                        {
                            exRBT_TMB.IsChecked = true;
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tbl_customer tbc = new CiniLithoApp.tbl_customer();
                tbc.Cname = txt_custoemrname.Text;
                tbc.address = txt_address.Text;
                tbc.cphone = txt_phone.Text;
                tbc.advance = decimal.Parse(txt_advanceamount.Text);
                tbc.cdate = DateTime.Now.ToString("dd-MM-yyyy");
                tbc.mode = paymode;
                Cinidb.tbl_customer.Add(tbc);
                int i= Cinidb.SaveChanges();
                if(i>0)
                {
                    tbl_logdetail tld = new CiniLithoApp.tbl_logdetail();
                    tld.Rno = "";
                    tld.Rname = txt_custoemrname.Text;
                    tld.Rmobile = txt_phone.Text;
                    tld.Orderdet = "Advance Amount Updated";
                    tld.paydet = txt_advanceamount.Text;
                    tld.mode = paymode;
                    tld.datim = DateTime.Now.ToString("dd-MM-yyyy");
                    tld.BankRef = "";
                    Cinidb.tbl_logdetail.Add(tld);
                    Cinidb.SaveChanges();

                    MessageBox.Show("Customer Added !", "Info");
                    txt_custoemrname.Text = "";
                    txt_address.Text = "";
                    txt_phone.Text = "";
                    txt_advanceamount.Text = "";
                    RBT_SBI.IsChecked = false;
                    RBT_Cash.IsChecked = false;
                    RBT_TMB.IsChecked = false;
                    txt_custoemrname.Focus();
                }
            }
            catch (Exception ex) { }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            txt_custoemrname.Text = ""; 
          txt_address.Text="";
            txt_phone.Text="";
          txt_advanceamount.Text="";
            RBT_SBI.IsChecked = false;
            RBT_Cash.IsChecked = false;
            RBT_TMB.IsChecked = false;
            txt_custoemrname.Focus();
        }

        private void txt_excustomername_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                var billname = txt_excustomername.Text.Trim();
                if (billname != "")
                {
                    var customer = Cinidb.tbl_customer.Where(b => b.Cname.Equals(billname)).SingleOrDefault();
                    if (customer != null)
                    {
                        txt_excustomername.Tag = customer.Sno;
                        txt_exaddress.Text = customer.address;
                        txt_exphone.Text = customer.cphone;
                        txt_exbalanceamount.Text = customer.advance.ToString();
                        if (customer.mode == "Cash") { exRBT_Cash.IsChecked = true; }
                        if (customer.mode == "SBI") { exRBT_SBI.IsChecked = true; }
                        if (customer.mode == "TMB") { exRBT_TMB.IsChecked = true; }
                        var CSTDETAIL = Cinidb.tbl_logdetail.Where(b => b.Rmobile == txt_exphone.Text || b.Rname == billname).ToList();
                        Searchgrid1.ItemsSource = CSTDETAIL;
                    }
                }
            }
            catch { }
        }

        private void btn_exsubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int sno = int.Parse(txt_excustomername.Tag.ToString());
                var USTot = (from S in Cinidb.tbl_customer where S.Sno ==sno  select S).Single();
                if (txt_exadvanceamount.Text != "0")
                {
                    USTot.advance = USTot.advance + decimal.Parse(txt_exadvanceamount.Text);                    
                }
                else
                {
                    USTot.advance = decimal.Parse(txt_exbalanceamount.Text);                   
                }
                USTot.Cname=txt_excustomername.Text;
                USTot.cphone = txt_exphone.Text;
                int s = Cinidb.SaveChanges();
                if(s>0)
                {
                    tbl_logdetail tld = new CiniLithoApp.tbl_logdetail();
                    tld.Rno = txt_excustomername.Tag.ToString();
                    tld.Rname = txt_excustomername.Text;
                    tld.Rmobile = txt_exphone.Text;                   
                    tld.Orderdet = "Advance Amount Updated";
                    tld.paydet = txt_exadvanceamount.Text;
                    tld.mode = paymode;
                    tld.datim = DateTime.Now.ToString("dd-MM-yyyy");
                    tld.BankRef = "";
                    Cinidb.tbl_logdetail.Add(tld);
                    Cinidb.SaveChanges();
                    MessageBox.Show("Update Successfully");
                    txt_excustomername.Text = "";
                    txt_exaddress.Text = "";
                    txt_exphone.Text = "";
                    txt_exbalanceamount.Text = "";
                    txt_exadvanceamount.Text = "0";
                    exRBT_Cash.IsChecked = false;
                    exRBT_SBI.IsChecked = false;
                    exRBT_TMB.IsChecked = false;
                }
               
                Reload_Click(null, null);
            }
            catch { }
        }

        private void btn_excancel_Click(object sender, RoutedEventArgs e)
        {
            txt_excustomername.Text = "";
            txt_exaddress.Text = "";
            txt_exphone.Text = "";
            txt_exbalanceamount.Text = "";
            txt_exadvanceamount.Text = "0";
           exRBT_Cash.IsChecked = false; 
           exRBT_SBI.IsChecked = false ; 
           exRBT_TMB.IsChecked = false;
        }

        private void defbtn_Click(object sender, RoutedEventArgs e)
        {
            object citem = Searchgrid.SelectedItem;
            int ID = int.Parse((Searchgrid.SelectedCells[0].Column.GetCellContent(citem) as TextBlock).Text);
            MessageBoxResult msg = MessageBox.Show("Do you Want To Delete this Item !! ", "Alert Message", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (msg == MessageBoxResult.Yes && ID != 0)
            {
                var result = Cinidb.tbl_customer.Where(b => b.Sno == ID).FirstOrDefault();
                if (result != null)
                {
                    Cinidb.tbl_customer.Remove(result);
                    Cinidb.SaveChanges();
                    Reload_Click(null, null);
                }

            }
        }

        private void btn_Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var CSTDETAIL = Cinidb.tbl_logdetail.Where(b => b.Rmobile == txt_Search.Text || b.Rname==txt_Search.Text).ToList();
                Searchgrid1.ItemsSource = CSTDETAIL;
            }
            catch { }
        }
    }
    public class customerlistclass
    {
        public int Sno { get; set; }
        public string Cname {get;set;}
        public string address {get;set;}
        public string cphone {get;set;}
        public string advance {get;set;}
        public string mode {get;set;}
        public string cdate {get;set;}
    }
}
