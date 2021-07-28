using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
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
    /// Interaction logic for DBSetting.xaml
    /// </summary>
    public partial class DBSetting : Window
    {
        private string _backupFolderFullPath;
        private string _connectionString;
        public DBSetting()
        {
            InitializeComponent();
            var instances = SqlDataSourceEnumerator.Instance.GetDataSources();
            foreach (DataRow instance in instances.AsEnumerable())
            {
                Console.WriteLine(instance["ServerName"] + "\\" + instance["InstanceName"]);
                cmb_servername.Items.Add(instance["ServerName"] + "\\" + instance["InstanceName"]);
            }
        }

        private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BTN_CANCEL_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
