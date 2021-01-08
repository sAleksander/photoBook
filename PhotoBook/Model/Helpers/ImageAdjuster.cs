using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Helpers
{
    static class ImageAdjuster
    {
        #region Constants
        const string PROCESSED_PHOTOS_DIRECTORY = ".\\PROCESSED";
        #endregion

        static public void InitializeProcessedImageDirectory()
        {
            if (Directory.Exists(PROCESSED_PHOTOS_DIRECTORY))
            {
                Directory.Delete(PROCESSED_PHOTOS_DIRECTORY, true);
            }
            Directory.CreateDirectory(PROCESSED_PHOTOS_DIRECTORY);
        }
        static private string extractFilename(string path)
        {
            string[] tmp = path.Split('\\');
            return tmp[tmp.Length - 1];
        }
        static public void CropImage(string path, int startX, int startY, int width, int height)
        {
            Image loaded = Image.FromFile(path);
            Bitmap bmpImg = new Bitmap(loaded);
            Bitmap bmpCrop = bmpImg.Clone(new Rectangle(startX, startY, width, height), bmpImg.PixelFormat);
            bmpCrop.Save($"{PROCESSED_PHOTOS_DIRECTORY}\\{extractFilename(path)}");
        }
    }
}
