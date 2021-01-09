using PhotoBook.Model.Backgrounds;
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
    /// Interaction logic for ColorPicker.xaml
    /// </summary>
    public partial class ColorPicker : UserControl
    {
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register(
            nameof(BackgroundColor),
            typeof(BackgroundColor),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnBackgroundColorChanged))
        );

        public BackgroundColor BackgroundColor
        {
            get => (BackgroundColor)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
        public ColorPicker()
        {
            InitializeComponent();
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if(colorPicker.SelectedColor.HasValue)
            {
                Color c = colorPicker.SelectedColor.Value;
                var red = c.R;
                var green = c.G;
                var blue = c.B;

                BackgroundColor = new BackgroundColor(
                    red,
                    green,
                    blue
                    );
            }
        }

        private static void OnBackgroundColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var newColor = d as ColorPicker;
            Color bColor = new Color();
            bColor = Color.FromRgb(0,0,0);

            
            Console.WriteLine($"Color before change {newColor.BackgroundColor.R}");
            newColor.colorPicker.SelectedColor = Color.FromRgb(newColor.BackgroundColor.R, newColor.BackgroundColor.G, newColor.BackgroundColor.B);

        }

    }
}
