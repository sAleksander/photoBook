using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoBook.Model.Pages;
using System.Collections.ObjectModel;
using PhotoBook.Model.Graphics;
using System.Collections.Generic;
using System;
using PhotoBook.Services;
using PhotoBook.ViewModel.Dialogs;

namespace PhotoBook.ViewModel.Settings
{
    public class ImageViewModel : ViewModelBase
    {
        private ViewModelLocator locator;
        private IDialogService dialogService;

        private ContentPage page;

        private int imageIndex;
        public int ImageIndex { get => imageIndex; }

        private string description = "";
        public string Description
        {
            get => description;
            set => Set(nameof(Description), ref description, value);
        }

        public string ImageFilter { get; } = "Image Files|*.png;*.jpg";

        private string chosenFilePath;
        public string ChosenFilePath
        {
            get => chosenFilePath;
            set => Set(nameof(ChosenFilePath), ref chosenFilePath, value);
        }

        private Dictionary<Filter.Type, string> filterTypeToName = new Dictionary<Filter.Type, string>()
        {
            { Filter.Type.None, "Brak" },
            { Filter.Type.Warm, "Ciepły" },
            { Filter.Type.Cold, "Zimny" },
            { Filter.Type.Greyscale, "Czarno-biały" }
        };

        private ObservableCollection<SelectableFilterViewModel> filters;
        public ObservableCollection<SelectableFilterViewModel> Filters
        {
            get => filters;
            set => Set(nameof(Filters), ref filters, value);
        }

        private RelayCommand fileChosen;
        public RelayCommand FileChosen
        {
            get
            {
                return fileChosen ?? (fileChosen = new RelayCommand(
                    () =>
                    {
                        try
                        {
                            page.LoadImage(imageIndex, ChosenFilePath);
                        }
                        catch (FailedToLoadImageException e)
                        {
                            dialogService.OpenDialog(
                                new DialogOKViewModel("Błąd: nieobsługiwany format zdjęcia")
                            );
                            return;
                        }

                        var cropPhotoVM = locator.CropPhoto;
                        cropPhotoVM.ImageToCrop = page.GetImage(imageIndex);

                        BuildFilters();

                        MainViewModel.Navigator.ChangeCurrentVM(cropPhotoVM);
                    }));
            }
        }

        public RelayCommand ApplyDescription => new RelayCommand(() =>
        {
            Description = Description.Trim();
            page.SetComment(imageIndex, Description);
        });

        public ImageViewModel(
            ViewModelLocator locator,
            IDialogService dialogService,
            ContentPage page,
            int imageIndex)
        {
            this.locator = locator;
            this.dialogService = dialogService;
            this.page = page;
            this.imageIndex = imageIndex;

            Description = page.GetComment(imageIndex);
            BuildFilters();
        }

        private void BuildFilters()
        {
            var image = page.GetImage(ImageIndex);

            if (image == null)
                return;

            var currentFilterType = image.CurrentFilter.currentType;

            filters = new ObservableCollection<SelectableFilterViewModel>();

            foreach (var item in filterTypeToName)
            {
                var filterType = item.Key;
                var filterName = filterTypeToName[filterType];
                var filterVM = new SelectableFilterViewModel(page, ImageIndex, filterType, filterName);
                filterVM.IsChecked = filterType == currentFilterType;

                filters.Add(filterVM);
            }
        }
    }
}
