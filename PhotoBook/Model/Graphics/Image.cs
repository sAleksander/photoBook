﻿using System;
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
        public Image(string path, int x, int y, int width, int height)
        {
            System.IO.FileAttributes attr = File.GetAttributes(OriginalPath);

            if (attr.HasFlag(FileAttributes.Directory))
                throw new Exception("Correct image path was not provided");
            if (Path.GetExtension(path) != ".jpg" && Path.GetExtension(path) != ".png")
                throw new Exception("File/image with wrong exception provided");

            OriginalPath = $"\\OriginalImages\\{Path.GetFileName(path)}";
            originalBitmap = new Bitmap(path);

            if (!Directory.Exists("\\OriginalImages"))
                Directory.CreateDirectory("OriginalImages");
            if (!File.Exists($"\\OriginalImages\\{Path.GetFileName(path)}"))
                File.Copy(path, $"\\OriginalImages\\{Path.GetFileName(path)}");            
                        
            CroppingRectangle = new Rectangle(x, y, width, height);

            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width, height);
            System.Drawing.Imaging.PixelFormat format = originalBitmap.PixelFormat;
            editedBitmap = originalBitmap.Clone(rectangle, format);

            if (!Directory.Exists("\\UsedImages"))
                Directory.CreateDirectory("UsedImages");
            if (!File.Exists($"\\UsedImages\\{Path.GetFileName(path)}"))            
                editedBitmap.Save($"\\UsedImages\\{Path.GetFileName(path)}");
                      
            #region Mockup            
            DisplayedPath = $"\\UsedImages\\{Path.GetFileName(path)}";

            Width = 1350;
            Height = 900;

            CurrentFilter = new Filter();
            #endregion            
        }

        private Rectangle Rectangle(int x, int y, int width, int height)
        {
            throw new NotImplementedException();
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
            #region Mockup
            throw new NotImplementedException("Not available in mockup version");
            #endregion

            if (filterType == Filter.Type.None)
            {
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(CroppingRectangle.X, CroppingRectangle.Y, CroppingRectangle.Width, CroppingRectangle.Height);
                System.Drawing.Imaging.PixelFormat format = originalBitmap.PixelFormat;
                editedBitmap = originalBitmap.Clone(rectangle, format);
            }
            else
            {
                CurrentFilter.SetFilterSettings(filterType);
                editedBitmap = CurrentFilter.applyFilter(originalBitmap);
            }

            if (File.Exists(DisplayedPath))
                File.Delete(DisplayedPath);

            editedBitmap.Save(DisplayedPath);
        }        
    }
}