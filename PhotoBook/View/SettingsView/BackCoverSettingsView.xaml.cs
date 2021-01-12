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

namespace PhotoBook.View
{
    /// <summary>
    /// Interaction logic for BackCoverSettingsView.xaml
    /// </summary>
    public partial class BackCoverSettingsView : UserControl
    {
        public BackCoverSettingsView()
        {
            InitializeComponent();
        }

        public const int defaultWidth = 385;

        public void PageSizeChange(object senser, SizeChangedEventArgs e)
        {
            double percentage = (ActualWidth / defaultWidth);

            btnPdf.SetValue(FontSizeProperty, 25 * percentage);
            btnHtml.SetValue(FontSizeProperty, 25 * percentage);
        }

    }
}
