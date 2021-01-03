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
    /// Interaction logic for CropPhotoView.xaml
    /// </summary>
    public partial class CropPhotoView : Page
    {
        public CropPhotoView()
        {
            InitializeComponent();
        }

        public Point MousePositionOnMouseDown;

        private void croppRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            MousePositionOnMouseDown = e.GetPosition(this);
            if (!croppRectangle.IsMouseCaptured)
                croppRectangle.CaptureMouse();
        }

        private void croppRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(croppRectangle.IsMouseCaptured)
            {
                Point currentMousePosition = e.GetPosition(this);

                double offsetX = currentMousePosition.X - MousePositionOnMouseDown.X;
                double offsetY = currentMousePosition.Y - MousePositionOnMouseDown.Y;

                Console.WriteLine($"Crop rectangle Left {Canvas.GetLeft(croppRectangle)}");
                Console.WriteLine($"Crop rectangle Top {Canvas.GetTop(croppRectangle)}");

                double rectangleX = Canvas.GetLeft(croppRectangle);
                double rectangleY = Canvas.GetTop(croppRectangle);

                double bottomLine = rectangleY + croppRectangle.ActualHeight + offsetY;
                double upperLine = rectangleY + offsetY;
                double leftLine = rectangleX + offsetX;
                double rightLine = rectangleX + croppRectangle.ActualWidth + offsetX;

                if (upperLine >= 0 && leftLine >= 0 && rightLine <= originalImage.ActualWidth && bottomLine <= originalImage.ActualHeight)
                {
                    Canvas.SetLeft(croppRectangle, rectangleX + offsetX);
                    Canvas.SetTop(croppRectangle, rectangleY + offsetY);
                }

                MousePositionOnMouseDown = currentMousePosition;
            }
        }

        private void croppRectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (croppRectangle.IsMouseCaptured)
                croppRectangle.ReleaseMouseCapture();
        }
    }
}
