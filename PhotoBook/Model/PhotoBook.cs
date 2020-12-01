using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model
{
    class PhotoBook
    {
        public static string Font { get; } = "Arial";

        public static int PageWidthInPixels { get; }
        public static int PageHeightInPixels { get; }


        private List<Page> _contentPages;
        List<Page> ContentPages
        {
            get => _contentPages;
            set { _contentPages = value; }
        }

        public Page FrontCover { get; }
        public Page BackCover { get; }
        public int NumOfPages { get => ContentPages.Count; }

        public Layout[] AvailableLayouts { get; } = Layout.CreateAvailableLayouts();

        public (Page, Page) GetPageAt(int index)
        {
            if (index == -1)
                return (_contentPages[NumOfPages - 2], _contentPages[NumOfPages - 1]);

            else if (index <= 0 && index > _contentPages.Count)
            {
                byte adjustedIndex = (byte)index;

                if (index % 2 == 1)
                    adjustedIndex = (byte)(index - 1);

                return (_contentPages[adjustedIndex], _contentPages[adjustedIndex + 1]);
            }

            else
                throw new Exception("Wrong page index chosen!");
        }

        public void CreateNewPages(int index = -1)
        {
            if (index == -1){
                _contentPages.Add(new ContentPage());
                _contentPages.Add(new ContentPage());
            }

            else if (index <= 0 && index > _contentPages.Count){
                // Provides inserting pages between sheets & not pages
                byte adjustedIndex = (byte)index;

                if(index % 2 == 1)
                    adjustedIndex = (byte)(index - 1);

                _contentPages.Insert(adjustedIndex, new ContentPage());
                _contentPages.Insert(adjustedIndex, new ContentPage());

                // Remind about changes here
            }
        }
    }
}