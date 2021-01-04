using PhotoBook.Model.Serialization;
using System;
using System.Collections.Generic;
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
                    _settings.Clear();
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

        public Bitmap applyFilter(Bitmap originalBitmap)
        {
            Bitmap editedBitmap = originalBitmap;
            BitmapData bitmapData = editedBitmap.LockBits(new Rectangle(0, 0, editedBitmap.Width, editedBitmap.Height),
            ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);

            unsafe
            {
                byte* pixelPointer = (byte*)bitmapData.Scan0.ToPointer();
                int endPixelAddress = (int)pixelPointer + bitmapData.Stride * bitmapData.Height;

                switch (currentType)
                {
                    case (Filter.Type.Greyscale):
                        while ((int)pixelPointer != endPixelAddress)
                        {
                            pixelPointer[0] = (byte)(_settings["R"] * pixelPointer[2] + _settings["G"] * pixelPointer[1] + _settings["B"] * pixelPointer[0]);
                            pixelPointer[1] = pixelPointer[0];
                            pixelPointer[2] = pixelPointer[0];
                            pixelPointer += 3;
                        }
                        break;

                    default:
                        while ((int)pixelPointer != endPixelAddress)
                        {
                            pixelPointer[2] = (byte)(_settings["R"] * pixelPointer[2]);
                            pixelPointer[1] = (byte)(_settings["G"] * pixelPointer[1]);
                            pixelPointer[0] = (byte)(_settings["B"] * pixelPointer[0]);
                        }
                        break;
                }
            }
            return editedBitmap;
        }

        public static Type[] GetAvailableTypes() => new Type[] { Type.Cold, Type.Warm, Type.Greyscale, Type.None };

        public int SerializeObject(Serializer serializer)
        {
            string filter = $"{nameof(currentType)}:{currentType}\n";

            int filterTypeID = serializer.AddObject(filter);

            return filterTypeID;
        }

        public Filter DeserializeObject(Serializer serializer, int objectID)
        {
            ObjectDataRelay objectData = serializer.GetObjectData2(objectID);

            string filter = objectData.Get<string>(nameof(currentType));

            switch (filter)
            {
                case "Filter.Type.Cold":
                    currentType = Filter.Type.Cold;
                    break;
                case "Filter.Type.Greyscale":
                    currentType = Filter.Type.Greyscale;
                    break;
                case "Filter.Type.None":
                    currentType = Filter.Type.None;
                    break;
                case "Filter.Type.Warm":
                    currentType = Filter.Type.Warm;
                    break;
                default:
                    throw new Exception("Wrong filter type while deserialising");
            }

            return this;
        }
    }
}
