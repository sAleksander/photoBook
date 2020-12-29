using GalaSoft.MvvmLight;

namespace PhotoBook.ViewModel.Pages
{
    public class BackCoverViewModel : BookViewModel
    {
        private Model.Pages.BackCover backCover;
        public Model.Pages.BackCover BackCover
        {
            get => backCover;
            private set => Set(nameof(BackCover), ref backCover, value);
        }

        public BackCoverViewModel(Model.Pages.BackCover backCover)
        {
            BackCover = backCover;
        }
    }
}
