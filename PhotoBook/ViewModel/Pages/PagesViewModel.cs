using PhotoBook.Model.Pages;
using System;
using System.ComponentModel;
using System.Linq;
using static PhotoBook.Model.Pages.ContentPage;
using PhotoBook.Model.Graphics;

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
        private PropertyChangedEventHandler[][] imagePropertyChangedHandlers = new PropertyChangedEventHandler[2][];
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

                imageChangedHandlers[i] = (layoutIndex) => OnImageChanged(curPageIndex, layoutIndex);
                ContentPages[i].ImageChanged += imageChangedHandlers[i];

                commentChangedHandlers[i] = (layoutIndex) => CommentChanged?.Invoke(curPageIndex, layoutIndex);
                ContentPages[i].CommentChanged += commentChangedHandlers[i];

                RegisterImagePropertyChanged(i);
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

                RegisterImagePropertyChanged(pageIndex);
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

                UnregisterImagePropertyChanged(i);
            }
        }

        private void OnImageChanged(int pageIndex, int layoutIndex)
        {
            ImageChanged?.Invoke(pageIndex, layoutIndex);
            RegisterImagePropertyChanged(pageIndex, layoutIndex);
        }

        private void RegisterImagePropertyChanged(int pageIndex)
        {
            var contentPage = ContentPages[pageIndex];
            var numOfImages = contentPage.Layout.NumOfImages;
            imagePropertyChangedHandlers[pageIndex] = new PropertyChangedEventHandler[numOfImages];

            for (int i = 0; i < numOfImages; i++)
            {
                RegisterImagePropertyChanged(pageIndex, i);
            }
        }

        private void RegisterImagePropertyChanged(int pageIndex, int layoutIndex)
        {
            var contentPage = ContentPages[pageIndex];
            var image = contentPage.GetImage(layoutIndex);

            if (image == null) return;

            PropertyChangedEventHandler handler = (s, args) => OnImagePropertyChanged(
                pageIndex, layoutIndex, args.PropertyName);

            imagePropertyChangedHandlers[pageIndex][layoutIndex] = handler;
            image.PropertyChanged += handler;
        }

        private void UnregisterImagePropertyChanged(int pageIndex)
        {
            var contentPage = ContentPages[pageIndex];
            var numOfImages = contentPage.Layout.NumOfImages;
            imagePropertyChangedHandlers[pageIndex] = new PropertyChangedEventHandler[numOfImages];

            for (int imgIndex = 0; imgIndex < numOfImages; imgIndex++)
            {
                var handler = imagePropertyChangedHandlers[pageIndex][imgIndex];

                if (handler != null)
                {
                    contentPage.GetImage(imgIndex).PropertyChanged -= handler;
                }
            }

            imagePropertyChangedHandlers[pageIndex] = null;
        }

        private void OnImagePropertyChanged(int pageIndex, int imageIndex, string propertyName)
        {
            if (propertyName.Equals(nameof(Image.DisplayedPath)))
            {
                ImageChanged?.Invoke(pageIndex, imageIndex);
            }
        }
    }
}
