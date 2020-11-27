using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Pages
{
    class FrontCover : Page
    {
        // In pixels
        static int FontSize { get; } = 18;
        string Title { get; set; }
    }
}
