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
    /// Interaction logic for Supplier_Frm.xaml
    /// </summary>
    public partial class Supplier_Frm : Window
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        public Supplier_Frm()
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            get_supplier_list();
            BTN_SAVE.Tag = "Save";
        }

        private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (BTN_SAVE.Tag == "Save")
                {
                    int det = Cinidb.tbl_Supplier.Where(b => b.CPName == txt_suppliername.Text).Count();
                    if (det == 0)
                    {
                        tbl_Supplier tsp = new CiniLithoApp.tbl_Supplier();
                        tsp.CPName = txt_suppliername.Text;                        
                        tsp.Address = txt_address.Text;
                        tsp.Phone = txt_phone.Text;
                        tsp.Mobile = txt_mobile.Text;                      
                        Cinidb.tbl_Supplier.Add(tsp);
                        int i = Cinidb.SaveChanges();
                        if (i == 1)
                        {
                            MessageBox.Show("Supplier Add Successfully", "Information");
                            get_supplier_list();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Supplier Already Available ", "Alert");
                    }
                }
                if(BTN_SAVE.Tag =="Update")
                {
                    var det = Cinidb.tbl_Supplier.Where(b => b.CPName == txt_suppliername.Text).SingleOrDefault();
                    if (det !=null)
                    {
                        det.CPName = txt_suppliername.Text;
                       
                        det.Address = txt_address.Text;
                        det.Phone = txt_phone.Text;
                        det.Mobile = txt_mobile.Text;
                             
                        Cinidb.SaveChanges();                        
                        MessageBox.Show("Supplier Update Successfully", "Information");
                        get_supplier_list();
                        BTN_CANCEL_Click(null, null);
                       
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            txt_suppliername.Text = "";
          
            txt_address.Text = "";
            txt_mobile.Text = "";
            txt_phone.Text = "";
           
            txt_suppliername.Focus();
            get_supplier_list();
            BTN_SAVE.Tag = "Save";
        }

        public void get_supplier_list()
        {
            try
            {
                var det = Cinidb.tbl_Supplier.ToList();
                if(det.Count>0)
                {
                    Searchgrid.ItemsSource = det;
                   
                }
            }
            catch { }
        }

        private void SearchGridScroll_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void Searchgrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Searchgrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            object item = Searchgrid.SelectedItem;
            if (item != null)
            {
                string ID = (Searchgrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text;
                // double Balance = double.Parse((Searchgrid.SelectedCells[9].Column.GetCellContent(item) as TextBlock).Text);
                if (ID != null && ID != "")
                {
                    get_detail(ID);

                }
                else
                {
                    MessageBox.Show("No Data Available..!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void defbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var det= Cinidb.tbl_Supplier.Where(b => b.CPName == txt_suppliername.Text).SingleOrDefault();
                Cinidb.tbl_Supplier.Remove(det);
                Cinidb.SaveChanges();
                get_supplier_list();
                MessageBox.Show("Supplier Deleted Successfully");
            }
            catch { }
        }
        void get_detail(string id)
        {
            try
            {
                var det = Cinidb.tbl_Supplier.Where(b => b.CPName == id).SingleOrDefault();
                if (det != null)
                {
                    txt_suppliername.Text = det.CPName;
                  
                    txt_address.Text = det.Address;
                    txt_mobile.Text = det.Mobile;
                    txt_phone.Text = det.Phone;
                 
                    BTN_SAVE.Tag = "Update";
                }
            }
            catch { }
        }
    }
}
