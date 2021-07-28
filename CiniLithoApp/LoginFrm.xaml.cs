using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for LoginFrm.xaml
    /// </summary>
    public partial class LoginFrm : Window
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        public static string localconnections = "";
        public LoginFrm()
        {
            InitializeComponent();
            string s2 = Process.GetCurrentProcess().MainModule.FileName;
            string ip= File.ReadAllText(System.IO.Path.GetDirectoryName(s2)+ "\\Reports\\IPDB.txt");
            localconnections = "data source="+ip+";initial catalog=billingdb;user id=sa;password=1";
            Cinidb.Database.Connection.ConnectionString = localconnections;

            var data = Cinidb.tbl_alfserial.FirstOrDefault();
            if(data!=null)
            {                
                Billpage.Y_lable = data.SERALB;
                Billpage.Recount = data.SERNO;
            }
        }

        private void BTN_SAVE_Click(object sender, RoutedEventArgs e1)
        {
           
            try
            {
               
                var loginstat = Cinidb.tbl_officeuse.Where(b => b.uname == cmb_username.Text && b.pword == txt_password.Password).Count();
                if (loginstat == 1)
                {                   
                    MainWindow MW = new CiniLithoApp.MainWindow(cmb_username.Text);                    
                    MW.Show();                   
                    this.Close();                    
                }
                else
                {
                    MessageBox.Show("Invalid Login", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (TargetInvocationException es)
            {
                MessageBox.Show(es.InnerException.Message);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message,"Error"); }

        }

        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void txt_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BTN_SAVE_Click(null, null);
            }
        }
    }
}
