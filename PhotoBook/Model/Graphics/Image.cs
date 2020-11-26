using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace PhotoBook.Model.Graphics
{
    class Image
    {
        Rectangle CroppingRectangle { get; set; }
        string OriginalPath { get; }
        string DisplayedPath { get; }
        int Width { get; } // TODO: Think whether it will even be required
        int Height { get; } // TODO: Think whether it will even be required

        Filter.Type CurrentFilter { get; }
        void SetFilter(Filter.Type filterType)
        {
            // TODO: Process the original picture and save it as a displayed picture + save it in project resources

        }
    }
}