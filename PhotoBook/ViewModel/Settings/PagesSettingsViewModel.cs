using GalaSoft.MvvmLight.Command;
using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Pages;

namespace PhotoBook.ViewModel.Settings
{
    public class PagesSettingsViewModel : SettingsViewModel
    {
        private ContentPage leftPage;
        private ContentPage rightPage;

        private ContentPage selectedPage;

        private BackgroundColor backgroundColor;
        public BackgroundColor BackgroundColor
        {
            get => backgroundColor;
            set
            {
                Set(nameof(BackgroundColor), ref backgroundColor, value);
                selectedPage.Background = value;
            }
        }

        public bool IsLeftPageChecked
        {
            get => selectedPage == leftPage;
            set
            {
                if (value) SelectPage(leftPage);
            }
        }

        public bool IsRightPageChecked
        {
            get => selectedPage == rightPage;
            set
            {
                if (value) SelectPage(rightPage);
            }
        }

        public PagesSettingsViewModel(ContentPage leftPage, ContentPage rightPage)
        {
            this.leftPage = leftPage;
            this.rightPage = rightPage;

            SelectPage(leftPage);
            IsLeftPageChecked = true;
        }

        private void SelectPage(ContentPage page)
        {
            selectedPage = page;

            // TODO: Handle BackgroundImage as well
            BackgroundColor = selectedPage.Background as BackgroundColor;
        }
    }
}
