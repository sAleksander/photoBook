using GalaSoft.MvvmLight;

namespace PhotoBook.ViewModel.Pages
{
    public class PagesViewModel : BookViewModel
    {
        private Model.Pages.ContentPage[] contentPages;
        public Model.Pages.ContentPage[] ContentPages
        {
            get => contentPages;
            private set => Set(nameof(ContentPages), ref contentPages, value);
        }

        public PagesViewModel(Model.Pages.ContentPage[] contentPages)
        {
            ResetPages(contentPages);
        }

        public void ResetPages(Model.Pages.ContentPage[] contentPages)
        {
            ContentPages = contentPages;
        }
    }
}
