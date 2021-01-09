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
        public static int FontSize { get; private set; } = 64;

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                InvokePropertyChanged(nameof(Title));
            }
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


        public FrontCover DeserializeObject(Serializer serializer, int objectID)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);
            FontSize = objectData.Get<int>(nameof(FontSize));

            Title = objectData.Get<string>(nameof(Title));

            string backgroundType = objectData.Get<string>(nameof(Background));
            int backgroundIndex = objectData.Get<int>(nameof(Background));

            switch (backgroundType)
            {
                case "BackgroundColor":
                    Background = serializer.Deserialize<BackgroundColor>(backgroundIndex);
                    break;
                case "BackgroundImage":
                    Background = serializer.Deserialize<BackgroundImage>(backgroundIndex);
                    break;
            }

            return this;
        }
    }
}
