using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Backgrounds
{
    class BackgroundColor : Background
    {
        BackgroundColor()
        {
            R = 0;
            G = 0;
            B = 0;
        }

        BackgroundColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        byte R { get; set; }
        byte G { get; set; }
        byte B { get; set; }
    }
}
