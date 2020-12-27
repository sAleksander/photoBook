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
    /// Interaction logic for FrontCoverSettingsView.xaml
    /// </summary>
    public partial class FrontCoverSettingsView : Page
    {
        public FrontCoverSettingsView()
        {
            InitializeComponent();
        }

        //Change theme
        public void showTheme(object sender, RoutedEventArgs e)
        {
            if (themeStackPanel.Visibility == Visibility.Collapsed)
                themeStackPanel.Visibility = Visibility.Visible;
            else
                themeStackPanel.Visibility = Visibility.Collapsed;

        }

        //Change title
        public void changeTitle(object sender, RoutedEventArgs e)
        {
            if (titleStackPanel.Visibility == Visibility.Collapsed)
                titleStackPanel.Visibility = Visibility.Visible;
            else
                titleStackPanel.Visibility = Visibility.Collapsed;
        }



        //Change fontFamily
        /*
        public void changeFontFamily(object sender, RoutedEventArgs e)
        {
            if (fontStackPanel.Visibility == Visibility.Collapsed)
                fontStackPanel.Visibility = Visibility.Visible;
            else
                fontStackPanel.Visibility = Visibility.Collapsed;
        }
        */


        //Changing font size
        public const int defaultWidth = 385;

        public void PageSizeChange(object senser, SizeChangedEventArgs e)
        {
            double percentage = (ActualWidth / defaultWidth);

            btnChangeTitle.SetValue(FontSizeProperty, 25 * percentage);
            //btnChangeFont.SetValue(FontSizeProperty, 25 * percentage);
            btnChangeTheme.SetValue(FontSizeProperty, 25 * percentage);

        }
    }
}
