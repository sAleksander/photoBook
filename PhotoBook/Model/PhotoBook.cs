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
        // TODO: Find ways to implement that
        static string Font { get; } = "Arial";

        static int PageWidthInPixels { get; }
        static int PageHeightInPixels { get; }

        // TODO: Think in what way the pages should be stored
        private List<Page> _contentPages;
        List<Page> ContentPages
        {
            get => _contentPages;            
            set
            {
                _contentPages = value;
            }
        }

        Page FrontCover { get; }
        Page BackCover { get; }
        int NumOfPages { get => ContentPages.Count; }        

        (Page, Page) GetPageAt(int index)
        {
            // TODO: Think in what way should the index be different when calling for FrontCover & first ContentPage
            // TODO: Implement retrieving CoverPage

            if (index == -1)
            {
                // TODO: Implement this so it return the BackCover and "blank page"
                return (BackCover, new BackCover());
            }
            else if (index <= 0 && index > _contentPages.Count)
            {
                byte adjustedIndex = (byte)index;

                if (index % 2 == 1)
                    adjustedIndex = (byte)(index - 1);

                return (_contentPages[adjustedIndex], _contentPages[adjustedIndex + 1]);
            }
            else if (true)
            {
                // TODO: Adjust the condition above for the CoverPage to be returned
                return (new FrontCover(), FrontCover);
            }
            else            
                throw new Exception("Wrong page index chosen!");            
        }

        void CreateNewPages(int index = -1)
        {
            // Think about whether this approach will change if GetPageAt will change
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