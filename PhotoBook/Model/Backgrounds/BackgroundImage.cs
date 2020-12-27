using PhotoBook.Model.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Backgrounds
{
    public class BackgroundImage : Background
    {
        public BackgroundImage(Image image) { Image = image; }

        public Image Image { set; get; }
    }
}
