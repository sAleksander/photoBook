using GalaSoft.MvvmLight.Command;
using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Pages;

namespace PhotoBook.ViewModel.Settings
{
    public class FrontCoverSettingsViewModel : SettingsViewModel
    {
        private FrontCover frontCover;
        private BackCover backCover;

        private string title;
        public string Title
        {
            get => title;
            set => Set(nameof(Title), ref title, value);
        }

        private BackgroundColor backgroundColor;
        public BackgroundColor BackgroundColor
        {
            get => backgroundColor;
            set {
                Set(nameof(BackgroundColor), ref backgroundColor, value);
                frontCover.Background = value;
                backCover.Background = value;
            }
        }

        public RelayCommand ApplyTitle => new RelayCommand(() =>
        {
            frontCover.Title = Title;
        });

        public FrontCoverSettingsViewModel(FrontCover frontCover, BackCover backCover)
        {
            this.frontCover = frontCover;
            this.backCover = backCover;

            Title = frontCover.Title;
            // TODO: Handle BackgroundImage as well
            BackgroundColor = frontCover.Background as BackgroundColor;
        }
    }
}
