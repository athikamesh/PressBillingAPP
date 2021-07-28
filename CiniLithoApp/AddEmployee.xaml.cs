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
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Window
    {
        CINIDBEntities Cinidb = new CINIDBEntities();
        string state = "Save"; int Empid = 0;
        public AddEmployee(string code)
        {
            InitializeComponent();
            Cinidb.Database.Connection.ConnectionString = LoginFrm.localconnections;
            if (code!="")
            {
                Empid = int.Parse(code);
                var tbemp = Cinidb.tbl_Employee.Where(b => b.id == Empid).SingleOrDefault();
                if(tbemp!=null)
                {
                    txt_name.Text = tbemp.Emp_Name;
                    txt_Mobile.Text = tbemp.Emp_Mobile;
                    state = "Update";
                }
            }
            get_employee_list();
        }

        private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (state == "Save")
                {
                    save();
                }
                if(state=="Update")
                {
                    update(Empid);
                }
                
                get_employee_list();
            }
            catch { }
        }

        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {
            txt_Mobile.Clear();
            txt_name.Clear();
            txt_name.Focusable = true;
            txt_name.Focus();
        }
        void save()
        {
            if (txt_name.Text == "")
            {
                MessageBox.Show("Enter Name");
                return;
            }
            tbl_Employee tbemp = new CiniLithoApp.tbl_Employee();
            tbemp.Emp_Name = txt_name.Text;
            tbemp.Emp_Mobile = txt_Mobile.Text;
            Cinidb.tbl_Employee.Add(tbemp);
            Cinidb.SaveChanges();
            txt_Mobile.Clear();
            txt_name.Clear();
            txt_name.Focusable = true;
            txt_name.Focus();
        }
        void update(int id)
        {
            if (txt_name.Text == "")
            {
                MessageBox.Show("Enter Name");
                return;
            }
            var tbemp = Cinidb.tbl_Employee.Where(b => b.id == id).SingleOrDefault();
            if (tbemp != null)
            {
                tbemp.Emp_Name = txt_name.Text;
                tbemp.Emp_Mobile = txt_Mobile.Text;
                Cinidb.SaveChanges();
                txt_Mobile.Clear();
                txt_name.Clear();
                txt_name.Focusable = true;
                txt_name.Focus();
                state = "Save";
                Empid = 0;
            }
        }
        public void get_employee_list()
        {
            try
            {
                var det = Cinidb.tbl_Employee.ToList();
                if (det.Count > 0)
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
                    Empid= int.Parse((Searchgrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text);
                }
                else
                {
                    MessageBox.Show("No Data Available..!", "Alert", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
        void get_detail(string id)
        {
            try
            {
                var det = Cinidb.tbl_Employee.Where(b => b.Emp_Name == id).SingleOrDefault();
                if (det != null)
                {
                    txt_name.Text = det.Emp_Name;
                    txt_Mobile.Text = det.Emp_Mobile;
                    state = "Update";
                }
            }
            catch { }
        }
        private void defbtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var det = Cinidb.tbl_Employee.Where(b => b.Emp_Name == txt_name.Text).SingleOrDefault();
                Cinidb.tbl_Employee.Remove(det);
                Cinidb.SaveChanges();
                txt_Mobile.Clear();
                txt_name.Clear();
                txt_name.Focusable = true;
                txt_name.Focus();
                get_employee_list();
                MessageBox.Show("Employee Deleted Successfully");
            }
            catch { }
        }
    }
}
