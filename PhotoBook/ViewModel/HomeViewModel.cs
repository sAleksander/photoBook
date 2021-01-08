using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.ViewModel
{
    public class HomeViewModel : ViewModelBase
    {
        private ViewModelLocator locator;

        public HomeViewModel(ViewModelLocator locator)
        {
            this.locator = locator;
        }

        public RelayCommand Edit => new RelayCommand(() =>
        {
            MainViewModel.Navigator.ChangeCurrentVM(locator.Editor);
        });
    }
}
