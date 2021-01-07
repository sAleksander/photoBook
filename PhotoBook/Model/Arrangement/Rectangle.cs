using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Rectangle DeserializeObject(Serializer serializer, int objectID)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

            Debug.WriteLine(objectData == null);

            X = objectData.Get<int>(nameof(X));
            Y = objectData.Get<int>(nameof(Y));
            Width = objectData.Get<int>(nameof(Width));
            Height = objectData.Get<int>(nameof(Height));

            return this;
        }

        public int SerializeObject(Serializer serializer)
        {
            string rectangle = $"{nameof(X)}:{X}\n";
            rectangle += $"{nameof(Y)}:{Y}\n";
            rectangle += $"{nameof(Width)}:{Width}\n";
            rectangle += $"{nameof(Height)}:{Height}";

            int rectangleID = serializer.AddObject(rectangle);

            return rectangleID;
        }
    }
}
