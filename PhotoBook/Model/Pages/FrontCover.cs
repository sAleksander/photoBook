using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Pages
{
    public class FrontCover : Page
    {
        // In pixels
        public static int FontSize { get; } = 64;
        public string Title { get; set; }
    }
}
