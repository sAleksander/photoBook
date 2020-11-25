using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Arrangement
{
    class Layout
    {
        static int CommentFontSize { get; }
        static int CommentOffsetInPixels { get; }

        string Name { get; }

        int NumOfImages { get; }
        Rectangle[] ImageConstraints { get; }
    }
}
