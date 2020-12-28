using System;
using System.Drawing;
using Rectangle = PhotoBook.Model.Arrangement.Rectangle;
using System.IO;

namespace PhotoBook.Model.Graphics
{
    public class Image
    {
        public Bitmap originalBitmap { get; }
        public Bitmap editedBitmap { get; private set; }

        public Rectangle CroppingRectangle { get; set; }
        public string OriginalPath { get; }
        public string OriginalAbsolutePath { get => Path.GetFullPath(OriginalPath); }
        public string DisplayedPath { get; }
        public string DisplayedAbsolutePath { get => Path.GetFullPath(DisplayedPath); }

        public int Width { get; }
        public int Height { get; }

        public Filter CurrentFilter { get; private set; } = new Filter();

        public Image(string path, int x, int y, int width, int height)
        {
            System.IO.FileAttributes attr = File.GetAttributes(path);
            var extension = Path.GetExtension(path);

            // TODO: Handle situations in which two files would have the same names

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

        public void SetFilter(Filter.Type filterType)
        {
            #region Mockup
            throw new NotImplementedException("Not available in mockup version");
            #endregion

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