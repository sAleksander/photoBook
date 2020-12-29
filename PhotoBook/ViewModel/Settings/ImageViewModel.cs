using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoBook.Model.Pages;

namespace PhotoBook.ViewModel.Settings
{
    public class ImageViewModel : ViewModelBase
    {
        private ContentPage page;
        private int imageIndex;

        private string description;
        public string Description
        {
            get => description;
            set
            {
                Set(nameof(Description), ref description, value);
                page.SetComment(imageIndex, value);
            }
        }

        public string ImageFilter { get; } = "Image Files|*.png;*.jpg";

        private string chosenFilePath;
        public string ChosenFilePath
        {
            get => chosenFilePath;
            set => Set(nameof(ChosenFilePath), ref chosenFilePath, value);
        }

        private RelayCommand fileChosen;
        public RelayCommand FileChosen
        {
            get
            {
                return fileChosen ?? (fileChosen = new RelayCommand(
                    () =>
                    {
                        page.LoadImage(imageIndex, ChosenFilePath);
                    }));
            }
        }

        public ImageViewModel(ContentPage page, int imageIndex)
        {
            this.page = page;
            this.imageIndex = imageIndex;

            Description = page.GetComment(imageIndex);
        }
    }
}
