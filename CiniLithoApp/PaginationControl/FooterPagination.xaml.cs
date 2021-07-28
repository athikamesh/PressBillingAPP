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

namespace CiniLithoApp.PaginationControl
{
    /// <summary>
    /// Interaction logic for FooterPagination.xaml
    /// </summary>
    public partial class FooterPagination : UserControl
    {

        private int _first;

        public int FirstIndex
        {
            get { return _first; }
            set { _first = value; }
        }

        private int _last;

        public int LastIndex
        {
            get { return _last; }
            set { _last = value; }
        }

        private int _next;

        public int NextIndex
        {
            get { return _next; }
            set { _next = value; }
        }

        private int _previous;

        public int PreviousIndex
        {
            get { return _previous; }
            set { _previous = value; }
        }

        private int _currentIndex;

        public int CurrentIndex
        {
            get { return _currentIndex; }
            set { _currentIndex = value; }
        }

        private int _recordsPerPage;

        public int RecordsPerPage
        {
            get { return _recordsPerPage; }
            set { _recordsPerPage = value; }
        }
        
        
        
        public FooterPagination()
        {
            InitializeComponent();
            var d= ShowRecords.SelectedItem;
            if (d != null)
            {
                RecordsPerPage = int.Parse(((ComboBoxItem)ShowRecords.SelectedItem).Tag.ToString());
            }
          
        }

        private void AllowDigits(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true;
                    break;
                }
            }
        }

    }
}
