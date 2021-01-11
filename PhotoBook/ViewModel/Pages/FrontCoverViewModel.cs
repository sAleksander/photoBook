using PhotoBook.Model.Backgrounds;
using System;
using System.ComponentModel;

namespace PhotoBook.ViewModel.Pages
{
    public class FrontCoverViewModel : BookViewModel
    {
        private Model.Pages.FrontCover frontCover;
        public Model.Pages.FrontCover FrontCover
        {
            get => frontCover;
            private set => Set(nameof(FrontCover), ref frontCover, value);
        }

        public string Title => FrontCover.Title;
        // TODO: Handle BackgroundImage as well
        public BackgroundColor Background => FrontCover.Background as BackgroundColor;

        public FrontCoverViewModel(Model.Pages.FrontCover frontCover)
        {
            FrontCover = frontCover;

            FrontCover.BackgroundChanged.Add(OnFrontCoverBackgroundChanged);
            FrontCover.TitleChanged.Add(OnFrontCoverTitleChanged);
        }

        public void OnFrontCoverBackgroundChanged(object s, EventArgs args)
        {
            RaisePropertyChanged(nameof(FrontCover.Background));
        }

        public void OnFrontCoverTitleChanged(object s, EventArgs args)
        {
            RaisePropertyChanged(nameof(FrontCover.Title));
        }
    }
}
