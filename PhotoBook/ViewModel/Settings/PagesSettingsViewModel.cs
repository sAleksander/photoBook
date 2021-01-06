using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Pages;
using PhotoBook.Services;
using System.Collections.ObjectModel;
using PhotoBookModel = PhotoBook.Model.PhotoBook;

namespace PhotoBook.ViewModel.Settings
{
    public class PagesSettingsViewModel : SettingsViewModel
    {
        private ViewModelLocator locator;
        private PhotoBookModel model;
        private IDialogService dialogService;

        private ContentPage leftPage;
        private ContentPage rightPage;

        private ContentPage selectedPage;

        private BackgroundColor backgroundColor;
        public BackgroundColor BackgroundColor
        {
            get => backgroundColor;
            set
            {
                Set(nameof(BackgroundColor), ref backgroundColor, value);
                selectedPage.Background = value;
            }
        }

        public bool IsLeftPageChecked
        {
            get => selectedPage == leftPage;
            set
            {
                RaisePropertyChanged(nameof(IsLeftPageChecked));
                if (value) SelectPage(leftPage);
            }
        }

        public bool IsRightPageChecked
        {
            get => selectedPage == rightPage;
            set
            {
                RaisePropertyChanged(nameof(IsRightPageChecked));
                if (value) SelectPage(rightPage);
            }
        }

        private ObservableCollection<ImageViewModel> images;
        public ObservableCollection<ImageViewModel> Images
        {
            get => images;
            set => Set(nameof(Images), ref images, value);
        }

        private ObservableCollection<SelectableLayoutViewModel> layouts;
        public ObservableCollection<SelectableLayoutViewModel> Layouts
        {
            get => layouts;
            set => Set(nameof(Layouts), ref layouts, value);
        }

        public PagesSettingsViewModel(IDialogService dialogService, ViewModelLocator locator, PhotoBookModel model, ContentPage leftPage, ContentPage rightPage)
        {
            this.dialogService = dialogService;
            this.locator = locator;
            this.model = model;

            ResetPages(leftPage, rightPage);
        }

        public void ResetPages(ContentPage leftPage, ContentPage rightPage)
        {
            this.leftPage = leftPage;
            this.rightPage = rightPage;

            SelectPage(leftPage);
            IsLeftPageChecked = true;
        }

        private void SelectPage(ContentPage page)
        {
            selectedPage = page;
            RefreshPageSettings(page);
        }

        private void RefreshPageSettings(ContentPage page)
        {
            // TODO: Handle BackgroundImage as well
            BackgroundColor = selectedPage.Background as BackgroundColor;

            var newImages = new ObservableCollection<ImageViewModel>();
            for (int imageIndex = 0; imageIndex < page.Layout.NumOfImages; imageIndex++)
            {
                newImages.Add(new ImageViewModel(locator, page, imageIndex));
            }
            Images = newImages;

            Layouts = new ObservableCollection<SelectableLayoutViewModel>();
            foreach (var item in model.AvailableLayouts)
            {
                var layoutVM = new SelectableLayoutViewModel(item.Value, item.Key);
                layoutVM.Selected += OnLayoutSelected;
                if (page.Layout == item.Value) layoutVM.IsChecked = true;
                layouts.Add(layoutVM);
            }
        }

        private void OnLayoutSelected(Layout.Type layoutType)
        {
            var dialogVM = new Dialogs.DialogYesNoViewModel("Zmiana układu strony spowoduje usunięcie zdjęć. Kontynuować?");
            dialogService.OpenDialog(dialogVM);

            if (dialogVM.Result == Dialogs.DialogResult.Yes)
            {
                selectedPage.Layout = model.AvailableLayouts[layoutType];
                RefreshPageSettings(selectedPage);
            }
            else
            {
                foreach (var vm in layouts)
                {
                    vm.IsChecked = vm.Layout == selectedPage.Layout;
                }
            }
        }
    }
}
