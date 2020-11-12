using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace PhotoBook.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private string test = "Nie ma fotoksiążki";
        public string Test
        {
            get { return test; }
            set
            {
                Set(ref test, value);
            }
        }

        private RelayCommand _sayHello;

        public RelayCommand SayHello
        {
            get
            {
                if (_sayHello == null)
                {
                    _sayHello = new RelayCommand(() =>
                    {
                        Test = $"Fotoksiążka zrobiona";
                    });
                }

                return _sayHello;
            }
        }
    }
}