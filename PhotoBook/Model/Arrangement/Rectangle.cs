using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Serialization;

namespace PhotoBook.Model.Arrangement
{
    public class Rectangle : SerializeInterface<Rectangle>
    {
        public Rectangle(int x, int y, int width, int height)
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

        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public Rectangle DeserializeObject()
        {
            throw new NotImplementedException();
        }

        public int SerializeObject(Serializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
