using System;
using System.Drawing;
using Rectangle = PhotoBook.Model.Arrangement.Rectangle;
using PhotoBook.Model.Serialization;
using System.IO;
using System.Diagnostics;

namespace PhotoBook.Model.Graphics
{
    public class Image : SerializeInterface<Image>
    {
        private Bitmap originalBitmap { get; set;}
        private Bitmap editedBitmap { get; set; }

        public Rectangle CroppingRectangle { get; set; }
        public string OriginalPath { get; private set; }
        public string OriginalAbsolutePath { get => Path.GetFullPath(OriginalPath); }
        public string DisplayedPath { get; private set; }
        public string DisplayedAbsolutePath { get => Path.GetFullPath(DisplayedPath); }

        public int Width { get; }
        public int Height { get; }

        public Filter CurrentFilter { get; private set; } = new Filter();

        public Image(string path, int x, int y, int width, int height)
        {
            System.IO.FileAttributes attr = File.GetAttributes(path);
            var extension = Path.GetExtension(path);

            if (attr.HasFlag(FileAttributes.Directory))
                throw new Exception("Correct image path was not provided");
            if (extension != ".jpg" && extension != ".png")
                throw new Exception("File/image with wrong exception provided");


            var randomFilename = GenerateRandomFilename(extension);
            var destinationFilename = $"OriginalImages\\{randomFilename}";

            while (File.Exists(destinationFilename))
            {
                randomFilename = GenerateRandomFilename(extension);
                destinationFilename = $"OriginalImages\\{randomFilename}";
            }

            Directory.CreateDirectory("OriginalImages");
            File.Copy(path, destinationFilename, false);

            OriginalPath = destinationFilename;
            DisplayedPath = destinationFilename;
            originalBitmap = new Bitmap(path);

            Width = originalBitmap.Width;
            Height = originalBitmap.Height;

            CurrentFilter = new Filter();
        }

        public Image(string path) : this(path, 0, 0, 0, 0) { }

        public Image()
        {
        }

        public void SetFilter(Filter.Type filterType)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(CroppingRectangle.X, CroppingRectangle.Y, CroppingRectangle.Width, CroppingRectangle.Height);
            System.Drawing.Imaging.PixelFormat format = originalBitmap.PixelFormat;

            if (filterType == Filter.Type.None)
                editedBitmap = originalBitmap.Clone(rectangle, format);

            else
            {
                CurrentFilter.SetFilterSettings(filterType);
                editedBitmap = CurrentFilter.applyFilter(originalBitmap.Clone(rectangle, format));
            }

            if (File.Exists(DisplayedPath))
                File.Delete(DisplayedPath);

            editedBitmap.Save(DisplayedPath);
        }

        public int SerializeObject(Serializer serializer)
        {
            string image = "";

            image += $"{nameof(OriginalPath)}:{OriginalPath}\n";
            image += $"{nameof(DisplayedPath)}:{DisplayedPath}\n";
            image += $"{nameof(CroppingRectangle)}:&{CroppingRectangle.SerializeObject(serializer)}\n";
            image += $"{nameof(CurrentFilter)}:&{CurrentFilter.SerializeObject(serializer)}";

            int imageID = serializer.AddObject(image);

            return imageID;
        }

        public Image DeserializeObject(Serializer serializer, int objectID)
        {
            if (objectID == -1)
                return null;
            ObjectDataRelay objectData = serializer.GetObjectData(objectID);

            OriginalPath = objectData.Get<string>(nameof(OriginalPath));
            originalBitmap = new Bitmap(OriginalPath);

            DisplayedPath = objectData.Get<string>(nameof(DisplayedPath));
            editedBitmap = new Bitmap(DisplayedPath);

            int croppingRecIndex = objectData.Get<int>(nameof(CroppingRectangle));

            CroppingRectangle = new Rectangle(0, 0, 1, 1).DeserializeObject(serializer, croppingRecIndex);

            int currentFilterIndex = objectData.Get<int>(nameof(CurrentFilter));
            CurrentFilter = CurrentFilter.DeserializeObject(serializer, currentFilterIndex);

            return this;
        }

        private static Random random = new Random();
        private string GenerateRandomFilename(string extension)
        {
            var availableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var chars = new char[8];

            for (int i = 0; i < chars.Length; i++)
            {
                chars[i] = availableChars[random.Next(availableChars.Length)];
            }

            return new string(chars) + extension;
        }

        private void CropBitmap()
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(
                CroppingRectangle.X, CroppingRectangle.Y,
                CroppingRectangle.Width == 0 ? originalBitmap.Width : CroppingRectangle.Width,
                CroppingRectangle.Height == 0 ? originalBitmap.Height : CroppingRectangle.Height);
            System.Drawing.Imaging.PixelFormat format = originalBitmap.PixelFormat;
            editedBitmap = originalBitmap.Clone(rectangle, format);
        }
    }
}