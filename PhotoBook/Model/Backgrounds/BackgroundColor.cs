using PhotoBook.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Backgrounds
{
    public class BackgroundColor : Background, SerializeInterface<Background>
    {
        public BackgroundColor()
        {
            R = 0;
            G = 0;
            B = 0;
        }

        public BackgroundColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }

        public Background DeserializeObject(Serializer serializer, int objectID)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

            R = objectData.Get<byte>(nameof(R));
            G = objectData.Get<byte>(nameof(G));
            B = objectData.Get<byte>(nameof(B));

            return this;
        }

        public int SerializeObject(Serializer serializer)
        {
            string backgroundColor = $"{nameof(R)}:{R}\n";
            backgroundColor += $"{nameof(G)}:{G}\n";
            backgroundColor += $"{nameof(B)}:{B}";

            int backgroundColorID = serializer.AddObject(backgroundColor);

            return backgroundColorID;
        }
    }
}
