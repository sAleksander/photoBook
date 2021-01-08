using PhotoBook.Model.Pages;
using System;
using System.ComponentModel;
using static PhotoBook.Model.Pages.ContentPage;

namespace PhotoBook.ViewModel.Pages
{
    public class PagesViewModel : BookViewModel
    {
        public delegate void RedrawEventHandler(int pageIndex);
        public event RedrawEventHandler Redraw;

        public delegate void BackgroundChangedEventHandler(int pageIndex);
        public event BackgroundChangedEventHandler BackgroundChanged;

        public delegate void PageImageChangedEventHandler(int pageIndex, int layoutIndex);
        public event PageImageChangedEventHandler ImageChanged;

        public delegate void PageCommentChangedEventHandler(int pageIndex, int layoutIndex);
        public event PageCommentChangedEventHandler CommentChanged;

        private ContentPage[] contentPages;
        public ContentPage[] ContentPages
        {
            get => contentPages;
            private set => Set(nameof(ContentPages), ref contentPages, value);
        }

        private PropertyChangedEventHandler[] pagePropertyChangedHandlers = new PropertyChangedEventHandler[2];
        private ImageChangedEventHandler[] imageChangedHandlers = new ImageChangedEventHandler[2];
        private CommentChangedEventHandler[] commentChangedHandlers = new CommentChangedEventHandler[2];

        public PagesViewModel(ContentPage[] contentPages)
        {
            ResetPages(contentPages);
        }

        public void ResetPages(ContentPage[] contentPages)
        {
            UnregisterEventHandlers();

            ContentPages = contentPages;

            for (int i = 0; i < 2; i++)
            {
                var curPageIndex = i;

                pagePropertyChangedHandlers[i] = (s, args) => OnPagePropertyChanged(curPageIndex, args.PropertyName);
                ContentPages[i].PropertyChanged += pagePropertyChangedHandlers[i];

                imageChangedHandlers[i] = (layoutIndex) => ImageChanged?.Invoke(curPageIndex, layoutIndex);
                ContentPages[i].ImageChanged += imageChangedHandlers[i];

                commentChangedHandlers[i] = (layoutIndex) => CommentChanged?.Invoke(curPageIndex, layoutIndex);
                ContentPages[i].CommentChanged += commentChangedHandlers[i];
            }
            
        }

        public void OnPagePropertyChanged(int pageIndex, string propertyName)
        {
            if (propertyName.Equals(nameof(ContentPage.Background)))
            {
                BackgroundChanged?.Invoke(pageIndex);
            }
            else if (propertyName.Equals(nameof(ContentPage.Layout)))
            {
                Redraw?.Invoke(pageIndex);
            }
        }

        public void OnRightPagePropertyChanged(object s, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals(nameof(ContentPage.Background)))
            {
                BackgroundChanged?.Invoke(1);
            }
        }

        public override void UnregisterEventHandlers()
        {
            if (contentPages == null) return;

            for (int i = 0; i < 2; i++)
            {
                ContentPages[i].PropertyChanged -= pagePropertyChangedHandlers[i];
                ContentPages[i].ImageChanged -= imageChangedHandlers[i];
                ContentPages[i].CommentChanged -= commentChangedHandlers[i];
            }
        }
    }
}
