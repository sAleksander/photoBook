﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhotoBook.Model.Arrangement;

namespace PhotoBook.Model.Graphics
{
    public class Image
    {
        public Image(string path)
        {
            #region Mockup
            DisplayedPath = path;

            Width = 1350;
            Height = 900;

            CurrentFilter = Filter.Type.None;
            #endregion

            OriginalPath = path;

            // Save the image somehow in the class
            // TODO: Make a photo copy in project's folders
            // TODO: Adjust other properties, settings, etc.
        }

        public Rectangle CroppingRectangle { get; set; }
        public string OriginalPath { get; }
        public string DisplayedPath { get; }
        public int Width { get; } // TODO: Think whether it will even be required
        public int Height { get; } // TODO: Think whether it will even be required

        public Filter.Type CurrentFilter { get; }
        public void SetFilter(Filter.Type filterType)
        {
            #region Mockup
            throw new NotImplementedException("Not available in mockup version");
            #endregion

            // TODO: Process the original picture and save it as a displayed picture + save it in project resources
            Filter.applyFilter(); // <-- This method should still be adjusted
        }
    }
}