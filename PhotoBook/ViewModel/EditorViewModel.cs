using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PhotoBook.Model.Exporters;
using PhotoBook.ViewModel.Settings;
using System.Collections.Generic;
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
        private PhotoBookModel model = new PhotoBookModel();

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

        public EditorViewModel()
        {
            bookViewModel = new BookViewModel(model);

            NotifyNestedViewModels();

        }

        private string fileNameExporter(string path)
        {
            string[] tmp = path.Split('\\');
            return tmp[tmp.Length - 1];
        }

        // Page related commands

        public RelayCommand ExportToPdf => new RelayCommand(() =>
        {

            ToPdf ob = new ToPdf();

            switch (model.FrontCover.Background)
            {
                case Model.Backgrounds.BackgroundColor bgColor:
                    ob.CreateFrontCover(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B), model.FrontCover.Title);
                    break;
                case Model.Backgrounds.BackgroundImage bgImage:
                    ob.CreateFrontCover(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)), model.FrontCover.Title);
                    break;
            }

            for (int i = 0; i < model.NumOfContentPages; i++)
            {
                List<string> photos = new List<string>();
                List<string> descriptions = new List<string>();

                for (int j = 0; j < model.GetContentPagesAt(i).Item1.Layout.NumOfImages; j++)
                {
                    photos.Add(fileNameExporter(model.GetContentPagesAt(i).Item1.GetImage(j).DisplayedPath));
                    descriptions.Add(model.GetContentPagesAt(i).Item1.GetComment(j));
                }

                switch (model.GetContentPagesAt(i).Item1.Background)
                {
                    case Model.Backgrounds.BackgroundColor bgColor:
                        ob.CreatePage(photos, descriptions, ToPdf.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                        break;
                    case Model.Backgrounds.BackgroundImage bgImage:
                        ob.CreatePage(photos, descriptions, ToPdf.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                        break;
                }

                photos = new List<string>();
                descriptions = new List<string>();

                for (int j = 0; j < model.GetContentPagesAt(i).Item2.Layout.NumOfImages; j++)
                {
                    photos.Add(fileNameExporter(model.GetContentPagesAt(i).Item2.GetImage(j).DisplayedPath));
                    descriptions.Add(model.GetContentPagesAt(i).Item2.GetComment(j));
                }

                switch (model.GetContentPagesAt(i).Item2.Background)
                {
                    case Model.Backgrounds.BackgroundColor bgColor:
                        ob.CreatePage(photos, descriptions, ToPdf.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                        break;
                    case Model.Backgrounds.BackgroundImage bgImage:
                        ob.CreatePage(photos, descriptions, ToPdf.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                        break;
                }
            }

            switch (model.BackCover.Background)
            {
                case Model.Backgrounds.BackgroundColor bgColor:
                    ob.CreateBackCover(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                    break;
                case Model.Backgrounds.BackgroundImage bgImage:
                    ob.CreateBackCover(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                    break;
            }

            ob.GeneratePdf();
        });

        public RelayCommand ExportToHtml => new RelayCommand(() =>
        {
            ToHtml ob = new ToHtml(model.NumOfContentPages);

            string fileNameExporter(string path)
            {
                string[] tmp = path.Split('\\');
                return tmp[tmp.Length - 1];
            }

            switch (model.FrontCover.Background)
            {
                case Model.Backgrounds.BackgroundColor bgColor:
                    ob.CreateFrontCover(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B), model.FrontCover.Title);
                    break;
                case Model.Backgrounds.BackgroundImage bgImage:
                    ob.CreateFrontCover(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)), model.FrontCover.Title);
                    break;
            }

            for (int i = 0; i < model.NumOfContentPages; i++)
            {
                ToHtml.Page left = new ToHtml.Page(ToHtml.CssBackground(0, 0, 0));

                switch (model.GetContentPagesAt(i).Item1.Background)
                {
                    case Model.Backgrounds.BackgroundColor bgColor:
                        left = new ToHtml.Page(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                        break;
                    case Model.Backgrounds.BackgroundImage bgImage:
                        left = new ToHtml.Page(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                        break;
                }

                for (int j = 0; j < model.GetContentPagesAt(i).Item1.Layout.NumOfImages; j++)
                {
                    left.AddPhotoWithDescription(
                        fileNameExporter(model.GetContentPagesAt(i).Item1.GetImage(j).DisplayedPath),
                        model.GetContentPagesAt(i).Item1.GetComment(j)
                        );
                }

                ToHtml.Page right = new ToHtml.Page(ToHtml.CssBackground(0, 0, 0));

                switch (model.GetContentPagesAt(i).Item2.Background)
                {
                    case Model.Backgrounds.BackgroundColor bgColor:
                        right = new ToHtml.Page(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                        break;
                    case Model.Backgrounds.BackgroundImage bgImage:
                        right = new ToHtml.Page(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                        break;
                }

                for (int j = 0; j < model.GetContentPagesAt(i).Item2.Layout.NumOfImages; j++)
                {
                    right.AddPhotoWithDescription(
                        fileNameExporter(model.GetContentPagesAt(i).Item2.GetImage(j).DisplayedPath),
                        model.GetContentPagesAt(i).Item2.GetComment(j)
                        );
                }

                ob.AddBookPages(left, right);
            }

            switch (model.BackCover.Background)
            {
                case Model.Backgrounds.BackgroundColor bgColor:
                    ob.CreateBackCover(ToHtml.CssBackground(bgColor.R, bgColor.G, bgColor.B));
                    break;
                case Model.Backgrounds.BackgroundImage bgImage:
                    ob.CreateBackCover(ToHtml.CssBackground(fileNameExporter(bgImage.Image.DisplayedPath)));
                    break;
            }
        });

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

            NotifyNestedViewModels();
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

            NotifyNestedViewModels();
        });

        private void NotifyNestedViewModels()
        {
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
                    SettingsViewModel = new PagesSettingsViewModel(leftPage, rightPage);
                    break;
                case PageType.BackCover:
                    bookViewModel.SetPages(currentPageType, new Page[]
                    {
                        model.BackCover
                    });
                    SettingsViewModel = new BackCoverSettingsViewModel(model.BackCover);
                    break;
            }
        }

        public RelayCommand Exit => new RelayCommand(() =>
        {
            MainViewModel.Navigator.ChangeCurrentVM<HomeViewModel>();
        });

        // Debug commands
        public RelayCommand ShowFrontCoverSettings => new RelayCommand(() =>
        {
            SettingsViewModel = new FrontCoverSettingsViewModel(model.FrontCover, model.BackCover);
        });

        public RelayCommand ShowBackCoverSettings => new RelayCommand(() =>
        {
            SettingsViewModel = new BackCoverSettingsViewModel(model.BackCover);
        });

        public RelayCommand ShowPagesSettings => new RelayCommand(() =>
        {
            var (leftPage, rightPage) = model.GetContentPagesAt(currentContentPageIndex);
            SettingsViewModel = new PagesSettingsViewModel(leftPage, rightPage);
        });
    }
}
