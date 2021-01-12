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

        public string FileFilter { get; } = "PhotoBookFile|*.pbf";

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
                    }));
            }
        }

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
