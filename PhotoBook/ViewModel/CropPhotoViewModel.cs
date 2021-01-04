using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace PhotoBook.ViewModel
{
    public class CropPhotoViewModel : ViewModelBase
    {
        private ViewModelLocator locator;

        private Model.Graphics.Image imageToCrop;
        public Model.Graphics.Image ImageToCrop
        {
            set {
                Set(nameof(ImageToCrop), ref imageToCrop, value);

                croppingRectangle = new Model.Arrangement.Rectangle(
                    imageToCrop.CroppingRectangle.X,
                    imageToCrop.CroppingRectangle.Y,
                    imageToCrop.CroppingRectangle.Width,
                    imageToCrop.CroppingRectangle.Height);
            }
        }

        public string ImagePath
        {
            get => imageToCrop.DisplayedAbsolutePath;
        }

        private Model.Arrangement.Rectangle croppingRectangle;
        public Model.Arrangement.Rectangle CroppingRectangle
        {
            get => croppingRectangle;
            set => Set(nameof(CroppingRectangle), ref croppingRectangle, value);
        }

        public CropPhotoViewModel(ViewModelLocator locator)
        {
            this.locator = locator;
        }

        private RelayCommand apply;
        public RelayCommand Apply
        {
            get
            {
                return apply ?? (apply = new RelayCommand(
                    () =>
                    {
                        imageToCrop.CroppingRectangle = CroppingRectangle;
                        MainViewModel.Navigator.ChangeCurrentVM(locator.Editor);
                    }));
            }
        }
    }
}
