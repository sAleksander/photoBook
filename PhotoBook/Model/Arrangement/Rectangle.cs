using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Arrangement
{
    class Rectangle
    {
        Rectangle(int x, int y, int width, int height)
        {
            if (width <= 0 || height <= 0)
                throw new Exception("Width & height of a rectangle must be longer than 0!");
            else
            {
                X = x;
                Y = y;
                Width = width;
                Height = height;
            }
        }

        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }
    }
}
