using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoBook.Services;
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
        private PhotoBookProviderService photoBookProvider;

        public string FileFilter { get; } = "PhotoBookFile|*.pbf";

        private string chosenFilePath;
        public string ChosenFilePath
        {
            get => chosenFilePath;
            set => Set(nameof(ChosenFilePath), ref chosenFilePath, value);
        }

        public HomeViewModel(ViewModelLocator locator, PhotoBookProviderService photoBookProvider)
        {
            this.locator = locator;
            this.photoBookProvider = photoBookProvider;
        }

        public RelayCommand Edit => new RelayCommand(() =>
        {
            MainViewModel.Navigator.ChangeCurrentVM(locator.Editor);
        });

        private RelayCommand fileChosen;
        public RelayCommand FileChosen
        {
            get
            {
                return fileChosen ?? (fileChosen = new RelayCommand(
                    () =>
                    {
                        photoBookProvider.Model = Model.PhotoBook.Load(chosenFilePath);
                        MainViewModel.Navigator.ChangeCurrentVM(locator.Editor);
                    }));
            }
        }

    }
}
