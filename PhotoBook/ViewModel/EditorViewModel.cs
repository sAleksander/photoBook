using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.ViewModel
{
    public class EditorViewModel : ViewModelBase
    {
        private BookViewModel bookViewModel = new BookViewModel();
        public BookViewModel BookViewModel
        {
            get => bookViewModel;
            set => Set(nameof(BookViewModel), ref bookViewModel, value);
        }

        private SettingsViewModel settingsViewModel;
        public SettingsViewModel SettingsViewModel
        {
            get => settingsViewModel;
            set => Set(nameof(SettingsViewModel), ref settingsViewModel, value);
        }

        public RelayCommand Exit => new RelayCommand(() =>
        {
            MainViewModel.Navigator.ChangeCurrentVM<HomeViewModel>();
        });

        public RelayCommand ShowFrontCoverSettings => new RelayCommand(() =>
        {
            SettingsViewModel = new FrontCoverSettingsViewModel();
        });

        public RelayCommand ShowBackCoverSettings => new RelayCommand(() =>
        {
            SettingsViewModel = new BackCoverSettingsViewModel();
        });

        public RelayCommand ShowPagesSettings => new RelayCommand(() =>
        {
            SettingsViewModel = new PagesSettingsViewModel();
        });
    }
}
