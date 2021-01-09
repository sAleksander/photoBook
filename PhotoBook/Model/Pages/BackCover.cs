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
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

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
