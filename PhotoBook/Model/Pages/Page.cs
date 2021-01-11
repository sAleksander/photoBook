using PhotoBook.Model.Backgrounds;
using SmartWeakEvent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoBook.Model.Pages
{
    public abstract class Page
    {
        public FastSmartWeakEvent<EventHandler> BackgroundChanged = new FastSmartWeakEvent<EventHandler>();

        private Background background;
        public Background Background
        {
            get => background;
            set
            {
                background = value;
                BackgroundChanged.Raise(this, EventArgs.Empty);
            }
        }
    }
}
