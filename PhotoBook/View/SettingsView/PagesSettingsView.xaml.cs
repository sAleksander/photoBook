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
    /// Interaction logic for PagesSettingsView.xaml
    /// </summary>
    public partial class PagesSettingsView : Page
    {
        public PagesSettingsView()
        {
            InitializeComponent();
        }

        public void showTheme(object sender, RoutedEventArgs e)
        {
            if (themeStackPanel.Visibility == Visibility.Collapsed)
                themeStackPanel.Visibility = Visibility.Visible;
            else
                themeStackPanel.Visibility = Visibility.Collapsed;

        }

        public void changeLayout(object sender, RoutedEventArgs e)
        {
            if (layoutStackPanel.Visibility == Visibility.Collapsed)
                layoutStackPanel.Visibility = Visibility.Visible;
            else
                layoutStackPanel.Visibility = Visibility.Collapsed;
        }

        //Changing font size
        public const int defaultWidth = 385;

        public void PageSizeChange(object senser, SizeChangedEventArgs e)
        {
            double percentage = (ActualWidth / defaultWidth);

            btnLayout.SetValue(FontSizeProperty, 25 * percentage);
            btnTheme.SetValue(FontSizeProperty, 25 * percentage);
            btnLeftSide.SetValue(FontSizeProperty, 20 * percentage);
            btnRightSide.SetValue(FontSizeProperty, 20 * percentage);

        }
    }
}
