using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Backgrounds;
using PhotoBook.Model.Serialization;

namespace PhotoBook.Model.Pages
{
    public class FrontCover : Page, SerializeInterface<FrontCover>
    {
        // In pixels
        public static int FontSize { get; } = 18;
        public string Title { get; set; }

        public FrontCover DeserializeObject()
        {
            throw new NotImplementedException();
        }

        public int SerializeObject(Serializer serializer)
        {
            string frontCover = "";

            frontCover += $"{nameof(FontSize)}:{FontSize}\n";
            frontCover += $"{nameof(Title)}:\"{Title}\"\n";

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

            frontCover += $"Background:&{backgroundID},{resultType}";

            int frontCoverID = serializer.AddObject(frontCover);

            return frontCoverID;
        }

        public override void setBackground(int R = -1, int G = -1, int B = -1, string path = "", int X = -1, int Y =- 1, int Width = -1, int Height = -1)
        {
            if((R == -1 || G == -1 || B == -1) && (path == "" || X == -1 || Y == -1 || Width == -1 || Height == -1))
                throw new Exception("Incorect data sent to setBackground method for FrontCover");

            if (R == -1 || G == -1 || B == -1)
                Background = new BackgroundImage(new Graphics.Image(path, X, Y, Width, Height));
            else
                Background = new BackgroundColor((byte)R, (byte)G, (byte)B);
        }
    }
}
