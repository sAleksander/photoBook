using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Arrangement;
using System.Drawing;
using Rectangle = PhotoBook.Model.Arrangement.Rectangle;
using PhotoBook.Model.Serialization;
using System.IO;
using System.Diagnostics;

namespace PhotoBook.Model.Graphics
{
    public class Image : SerializeInterface<Image>
    {
        public Image()
        {
            // Constructor only for serialization purposes
        }

        public Image(string path, int x, int y, int width, int height)
        {
            System.IO.FileAttributes attr = File.GetAttributes(path);

            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            if (attr.HasFlag(FileAttributes.Directory))
                throw new Exception("Correct image path was not provided");
            if (Path.GetExtension(path) != ".jpg" && Path.GetExtension(path) != ".png")
                throw new Exception("File/image with wrong exception provided");

            if (!Directory.Exists("\\OriginalImages"))
                Directory.CreateDirectory("OriginalImages");

            if (!File.Exists($"OriginalImages\\{Path.GetFileName(path)}"))
            {
                //File.Copy(path, $"\\OriginalImages\\{Path.GetFileName(path)}");
                File.Copy(path, $"OriginalImages\\{Path.GetFileName(path)}");

                OriginalPath = $"OriginalImages\\{Path.GetFileName(path)}";

                originalBitmap = new Bitmap(path);
            }
            else
            {
                Random random;
                string newRandomName;

                do
                {
                    random = new Random();
                    newRandomName = new string(Enumerable.Repeat(characters, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                } while (File.Exists($"OriginalImages\\{newRandomName}"));

                string extension = Path.GetExtension(path);

                File.Copy(path, $"OriginalImages\\{newRandomName}{extension}");
                OriginalPath = $"OriginalImages\\{newRandomName}{extension}";
                originalBitmap = new Bitmap(path);
            }

            CroppingRectangle = new Rectangle(x, y, width, height);

            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
            System.Drawing.Imaging.PixelFormat format = originalBitmap.PixelFormat;
            editedBitmap = originalBitmap.Clone(rectangle, format);

            if (!Directory.Exists("\\UsedImages"))
                Directory.CreateDirectory("UsedImages");

            if (!File.Exists($"UsedImages\\{Path.GetFileName(path)}")) {
                editedBitmap.Save($"UsedImages\\{Path.GetFileName(path)}");
                DisplayedPath = $"UsedImages\\{Path.GetFileName(path)}";
            }
            else
            {
                Random random;
                string newRandomName;

                do
                {
                    random = new Random();
                    newRandomName = new string(Enumerable.Repeat(characters, 10).Select(s => s[random.Next(s.Length)]).ToArray());
                } while (File.Exists($"UsedImages\\{newRandomName}"));

                string extension = Path.GetExtension(path);

                editedBitmap.Save($"UsedImages\\{newRandomName}{extension}");
                DisplayedPath = $"UsedImages\\{newRandomName}{extension}";
            }

            Width = originalBitmap.Width;
            Height = originalBitmap.Height;

            CurrentFilter = new Filter();
        }

        public Image(string path) : this(path, 0, 0, 0, 0) { }

        public Bitmap originalBitmap { get; private set; }
        public Bitmap editedBitmap { get; private set; }

        public Rectangle CroppingRectangle { get; set; }
        public string OriginalPath { get; private set; }
        public string DisplayedPath { get; private set; }

        // TODO: Think even more whether the width & height will be necessary (due to the use of bitmap)
        public int Width { get; } // TODO: Think whether it will even be required
        public int Height { get; } // TODO: Think whether it will even be required

        public Filter CurrentFilter { get; private set; } = new Filter();
        public void SetFilter(Filter.Type filterType)
        {
            #region Mockup
            //throw new NotImplementedException("Not available in mockup version");
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
                return this;
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
    }
}