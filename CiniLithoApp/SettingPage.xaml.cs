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
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    public partial class SettingPage : Page
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        public SettingPage()
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            var compinfo = Cinidb.tbl_officeuse.Where(b=>b.uname=="admin").SingleOrDefault();
            txt_companyname.Text = compinfo.cname;
            txt_emailid.Text = compinfo.emailid;
            txt_phoneno.Text = compinfo.Phone;
        }

        private void btn_submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(txt_oldpassword.Password!="" && txt_oldpassword.Password!="" && txt_repassword.Password!="" )
                {
                    var pasdet = Cinidb.tbl_officeuse.Where(b => b.pword == txt_oldpassword.Password).SingleOrDefault();
                    if(pasdet!=null)
                    {
                        if(txt_newpassword.Password==txt_repassword.Password)
                        {
                            pasdet.pword = txt_newpassword.Password;
                            Cinidb.SaveChanges();
                        }
                    }
                }
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            txt_newpassword.Password = "";
            txt_repassword.Password = "";
            txt_oldpassword.Password = "";
            txt_oldpassword.Focus();
        }

        private void btn_submit_pin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_oldpin.Password != "" && txt_newpin.Password != "" && txt_repin.Password != "")
                {
                    var pasdet = Cinidb.tbl_officeuse.Where(b => b.pin == txt_oldpin.Password).SingleOrDefault();
                    if (pasdet != null)
                    {
                        if (txt_newpin.Password == txt_repin.Password)
                        {
                            pasdet.pin  = txt_newpin.Password;
                            Cinidb.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_cancel_pin_Click(object sender, RoutedEventArgs e)
        {
            txt_newpin.Password = "";
            txt_repin.Password = "";
            txt_oldpin.Password = "";
            txt_oldpin.Focus();
        }

        private void btn_submit_company_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_companyname.Text  != "" && txt_phoneno.Text != "" && txt_emailid.Text != "")
                {
                    var pasdet = Cinidb.tbl_officeuse.SingleOrDefault();
                    if (pasdet != null)
                    {
                        pasdet.cname = txt_companyname.Text;
                        pasdet.Phone = txt_phoneno.Text;
                        pasdet.emailid = txt_emailid.Text;
                        Cinidb.SaveChanges();
                      
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_cancel_company_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
