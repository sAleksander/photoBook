using GalaSoft.MvvmLight;
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

        public ImageViewModel(ContentPage page, int imageIndex)
        {
            this.page = page;
            this.imageIndex = imageIndex;

            Description = page.GetComment(imageIndex);
        }
    }
}
