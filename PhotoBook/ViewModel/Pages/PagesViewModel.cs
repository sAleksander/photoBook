using GalaSoft.MvvmLight;
using PhotoBook.Model.Pages;
using System.ComponentModel;

namespace PhotoBook.ViewModel.Pages
{
    public class PagesViewModel : BookViewModel
    {
        public delegate void BackgroundChangedEventHandler(int pageIndex);
        public event BackgroundChangedEventHandler BackgroundChanged;

        private ContentPage[] contentPages;
        public ContentPage[] ContentPages
        {
            get => contentPages;
            private set => Set(nameof(ContentPages), ref contentPages, value);
        }

        public PagesViewModel(ContentPage[] contentPages)
        {
            ResetPages(contentPages);
        }

        ~PagesViewModel()
        {
            UnregisterEventHandlers();
        }

        public void ResetPages(ContentPage[] contentPages)
        {
            UnregisterEventHandlers();

            ContentPages = contentPages;

            ContentPages[0].PropertyChanged += OnLeftPagePropertyChanged;
            ContentPages[1].PropertyChanged += OnRightPagePropertyChanged;
        }

        public void OnLeftPagePropertyChanged(object s, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(ContentPage.Background)))
            {
                BackgroundChanged?.Invoke(0);
            }
        }

        public void OnRightPagePropertyChanged(object s, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(ContentPage.Background)))
            {
                BackgroundChanged?.Invoke(1);
            }
        }

        private void UnregisterEventHandlers()
        {
            if (contentPages == null) return;

            ContentPages[0].PropertyChanged -= OnLeftPagePropertyChanged;
            ContentPages[1].PropertyChanged -= OnRightPagePropertyChanged;
        }
    }
}
