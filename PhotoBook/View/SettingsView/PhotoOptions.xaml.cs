﻿using PhotoBook.ViewModel.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
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

    class ImageIndexToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int == false) throw new Exception(
                $"{nameof(ImageIndexToStringConverter)}: value must be int");

            int imageIndex = (int)value;
            return $"Zdjęcie {imageIndex + 1}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception($"{nameof(ImageIndexToStringConverter)} doesn't support ConvertBack");
        }
    }
}
