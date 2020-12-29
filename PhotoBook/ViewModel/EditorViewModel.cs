using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
        private PhotoBookModel model = PhotoBookModel.CreateMockup();
        private ViewModelLocator locator;

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

        public EditorViewModel(ViewModelLocator locator)
        {
            this.locator = locator;
            bookViewModel = new BookViewModel(model);

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

                        if (model.NumOfContentPages == 0)
                        {
                            currentPageType = PageType.FrontCover;
                            currentContentPageIndex = 0;
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
                        model.CreateNewPages(currentContentPageIndex);
                        UpdateView();
                    },
                    () => currentPageType != PageType.BackCover));
            }
        }

        private void UpdateView()
        {
            DeletePages.RaiseCanExecuteChanged();
            InsertPages.RaiseCanExecuteChanged();

            switch (currentPageType)
            {
                case PageType.FrontCover:
                    bookViewModel.SetPages(currentPageType, new Page[]
                    {
                        model.FrontCover
                    });
                    SettingsViewModel = new FrontCoverSettingsViewModel(model.FrontCover, model.BackCover);
                    break;
                case PageType.Content:
                    var (leftPage, rightPage) = model.GetContentPagesAt(currentContentPageIndex);
                    bookViewModel.SetPages(currentPageType, new Page[]
                    {
                        leftPage, rightPage
                    });

                    // TODO: This is a really stupid fix for view not updating
                    //       its Content property when SettingsViewModel of the same
                    //       type changes. Is there a better way to do it?
                    switch (SettingsViewModel)
                    {
                        case PagesSettingsViewModel pagesSettings:
                            pagesSettings.ResetPages(leftPage, rightPage);
                            break;
                        default:
                            SettingsViewModel = new PagesSettingsViewModel(leftPage, rightPage);
                            break;
                    }
                    break;
                case PageType.BackCover:
                    bookViewModel.SetPages(currentPageType, new Page[]
                    {
                        model.BackCover
                    });
                    SettingsViewModel = new BackCoverSettingsViewModel(model);
                    break;
            }
        }

        public RelayCommand Exit => new RelayCommand(() =>
        {
            MainViewModel.Navigator.ChangeCurrentVM(locator.Home);
            locator.DestroyViewModel<EditorViewModel>();
        });
    }
}
