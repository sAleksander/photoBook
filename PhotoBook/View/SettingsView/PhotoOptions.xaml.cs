using PhotoBook.ViewModel.Settings;
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

namespace PhotoBook.View.SettingsView
{
    /// <summary>
    /// Interaction logic for PhotoOptions.xaml
    /// </summary>
    public partial class PhotoOptions : UserControl
    {
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register(
            nameof(Description),
            typeof(string),
            typeof(PhotoOptions),
            new PropertyMetadata("")
        );

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public PhotoOptions()
        {
            InitializeComponent();
        }

        //Add Description
        public void addDescription(object sender, RoutedEventArgs e)
        {
            if (addDescStackPanel.Visibility == Visibility.Collapsed)
                addDescStackPanel.Visibility = Visibility.Visible;
            else
                addDescStackPanel.Visibility = Visibility.Collapsed;
        }

        //Add Effects 
        public void addFilter(object sender, RoutedEventArgs e)
        {
            if (filterStackPanel.Visibility == Visibility.Collapsed)
                filterStackPanel.Visibility = Visibility.Visible;
            else
                filterStackPanel.Visibility = Visibility.Collapsed;
        }
        private void btnPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (photoOptionStackPanel.Visibility == Visibility.Collapsed)
                photoOptionStackPanel.Visibility = Visibility.Visible;
            else
                photoOptionStackPanel.Visibility = Visibility.Collapsed;
        }

        //Changing font size
        public const int defaultWidth = 385;

        public void PageSizeChange(object senser, SizeChangedEventArgs e)
        {
            double percentage = (ActualWidth / defaultWidth);

            btnPhoto.SetValue(FontSizeProperty, 25 * percentage);

        }
    }
}
