using PhotoBook.Model.Arrangement;
using PhotoBook.Model.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Pages
{
    class ContentPage : Page
    {
        Layout Layout { get; set; }
        void LoadImage(int layoutImageIndex, string imagePath)
        {

        }
        Image GetImage(int layoutImageIndex)
        {
            return new Image();
        }

        string GetComment(int layoutIndex)
        {
            return "";
        }
        void SetComment(int layoutIndex, string comment)
        {

        }
    }
}
