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
        static string Font
        {
            get;
        }

        static int PageWidthInPixels
        {
            get;
        }
        static int PageHeightInPixels
        {
            get;
        }

        // TODO: Think in what way the pages should be stored
        /*private Page[] _allPages;
        Page[] AllPages
        {
            get 
            {
                return _allPages;
            }
            set
            {
                // TODO: Add proper modyfing conditions
                _allPages = value;
            }
        }*/

        Page FrontCover { get; }
        Page BackCover { get; }
        int NumOfPages { get; }        

        (Page, Page) GetPageAt(int index)
        {
            return (new BackCover(), new BackCover());
        }

        void CreateNewPages(int index = -1)
        {

        }
        Layout[] AvailableLayouts // ?? Should it return all available layouts in general ??
        {
            get;
        }

    }
}
