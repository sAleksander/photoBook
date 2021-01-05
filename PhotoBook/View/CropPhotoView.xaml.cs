using PhotoBook.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private CropPhotoViewModel viewModel;

        private Model.Arrangement.Rectangle imgCroppingRectangle;

        public CropPhotoView()
        {
            InitializeComponent();

            viewModel = (CropPhotoViewModel)DataContext;
            imgCroppingRectangle = viewModel.CroppingRectangle;

            originalImage.Source = new BitmapImage(new Uri(viewModel.ImagePath));

            Canvas.SetLeft(croppRectangle, imgCroppingRectangle.X);
            Canvas.SetTop(croppRectangle, imgCroppingRectangle.Y);
            croppRectangle.Width = imgCroppingRectangle.Width;
            croppRectangle.Height = imgCroppingRectangle.Height;

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

        private void croppRectangle_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            MousePositionOnMouseDown = e.GetPosition(this);
            if (!croppRectangle.IsMouseCaptured)
                croppRectangle.CaptureMouse();
        }

        private void croppRectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (croppRectangle.IsMouseCaptured)
            {
                Point currentMousePosition = e.GetPosition(this);

                double offsetX = currentMousePosition.X - MousePositionOnMouseDown.X;
                double offsetY = currentMousePosition.Y - MousePositionOnMouseDown.Y;


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

        private void croppRectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (croppRectangle.IsMouseCaptured)
            {
                croppRectangle.ReleaseMouseCapture();

                int[] cooridnates = croppingRectangleCoordinates();
                viewModel.CroppingRectangle = new Model.Arrangement.Rectangle(cooridnates[0], cooridnates[1], cooridnates[2], cooridnates[3]);
            }

        }

        private int [] croppingRectangleCoordinates()
        {
            //croppRectangle[0] - X, croppRectangle[1] - Y
            int[] rectangle = new int[] { Convert.ToInt32(Canvas.GetLeft(croppRectangle)),
                                        Convert.ToInt32(Canvas.GetTop(croppRectangle)), 
                                        Convert.ToInt32(croppRectangle.ActualWidth), 
                                        Convert.ToInt32(croppRectangle.ActualHeight) };
            return rectangle;
        }


    }
}
