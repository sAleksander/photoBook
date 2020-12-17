using PhotoBook.Model.Pages;

namespace PhotoBook.ViewModel
{
    public class FrontCoverSettingsViewModel : SettingsViewModel
    {
        private FrontCover frontCover;
        public FrontCover FrontCover
        {
            get => frontCover;
            private set => Set(nameof(FrontCover), ref frontCover, value);
        }

        public FrontCoverSettingsViewModel(FrontCover frontCover)
        {
            FrontCover = frontCover;
        }
    }
}
