using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Globalization;
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
    /// Interaction logic for BalanceList.xaml
    /// </summary>
    public partial class BalanceList : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        ReportDocument RPT = new ReportDocument();
        public static bool activated = false;
        public static string Gridvalue = "";
        string rights;
        public List<String> filterPattern = new List<string>();
        public List<balancelistclass> dataToShow2 = new List<balancelistclass>();
        public List<balancelistclass> dataToShow = new List<balancelistclass>();
        public BalanceList()
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            GetMonth();
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
            EDButtons();
            LoadPartialGrid(start, end);
            Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            var item = Searchgrid.Items.CurrentItem;
            if (MainWindow.user_detail.ToUpper() == "STAFF")
            {
                Searchgrid.Columns[11].Visibility = Visibility.Collapsed;
            }
        }
        void GetMonth()
        {
            cmb_monthy.SelectedValuePath = "Key";
            cmb_monthy.DisplayMemberPath = "Value";
            string[] names = DateTimeFormatInfo.CurrentInfo.MonthNames;
            foreach (var s in names)
            {
                if (s != "")
                {
                    string val = "";
                    if (s.Length > 3)
                    {
                        val = s.Remove(3);
                    }
                    else
                    {
                        val = s;
                    }
                    cmb_monthy.Items.Add(new KeyValuePair<string, string>(val,s));

                }

            }
            for(int i=2000;i<=3000;i++)
            {
                cmb_year.Items.Add(i.ToString());
            }
            cmb_year.SelectedValue = DateTime.Now.Year.ToString();
        }
        void loaddata()
        {
            try
            {
                dataToShow2 = new List<balancelistclass>();
                dataToShow = new List<balancelistclass>();
                int SNOCount = 0;
                var dt = DateTime.Now.Month;
                string bill_No="";
                var shiftdetail = (from l in Cinidb.Balance_List where l.Balance!=0  select new {l.orderdate,l.Name,l.Billno,l.Size,l.Color,l.Copies,l.Rate,l.Advance,l.Payment,l.Balance,l.Total,l.M_Status,l.Adate,l.PDate}).OrderBy(b=>b.Billno).ToList();

                foreach (var se in shiftdetail)
                {                   
                    if (bill_No != se.Billno)
                    {
                        SNOCount++;
                        bill_No = se.Billno;
                        balancelistclass BLC = new balancelistclass();
                        BLC.sno = SNOCount;
                        BLC.orderdate = se.orderdate.Date;
                        BLC.name = se.Name;
                        BLC.billno = se.Billno;
                        BLC.detailcolor = se.Size + "-" + se.Color;
                        BLC.copies = se.Copies.ToString();
                        BLC.rate = se.Rate.ToString();
                        if (se.Advance != 0)
                        {
                            BLC.advance = se.Advance.ToString() + "(" + se.Adate + ")";
                        }
                        else { BLC.advance = "Nill"; }
                        if (se.Payment != 0)
                        {
                            BLC.payment = se.Payment.ToString() + "(" + se.PDate + ")";
                        }
                        else { BLC.payment = "Nill"; }                        
                        BLC.balance = se.Balance.ToString();
                        BLC.total = se.Total.ToString();                        
                        BLC.Status = se.M_Status;
                        dataToShow2.Add(BLC);
                        double bal = 0; double total = 0;
                        foreach (var s in dataToShow2)
                        {
                            bal = bal + double.Parse(s.balance);
                            Pagination.TotalBalance.Content = bal.ToString();
                            total = total + double.Parse(s.total);
                            Pagination.Totalamount.Content = total.ToString();
                        }
                    }
                    else
                    {
                        var rowupdate = dataToShow2.Where(b => b.billno == bill_No).SingleOrDefault();
                        rowupdate.detailcolor += Environment.NewLine + se.Size + "-" + se.Color;
                        rowupdate.copies += Environment.NewLine + se.Copies.ToString();
                        rowupdate.rate += Environment.NewLine + se.Rate.ToString();
                    }
                    
                }
                dataToShow = dataToShow2.OrderByDescending(b => b.sno).ToList();
            }
            catch(Exception ex)
            { }
        }
        public void DoSearch(String SearchKey)
        {
            try
            {
                var data = dataToShow2.Where(b => b.balance.StartsWith(SearchKey.ToUpper())).ToList();

                if (data.Count > 0)
                {

                    dataToShow = data;

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
                    EDButtons();
                    LoadPartialGrid(start, end);
                    Pagination.TotalEntries.Content = dataToShow.Count.ToString();
                    double bal = 0; double total = 0;
                    foreach (var s in dataToShow)
                    {
                        bal = bal + double.Parse(s.balance);
                        Pagination.TotalBalance.Content = bal.ToString();
                        total = total + double.Parse(s.total);
                        Pagination.Totalamount.Content = total.ToString();
                    }
                }
                else
                {
                    //new NBox().Show(Loginfrm.notfound, MessageType.info);
                     MessageBox.Show("Data Not Found", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
              //  new NBox().Show(Loginfrm.exception, MessageType.error);
                 MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
               // ErrorLog.WriterLog(ex, "EmployeeSearchlist", "DoSearch");
            }
        }
        public void DoSearch1(int SearchKeyMonth,int SearchKeyYear)
        {
            try
            {
                var data = dataToShow2.Where(b => b.orderdate.Month==SearchKeyMonth && b.orderdate.Year==SearchKeyYear).ToList();

                if (data.Count > 0)
                {

                    dataToShow = data;

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
                    EDButtons();
                    LoadPartialGrid(start, end);
                    Pagination.TotalEntries.Content = dataToShow.Count.ToString();
                    double bal = 0; double total = 0;
                    foreach (var s in dataToShow)
                    {
                        bal = bal + double.Parse(s.balance);
                        Pagination.TotalBalance.Content = bal.ToString();
                        total = total + double.Parse(s.total);
                        Pagination.Totalamount.Content = total.ToString();
                    }
                }
                else
                {
                    //new NBox().Show(Loginfrm.notfound, MessageType.info);
                    MessageBox.Show("Data Not Found", "Alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception ex)
            {
                //  new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                // ErrorLog.WriterLog(ex, "EmployeeSearchlist", "DoSearch");
            }
        }
        private void allowDecimal(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Char newChar = e.Text.ToString()[0];
                e.Handled = !(Char.IsNumber(newChar));
            }
            catch (Exception ex)
            {
               // new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "Add_Tax", "txt_Tax_PreviewTextInput");
            }
        }
        public void LoadPartialGrid(int start, int end)
        {
            try
            {
                List<balancelistclass> temp = new List<balancelistclass>();
                for (int i = start; i < end; i++)
                {
                    temp.Add(dataToShow[i]);
                }
                Searchgrid.ItemsSource = temp;
                if (MainWindow.user_detail == "STAFF")
                {
                    Searchgrid.Columns[11].Visibility = Visibility.Collapsed;
                }
               
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                 MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
               // ErrorLog.WriterLog(ex, "EmployeeSearchlist", "LoadPartialGrid()");
            }
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
                        EDButtons();
                        LoadPartialGrid(start, end);
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
                    EDButtons();
                    LoadPartialGrid(start, end);
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
                EDButtons();
                LoadPartialGrid(start, end);
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
                EDButtons();
                LoadPartialGrid(start, end);
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
                EDButtons();
                LoadPartialGrid(start, end);
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
                EDButtons();
                LoadPartialGrid(start, end);
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
                EDButtons();
                LoadPartialGrid(start, end);
                Pagination.TotalEntries.Content = dataToShow.Count.ToString();
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "Reload_Click", "Reload_Click()");
            }
        }

        private void SearchGridScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void chk_Status_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                var item = Searchgrid.SelectedItem;
                var status = !(sender as CheckBox).IsChecked;

                if (item != null || status != false)
                {
                    if (MainWindow.user_detail.ToUpper() != "STAFF")
                    {
                        var pro_code = (Searchgrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                        if (status == true)
                        {
                            var product = (from S in Cinidb.tbl_newbill where S.Billno == pro_code select S).Single();
                            product.M_Status = status.ToString();
                            var i = Cinidb.SaveChanges();
                        }
                        else
                        {
                            var product = (from S in Cinidb.tbl_newbill where S.Billno == pro_code select S).Single();
                            product.M_Status = status.ToString();
                            var i = Cinidb.SaveChanges();
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                //new NBox().Show(Loginfrm.exception, MessageType.error);
                MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
                //ErrorLog.WriterLog(ex, "ProductSearchlist", "chk_Status_Click");
            }
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
                EDButtons();
                LoadPartialGrid(start, end);
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
              EDButtons();
                LoadPartialGrid(start, end);
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
                EDButtons();
                LoadPartialGrid(start, end);
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
                EDButtons();
                LoadPartialGrid(start, end);
               // Pagination.TotalEntries.Content = dataToShow.Count.ToString();
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
                EDButtons();
                LoadPartialGrid(start, end);
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
                dataToShow = dataToShow2.Where(item => pattern.Any(l => item.name.StartsWith(l))).OrderBy(o => o.sno).ToList();
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
               // new NBox().Show(Loginfrm.exception, MessageType.error);
                 MessageBox.Show(ex.Message, "Information", MessageBoxButton.OK, MessageBoxImage.Error);
               // ErrorLog.WriterLog(ex, "CustomerSearchlist", "EDButtons()");
            }
        }
        private void txt_search_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    DoSearch(txt_search.Text);
                }
            }
            catch(Exception ex)
            { }
        }

        private void cmb_monthy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(cmb_monthy.SelectedValue.ToString() != "" && cmb_year.SelectedValue.ToString() != "")
                {
                   
                    var Findmonth = DateTime.ParseExact(cmb_monthy.SelectedValue.ToString(), "MMM", CultureInfo.CurrentCulture).Month;
                    DoSearch1(Findmonth, int.Parse(cmb_year.SelectedValue.ToString()));
                }
            }
            catch (Exception ex)
            { }
        }

        private void cmb_year_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmb_monthy.Text != "" && cmb_year.SelectedValue.ToString() != "")
                {
                    var Findmonth = DateTime.ParseExact(cmb_monthy.Text, "MMM", CultureInfo.CurrentCulture).Month;
                    DoSearch1(Findmonth, int.Parse(cmb_year.SelectedValue.ToString()));
                }
            }
            catch (Exception ex)
            { }
        }

        private void Searchgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void Searchgrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object item = Searchgrid.SelectedItem;
            if (item != null)
            {
                string ID = (Searchgrid.SelectedCells[3].Column.GetCellContent(item) as TextBlock).Text;
                double Balance = double.Parse((Searchgrid.SelectedCells[9].Column.GetCellContent(item) as TextBlock).Text);
                if (Balance > 0)
                {
                    Payment Pay = new CiniLithoApp.Payment(ID);
                    Pay.ShowDialog();
                }
                else
                {
                    MessageBox.Show("No Pending Payment..!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void ctx_down_prod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<BalanceReport> dataToShow3 = new List<BalanceReport>();               
                int SNOCount = 0;
                var dt = DateTime.Now.Month;
                string bill_No = "";
                if (txt_search.Text != "")
                {
                    double txtsrc = double.Parse(txt_search.Text);
                    var shiftdetail = (from l in Cinidb.Balance_List where l.Balance == txtsrc select new { l.orderdate, l.Name, l.Billno, l.Size, l.Color, l.Copies, l.Rate, l.Advance, l.Payment, l.Balance, l.Total, l.M_Status,l.Mobile }).OrderBy(b => b.Billno).ToList();
                    foreach (var se in shiftdetail)
                    {
                        if (bill_No != se.Billno)
                        {
                            SNOCount++;
                            bill_No = se.Billno;
                            BalanceReport BLC = new BalanceReport();
                            BLC.sno = SNOCount;
                            BLC.orderdate = se.orderdate.Date;
                            BLC.name = se.Name;
                            BLC.billno = se.Billno;
                            BLC.rate = (double)se.Rate;
                            BLC.advance = (double)se.Advance;
                            BLC.payment = (double)se.Payment;
                            BLC.balance = (double)se.Balance;
                            BLC.total = (double)se.Total;
                            BLC.Mobile = se.Mobile;
                            dataToShow3.Add(BLC);
                        }
                        else
                        {
                            var rowupdate = dataToShow3.Where(b => b.billno == bill_No).SingleOrDefault();
                            rowupdate.rate += (double)se.Rate;
                        }

                    }
                    string s2 = Process.GetCurrentProcess().MainModule.FileName;
                    RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Balance_RPT.rpt");
                    RPT.Database.Tables[0].SetDataSource(dataToShow3);
                    SaveFileDialog openFileDialog = new SaveFileDialog();
                    openFileDialog.Filter = "Excel files (.xls)|*.xls";
                    openFileDialog.FileName = DateTime.Now.ToString("dd-MM-yyyy hhmmss");
                    if (openFileDialog.ShowDialog() == true)
                    {
                        RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, openFileDialog.FileName);
                        MessageBox.Show("REPORT EXPORTED!", "Alert");
                    }
                }
                else
                {
                    var shiftdetail = (from l in Cinidb.Balance_List where l.Balance != 0 select new { l.orderdate, l.Name, l.Billno, l.Size, l.Color, l.Copies, l.Rate, l.Advance, l.Payment, l.Balance, l.Total, l.M_Status,l.Mobile }).OrderBy(b => b.Billno).ToList();
                    foreach (var se in shiftdetail)
                    {
                        if (bill_No != se.Billno)
                        {
                            SNOCount++;
                            bill_No = se.Billno;
                            BalanceReport BLC = new BalanceReport();
                            BLC.sno = SNOCount;
                            BLC.orderdate = se.orderdate.Date;
                            BLC.name = se.Name;
                            BLC.billno = se.Billno;
                            BLC.rate = (double)se.Rate;
                            BLC.advance = (double)se.Advance;
                            BLC.payment = (double)se.Payment;
                            BLC.balance = (double)se.Balance;
                            BLC.total = (double)se.Total;
                            BLC.Mobile = se.Mobile;
                            dataToShow3.Add(BLC);
                        }
                        else
                        {
                            var rowupdate = dataToShow3.Where(b => b.billno == bill_No).SingleOrDefault();
                            rowupdate.rate += (double)se.Rate;
                        }

                    }
                    string s2 = Process.GetCurrentProcess().MainModule.FileName;
                    RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Balance_RPT.rpt");
                    RPT.Database.Tables[0].SetDataSource(dataToShow3);
                    SaveFileDialog openFileDialog = new SaveFileDialog();
                    openFileDialog.Filter = "Excel files (.xls)|*.xls";
                    openFileDialog.FileName = DateTime.Now.ToString("dd-MM-yyyy hhmmss");
                    if (openFileDialog.ShowDialog() == true)
                    {
                        RPT.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, openFileDialog.FileName);
                        MessageBox.Show("REPORT EXPORTED!", "Alert");
                    }

                }
            }
            catch { }
        }

        private void ctx_print_all_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<BalanceReport> dataToShow3 = new List<BalanceReport>();
                int SNOCount = 0;
                var dt = DateTime.Now.Month;
                string bill_No = "";
                if (txt_search.Text != "")
                {
                    double txtsrc = double.Parse(txt_search.Text);
                    var shiftdetail = (from l in Cinidb.Balance_List where l.Balance == txtsrc select new { l.orderdate, l.Name, l.Billno, l.Size, l.Color, l.Copies, l.Rate, l.Advance, l.Payment, l.Balance, l.Total, l.M_Status,l.Mobile }).OrderBy(b => b.Billno).ToList();
                    foreach (var se in shiftdetail)
                    {
                        if (bill_No != se.Billno)
                        {
                            SNOCount++;
                            bill_No = se.Billno;
                            BalanceReport BLC = new BalanceReport();
                            BLC.sno = SNOCount;
                            BLC.orderdate = se.orderdate.Date;
                            BLC.name = se.Name;
                            BLC.billno = se.Billno;
                            BLC.rate = (double)se.Rate;
                            BLC.advance = (double)se.Advance;
                            BLC.payment = (double)se.Payment;
                            BLC.balance = (double)se.Balance;
                            BLC.total = (double)se.Total;
                            BLC.Mobile = se.Mobile;
                            dataToShow3.Add(BLC);
                            //double bal = 0; double total = 0;
                            //foreach (var s in dataToShow3)
                            //{
                            //    bal = bal + double.Parse(s.balance);
                            //    Pagination.TotalBalance.Content = bal.ToString();
                            //    total = total + double.Parse(s.total);
                            //    Pagination.Totalamount.Content = total.ToString();
                            //}

                        }
                        else
                        {
                            var rowupdate = dataToShow3.Where(b => b.billno == bill_No).SingleOrDefault();
                            rowupdate.rate += (double)se.Rate;
                        }

                    }
                    string s2 = Process.GetCurrentProcess().MainModule.FileName;
                    RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Balance_RPT.rpt");
                    RPT.Database.Tables[0].SetDataSource(dataToShow3);
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
                            MessageBox.Show("REPORT PRINTED!", "Alert");
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.ToString());
                        }
                    }
                }
                else
                {
                    var shiftdetail = (from l in Cinidb.Balance_List where l.Balance != 0 select new { l.orderdate, l.Name, l.Billno, l.Size, l.Color, l.Copies, l.Rate, l.Advance, l.Payment, l.Balance, l.Total, l.M_Status,l.Mobile }).OrderBy(b => b.Billno).ToList();
                    foreach (var se in shiftdetail)
                    {
                        if (bill_No != se.Billno)
                        {
                            SNOCount++;
                            bill_No = se.Billno;
                            BalanceReport BLC = new BalanceReport();
                            BLC.sno = SNOCount;
                            BLC.orderdate = se.orderdate.Date;
                            BLC.name = se.Name;
                            BLC.billno = se.Billno;
                            BLC.rate = (double)se.Rate;
                            BLC.advance = (double)se.Advance;
                            BLC.payment = (double)se.Payment;
                            BLC.balance = (double)se.Balance;
                            BLC.total = (double)se.Total;
                            BLC.Mobile = se.Mobile;
                            dataToShow3.Add(BLC);
                            //double bal = 0; double total = 0;
                            //foreach (var s in dataToShow3)
                            //{
                            //    bal = bal + double.Parse(s.balance);
                            //    Pagination.TotalBalance.Content = bal.ToString();
                            //    total = total + double.Parse(s.total);
                            //    Pagination.Totalamount.Content = total.ToString();
                            //}

                        }
                        else
                        {
                            var rowupdate = dataToShow3.Where(b => b.billno == bill_No).SingleOrDefault();
                            rowupdate.rate += (double)se.Rate;
                        }

                    }
                    string s2 = Process.GetCurrentProcess().MainModule.FileName;
                    RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Balance_RPT.rpt");
                    RPT.Database.Tables[0].SetDataSource(dataToShow3);
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
                            MessageBox.Show("REPORT PRINTED!", "Alert");
                        }
                        catch (Exception err)
                        {
                            MessageBox.Show(err.ToString());
                        }
                    }
                }
            }
            catch { }
        }

        private void Btn_option_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu cm = this.FindResource("cntx_prod_menu") as ContextMenu;
            cm.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }
    }
   
}
