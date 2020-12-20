using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Backgrounds;

namespace PhotoBook.Model.Pages
{
    public class FrontCover : Page
    {
        // In pixels
        public static int FontSize { get; } = 18;
        public string Title { get; set; }
        public Background Background { get; set; }
    }
}
