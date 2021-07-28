using CrystalDecisions.CrystalReports.Engine;
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
    /// Interaction logic for CashPreview.xaml
    /// </summary>
    public partial class CashPreview : Window
    {
        
        public CashPreview(ReportDocument RPT)
        {
            InitializeComponent();
            try
            {
                rpt_preview.ViewerCore.ReportSource = RPT;
            }
            catch { }

        }
    }
}
