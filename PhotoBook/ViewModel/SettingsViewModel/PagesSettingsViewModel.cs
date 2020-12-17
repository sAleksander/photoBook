using PhotoBook.Model.Pages;

namespace PhotoBook.ViewModel
{
    public class PagesSettingsViewModel : SettingsViewModel
    {
        private ContentPage leftPage;
        public ContentPage LeftPage
        {
            get => leftPage;
            private set => Set(nameof(LeftPage), ref leftPage, value);
        }

        private ContentPage rightPage;
        public ContentPage RightPage
        {
            get => rightPage;
            private set => Set(nameof(RightPage), ref rightPage, value);
        }

        public PagesSettingsViewModel(ContentPage leftPage, ContentPage rightPage)
        {
            LeftPage = leftPage;
            RightPage = rightPage;
        }
    }
}
