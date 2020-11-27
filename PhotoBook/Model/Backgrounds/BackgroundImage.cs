﻿using PhotoBook.Model.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Backgrounds
{
    class BackgroundImage : Background
    {
        BackgroundImage(Image image)
        {
            Image = image;
        }

        Image Image { set; get; }
    }
}