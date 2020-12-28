using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace PhotoBook.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(ViewModelLocator locator)
        {
            Navigator.ChangeCurrentVM(locator.Home);

            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        public static Navigator Navigator { get; private set; } = new Navigator();
    }
}