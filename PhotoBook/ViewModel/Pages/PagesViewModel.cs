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

        private EventHandler[] pageBackgroundChangedHandlers = new EventHandler[2];
        private EventHandler[] pageLayoutChangedHandlers = new EventHandler[2];
        private EventHandler[][] imagePropertyChangedHandlers = new EventHandler[2][];
        private EventHandler<ImageChangedEventArgs>[] imageChangedHandlers = new EventHandler<ImageChangedEventArgs>[2];
        private EventHandler<CommentChangedEventArgs>[] commentChangedHandlers = new EventHandler<CommentChangedEventArgs>[2];

        public PagesViewModel(ContentPage[] contentPages)
        {
            ResetPages(contentPages);
        }

        public void ResetPages(ContentPage[] contentPages)
        {
            ContentPages = contentPages;

            for (int i = 0; i < 2; i++)
            {
                var curPageIndex = i;

                pageBackgroundChangedHandlers[i] = (s, args) => BackgroundChanged?.Invoke(curPageIndex);
                ContentPages[i].BackgroundChanged.Add(pageBackgroundChangedHandlers[i]);

                pageLayoutChangedHandlers[i] = (s, args) =>
                {
                    Redraw?.Invoke(curPageIndex);
                    RegisterImagePropertyChanged(curPageIndex);
                };
                ContentPages[i].LayoutChanged.Add(pageLayoutChangedHandlers[i]);

                imageChangedHandlers[i] = (s, args) => OnImageChanged(curPageIndex, args.LayoutIndex);
                ContentPages[i].ImageChanged.Add(imageChangedHandlers[i]);

                commentChangedHandlers[i] = (s, args) => CommentChanged?.Invoke(curPageIndex, args.LayoutIndex);
                ContentPages[i].CommentChanged.Add(commentChangedHandlers[i]);

                RegisterImagePropertyChanged(i);
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
            imagePropertyChangedHandlers[pageIndex] = new EventHandler[numOfImages];

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

            imagePropertyChangedHandlers[pageIndex][layoutIndex] =
                (s, args) => ImageChanged?.Invoke(pageIndex, layoutIndex);
            image.DisplayPathChanged.Add(imagePropertyChangedHandlers[pageIndex][layoutIndex]);
        }
    }
}
