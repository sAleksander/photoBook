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
    /// Interaction logic for FrontCover.xaml
    /// </summary>
    public partial class FrontCover : Page
    {
        private FrontCoverViewModel viewModel;

        private Canvas canvas;

        private Label titleLabel;
        

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
        }

        private void DrawFrontCover()
        {
            canvas.Children.Clear();

            canvas.Width = PhotoBookModel.PageWidthInPixels;
            canvas.Height = PhotoBookModel.PageHeightInPixels;

            var frontCover = viewModel.FrontCover;

            canvas.Children.Add(
                PageDrawingUtilities.CreateBackgroundRectangle(frontCover.Background)
            );

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

            canvas.Children.Add(titleLabel);
        }
    }
}
