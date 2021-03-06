﻿using PhotoBook.Model.Graphics;
using PhotoBook.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Backgrounds
{
    public class BackgroundImage : Background, SerializeInterface<BackgroundImage>
    {
        public BackgroundImage() { }
        public BackgroundImage(Image image) { Image = image; }

        public Image Image { set; get; }

        public BackgroundImage DeserializeObject(Serializer serializer, int objectID)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

            int imageIndex = objectData.Get<int>(nameof(Image));

            Image = serializer.Deserialize<Image>(imageIndex);

            return this;
        }

        public int SerializeObject(Serializer serializer)
        {
            string backgroundImage = $"{nameof(Image)}:&{Image.SerializeObject(serializer)}";

            int backgroundImageID = serializer.AddObject(backgroundImage);

            return backgroundImageID;
        }
    }
}
