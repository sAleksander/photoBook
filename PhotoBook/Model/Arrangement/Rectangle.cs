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

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Rectangle DeserializeObject(Serializer serializer, int objectID)
        {
            string rectangleData = serializer.GetObjectData(objectID);

            int attributeIndex = rectangleData.IndexOf($"{nameof(X)}");
            int dividerIndex = rectangleData.IndexOf(':', attributeIndex);
            int endOfLineIndex = rectangleData.IndexOf('\n', dividerIndex);

            X = int.Parse(rectangleData.Substring(dividerIndex + 1, endOfLineIndex));

            attributeIndex = rectangleData.IndexOf($"{nameof(Y)}");
            dividerIndex = rectangleData.IndexOf(':', attributeIndex);
            endOfLineIndex = rectangleData.IndexOf('\n', dividerIndex);

            Y = int.Parse(rectangleData.Substring(dividerIndex + 1, endOfLineIndex));

            attributeIndex = rectangleData.IndexOf($"{nameof(Width)}");
            dividerIndex = rectangleData.IndexOf(':', attributeIndex);
            endOfLineIndex = rectangleData.IndexOf('\n', dividerIndex);

            Width = int.Parse(rectangleData.Substring(dividerIndex + 1, endOfLineIndex));

            attributeIndex = rectangleData.IndexOf($"{nameof(Height)}");
            dividerIndex = rectangleData.IndexOf(':', attributeIndex);
            endOfLineIndex = rectangleData.IndexOf('\n', dividerIndex);

            Height = int.Parse(rectangleData.Substring(dividerIndex + 1, endOfLineIndex));


            return this;
        }

        public int SerializeObject(Serializer serializer)
        {
            string rectangle = $"{nameof(X)}:{X}\n";
            rectangle += $"{nameof(Y)}:{Y}\n";
            rectangle += $"{nameof(Width)}:{Width}\n";
            rectangle += $"{nameof(Height)}:{Height}\n";

            int rectangleID = serializer.AddObject(rectangle);

            return rectangleID;
        }
    }
}
