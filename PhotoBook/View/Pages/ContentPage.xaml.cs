using PhotoBook.ViewModel.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using PhotoBookModel = PhotoBook.Model.PhotoBook;

namespace PhotoBook.View.Pages
{
    /// <summary>
    /// Interaction logic for ContentPage.xaml
    /// </summary>
    public partial class ContentPage : Page
    {
        private PagesViewModel viewModel;

        private Canvas canvas;

        public ContentPage()
        {
            InitializeComponent();

            viewModel = (PagesViewModel)DataContext;
            canvas = BookCanvas;

            DrawContentPages();

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(viewModel.ContentPages)))
            {
                DrawContentPages();
            }
        }

        private void DrawContentPages()
        {
            canvas.Children.Clear();

            canvas.Width = 2 * PhotoBookModel.PageWidthInPixels;
            canvas.Height = PhotoBookModel.PageHeightInPixels;

            for (int i = 0; i < 2; i++)
            {
                DrawContentPage(viewModel.ContentPages[i], i);
            }
        }

        private void DrawContentPage(Model.Pages.ContentPage page, int index)
        {
            var leftOffset = index * PhotoBookModel.PageWidthInPixels;

            // Create background
            var bgRectangle = PageDrawingUtilities.CreateBackgroundRectangle(page.Background);
            Canvas.SetLeft(bgRectangle, leftOffset);
            canvas.Children.Add(bgRectangle);

            var layout = page.Layout;

            for (int imgIndex = 0; imgIndex < layout.NumOfImages; imgIndex++)
            {
                var imgConstraints = layout.ImageConstraints[imgIndex];
                var image = page.GetImage(imgIndex);

                if (image != null)
                {
                    DrawImage(image, imgConstraints, leftOffset);
                }

                DrawImageComment(page.GetComment(imgIndex), imgConstraints, leftOffset);
            }
        }

        private void DrawImage(
            PhotoBook.Model.Graphics.Image image,
            PhotoBook.Model.Arrangement.Rectangle imgConstraints,
            int leftOffset)
        {
            var wpfImage = new Image
            {
                Width = imgConstraints.Width,
                Height = imgConstraints.Height,
                Source = new CroppedBitmap(
                    new BitmapImage(new Uri(image.DisplayedAbsolutePath)),
                    new Int32Rect(
                        image.CroppingRectangle.X,
                        image.CroppingRectangle.Y,
                        image.CroppingRectangle.Width,
                        image.CroppingRectangle.Height
                    )
                )
            };

            Canvas.SetLeft(wpfImage, leftOffset + imgConstraints.X);
            Canvas.SetTop(wpfImage, imgConstraints.Y);

            canvas.Children.Add(wpfImage);
        }

        private void DrawImageComment(
            string comment,
            PhotoBook.Model.Arrangement.Rectangle imgConstraints,
            int leftOffset)
        {
            var imgBottom = imgConstraints.Y + imgConstraints.Height;

            var commentLabel = new Label()
            {
                Content = comment,
                Width = PhotoBookModel.PageWidthInPixels,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = Model.Arrangement.Layout.CommentFontSize,

                // TODO: Should we hardcode it in the model or let the user change it?
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            };

            Canvas.SetLeft(commentLabel, leftOffset);
            Canvas.SetTop(commentLabel, imgBottom + Model.Arrangement.Layout.CommentOffsetInPixels);

            canvas.Children.Add(commentLabel);
        }
    }
}
