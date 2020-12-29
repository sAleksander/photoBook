using GalaSoft.MvvmLight;

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

        public FrontCoverViewModel(Model.Pages.FrontCover frontCover)
        {
            FrontCover = frontCover;
        }

        public void OnTitleChanged()
        {
            RaisePropertyChanged(nameof(Title));
        }
    }
}
