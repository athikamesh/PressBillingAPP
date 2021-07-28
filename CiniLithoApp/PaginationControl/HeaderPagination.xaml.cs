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
    /// Interaction logic for HeaderPagination.xaml
    /// </summary>
    public partial class HeaderPagination : UserControl
    {
        public HeaderPagination()
        {
            InitializeComponent();
            atoz.Foreground = new SolidColorBrush(Colors.White);
            atoz.Background = (SolidColorBrush)FindResource("BaseColor");
        }

        private void atof_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            atof.Foreground = new SolidColorBrush(Colors.White);
            atof.Background = (SolidColorBrush)FindResource("BaseColor");

            gtol.Background = new SolidColorBrush(Colors.White);
            gtol.Foreground = (SolidColorBrush)FindResource("BaseColor");
          
            mtos.Background = new SolidColorBrush(Colors.White);
            mtos.Foreground = (SolidColorBrush)FindResource("BaseColor");
          
            ttoz.Background = new SolidColorBrush(Colors.White);
            ttoz.Foreground = (SolidColorBrush)FindResource("BaseColor");

            atoz.Background = new SolidColorBrush(Colors.White);
            atoz.Foreground = (SolidColorBrush)FindResource("BaseColor");
        }

        private void gtol_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            gtol.Foreground = new SolidColorBrush(Colors.White);
            gtol.Background = (SolidColorBrush)FindResource("BaseColor");

            atof.Background = new SolidColorBrush(Colors.White);
            atof.Foreground = (SolidColorBrush)FindResource("BaseColor");

            mtos.Background = new SolidColorBrush(Colors.White);
            mtos.Foreground = (SolidColorBrush)FindResource("BaseColor");

            ttoz.Background = new SolidColorBrush(Colors.White);
            ttoz.Foreground = (SolidColorBrush)FindResource("BaseColor");

            atoz.Background = new SolidColorBrush(Colors.White);
            atoz.Foreground = (SolidColorBrush)FindResource("BaseColor");
        }

        private void mtos_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mtos.Foreground = new SolidColorBrush(Colors.White);
            mtos.Background = (SolidColorBrush)FindResource("BaseColor");

            gtol.Background = new SolidColorBrush(Colors.White);
            gtol.Foreground = (SolidColorBrush)FindResource("BaseColor");

            atof.Background = new SolidColorBrush(Colors.White);
            atof.Foreground = (SolidColorBrush)FindResource("BaseColor");

            ttoz.Background = new SolidColorBrush(Colors.White);
            ttoz.Foreground = (SolidColorBrush)FindResource("BaseColor");

            atoz.Background = new SolidColorBrush(Colors.White);
            atoz.Foreground = (SolidColorBrush)FindResource("BaseColor");
        }

        private void ttoz_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ttoz.Foreground = new SolidColorBrush(Colors.White);
            ttoz.Background = (SolidColorBrush)FindResource("BaseColor");

            gtol.Background = new SolidColorBrush(Colors.White);
            gtol.Foreground = (SolidColorBrush)FindResource("BaseColor");

            mtos.Background = new SolidColorBrush(Colors.White);
            mtos.Foreground = (SolidColorBrush)FindResource("BaseColor");

            atof.Background = new SolidColorBrush(Colors.White);
            atof.Foreground = (SolidColorBrush)FindResource("BaseColor");

            atoz.Background = new SolidColorBrush(Colors.White);
            atoz.Foreground = (SolidColorBrush)FindResource("BaseColor");
        }

        private void atoz_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            atoz.Foreground = new SolidColorBrush(Colors.White);
            atoz.Background = (SolidColorBrush)FindResource("BaseColor");

            ttoz.Background = new SolidColorBrush(Colors.White);
            ttoz.Foreground = (SolidColorBrush)FindResource("BaseColor");

            gtol.Background = new SolidColorBrush(Colors.White);
            gtol.Foreground = (SolidColorBrush)FindResource("BaseColor");

            mtos.Background = new SolidColorBrush(Colors.White);
            mtos.Foreground = (SolidColorBrush)FindResource("BaseColor");

            atof.Background = new SolidColorBrush(Colors.White);
            atof.Foreground = (SolidColorBrush)FindResource("BaseColor");
        }
    }
}
