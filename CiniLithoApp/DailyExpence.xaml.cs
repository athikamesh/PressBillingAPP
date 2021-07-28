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
    /// Interaction logic for DailyExpence.xaml
    /// </summary>
    public partial class DailyExpence : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        List<Daily_Expence> DE = new List<Daily_Expence>();
        String EXPTyp = "";
        ReportDocument RPT = new ReportDocument();
        public DailyExpence()
        {
            InitializeComponent();
           
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            txt_expno.Text = GetEXPCode();
            txt_expdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            txt_expname.Focus();
            loadExpList();
            LoadEmployee();
        }

        private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_expname.Text != "" && txt_expemployee.Text != "" && txt_expamount.Text != "" && txt_expdate.Text != "")
                {
                    tbl_expence TEXP = new tbl_expence();
                    TEXP.Exp_No = txt_expno.Text;
                    TEXP.Exp_Detail = txt_expname.Text;
                    TEXP.Exp_Name = txt_expemployee.Text;
                    TEXP.Exp_Amount = decimal.Parse(txt_expamount.Text);                   
                    TEXP.Exp_date = DateTime.Parse(txt_expdate.Text).Date;
                    TEXP.CR_date = DateTime.Now.Date;
                    TEXP.Exp_Type = EXPTyp;
                    Cinidb.tbl_expence.Add(TEXP);
                    Cinidb.SaveChanges();
                    MessageBox.Show("Expences Add Successfully","Info");
                    clear();
                    
                    loadExpList();
                }
            }
            catch(Exception ex) { }
        }

        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }
        void clear()
        {
            txt_expno.Text = GetEXPCode();
            txt_expname.Text = "";           
            txt_expemployee.Text = "";
            txt_expamount.Text = "0";           
            txt_expdate.Text= DateTime.Now.ToString("dd-MM-yyyy");
            txt_expname.Focus();
            fromdate.Text = "";
            todate.Text = "";
            txt_Search.Text = "";
            DE.Clear();
            loadExpList();
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
        String GetEXPCode()
        {
            int max = 1001;
            try
            {
                max = Cinidb.tbl_expence.Max(m => m.sno) + 1;
            }
            catch (Exception ex)
            {
                //ErrorLog.WriterLog(ex,"Add_BrandMaster","Add_BrandMaster");
                max = 1001;
            }
            return "EXP"+max.ToString();
           
        }
        void loadExpList()
        {
            decimal Amount = 0,Advance=0;
            try
            {
                DE = new List<CiniLithoApp.Daily_Expence>();
                DateTime dt = DateTime.Now.Date;
                var detlst = Cinidb.tbl_expence.Where(b=>b.Exp_date==dt).ToList();
                foreach(var s in detlst)
                {
                    Daily_Expence DEP = new Daily_Expence();
                    DEP.ExpNo = s.Exp_No;
                    DEP.ExpDet = s.Exp_Detail;
                    DEP.Emp = s.Exp_Name;
                    if (s.Exp_Amount != null)
                    {
                        DEP.Amount = (Decimal)s.Exp_Amount;
                    }
                    else
                    {
                        DEP.Amount = 0;
                    }
                    if (s.Exp_Advance != null)
                    {
                        DEP.Advance = (Decimal)s.Exp_Advance;
                    }
                    else
                    {
                        DEP.Advance = 0;
                    }
                    DEP.ExpDate = (DateTime)s.Exp_date;
                    DEP.ExpType = s.Exp_Type;
                    DE.Add(DEP);
                    if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                    {
                        Amount += decimal.Parse(s.Exp_Amount.ToString());
                    }
                    else
                    {
                        Advance += decimal.Parse(s.Exp_Amount.ToString());
                    }
                }
                Searchgrid.ItemsSource = DE;
                Searchgrid.Items.Refresh();
                TotalAmt.Text = Amount.ToString();
                TotalAdv.Text = Advance.ToString();
            }
            catch(Exception ex) { }
        }
        private void Searchgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void fromdate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            decimal Amount = 0, Advance = 0; DE.Clear();
            try
            {
                if (fromdate.Text != "" && todate.Text != "")
                {
                    if (EXPTyp == "EMP")
                    {
                        if (txt_exp_employee.Text != "")
                        {
                            var detlst = Cinidb.tbl_expence.Where(b => b.Exp_date >= fromdate.SelectedDate && b.Exp_date <= todate.SelectedDate && b.Exp_Type == "EMP" && b.Exp_Name == txt_exp_employee.Text).ToList();
                            foreach (var s in detlst)
                            {
                                Daily_Expence DEP = new Daily_Expence();
                                DEP.ExpNo = s.Exp_No;
                                DEP.ExpDet = s.Exp_Detail;
                                DEP.Emp = s.Exp_Name;
                                if (s.Exp_Amount != null)
                                {
                                    DEP.Amount = (Decimal)s.Exp_Amount;
                                }
                                else
                                {
                                    DEP.Amount = 0;
                                }
                                if (s.Exp_Advance != null)
                                {
                                    DEP.Advance = (Decimal)s.Exp_Advance;
                                }
                                else
                                {
                                    DEP.Advance = 0;
                                }
                                DEP.ExpDate = (DateTime)s.Exp_date;
                                DEP.ExpType = s.Exp_Type;
                                DE.Add(DEP);
                                if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                                {
                                    Amount += decimal.Parse(s.Exp_Amount.ToString());
                                }
                                else
                                {
                                    Advance += decimal.Parse(s.Exp_Amount.ToString());
                                }
                            }
                            if (detlst.Count == 0)
                            {
                                MessageBox.Show("No Data Found", "Alert");
                            }
                        }
                        else
                        {
                            var detlst = Cinidb.tbl_expence.Where(b => b.Exp_date >= fromdate.SelectedDate && b.Exp_date <= todate.SelectedDate && b.Exp_Type == "EMP").ToList();
                            foreach (var s in detlst)
                            {
                                Daily_Expence DEP = new Daily_Expence();
                                DEP.ExpNo = s.Exp_No;
                                DEP.ExpDet = s.Exp_Detail;
                                DEP.Emp = s.Exp_Name;
                                if (s.Exp_Amount != null)
                                {
                                    DEP.Amount = (Decimal)s.Exp_Amount;
                                }
                                else
                                {
                                    DEP.Amount = 0;
                                }
                                if (s.Exp_Advance != null)
                                {
                                    DEP.Advance = (Decimal)s.Exp_Advance;
                                }
                                else
                                {
                                    DEP.Advance = 0;
                                }
                                DEP.ExpDate = (DateTime)s.Exp_date;
                                DEP.ExpType = s.Exp_Type;
                                DE.Add(DEP);
                                if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                                {
                                    Amount += decimal.Parse(s.Exp_Amount.ToString());
                                }
                                else
                                {
                                    Advance += decimal.Parse(s.Exp_Amount.ToString());
                                }
                            }
                            if (detlst.Count == 0)
                            {
                                MessageBox.Show("No Data Found", "Alert");
                            }
                        }
                    }
                    else
                    {
                        var detlst = Cinidb.tbl_expence.Where(b => b.Exp_date >= fromdate.SelectedDate && b.Exp_date <= todate.SelectedDate).ToList();
                        foreach (var s in detlst)
                        {
                            Daily_Expence DEP = new Daily_Expence();
                            DEP.ExpNo = s.Exp_No;
                            DEP.ExpDet = s.Exp_Detail;
                            DEP.Emp = s.Exp_Name;
                            DEP.Amount = (Decimal)s.Exp_Amount;
                            DEP.Advance = (Decimal)s.Exp_Advance;
                            DEP.ExpDate = (DateTime)s.Exp_date;
                            DEP.ExpType = s.Exp_Type;
                            DE.Add(DEP);
                            if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                            {
                                Amount += decimal.Parse(s.Exp_Amount.ToString());
                            }
                            else
                            {
                                Advance += decimal.Parse(s.Exp_Amount.ToString());
                            }
                        }
                        if (detlst.Count == 0)
                        {
                            MessageBox.Show("No Data Found", "Alert");
                        }
                    }

                    Searchgrid.ItemsSource = DE;
                    Searchgrid.Items.Refresh();
                    TotalAmt.Text = Amount.ToString();

                }
            }
            catch (Exception ex) { }
        }

        private void todate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            decimal Amount = 0, Advance = 0;
            DE.Clear();
            try
            {
                if (fromdate.Text != "" && todate.Text != "")
                {
                    if (EXPTyp == "EMP")
                    {
                        if (txt_exp_employee.Text != "")
                        {
                            var detlst = Cinidb.tbl_expence.Where(b => b.Exp_date >= fromdate.SelectedDate && b.Exp_date <= todate.SelectedDate && b.Exp_Type == "EMP" && b.Exp_Name == txt_exp_employee.Text).ToList();
                            foreach (var s in detlst)
                            {
                                Daily_Expence DEP = new Daily_Expence();
                                DEP.ExpNo = s.Exp_No;
                                DEP.ExpDet = s.Exp_Detail;
                                DEP.Emp = s.Exp_Name;
                                if (s.Exp_Amount != null)
                                {
                                    DEP.Amount = (Decimal)s.Exp_Amount;
                                }
                                else
                                {
                                    DEP.Amount = 0;
                                }
                                if (s.Exp_Advance != null)
                                {
                                    DEP.Advance = (Decimal)s.Exp_Advance;
                                }
                                else
                                {
                                    DEP.Advance = 0;
                                }
                                DEP.ExpDate = (DateTime)s.Exp_date;
                                DEP.ExpType = s.Exp_Type;
                                DE.Add(DEP);
                                if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                                {
                                    Amount += decimal.Parse(s.Exp_Amount.ToString());
                                }
                                else
                                {
                                    Advance += decimal.Parse(s.Exp_Amount.ToString());
                                }
                            }
                            if (detlst.Count == 0)
                            {
                                MessageBox.Show("No Data Found", "Alert");
                            }
                        }
                        else
                        {
                            var detlst = Cinidb.tbl_expence.Where(b => b.Exp_date >= fromdate.SelectedDate && b.Exp_date <= todate.SelectedDate && b.Exp_Type == "EMP").ToList();
                            foreach (var s in detlst)
                            {
                                Daily_Expence DEP = new Daily_Expence();
                                DEP.ExpNo = s.Exp_No;
                                DEP.ExpDet = s.Exp_Detail;
                                DEP.Emp = s.Exp_Name;
                                if (s.Exp_Amount != null)
                                {
                                    DEP.Amount = (Decimal)s.Exp_Amount;
                                }
                                else
                                {
                                    DEP.Amount = 0;
                                }
                                if (s.Exp_Advance != null)
                                {
                                    DEP.Advance = (Decimal)s.Exp_Advance;
                                }
                                else
                                {
                                    DEP.Advance = 0;
                                }
                                DEP.ExpDate = (DateTime)s.Exp_date;
                                DEP.ExpType = s.Exp_Type;
                                DE.Add(DEP);
                                if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                                {
                                    Amount += decimal.Parse(s.Exp_Amount.ToString());
                                }
                                else
                                {
                                    Advance += decimal.Parse(s.Exp_Amount.ToString());
                                }
                            }
                            if (detlst.Count == 0)
                            {
                                MessageBox.Show("No Data Found", "Alert");
                            }
                        }
                    }
                    else
                    {
                        var detlst = Cinidb.tbl_expence.Where(b => b.Exp_date >= fromdate.SelectedDate && b.Exp_date <= todate.SelectedDate).ToList();
                        foreach (var s in detlst)
                        {
                            Daily_Expence DEP = new Daily_Expence();
                            DEP.ExpNo = s.Exp_No;
                            DEP.ExpDet = s.Exp_Detail;
                            DEP.Emp = s.Exp_Name;
                            DEP.Amount = (Decimal)s.Exp_Amount;
                            DEP.Advance = (Decimal)s.Exp_Advance;
                            DEP.ExpDate = (DateTime)s.Exp_date;
                            DEP.ExpType = s.Exp_Type;
                            DE.Add(DEP);
                            if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                            {
                                Amount += decimal.Parse(s.Exp_Amount.ToString());
                            }
                            else
                            {
                                Advance += decimal.Parse(s.Exp_Amount.ToString());
                            }
                        }
                        if (detlst.Count == 0)
                        {
                            MessageBox.Show("No Data Found", "Alert");
                        }
                    }

                    Searchgrid.ItemsSource = DE;
                    Searchgrid.Items.Refresh();
                    TotalAmt.Text = Amount.ToString();

                }
            }
            catch (Exception ex) { }
        }

        private void frombill_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            List<tbl_expence> detlst=new List<tbl_expence>();
            decimal Amount = 0, Advance = 0;
            DE.Clear();
            try
            {
                if (e.Key == Key.Enter)
                {
                    if (txt_Search.Text != "" || txt_exp_employee.Text != "")
                    {

                        //  var detlst = Cinidb.tbl_expence.Where(b => b.Exp_Detail== txt_Search.Text || b.Exp_Name== txt_Search.Text ).ToList();
                        if (EXPTyp == "EMP")
                        {
                            if (fromdate.Text == "" || todate.Text == "")
                            {
                                detlst = Cinidb.tbl_expence.Where(b => b.Exp_Name == txt_exp_employee.Text && b.Exp_Type == "EMP").ToList();
                            }
                            if (fromdate.Text != "" && todate.Text != "")
                            {
                                detlst = Cinidb.tbl_expence.Where(b => b.Exp_Name == txt_exp_employee.Text && b.Exp_date >= fromdate.SelectedDate && b.Exp_date <= todate.SelectedDate && b.Exp_Type == "EMP").ToList();
                            }
                            foreach (var s in detlst)
                            {
                                Daily_Expence DEP = new Daily_Expence();
                                DEP.ExpNo = s.Exp_No;
                                DEP.ExpDet = s.Exp_Detail;
                                DEP.Emp = s.Exp_Name;
                                DEP.Amount = (Decimal)s.Exp_Amount;
                                if (s.Exp_Amount != null)
                                {
                                    DEP.Amount = (Decimal)s.Exp_Amount;
                                }
                                else
                                {
                                    DEP.Amount = 0;
                                }
                                if (s.Exp_Advance != null)
                                {
                                    DEP.Advance = (Decimal)s.Exp_Advance;
                                }
                                else
                                {
                                    DEP.Advance = 0;
                                }
                                DEP.ExpType = s.Exp_Type;
                                DE.Add(DEP);
                                if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                                {
                                    Amount += decimal.Parse(s.Exp_Amount.ToString());
                                }
                                else
                                {
                                    Advance += decimal.Parse(s.Exp_Amount.ToString());
                                }
                            }
                            if (detlst.Count == 0)
                            {
                                MessageBox.Show("No Data Found", "Alert");
                            }
                        }
                        else
                        {
                            detlst = Cinidb.tbl_expence.Where(b => b.Exp_Detail == txt_Search.Text || b.Exp_Name == txt_Search.Text && b.Exp_Type != "EMP").ToList();
                            foreach (var s in detlst)
                            {
                                Daily_Expence DEP = new Daily_Expence();
                                DEP.ExpNo = s.Exp_No;
                                DEP.ExpDet = s.Exp_Detail;
                                DEP.Emp = s.Exp_Name;
                                DEP.Amount = (Decimal)s.Exp_Amount;
                                if (s.Exp_Advance == null)
                                {
                                    DEP.Advance = 0;
                                }
                                else
                                {
                                    DEP.Advance = (Decimal)s.Exp_Advance;
                                }
                                DEP.ExpDate = (DateTime)s.Exp_date;
                                DEP.ExpType = s.Exp_Type;
                                DE.Add(DEP);
                                if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                                {
                                    Amount += decimal.Parse(s.Exp_Amount.ToString());
                                }
                                else
                                {
                                    Advance += decimal.Parse(s.Exp_Amount.ToString());
                                }
                            }
                            if (detlst.Count == 0)
                            {
                                MessageBox.Show("No Data Found", "Alert");
                            }
                        }
                        Searchgrid.ItemsSource = DE;
                        Searchgrid.Items.Refresh();
                        TotalAmt.Text = Amount.ToString();

                    }
                }
            }
            catch (Exception ex) { }
        }

        private void EXP_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                EXPTyp = "EXP";
                txt_exp_employee.Visibility = Visibility.Collapsed;
                txt_Search.Visibility = Visibility.Visible;
                BTN_PRINT.Visibility = Visibility.Collapsed;
                txt_expname.Text = "";
                txt_expname.IsEnabled = true;
            }
            catch { }
        }

        private void EMP_Checked(object sender, RoutedEventArgs e)
        {
            try { 
            EXPTyp = "EMP";
            txt_exp_employee.Visibility = Visibility.Visible;
            txt_Search.Visibility = Visibility.Collapsed;
             BTN_PRINT.Visibility = Visibility.Visible;
                txt_expname.Text = "Advance";
                txt_expname.IsEnabled = false;
            }
            catch { }
        }

        private void OTH_Checked(object sender, RoutedEventArgs e)
        {
            try { 
            EXPTyp = "OTH";
            txt_exp_employee.Visibility = Visibility.Collapsed;
            txt_Search.Visibility = Visibility.Visible;
                BTN_PRINT.Visibility = Visibility.Collapsed;
                txt_expname.Text = "";
                txt_expname.IsEnabled = true;
            }
            catch { }
        }

        private void defbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               
                    object citem = Searchgrid.SelectedItem;
                    string ID = (Searchgrid.SelectedCells[0].Column.GetCellContent(citem) as TextBlock).Text;
                    string Exp_type= (Searchgrid.SelectedCells[4].Column.GetCellContent(citem) as TextBlock).Text;
                    string Exp_date = (Searchgrid.SelectedCells[5].Column.GetCellContent(citem) as TextBlock).Text;
                    MessageBoxResult msg = MessageBox.Show("Do you Want To Delete this Item !! ", "Alert Message", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (msg == MessageBoxResult.Yes)
                    {
                    if (MainWindow.user_detail.ToUpper()=="ADMIN" || MainWindow.user_detail.ToUpper() == "STAFF" && Exp_type != "EMP" && Exp_date == DateTime.Now.Date.ToString("dd-MM-yyyy"))
                    {
                        var result = Cinidb.tbl_expence.Where(b => b.Exp_No == ID).FirstOrDefault();
                        Cinidb.tbl_expence.Remove(result);
                        int s = Cinidb.SaveChanges();
                        loadExpList();
                    }
                    else
                    {
                        MessageBox.Show("Entry not delete.Kindly Contact Admin..!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    }
                
           }
            catch { }
        }

        private void btn_addemp_Click(object sender, RoutedEventArgs e)
        {
            AddEmployee AE = new CiniLithoApp.AddEmployee("");
            AE.ShowDialog();
            LoadEmployee();
        }
        void LoadEmployee()
        {
            var emp= Cinidb.tbl_Employee.Select(b => b.Emp_Name).ToList();
            txt_expemployee.ItemsSource = emp;
            txt_exp_employee.ItemsSource = emp;
        }

        private void txt_exp_employee_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void txt_exp_employee_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<tbl_expence> detlst = new List<tbl_expence>();
            decimal Amount = 0, Advance = 0;
            string slttxt=(sender as ComboBox).SelectedItem as string;
            DE.Clear();
            try
            {
                if (slttxt != "")
                {

                    //  var detlst = Cinidb.tbl_expence.Where(b => b.Exp_Detail== txt_Search.Text || b.Exp_Name== txt_Search.Text ).ToList();
                    if (EXPTyp == "EMP")
                    {
                        if (fromdate.Text == "" || todate.Text == "")
                        {
                            detlst = Cinidb.tbl_expence.Where(b => b.Exp_Name == slttxt && b.Exp_Type == "EMP").ToList();
                        }
                        if (fromdate.Text != "" && todate.Text != "")
                        {
                            detlst = Cinidb.tbl_expence.Where(b => b.Exp_Name == slttxt && b.Exp_date >= fromdate.SelectedDate && b.Exp_date <= todate.SelectedDate && b.Exp_Type == "EMP").ToList();
                        }
                        foreach (var s in detlst)
                        {
                            Daily_Expence DEP = new Daily_Expence();
                            DEP.ExpNo = s.Exp_No;
                            DEP.ExpDet = s.Exp_Detail;
                            DEP.Emp = s.Exp_Name;
                            if (s.Exp_Amount != null)
                            {
                                DEP.Amount = (Decimal)s.Exp_Amount;
                            }
                            else
                            {
                                DEP.Amount = 0;
                            }
                            if (s.Exp_Advance != null)
                            {
                                DEP.Advance = (Decimal)s.Exp_Advance;
                            }
                            else
                            {
                                DEP.Advance = 0;
                            }
                            
                            DEP.ExpDate = (DateTime)s.Exp_date;
                            DEP.ExpType = s.Exp_Type;
                            DE.Add(DEP);
                            if (s.Exp_Type == "EXP" || s.Exp_Type == "EMP")
                            {
                                Amount += decimal.Parse(s.Exp_Amount.ToString());
                            }
                            else
                            {
                                Advance += decimal.Parse(s.Exp_Amount.ToString());
                            }
                        }
                        if (detlst.Count == 0)
                        {
                            MessageBox.Show("No Data Found", "Alert");
                        }
                    }
                    Searchgrid.ItemsSource = DE;
                    Searchgrid.Items.Refresh();
                    TotalAmt.Text = Amount.ToString();

                }
            }
            catch (Exception ex) { }
        }

        private void BTN_PRINT_Click(object sender, RoutedEventArgs e)
        {
            string s2 = Process.GetCurrentProcess().MainModule.FileName;
            RPT.Load(System.IO.Path.GetDirectoryName(s2) + "\\Reports\\Expense_RPT.rpt");
            RPT.Database.Tables[0].SetDataSource(DE);

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
    public class Daily_Expence
    {
        public string ExpNo { get; set; }
        public string ExpDet { get; set; }
        public string Emp { get; set; }
        public decimal Amount { get; set; }
        public decimal Advance { get; set; }
        public DateTime ExpDate { get; set; }
        public String ExpType { get; set; }

    }
}
