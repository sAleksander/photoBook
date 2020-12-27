using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Pages;
using PhotoBook.ViewModel;

using WPFRectangle = System.Windows.Shapes.Rectangle;
using PhotoBookModel = PhotoBook.Model.PhotoBook;

namespace PhotoBook.View
{
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    public partial class BookView : System.Windows.Controls.Page
    {
        private Model.Pages.Page[] pages;
        private PageType pageType;
        private BookViewModel viewModel;
        private Canvas canvas;

        public BookView()
        {
            InitializeComponent();

            viewModel = (BookViewModel)DataContext;
            canvas = BookCanvas;

            onPagesChanged();

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(nameof(viewModel.Pages)))
                {
                    onPagesChanged();
                }
            };
        }

        private void onPagesChanged()
        {
            pageType = viewModel.CurrentPageType;
            pages = viewModel.Pages;

            switch (pageType)
            {
                case PageType.FrontCover:
                    DrawFrontCover();
                    break;
                case PageType.Content:
                    DrawContentPages();
                    break;
                case PageType.BackCover:
                    DrawBackCover();
                    break;
            }
        }

        private void DrawFrontCover()
        {
            canvas.Children.Clear();

            canvas.Width = PhotoBookModel.PageWidthInPixels;
            canvas.Height = PhotoBookModel.PageHeightInPixels;

            var frontCover = pages[0] as FrontCover;

            canvas.Children.Add(
                CreateBackgroundRectangle(frontCover.Background)
            );

            var titleLabel = new Label()
            {
                Content = frontCover.Title,
                Width = PhotoBookModel.PageWidthInPixels,
                Height = PhotoBookModel.PageHeightInPixels,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = FrontCover.FontSize,

                // TODO: Should we hardcode it in the model or let the user change it?
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            };

            canvas.Children.Add(titleLabel);
        }

        private void DrawContentPages()
        {
            canvas.Children.Clear();

            canvas.Width = 2 * PhotoBookModel.PageWidthInPixels;
            canvas.Height = PhotoBookModel.PageHeightInPixels;

            for (int i = 0; i < 2; i++)
            {
                DrawContentPage(pages[i] as ContentPage, i);
            }
        }

        private void DrawContentPage(ContentPage page, int index)
        {
            var leftOffset = index * PhotoBookModel.PageWidthInPixels;

            // Create background
            var bgRectangle = CreateBackgroundRectangle(page.Background);
            Canvas.SetLeft(bgRectangle, leftOffset);
            canvas.Children.Add(bgRectangle);

            var layout = page.Layout;

            for (int imgIndex = 0; imgIndex < layout.NumOfImages; imgIndex++)
            {
                var imgConstraints = layout.ImageConstraints[imgIndex];
                var image = page.GetImage(imgIndex);

                // Create image
                var wpfImage = new Image
                {
                    Width = imgConstraints.Width,
                    Height = imgConstraints.Height,
                    Source = new CroppedBitmap(
                        new BitmapImage(new Uri(image.DisplayedPath)),
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

                // Create comment label
                var imgBottom = imgConstraints.Y + imgConstraints.Height;

                var commentLabel = new Label()
                {
                    Content = page.GetComment(imgIndex),
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

        private void DrawBackCover()
        {
            canvas.Children.Clear();

            canvas.Width = PhotoBookModel.PageWidthInPixels;
            canvas.Height = PhotoBookModel.PageHeightInPixels;

            var backCover = pages[0] as BackCover;

            canvas.Children.Add(
                CreateBackgroundRectangle(backCover.Background)
            );
        }

        private WPFRectangle CreateBackgroundRectangle(Background background)
        {
            var rectangle = new WPFRectangle()
            {
                Width = PhotoBookModel.PageWidthInPixels,
                Height = PhotoBookModel.PageHeightInPixels,
            };

            switch (background)
            {
                case BackgroundImage bgImage:
                    throw new NotImplementedException();
                case BackgroundColor bgColor:
                    var color = Color.FromRgb(bgColor.R, bgColor.G, bgColor.B);
                    rectangle.Fill = new SolidColorBrush(color);
                    break;
            }

            return rectangle;
        }
    }
}
