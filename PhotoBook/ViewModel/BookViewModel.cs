using GalaSoft.MvvmLight;
using PhotoBookModel = PhotoBook.Model.PhotoBook;

namespace PhotoBook.ViewModel
{
    public class BookViewModel : ViewModelBase
    {
        private PhotoBookModel model;
        public PhotoBookModel Model
        {
            get => model;
        }

        private Model.Pages.Page[] pages;
        public Model.Pages.Page[] Pages
        {
            get => pages;
            private set => Set(nameof(Pages), ref pages, value);
        }

        private PageType currentPageType = PageType.FrontCover;
        public PageType CurrentPageType
        {
            get => currentPageType;
            private set => Set(nameof(CurrentPageType), ref currentPageType, value);
        }

        public BookViewModel(PhotoBookModel model)
        {
            this.model = model;
        }

        public void SetPages(PageType type, Model.Pages.Page[] pages)
        {
            CurrentPageType = type;
            Pages = pages;
        }
    }
}
