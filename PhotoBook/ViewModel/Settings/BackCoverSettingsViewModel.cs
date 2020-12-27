using PhotoBook.Model.Pages;

namespace PhotoBook.ViewModel.Settings
{
    public class BackCoverSettingsViewModel : SettingsViewModel
    {
        private BackCover backCover;
        public BackCover BackCover
        {
            get => backCover;
            private set => Set(nameof(BackCover), ref backCover, value);
        }

        public BackCoverSettingsViewModel(BackCover backCover)
        {
            BackCover = backCover;
        }
    }
}
