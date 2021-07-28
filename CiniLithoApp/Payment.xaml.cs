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
using System.Windows.Shapes;

namespace CiniLithoApp
{
    /// <summary>
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        string paymentmode = "";string mobile = ""; int sno = 0;
        string current_year = DateTime.Now.Year.ToString();
        public Payment(string code)
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            txt_orderno.Focus();
            txt_orderno.Text = code;
            LoadData();
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

        private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txt_payment.Text == "" || double.Parse(txt_payment.Text)==0)
                {
                    MessageBox.Show("Invalid Payment amount", "Alert");
                    return;
                }
                if (double.Parse(txt_balance.Text) < 0)
                {
                    MessageBox.Show("Negative balance amount", "Alert");
                    return;
                }
                if (paymentmode == "")
                {
                    MessageBox.Show("Please Select Payment Mode", "Alert");
                    return;
                }
                if (paymentmode=="SBI" || paymentmode=="TMB")
                {
                    if(txt_bank.Text=="")
                    {
                        MessageBox.Show("Please add Bank Reference", "Alert");
                        return;
                    }
                }
                var sts = Cinidb.tbl_totaldetails.Where(b => b.Billno == txt_orderno.Text).SingleOrDefault();
                if(sts==null)
                {
                    MessageBox.Show("Bill No Is Invalid", "Alert");
                    return;
                }

                tbl_logdetail totdet = new tbl_logdetail();
                totdet.Rno = txt_orderno.Text;
                totdet.Rname = txt_name.Text;
                totdet.Rmobile = mobile;
                totdet.mode = paymentmode;
                totdet.paydet = txt_payment.Text;
                totdet.datim = DateTime.Now.ToString("dd-MM-yyyy");
                totdet.Orderdet = "Payment Amount Update";
                totdet.BankRef = txt_bank.Text;             
                Cinidb.tbl_logdetail.Add(totdet);
                Cinidb.SaveChanges();

                var upproduct = Cinidb.tbl_totaldetails.Where(b => b.Billno == txt_orderno.Text).SingleOrDefault();
                upproduct.Payment = upproduct.Payment+ double.Parse(txt_payment.Text);
                upproduct.BankRef = txt_bank.Text;
                upproduct.Balance = double.Parse(txt_balance.Text);
                upproduct.PDate= DateTime.Now.ToString("dd-MM-yyyy"); 
                Cinidb.SaveChanges();

                var upcustomeradvance = Cinidb.tbl_customer.Where(b => b.Sno == sno).SingleOrDefault();
                decimal adv = 0;
                if (upcustomeradvance != null)
                {
                    adv = (decimal)upcustomeradvance.advance;
                }
                if(adv>=decimal.Parse(txt_payment.Text))
                {
                    decimal SADV = adv - decimal.Parse(txt_payment.Text);
                    upcustomeradvance.advance = SADV;
                    Cinidb.SaveChanges();
                }
                else
                {
                    if (adv > 0)
                    {
                        decimal SADV = decimal.Parse(txt_payment.Text) - adv;
                        upcustomeradvance.advance = SADV;
                        Cinidb.SaveChanges();
                    }
                }

             //   upcustomeradvance.advance-=decimal.Parse(txt_payment.Text)

                this.Close();

            }
            catch(Exception ex) { }
        }

        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            txt_orderno.Text="";
            txt_name.Text="";
            mobile="";
            paymentmode="";
            txt_payment.Text="";
            txt_balance.Text = "";
            txt_orderno.Text = "";
            txt_total.Text = "";
            txt_paid.Text = "";
            txt_balance.Text = "";
            txb_acbal.Text = "Account Balance : 000";
            txt_orderno.Focus();
            RBT_Cash.IsChecked = false;
            RBT_SBI.IsChecked = false;
            RBT_TMB.IsChecked = false;
        }

        private void txt_orderno_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    if(txt_orderno.Text!="")
                    {
                        var det = Cinidb.tbl_totaldetails.Where(b => b.Billno == "CL"+txt_orderno.Text+"/"+ current_year).SingleOrDefault();
                        if(det!=null)
                        {
                            double tot = det.Advance + det.Payment;
                            txt_total.Text = (det.Total - tot).ToString();
                            txt_paid.Text = tot.ToString();
                            txt_balance.Text = det.Balance.ToString();
                            txt_bank.Text = det.BankRef;
                            txt_orderno.Text = det.Billno;
                            var det1=Cinidb.tbl_newbill.Where(b=>b.Billno==txt_orderno.Text).SingleOrDefault();
                            if(det1!=null)
                            {
                                txt_name.Text = det1.Name;
                                sno = det1.Sno;
                                mobile = det1.Mobile;                      
                            }
                           
                            var det2 = Cinidb.tbl_customer.Where(b => b.Sno == sno).SingleOrDefault();
                            if (det2 != null)
                            {
                                txt_name.Text = det2.Cname;
                                mobile = det2.cphone;
                                txb_acbal.Text = "Account Balance : " + det2.advance;
                                txb_acbal.Tag = det2.advance;
                                txt_payment.Focus();
                                txt_payment.Focusable = true;
                                txt_payment.SelectAll();
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            { }
        }

      

        private void txt_payment_PreviewKeyDown(object sender, KeyEventArgs e)
        {
          
        }

        private void txt_payment_KeyUp(object sender, KeyEventArgs e)
        {
            
        }
     
        void LoadData()
        {
            if (txt_orderno.Text != "")
            {
                var det = Cinidb.tbl_totaldetails.Where(b => b.Billno == txt_orderno.Text).SingleOrDefault();
                if (det != null)
                {
                    double tot = det.Advance + det.Payment;                                                           
                    txt_total.Text = (det.Total - tot).ToString();
                    txt_paid.Text = tot.ToString();
                    txt_balance.Text = det.Balance.ToString();
                    txt_bank.Text = det.BankRef;
                    txt_orderno.Text = det.Billno;
                    var det1 = Cinidb.tbl_newbill.Where(b => b.Billno == txt_orderno.Text).SingleOrDefault();
                    if (det1 != null)
                    {
                        txt_name.Text = det1.Name;
                        sno = det1.Sno;
                        mobile = det1.Mobile;
                    }

                    var det2 = Cinidb.tbl_customer.Where(b => b.Sno == sno).SingleOrDefault();
                    if (det2 != null)
                    {
                        txt_name.Text = det2.Cname;
                        mobile = det2.cphone;
                        txb_acbal.Text = "Account Balance : " + det2.advance;
                        txb_acbal.Tag = det2.advance;
                        txt_payment.Focus();
                        txt_payment.Focusable = true;
                        txt_payment.SelectAll();
                    }
                }
            }
        }


        private void txt_payment_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txt_payment.Text != "")
            {
                if (double.Parse(txt_payment.Text) > 0 && double.Parse(txt_balance.Text) > 0)
                {
                    if (double.Parse(txt_balance.Text) > 0 && double.Parse(txt_payment.Text) <= double.Parse(txt_total.Text))
                    {
                        double rami = double.Parse(txt_total.Text) - double.Parse(txt_payment.Text);
                        txt_balance.Text = rami.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Paid Amount More Than Balance Amount!!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                        txt_payment.Text = "000";
                        txt_balance.Text = txt_total.Text;
                        txt_payment.SelectAll();
                    }
                }
                else
                {
                    //MessageBox.Show("Paid Amount More Than Balance Amount!!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                    txt_payment.Text = "000";
                    txt_balance.Text = txt_total.Text;
                    txt_payment.Focusable = true;
                    txt_payment.Focus();
                    txt_payment.SelectAll();
                }

            }
        }
    }
}
