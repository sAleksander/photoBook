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

        //Adjust cropping reectangle size
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            double [] rectangleSize = AdjustCroppingRectangleDimensions(originalImage.ActualWidth, originalImage.ActualHeight, croppRectangle.ActualWidth, croppRectangle.ActualHeight);

            croppRectangle.Width = rectangleSize[0];
            croppRectangle.Height = rectangleSize[1];
        }

        // returns array of dimensions [0] - width [1] - height, Width/Height proportions is for aspect ration eg: 16:9, WidthProportion = 16, HeightProportion = 9
        private double [] AdjustCroppingRectangleDimensions(double imageWidth, double imageHeight, double rectangleWidthProportion, double rectangleHeightProportion)
        {
            double[] dimensions = new double[2];
            dimensions[0] = imageWidth;
            dimensions[1] = imageWidth * (rectangleHeightProportion / rectangleWidthProportion);
            if (dimensions[1] > imageHeight)
            {
                dimensions[0] = imageHeight * (rectangleWidthProportion / rectangleHeightProportion);
                dimensions[1] = imageHeight;
            }

            return dimensions;
        }

        //Position of mouse when left side id clicked
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

        private double [] croppingRectangleCoordinates()
        {
            //croppRectangle[0] - X, croppRectangle[1] - Y
            double[] rectangle = new double[] { Canvas.GetLeft(croppRectangle), Canvas.GetTop(croppRectangle) };
            return rectangle;
        }

    }
}
