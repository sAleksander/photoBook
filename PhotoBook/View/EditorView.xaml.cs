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
    /// Interaction logic for EditorView.xaml
    /// </summary>
    public partial class EditorView : Page
    {
        public EditorView()
        {
            InitializeComponent();
        }



        public const int defaultHeight = 1020;

        public void PageSizeChange(object senser, SizeChangedEventArgs e)
        {
            double percentage = (ActualHeight / defaultHeight);

            menuTop.SetValue(FontSizeProperty, 18*percentage);
            headerName.SetValue(FontSizeProperty, 35 * percentage);
            logoImage.SetValue(WidthProperty, 120 * percentage);

            //Bottom buttons
            imagePrev.SetValue(WidthProperty, 16 * percentage);
            labelPrev.SetValue(FontSizeProperty, 20 * percentage);

            imageDelete.SetValue(WidthProperty, 16 * percentage);
            labelDelete.SetValue(FontSizeProperty, 20 * percentage);

            imageAdd.SetValue(WidthProperty, 16 * percentage);
            labelAdd.SetValue(FontSizeProperty, 20 * percentage);

            imageNext.SetValue(WidthProperty, 16 * percentage);
            labelNext.SetValue(FontSizeProperty, 20 * percentage);

        }
    }
}
