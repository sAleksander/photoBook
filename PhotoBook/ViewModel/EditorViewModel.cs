using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoBook.Model.Arrangement;
using PhotoBook.Services;
using PhotoBook.ViewModel.Pages;
using PhotoBook.ViewModel.Settings;
using Page = PhotoBook.Model.Pages.Page;
using PhotoBookModel = PhotoBook.Model.PhotoBook;

namespace PhotoBook.ViewModel
{
    public enum PageType
    {
        FrontCover,
        Content,
        BackCover
    }

    public class EditorViewModel : ViewModelBase
    {
        private PhotoBookModel model;
        private IDialogService dialogService;
        private ViewModelLocator locator;
        private PhotoBookProviderService photoBookProvider;

        private int currentContentPageIndex = 0;
        private PageType currentPageType = PageType.FrontCover;

        // Nested view models
        private BookViewModel bookViewModel;
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

        public EditorViewModel(
            ViewModelLocator locator,
            IDialogService dialogService,
            PhotoBookProviderService photoBookProvider)
        {
            this.locator = locator;
            this.dialogService = dialogService;
            this.photoBookProvider = photoBookProvider;
            this.model = this.photoBookProvider.Model;

            UpdateView();
        }

        public RelayCommand NextPage => new RelayCommand(() =>
        {
            switch (currentPageType)
            {
                case PageType.FrontCover:
                    if (model.NumOfContentPages == 0)
                    {
                        currentPageType = PageType.BackCover;
                    }
                    else
                    {
                        currentPageType = PageType.Content;
                        currentContentPageIndex = 0;
                    }
                    break;
                case PageType.Content:
                    if (currentContentPageIndex + 2 < model.NumOfContentPages)
                        currentContentPageIndex += 2;
                    else
                        currentPageType = PageType.BackCover;
                    break;
                case PageType.BackCover: return;
            }

            UpdateView();
        });

        public RelayCommand PreviousPage => new RelayCommand(() =>
        {
            switch (currentPageType)
            {
                case PageType.FrontCover: return;
                case PageType.Content:
                    if (currentContentPageIndex - 2 >= 0)
                        currentContentPageIndex -= 2;
                    else
                        currentPageType = PageType.FrontCover;
                    break;
                case PageType.BackCover:
                    if (model.NumOfContentPages == 0)
                    {
                        currentPageType = PageType.FrontCover;
                    }
                    else
                    {
                        currentPageType = PageType.Content;
                        currentContentPageIndex = model.NumOfContentPages - 2;
                    }
                    break;
            }

            UpdateView();
        });

        private RelayCommand deletePages;
        public RelayCommand DeletePages
        {
            get
            {
                return deletePages ?? (deletePages = new RelayCommand(
                    () =>
                    {
                        model.DeletePages(currentContentPageIndex);

                        if (currentContentPageIndex >= model.NumOfContentPages)
                        {
                            currentPageType = PageType.BackCover;
                        }

                        UpdateView();
                    },
                    () => currentPageType == PageType.Content));
            }
        }

        private RelayCommand insertPages;
        public RelayCommand InsertPages
        {
            get
            {
                return insertPages ?? (insertPages = new RelayCommand(
                    () =>
                    {
                        Model.Pages.ContentPage left, right;

                        switch (currentPageType)
                        {
                            case PageType.FrontCover:
                                currentPageType = PageType.Content;
                                currentContentPageIndex = 0;

                                (left, right) = model.CreateNewPages(0);
                                break;
                            case PageType.Content:
                                (left, right) = model.CreateNewPages(currentContentPageIndex);
                                break;
                            case PageType.BackCover:
                                currentPageType = PageType.Content;
                                currentContentPageIndex = model.NumOfContentPages;

                                (left, right) = model.CreateNewPages(model.NumOfContentPages);
                                break;
                            default: throw new System.Exception("Unreachable");
                        }

                        left.Layout = model.AvailableLayouts[Layout.Type.TwoPictures];
                        right.Layout = model.AvailableLayouts[Layout.Type.TwoPictures];

                        UpdateView();
                    }));
            }
        }

        private RelayCommand save;
        public RelayCommand Save
        {
            get
            {
                return save ?? (save = new RelayCommand(
                    () =>
                    {
                        model.SavePhotoBook();
                    }));
            }
        }

        private RelayCommand load;
        public RelayCommand Load
        {
            get
            {
                return load ?? (load = new RelayCommand(
                    () =>
                    {
                        model.LoadPhotoBook();
                        UpdateView();
                    }));
            }
        }

        private void UpdateView()
        {
            DeletePages.RaiseCanExecuteChanged();
            InsertPages.RaiseCanExecuteChanged();

            switch (currentPageType)
            {
                case PageType.FrontCover:
                    CreateFrontCoverViewModels();
                    break;
                case PageType.Content:
                    CreateContentViewModels();
                    break;
                case PageType.BackCover:
                    CreateBackCoverViewModels();
                    break;
            }
        }

        private void CreateFrontCoverViewModels()
        {
            var bookVM = new FrontCoverViewModel(model.FrontCover);
            var settingsVM = new FrontCoverSettingsViewModel(model.FrontCover, model.BackCover);

            SettingsViewModel = settingsVM;
            BookViewModel = bookVM;
        }

        private void CreateContentViewModels()
        {
            var (leftPage, rightPage) = model.GetContentPagesAt(currentContentPageIndex);
            var contentPages = new Model.Pages.ContentPage[] { leftPage, rightPage };

            SettingsViewModel = new PagesSettingsViewModel(dialogService, locator, model, leftPage, rightPage);
            BookViewModel = new PagesViewModel(contentPages);
        }

        private void CreateBackCoverViewModels()
        {
            var bookVM = new BackCoverViewModel(model.BackCover);
            var settingsVM = new BackCoverSettingsViewModel(model);

            SettingsViewModel = settingsVM;
            BookViewModel = bookVM;
        }

        public RelayCommand Exit => new RelayCommand(() =>
        {
            MainViewModel.Navigator.ChangeCurrentVM(locator.Home);
            locator.DestroyViewModel<EditorViewModel>();
        });
    }
}
