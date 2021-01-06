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
    public partial class ContentPage : UserControl
    {
        private PagesViewModel viewModel;

        private Canvas canvas;

        private Rectangle[] backgrounds = new Rectangle[2];
        private UIElement[][][] images = new UIElement[2][][];
        private Label[][] labels = new Label[2][];

        public ContentPage()
        {
            InitializeComponent();

            viewModel = (PagesViewModel)DataContext;
            canvas = BookCanvas;

            DrawContentPages();

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
            viewModel.Redraw += OnViewModelRedraw;
            viewModel.BackgroundChanged += OnViewModelBackgroundChanged;
            viewModel.ImageChanged += OnViewModelImageChanged;
            viewModel.CommentChanged += OnViewModelCommentChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(viewModel.ContentPages)))
            {
                DrawContentPages();
            }
        }

        private void OnViewModelRedraw(int pageIndex)
        {
            DrawContentPages();
        }

        private void OnViewModelBackgroundChanged(int pageIndex)
        {
            var fill = backgrounds[pageIndex].Fill as SolidColorBrush;
            var newColor = viewModel.ContentPages[pageIndex].Background as Model.Backgrounds.BackgroundColor;
            fill.Color = Color.FromRgb(newColor.R, newColor.G, newColor.B);
        }

        private void OnViewModelImageChanged(int pageIndex, int layoutIndex)
        {
            foreach (var child in images[pageIndex][layoutIndex])
            {
                canvas.Children.Remove(child);
            }

            var page = viewModel.ContentPages[pageIndex];

            var image = page.GetImage(layoutIndex);
            var imgConstraints = page.Layout.ImageConstraints[layoutIndex];
            var leftOffset = pageIndex * PhotoBookModel.PageWidthInPixels;

            if (image != null)
            {
                images[pageIndex][layoutIndex] = DrawImage(image, imgConstraints, leftOffset);
            }
            else
            {
                images[pageIndex][layoutIndex] = DrawPlaceholderImage(imgConstraints, leftOffset);
            }
        }

        private void OnViewModelCommentChanged(int pageIndex, int layoutIndex)
        {
            var newComment = viewModel.ContentPages[pageIndex].GetComment(layoutIndex);
            labels[pageIndex][layoutIndex].Content = newComment;
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

        private void DrawContentPage(Model.Pages.ContentPage page, int pageIndex)
        {
            var leftOffset = pageIndex * PhotoBookModel.PageWidthInPixels;

            // Create background
            var bgRectangle = PageDrawingUtilities.CreateBackgroundRectangle(page.Background);
            Canvas.SetLeft(bgRectangle, leftOffset);
            canvas.Children.Add(bgRectangle);
            backgrounds[pageIndex] = bgRectangle;

            var layout = page.Layout;
            images[pageIndex] = new UIElement[layout.NumOfImages][];
            labels[pageIndex] = new Label[layout.NumOfImages];

            for (int imgIndex = 0; imgIndex < layout.NumOfImages; imgIndex++)
            {
                var imgConstraints = layout.ImageConstraints[imgIndex];
                var image = page.GetImage(imgIndex);

                if (image != null)
                {
                    images[pageIndex][imgIndex] = DrawImage(image, imgConstraints, leftOffset);
                }
                else
                {
                    images[pageIndex][imgIndex] = DrawPlaceholderImage(imgConstraints, leftOffset);
                }

                labels[pageIndex][imgIndex] = DrawImageComment(page.GetComment(imgIndex), imgConstraints, leftOffset);
            }
        }

        private UIElement[] DrawImage(
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

            return new UIElement[] { wpfImage };
        }

        private UIElement[] DrawPlaceholderImage(
            PhotoBook.Model.Arrangement.Rectangle imgConstraints,
            int leftOffset)
        {
            var placeholder = new Rectangle()
            {
                Width = imgConstraints.Width,
                Height = imgConstraints.Height,
                Fill = new SolidColorBrush(Color.FromRgb(75, 75, 75)),
            };

            Canvas.SetLeft(placeholder, leftOffset + imgConstraints.X);
            Canvas.SetTop(placeholder, imgConstraints.Y);

            var label = new Label()
            {
                Width = imgConstraints.Width,
                Height = imgConstraints.Height,
                Foreground = Brushes.White,
                Content = "Brak zdjęcia",
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 20,
            };

            Canvas.SetLeft(label, leftOffset + imgConstraints.X);
            Canvas.SetTop(label, imgConstraints.Y);

            canvas.Children.Add(placeholder);
            canvas.Children.Add(label);

            return new UIElement[] { placeholder, label };
        }

        private Label DrawImageComment(
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

            return commentLabel;
        }
    }
}
