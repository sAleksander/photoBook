using PhotoBook.Model.Helpers;
using PhotoBook.ViewModel.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

using PhotoBookModel = PhotoBook.Model.PhotoBook;

namespace PhotoBook.View.Pages
{
    /// <summary>
    /// Interaction logic for FrontCover.xaml
    /// </summary>
    public partial class FrontCover : UserControl
    {
        private FrontCoverViewModel viewModel;

        private Canvas canvas;

        private Label titleLabel;
        private Rectangle backgroundRectangle;

        public FrontCover()
        {
            InitializeComponent();

            viewModel = (FrontCoverViewModel)DataContext;
            canvas = BookCanvas;

            DrawFrontCover();

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(viewModel.FrontCover)))
            {
                DrawFrontCover();
            }
            else if (args.PropertyName.Equals(nameof(viewModel.Title)))
            {
                titleLabel.Content = viewModel.Title;
            }
            else if (args.PropertyName.Equals(nameof(viewModel.Background)))
            {
                AdjustFontColor();
            }
        }

        private void AdjustFontColor()
        {
            var fill = backgroundRectangle.Fill as SolidColorBrush;
            var newColor = viewModel.Background;
            fill.Color = Color.FromRgb(newColor.R, newColor.G, newColor.B);

            FontAdjuster.AdjustFont(titleLabel, newColor.R, newColor.G, newColor.B);
        }

        private void DrawFrontCover()
        {
            canvas.Children.Clear();

            canvas.Width = PhotoBookModel.PageWidthInPixels;
            canvas.Height = PhotoBookModel.PageHeightInPixels;

            var frontCover = viewModel.FrontCover;

            backgroundRectangle = PageDrawingUtilities.CreateBackgroundRectangle(frontCover.Background);

            canvas.Children.Add(backgroundRectangle);

            titleLabel = new Label()
            {
                Content = frontCover.Title,
                Width = PhotoBookModel.PageWidthInPixels,
                Height = PhotoBookModel.PageHeightInPixels,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = Model.Pages.FrontCover.FontSize,

                // TODO: Should we hardcode it in the model or let the user change it?
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
            };

            AdjustFontColor();

            canvas.Children.Add(titleLabel);
        }
    }
}
