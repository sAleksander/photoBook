using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace PhotoBook.Model.Helpers
{
    static class FontAdjuster
    {
        public static void AdjustFont(Label label, int R, int G, int B)
        {
            if ((R + G + B) > 382) label.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            else label.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
