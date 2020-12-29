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
    public partial class BackCover : Page
    {
        private BackCoverViewModel viewModel;

        private Canvas canvas;

        public BackCover()
        {
            InitializeComponent();

            viewModel = (BackCoverViewModel)DataContext;
            canvas = BookCanvas;

            DrawBackCover();

            viewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(viewModel.BackCover)))
            {
                DrawBackCover();
            }
        }

        private void DrawBackCover()
        {
            canvas.Children.Clear();

            canvas.Width = PhotoBookModel.PageWidthInPixels;
            canvas.Height = PhotoBookModel.PageHeightInPixels;

            var backCover = viewModel.BackCover;

            canvas.Children.Add(
                PageDrawingUtilities.CreateBackgroundRectangle(backCover.Background)
            );
        }
    }
}
