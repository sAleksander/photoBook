using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Serialization;

namespace PhotoBook.Model.Pages
{
    public class BackCover : Page, SerializeInterface<BackCover>
    {
        public BackCover()
        {
            setBackground(255, 255, 255);
        }

        public override void setBackground(int R = -1, int G = -1, int B = -1, string path = "", int X = -1, int Y = -1, int Width = -1, int Height = -1)
        {
            if ((R == -1 || G == -1 || B == -1) && (path == "" || X == -1 || Y == -1 || Width == -1 || Height == -1))
                throw new Exception("Incorect data sent to setBackground method for FrontCover");

            if (R == -1 || G == -1 || B == -1)
                Background = new BackgroundImage(new Graphics.Image(path, X, Y, Width, Height));
            else
                Background = new BackgroundColor((byte)R, (byte)G, (byte)B);
        }

        public int SerializeObject(Serializer serializer)
        {
            string backCover = "";

            int backgroundID = 0;
            string resultType = "";

            switch (Background)
            {
                case BackgroundColor bgc:
                    backgroundID = bgc.SerializeObject(serializer);
                    resultType = "BackgroundColor";
                    break;

                case BackgroundImage bgi:
                    backgroundID = bgi.SerializeObject(serializer);
                    resultType = "BackgroundImage";
                    break;
            }

            backCover += $"Background:&{backgroundID},{resultType}";

            int backCoverID = serializer.AddObject(backCover);

            return backCoverID;
        }


        public BackCover DeserializeObject(Serializer serializer, int objectID)
        {
            Debug.WriteLine($"backcoverid: {objectID}");

            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

            string backgroundType = objectData.Get<string>(nameof(Background));
            int backgroundIndex = objectData.Get<int>(nameof(Background));

            switch (backgroundType)
            {
                case "BackgroundColor":
                    Background = (Background as BackgroundColor).DeserializeObject(serializer, backgroundIndex);
                    break;
                case "BackgroundImage":
                    Background = (Background as BackgroundImage).DeserializeObject(serializer, backgroundIndex);
                    break;
            }

            return this;
        }
    }
}
