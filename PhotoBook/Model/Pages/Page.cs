using PhotoBook.Model.Backgrounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Pages
{
    public abstract class Page
    {
        public Background Background { get; set; }
    }
}
