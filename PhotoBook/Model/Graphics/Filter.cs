using PhotoBook.Model.Serialization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Graphics
{
    public class Filter : SerializeInterface<Filter>
    {
        public Filter() {
            SetFilterSettings(Filter.Type.None);
            currentType = Type.None;
        }
        public Filter(Filter.Type filterType)
        {
            SetFilterSettings(filterType);
            currentType = filterType;
        }

        public Type currentType { get; private set; }
        Dictionary<string, double> _settings = new Dictionary<string, double>();
        Dictionary<string, double> Settings { get => _settings; }

        public enum Type
        {
            None,
            Warm,
            Cold,
            Greyscale
        }

        public void SetFilterSettings(Filter.Type filterType)
        {
            if (filterType == currentType)
                return;

            currentType = filterType;
            _settings.Clear();

            switch (filterType) {
                case Filter.Type.Cold:
                    _settings.Add("R", 0.8);
                    _settings.Add("G", 0.8);
                    _settings.Add("B", 1);
                    break;

                case Filter.Type.Warm:
                    _settings.Add("R", 1);
                    _settings.Add("G", 0.8);
                    _settings.Add("B", 0.8);
                    break;

                case Filter.Type.Greyscale:
                    _settings.Add("R", 0.114);
                    _settings.Add("G", 0.587);
                    _settings.Add("B", 0.299);
                    break;

                case Filter.Type.None:
                    _settings.Add("R", 1);
                    _settings.Add("G", 1);
                    _settings.Add("B", 1);
                    break;
            }
        }

        public Bitmap applyFilter(Bitmap bitmap)
        {
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* pixelPtr = (byte*)bitmapData.Scan0.ToPointer();
                int offset = bitmapData.Stride - 3 * bitmapData.Width;

                if (currentType == Filter.Type.Greyscale)
                {
                    for (int y = 0; y < bitmapData.Height; y++)
                    {
                        for (int x = 0; x < bitmapData.Width; x++)
                        {
                            pixelPtr[0] = (byte)(_settings["R"] * pixelPtr[2] + _settings["G"] * pixelPtr[1] + _settings["B"] * pixelPtr[0]);
                            pixelPtr[1] = pixelPtr[0];
                            pixelPtr[2] = pixelPtr[0];

                            pixelPtr += 3;
                        }

                        pixelPtr += offset;
                    }
                }
                else
                {
                    for (int y = 0; y < bitmapData.Height; y++)
                    {
                        for (int x = 0; x < bitmapData.Width; x++)
                        {
                            pixelPtr[0] = (byte)(_settings["B"] * pixelPtr[0]);
                            pixelPtr[1] = (byte)(_settings["G"] * pixelPtr[1]);
                            pixelPtr[2] = (byte)(_settings["R"] * pixelPtr[2]);

                            pixelPtr += 3;
                        }

                        pixelPtr += offset;
                    }
                }
            }

            bitmap.UnlockBits(bitmapData);
            return bitmap;
        }

        public static Type[] GetAvailableTypes() => new Type[] { Type.Cold, Type.Warm, Type.Greyscale, Type.None };

        public int SerializeObject(Serializer serializer)
        {
            string filter = $"{nameof(currentType)}:{currentType}";

            int filterTypeID = serializer.AddObject(filter);

            return filterTypeID;
        }

        public Filter DeserializeObject(Serializer serializer, int objectID)
        {
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

            string filter = objectData.Get<string>(nameof(currentType));

            Enum.TryParse(filter, out Filter.Type filterEnum);

            if (filterEnum != Filter.Type.None && filterEnum != Filter.Type.Cold && filterEnum != Filter.Type.Warm && filterEnum != Filter.Type.Greyscale)
                throw new Exception("Wrong filter type met while deserialising");
            else
                currentType = filterEnum;

            return this;
        }
    }
}
