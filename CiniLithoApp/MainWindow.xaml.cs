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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        public static string user_detail="";
        public MainWindow(string username)
        {
            user_detail = username;
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            var obj = Type.GetType("CiniLithoApp.Billpage");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
            var s = Cinidb.tbl_officeuse.Where(b => b.uname == username).Single();
            if(s.uname.ToUpper()=="STAFF")
            {
                Btn_statement.Visibility = Visibility.Collapsed;
                Btn_setting.Visibility = Visibility.Collapsed;              
                Btn_rbackup.Visibility = Visibility.Collapsed;
            }

        }

     

        private void Btn_Newbill_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.Billpage");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
        }

        private void Btn_payment_Click(object sender, RoutedEventArgs e)
        {
            Payment PYT = new CiniLithoApp.Payment("");
            PYT.ShowDialog();
        }

        private void Btn_balance_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.BalanceList");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
        }

     

        private void Btn_statement_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.StatementList");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
        }

        private void Btn_partyname_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.PartyList");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
        }

        private void Btn_setting_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.SettingPage");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
        }

        private void Btn_cusomertdetail_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.CustomerPage");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
        }

        private void Btn_cashinhand_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.Cashinhandpage");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
        }

        private void Btn_EditNewbill_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.EditBillPage");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
        }

        private void Btn_login_Click(object sender, RoutedEventArgs e)
        {
            DBSetting LG = new DBSetting();
            LG.ShowDialog();
    
        }

        private void Btn_rbackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var s = Cinidb.tbl_officeuse.Where(b => b.uname == user_detail).Single();
                if (s.uname.ToUpper() == "ADMIN")
                {
                    string dbname = Cinidb.Database.Connection.Database;
                    string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'Billing Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                    Cinidb.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname, "D:\\DBBackups\\BackupBillingDB_"+DateTime.Now.ToString("ddMMyyyy hhmmss")));

                    Cinidb.Database.ExecuteSqlCommand("TRUNCATE TABLE tbl_newbill");
                    Cinidb.Database.ExecuteSqlCommand("TRUNCATE TABLE tbl_billdetails");
                    Cinidb.Database.ExecuteSqlCommand("TRUNCATE TABLE tbl_totaldetails");
                    Cinidb.Database.ExecuteSqlCommand("TRUNCATE TABLE tbl_logdetail");

                    LoginFrm LGF = new CiniLithoApp.LoginFrm();
                    LGF.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Your Not Connected", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch(Exception ex) { }
        }

        private void Btn_addexpence_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.DailyExpence");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);
           
        }

        private void Btn_purchase_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.PurchasePage");
            var pageObj = Activator.CreateInstance(obj,"", "SAVE");
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);

        }

        private void Btn_purchaselist_Click(object sender, RoutedEventArgs e)
        {
            var obj = Type.GetType("CiniLithoApp.PurchaseList");
            var pageObj = Activator.CreateInstance(obj);
            PageNavigator.NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden;
            PageNavigator.NavigationService.Navigate(pageObj);

        }

        //public static void Backup(string FileName, string[] Tables)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Microsoft.SqlServer.Server srv = new Server(new Microsoft.SqlServer.Management.Common.ServerConnection("<db name>", "<user name>", "<password>"));
        //    Database dbs = srv.Databases["<db name>"];
        //    ScriptingOptions options = new ScriptingOptions();
        //    options.ScriptData = true;
        //    options.ScriptDrops = false;
        //    options.FileName = FileName;
        //    options.EnforceScriptingOptions = true;
        //    options.ScriptSchema = true;
        //    options.IncludeHeaders = true;
        //    options.AppendToFile = true;
        //    options.Indexes = true;
        //    options.WithDependencies = true;
        //    foreach (var tbl in Tables)
        //    {
        //        dbs.Tables[tbl].EnumScript(options);
        //    }
        //}
    }
}
