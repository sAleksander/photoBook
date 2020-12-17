using GalaSoft.MvvmLight;
using PhotoBook.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.ViewModel
{
    public class BookViewModel : ViewModelBase
    {
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

        public void SetPages(PageType type, Model.Pages.Page[] pages)
        {
            CurrentPageType = type;
            Pages = pages;
        }
    }
}
