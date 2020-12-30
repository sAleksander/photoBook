using PhotoBook.Model.Graphics;
using PhotoBook.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Backgrounds
{
    public class BackgroundImage : Background, SerializeInterface<Background>
    {
        public BackgroundImage(Image image) { Image = image; }

        public Image Image { set; get; }

        public Background DeserializeObject(Serializer serializer, int objectID)
        {
            string backgroundImageData = serializer.GetObjectData(objectID);

            int attributeIndex = backgroundImageData.IndexOf($"{nameof(Image)}");
            int dividerIndex = backgroundImageData.IndexOf(':', attributeIndex);
            int endOfLineIndex = backgroundImageData.IndexOf('\n', dividerIndex);

            int backgroundImageID = int.Parse(backgroundImageData.Substring(dividerIndex + 2, endOfLineIndex));

            Image = Image.DeserializeObject(serializer, backgroundImageID);

            return this;
        }

        public int SerializeObject(Serializer serializer)
        {
            string backgroundImage = $"{nameof(Image)}:&{Image.SerializeObject(serializer)}\n";

            int backgroundImageID = serializer.AddObject(backgroundImage);

            return backgroundImageID;
        }
    }
}
