using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Graphics
{
    class FailedToLoadImageException : Exception
    {
        public FailedToLoadImageException() : base("Failed to load image")
        {
        }
    }
}
