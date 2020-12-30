using System;
using System.Collections.Generic;
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
            string backCoverData = serializer.GetObjectData(objectID);

            int attributeIndex = backCoverData.IndexOf($"{nameof(Background)}");
            int dividerIndex = backCoverData.IndexOf(':', attributeIndex);
            int commaIndex = backCoverData.IndexOf(':', dividerIndex);
            int endOfLineIndex = backCoverData.IndexOf("\n", commaIndex);

            int backgroundIndex = int.Parse(backCoverData.Substring(dividerIndex + 2, commaIndex));
            string backgroundType = backCoverData.Substring(commaIndex + 1, endOfLineIndex);

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
