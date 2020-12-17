using PhotoBook.ViewModel;
using System.Windows.Controls;

namespace PhotoBook.View
{
    /// <summary>
    /// Interaction logic for BookView.xaml
    /// </summary>
    public partial class BookView : Page
    {
        private Model.Pages.Page[] pages;
        private PageType pageType;
        private BookViewModel viewModel;

        public BookView()
        {
            InitializeComponent();

            viewModel = (BookViewModel)DataContext;
            SetLabelText();

            viewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName.Equals(nameof(viewModel.CurrentPageType)))
                {
                    pageType = viewModel.CurrentPageType;
                    SetLabelText();
                }
            };
        }

        private void SetLabelText()
        {
            switch (pageType)
            {
                case PageType.FrontCover:
                    TestLabel.Content = "FrontCover";
                    break;
                case PageType.Content:
                    TestLabel.Content = "ContentPage";
                    break;
                case PageType.BackCover:
                    TestLabel.Content = "BackCover";
                    break;
            }
        }
    }
}
