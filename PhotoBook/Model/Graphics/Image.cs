using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Arrangement;
using System.Drawing;
using Rectangle = PhotoBook.Model.Arrangement.Rectangle;
using System.IO;

namespace PhotoBook.Model.Graphics
{
    public class Image
    {
        public Image(string path)
        {
            System.IO.FileAttributes attr = File.GetAttributes(OriginalPath);

            if (attr.HasFlag(FileAttributes.Directory))
                throw new Exception("Correct image path was not provided");
            if (Path.GetExtension(path) != ".jpg" && Path.GetExtension(path) != ".png")
                throw new Exception("File/image with wrong exception provided");

            #region Mockup
            DisplayedPath = path;

            Width = 1350;
            Height = 900;

            CurrentFilter = new Filter();
            #endregion

            OriginalPath = path;
            originalBitmap = new Bitmap(path);
            editedBitmap = new Bitmap(path);

            // Save the image somehow in the class
            // TODO: Think of a folder structure to store images in project repository & make a copy of every picture being loaded (think of deleting as well)
        }

        public Bitmap originalBitmap { get; }
        public Bitmap editedBitmap { get; private set; }

        public Rectangle CroppingRectangle { get; set; }
        public string OriginalPath { get; }
        public string DisplayedPath { get; }

        // TODO: Think even more whether the width & height will be necessary (due to the use of bitmap)
        public int Width { get; } // TODO: Think whether it will even be required
        public int Height { get; } // TODO: Think whether it will even be required

        public Filter CurrentFilter { get; private set; } = new Filter();
        public void SetFilter(Filter.Type filterType)
        {
            // Not sure whether I should leave it of not
            #region Mockup
            throw new NotImplementedException("Not available in mockup version");
            #endregion

            if (filterType == Filter.Type.None) editedBitmap = originalBitmap;            
            else
            {
                CurrentFilter.SetFilterSettings(filterType);
                editedBitmap = CurrentFilter.applyFilter(originalBitmap);
            }
        }
    }
}